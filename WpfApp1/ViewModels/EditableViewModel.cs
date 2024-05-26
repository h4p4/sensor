namespace WpfApp1.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using iTextSharp.text;
    using iTextSharp.text.pdf;

    using Microsoft.Office.Interop.Excel;
    using Microsoft.Office.Interop.Word;

    using Newtonsoft.Json;

    using WpfApp1.Views;

    using Application = Microsoft.Office.Interop.Excel.Application;
    using DataTable = System.Data.DataTable;
    using Document = Microsoft.Office.Interop.Word.Document;
    using Font = iTextSharp.text.Font;

    public abstract class EditableViewModel : ViewModel
    {
        private static bool _isRedoProcess;
        private static bool _isUndoProcess;

        private static Stack<(object Obj, string Prop, object OldValue)> _redoHistory
            = new Stack<(object Obj, string Prop, object OldValue)>();

        private static Stack<(object Obj, string Prop, object OldValue)> _undoHistory = new();

        private bool _isEditing;

        public EditableViewModel()
        {
            StartEditCommand = new RelayCommand(Edit);
            GenerateExcelCommand = new RelayCommand(GenerateExcel);
            GeneratePdfCommand = new RelayCommand(GeneratePdf);
            GenerateWordCommand = new RelayCommand(GenerateWord);
            Logger.Instance.AddViewModel(this);
        }

        [JsonIgnore]
        public static DelegateCommand ClearHistoryCommand { get; }
            = new DelegateCommand(_ => ClearHistory());
        [JsonIgnore]

        public RelayCommand GenerateExcelCommand { get; set; }
        [JsonIgnore]
        public RelayCommand GeneratePdfCommand { get; set; }
        [JsonIgnore]
        public RelayCommand GenerateWordCommand { get; set; }

        [JsonIgnore]
        internal bool IsEditing
        {
            get => _isEditing;
            set => SetField(ref _isEditing, value);
        }

        [JsonIgnore]
        public static DelegateCommand RedoCommand { get; }
            = new DelegateCommand(_ => Redo(), _ => _redoHistory.Count > 0);

        [JsonIgnore]
        public RelayCommand StartEditCommand { get; }

        [JsonIgnore]
        public static DelegateCommand UndoCommand { get; }
            = new DelegateCommand(_ => Undo(), _ => _undoHistory.Count > 0);

        public static DataTable DataGridToDataTable(DataGrid dg)
        {
            dg.SelectionMode = DataGridSelectionMode.Extended;
            dg.SelectAllCells();
            dg.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, dg);
            dg.UnselectAllCells();
            var result = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
            var lines = result.Split(new[] { "\r\n"/*, "\n"*/ },
                StringSplitOptions.None);
            var fields = lines[0].Split(',');
            var cols = fields.GetLength(0);
            var dt = new DataTable();

            for (var i = 0; i < cols; i++)
                dt.Columns.Add(fields[i].ToUpper(), typeof(string));
            for (var i = 1; i < lines.GetLength(0) - 1; i++)
            {
                fields = lines[i].Split(',');
                cols = fields.GetLength(0);
                var row = dt.NewRow();
                for (var f = 0; f < cols; f++)
                    row[f] = fields[f];
                dt.Rows.Add(row);
            }

            return dt;
        }


        public void ExportToPdf(DataTable dataTable, string destinationPath)
        {
            var document = new iTextSharp.text.Document();
            var writer = PdfWriter.GetInstance(document, new FileStream(destinationPath, FileMode.Create));
            document.Open();

            var table = new PdfPTable(dataTable.Columns.Count);
            table.WidthPercentage = 100;
            var baseFont = BaseFont.CreateFont("c:/Windows/Fonts/arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            var font = new Font(baseFont);
            //Set columns names in the pdf file
            for (var k = 0; k < dataTable.Columns.Count; k++)
            {
                var cell = new PdfPCell(new Phrase(dataTable.Columns[k].ColumnName, font));

                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = new BaseColor(51, 102, 102);

                table.AddCell(cell);
            }

            //Add values of DataTable in pdf file
            for (var i = 0; i < dataTable.Rows.Count; i++)
            {
                for (var j = 0; j < dataTable.Columns.Count; j++)
                {
                    var cell = new PdfPCell(new Phrase(dataTable.Rows[i][j].ToString(), font));

                    //Align the cell in the center
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.VerticalAlignment = Element.ALIGN_CENTER;

                    table.AddCell(cell);
                }
            }

            document.Add(table);
            document.Close();
        }

        public void ExportToWord(DataTable dt)
        {
            var rowCount = dt.Rows.Count;
            var columnCount = dt.Columns.Count;
            var dataArray = new object[rowCount + 1, columnCount + 1];
            //int RowCount = 0; int ColumnCount = 0;
            var r = 0;
            for (var c = 0; c <= columnCount - 1; c++)
            {
                dataArray[r, c] = dt.Columns[c].ColumnName;
                for (r = 0; r <= rowCount - 1; r++)
                    dataArray[r, c] = dt.Rows[r][c]; //end row loop
            } //end column loop

            var oDoc = new Document
            {
                Application =
                {
                    Visible = true
                },
                PageSetup =
                {
                    Orientation = WdOrientation.wdOrientLandscape
                }
            };

            dynamic oRange = oDoc.Content.Application.Selection.Range;
            var oTemp = "";
            for (r = 0; r <= rowCount - 1; r++)
            {
                for (var c = 0; c <= columnCount - 1; c++)
                    oTemp = oTemp + dataArray[r, c] + "\t";
            }

            oRange.Text = oTemp;

            object separator = WdTableFieldSeparator.wdSeparateByTabs;
            object format = WdTableFormat.wdTableFormatWeb1;
            object applyBorders = true;
            object autoFit = true;

            object autoFitBehavior = WdAutoFitBehavior.wdAutoFitContent;
            oRange.ConvertToTable(ref separator,
                ref rowCount, ref columnCount, Type.Missing, ref format,
                ref applyBorders, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, ref autoFit, ref autoFitBehavior,
                Type.Missing);

            oRange.Select();
            oDoc.Application.Selection.Tables[1].Select();
            oDoc.Application.Selection.Tables[1].Rows.AllowBreakAcrossPages = 0;
            oDoc.Application.Selection.Tables[1].Rows.Alignment = 0;
            oDoc.Application.Selection.Tables[1].Rows[1].Select();
            oDoc.Application.Selection.InsertRowsAbove(1);
            oDoc.Application.Selection.Tables[1].Rows[1].Select();

            //gotta do the header row manually
            for (var c = 0; c <= columnCount - 1; c++)
                oDoc.Application.Selection.Tables[1].Cell(1, c + 1).Range.Text = dt.Columns[c].ColumnName;

            oDoc.Application.Selection.Tables[1].Rows[1].Select();
            oDoc.Application.Selection.Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
            oDoc.Activate();
        }

        public abstract string GetTitle();

        protected virtual void Edit(object obj)
        {
            IsEditing = true;

            CollectionHeaderRelator[] relators;
            var title = string.Empty;
            if (obj is EditableViewModel viewModel)
            {
                relators = viewModel.GetRelators();
                title = viewModel.GetTitle();
            }
            else
            {
                title = GetTitle();
                relators = GetRelators();
            }


            new EditWindow(relators)
            {
                Title = string.Concat("Редактирование - ", $"{title.Replace('\n', ' ')}")
            }.ShowDialog();

            IsEditing = false;
        }

        protected virtual CollectionHeaderRelator[] GetRelators()
        {
            return Array.Empty<CollectionHeaderRelator>();
        }

        protected override bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            SaveHistory(this, propertyName, field);
            OnPropertyChanging(new PropertyChangingEventArgs(propertyName));
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private static void ClearHistory()
        {
            _undoHistory.Clear();
            UndoCommand.RaiseCanExecuteChanged();
            _redoHistory.Clear();
            RedoCommand.RaiseCanExecuteChanged();
        }

        private static void ExportToExcel(DataTable dataTable)
        {
            var excel = new Application();
            var wb = excel.Workbooks.Add();
            var ws = (Worksheet)wb.ActiveSheet;
            ws.Columns.AutoFit();
            ws.Columns.EntireColumn.ColumnWidth = 25;

            // Header row
            for (var i = 0; i < dataTable.Columns.Count; i++)
                ws.Range["A1"].Offset[0, i].Value = dataTable.Columns[i].ColumnName;

            // Data Rows
            for (var i = 0; i < dataTable.Rows.Count; i++)
            {
                ws.Range["A2"].Offset[i].Resize[1, dataTable.Columns.Count].Value =
                    dataTable.Rows[i].ItemArray;
            }

            excel.Visible = true;
            wb.Activate();
        }

        private static void Redo()
        {
            if (_redoHistory.Count == 0)
                return;
            var redo = _redoHistory.Pop();
            RedoCommand.RaiseCanExecuteChanged();
            try
            {
                _isRedoProcess = true;
                redo.Obj.GetType().GetProperty(redo.Prop).SetValue(redo.Obj, redo.OldValue);
            }
            finally
            {
                _isRedoProcess = false;
            }
        }

        private static void SaveHistory(object obj, string propertyName, object value)
        {
            if (!Logger.Instance.IsEnabled)
                return;

            if (obj.GetType()
                    .GetProperty(propertyName)?
                    .GetCustomAttributes(typeof(UndoRedoAttribute), true)
                    .Length == 0)
                return;

            if (_isUndoProcess)
            {
                _redoHistory.Push((obj, propertyName, value));
                RedoCommand.RaiseCanExecuteChanged();
            }
            else if (_isRedoProcess)
            {
                _undoHistory.Push((obj, propertyName, value));
                UndoCommand.RaiseCanExecuteChanged();
            }
            else
            {
                _undoHistory.Push((obj, propertyName, value));
                UndoCommand.RaiseCanExecuteChanged();
                _redoHistory.Clear();
                RedoCommand.RaiseCanExecuteChanged();
            }
        }

        private static void Undo()
        {
            if (_undoHistory.Count == 0)
                return;
            var undo = _undoHistory.Pop();
            UndoCommand.RaiseCanExecuteChanged();
            // Обернуто для того чтобы в случае исключения флаг всё равно снимался
            try
            {
                _isUndoProcess = true;

                undo.Obj.GetType().GetProperty(undo.Prop)?.SetValue(undo.Obj, undo.OldValue);
            }
            finally
            {
                _isUndoProcess = false;
            }
        }

        private void GenerateExcel(object obj)
        {
            if (!IsDataGrid(obj, out var dataGrid))
                return;

            var dataTable = DataGridToDataTable(dataGrid);
            ExportToExcel(dataTable);
        }

        private void GeneratePdf(object obj)
        {
            if (!IsDataGrid(obj, out var dataGrid))
                return;
            var dataTable = DataGridToDataTable(dataGrid);

            var folderPath = SaveHelper.GetSaveFolder();
            var filePath = Path.Combine(folderPath, $"report_{DateTime.Now:MMddHHmm}.pdf");
            ExportToPdf(dataTable, filePath);
        }

        private void GenerateWord(object obj)
        {
            if (!IsDataGrid(obj, out var dataGrid))
                return;

            var dataTable = DataGridToDataTable(dataGrid);
            ExportToWord(dataTable);
        }

        private bool IsDataGrid(object obj, out DataGrid dataGrid)
        {
            if (obj is DataGrid dGrid)
            {
                dataGrid = dGrid;
                return true;
            }

            dataGrid = null;
            return false;
        }
    }

    public class DelegateCommand : ICommand
    {
        protected readonly Action<object> _execute;
        protected readonly Predicate<object> _canExecute;

        public DelegateCommand(Action<object> execute) : this(execute, _ => true)
        {
        }

        public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public abstract class ViewModel : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanging(PropertyChangingEventArgs e)
        {
            PropertyChanging?.Invoke(this, e);
        }

        protected virtual bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            OnPropertyChanging(new PropertyChangingEventArgs(propertyName));
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }

    [JsonConverter(typeof(ViewModelToIntConverter))]
    public class IntegerViewModel : ViewModel
    {
        private int _value;

        [UndoRedo]
        [DisplayName("Значения")]
        public int Value
        {
            get => _value;
            set => SetField(ref _value, value);
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    internal class UndoRedoAttribute : Attribute
    {
    }
}