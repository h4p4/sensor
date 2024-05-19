// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

namespace WpfApp1.ViewModels
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    using Newtonsoft.Json;

    public class Action : EditableViewModel
    {
        private int _actionId;
        private int _execute;
        private int _param;
        private int _type;
        private int _value;
        private int? _delay;
        private int? _display;

        [DisplayName("Действие")]
        [JsonProperty("action")]
        public int ActionId
        {
            get => _actionId;
            set => SetField(ref _actionId, value);
        }

        [DisplayName("Задержка")]
        [JsonProperty("delay")]
        public int? Delay
        {
            get => _delay;
            set => SetField(ref _delay, value);
        }

        [DisplayName("Отображение")]
        [JsonProperty("display")]
        public int? Display
        {
            get => _display;
            set => SetField(ref _display, value);
        }

        [DisplayName("Выполнение")]
        [JsonProperty("execute")]
        public int Execute
        {
            get => _execute;
            set => SetField(ref _execute, value);
        }

        [DisplayName("Параметр")]
        [JsonProperty("param")]
        public int Param
        {
            get => _param;
            set => SetField(ref _param, value);
        }

        [DisplayName("Тип")]
        [JsonProperty("type")]
        public int Type
        {
            get => _type;
            set => SetField(ref _type, value);
        }

        [DisplayName("Значение")]
        [JsonProperty("value")]
        public int Value
        {
            get => _value;
            set => SetField(ref _value, value);
        }
    }

    public class Answer : EditableViewModel
    {
        private ObservableCollection<IntegerViewModel> _data;
        private ObservableCollection<IntegerViewModel> _params;

        public Answer()
        {
            Data = new ObservableCollection<IntegerViewModel>();
            Params = new ObservableCollection<IntegerViewModel>();
        }

        [DisplayName("Данные")]
        [JsonProperty("data")]
        public ObservableCollection<IntegerViewModel> Data
        {
            get => _data;
            set => SetField(ref _data, value);
        }

        [DisplayName("Параметры")]
        [JsonProperty("params")]
        public ObservableCollection<IntegerViewModel> Params
        {
            get => _params;
            set => SetField(ref _params, value);
        }
    }

    public class Command : EditableViewModel
    {
        private int _commandId;
        private ObservableCollection<IntegerViewModel> _errors;
        private ObservableCollection<Transaction> _transactions;

        [DisplayName("Команда")]
        [JsonProperty("command")]
        public int CommandId
        {
            get => _commandId;
            set => SetField(ref _commandId, value);
        }

        [DisplayName("Ошибки")]
        [JsonProperty("errors")]
        public ObservableCollection<IntegerViewModel> Errors
        {
            get => _errors;
            set => SetField(ref _errors, value);
        }

        [JsonProperty("transactions")]
        public ObservableCollection<Transaction> Transactions
        {
            get => _transactions;
            set => SetField(ref _transactions, value);
        }

        protected override CollectionHeaderRelator[] GetRelators()
        {
            return new[]
            {
                new CollectionHeaderRelator(_errors, null, "Ошибки"),
                new CollectionHeaderRelator(_transactions, StartEditCommand, "Транзакции"),
            };
        }
    }

    public class Control : EditableViewModel
    {
        private int _action;
        private int _enabled;
        private int _freq;
        private int _type;
        private int _value;
        private int? _spacing;
        private ObservableCollection<Control> _controls;
        private ObservableCollection<IntegerViewModel> _margins;
        private ObservableCollection<Line> _lines;
        private string _default;
        private string _header;
        private string _text;

        [DisplayName("Действие")]
        [JsonProperty("action")]
        public int Action
        {
            get => _action;
            set => SetField(ref _action, value);
        }

        [JsonProperty("controls")]
        public ObservableCollection<Control> Controls
        {
            get => _controls;
            set => SetField(ref _controls, value);
        }

        [DisplayName("По умолчанию")]
        [JsonProperty("default")]
        public string Default
        {
            get => _default;
            set => SetField(ref _default, value);
        }

        [DisplayName("Включено")]
        [JsonProperty("enabled")]
        public int Enabled
        {
            get => _enabled;
            set => SetField(ref _enabled, value);
        }

        [DisplayName("Частота")]
        [JsonProperty("freq")]
        public int Freq
        {
            get => _freq;
            set => SetField(ref _freq, value);
        }

        [DisplayName("Заголовок")]
        [JsonProperty("header")]
        public string Header
        {
            get => _header;
            set => SetField(ref _header, value);
        }

        [JsonProperty("lines")]
        public ObservableCollection<Line> Lines
        {
            get => _lines;
            set => SetField(ref _lines, value);
        }

        [DisplayName("Отступы")]
        [JsonProperty("margins")]
        public ObservableCollection<IntegerViewModel> Margins
        {
            get => _margins;
            set => SetField(ref _margins, value);
        }

        [DisplayName("Расстояние")]
        [JsonProperty("spacing")]
        public int? Spacing
        {
            get => _spacing;
            set => SetField(ref _spacing, value);
        }

        [DisplayName("Текст")]
        [JsonProperty("text")]
        public string Text
        {
            get => _text;
            set => SetField(ref _text, value);
        }

        [DisplayName("Тип")]
        [JsonProperty("type")]
        public int Type
        {
            get => _type;
            set => SetField(ref _type, value);
        }

        [DisplayName("Значение")]
        [JsonProperty("value")]
        public int Value
        {
            get => _value;
            set => SetField(ref _value, value);
        }

        protected override CollectionHeaderRelator[] GetRelators()
        {
            return new[]
            {
                new CollectionHeaderRelator(_lines, StartEditCommand, "Линии"),
                new CollectionHeaderRelator(_controls, StartEditCommand, "Управления"),
                new CollectionHeaderRelator(_margins, null, "Отступы"),
            };
        }
    }

    public class Hart : EditableViewModel
    {
        private EditableViewModel _selectedCommand;
        private EditableViewModel _selectedParameter;
        private EditableViewModel _selectedSequence;
        private EditableViewModel _selectedView;
        private ObservableCollection<Command> _commands;
        private ObservableCollection<Parameter> _parameters;
        private ObservableCollection<Sequence> _sequences;
        private ObservableCollection<View> _views;

        public Hart()
        {
            Commands = new ObservableCollection<Command>();
            Parameters = new ObservableCollection<Parameter>();
            Sequences = new ObservableCollection<Sequence>();
            Views = new ObservableCollection<View>();
        }

        [JsonProperty("commands")]
        public ObservableCollection<Command> Commands
        {
            get => _commands;
            set => SetField(ref _commands, value);
        }

        [JsonProperty("parameters")]
        public ObservableCollection<Parameter> Parameters
        {
            get => _parameters;
            set => SetField(ref _parameters, value);
        }

        [JsonIgnore]
        public EditableViewModel SelectedCommand
        {
            get => _selectedCommand;
            set => SetField(ref _selectedCommand, value);
        }

        [JsonIgnore]
        public EditableViewModel SelectedParameter
        {
            get => _selectedParameter;
            set => SetField(ref _selectedParameter, value);
        }

        [JsonIgnore]
        public EditableViewModel SelectedSequence
        {
            get => _selectedSequence;
            set => SetField(ref _selectedSequence, value);
        }

        [JsonIgnore]
        public EditableViewModel SelectedView
        {
            get => _selectedView;
            set => SetField(ref _selectedView, value);
        }

        [JsonProperty("sequences")]
        public ObservableCollection<Sequence> Sequences
        {
            get => _sequences;
            set => SetField(ref _sequences, value);
        }

        [JsonProperty("views")]
        public ObservableCollection<View> Views
        {
            get => _views;
            set => SetField(ref _views, value);
        }
    }

    public class Line : EditableViewModel
    {
        private int _value;
        private string _text;

        [DisplayName("Текст")]
        [JsonProperty("text")]
        public string Text
        {
            get => _text;
            set => SetField(ref _text, value);
        }

        [DisplayName("Значение")]
        [JsonProperty("value")]
        public int Value
        {
            get => _value;
            set => SetField(ref _value, value);
        }
    }

    public class Param : EditableViewModel
    {
        private int _paramId;
        private int _source;
        private int _value;

        [DisplayName("Параметр")]
        [JsonProperty("param")]
        public int ParamId
        {
            get => _paramId;
            set => SetField(ref _paramId, value);
        }

        [DisplayName("Источник")]
        [JsonProperty("source")]
        public int Source
        {
            get => _source;
            set => SetField(ref _source, value);
        }

        [DisplayName("Значение")]
        [JsonProperty("value")]
        public int Value
        {
            get => _value;
            set => SetField(ref _value, value);
        }
    }

    public class Parameter : EditableViewModel
    {
        private int _freq;
        private int _index;
        private int _scale;
        private int _type;
        private string _name;

        [DisplayName("Частота")]
        [JsonProperty("freq")]
        public int Freq
        {
            get => _freq;
            set => SetField(ref _freq, value);
        }

        [DisplayName("Индекс")]
        [JsonProperty("index")]
        public int Index
        {
            get => _index;
            set => SetField(ref _index, value);
        }

        [DisplayName("Наименование")]
        [JsonProperty("name")]
        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }

        [DisplayName("Масштаб")]
        [JsonProperty("scale")]
        public int Scale
        {
            get => _scale;
            set => SetField(ref _scale, value);
        }

        [DisplayName("Тип")]
        [JsonProperty("type")]
        public int Type
        {
            get => _type;
            set => SetField(ref _type, value);
        }
    }

    public class Request : EditableViewModel
    {
        private ObservableCollection<IntegerViewModel> _data;
        private ObservableCollection<IntegerViewModel> _params;

        public Request()
        {
            Data = new ObservableCollection<IntegerViewModel>();
            Params = new ObservableCollection<IntegerViewModel>();
        }

        [DisplayName("Данные")]
        [JsonProperty("data")]
        public ObservableCollection<IntegerViewModel> Data
        {
            get => _data;
            set => SetField(ref _data, value);
        }

        [DisplayName("Параметры")]
        [JsonProperty("params")]
        public ObservableCollection<IntegerViewModel> Params
        {
            get => _params;
            set => SetField(ref _params, value);
        }

        protected override CollectionHeaderRelator[] GetRelators()
        {
            return new[]
            {
                new CollectionHeaderRelator(_data, null, "Данные"),
                new CollectionHeaderRelator(_params, null, "Параметры"),
            };
        }
    }

    public class Sequence : EditableViewModel
    {
        private int _sequenceId;
        private ObservableCollection<Param> _params;

        public Sequence()
        {
            Params = new ObservableCollection<Param>();
        }

        [JsonProperty("params")]
        public ObservableCollection<Param> Params
        {
            get => _params;
            set => SetField(ref _params, value);
        }

        [DisplayName("Последовательность")]
        [JsonProperty("sequence")]
        public int SequenceId
        {
            get => _sequenceId;
            set => SetField(ref _sequenceId, value);
        }

        protected override CollectionHeaderRelator[] GetRelators()
        {
            return new[]
            {
                new CollectionHeaderRelator(_params, StartEditCommand, "Параметры"),
            };
        }
    }

    public class Transaction : EditableViewModel
    {
        private static int _transactionCounter;
        private Answer _answer;
        private int _transactionNumber;
        private Request _request;
        private string _viewName;

        public Transaction()
        {
            _transactionCounter++;
            _transactionNumber = _transactionCounter;
            Answer = new Answer();
            Request = new Request();
        }

        [JsonProperty("answer")]
        public Answer Answer
        {
            get => _answer;
            set => SetField(ref _answer, value);
        }

        [JsonProperty("request")]
        public Request Request
        {
            get => _request;
            set => SetField(ref _request, value);
        }

        [DisplayName("Транзакция")]
        [JsonIgnore]
        public string ViewName
        {
            get => $"Транзакция №{_transactionNumber}";
            set => _viewName = value;
        }

        protected override CollectionHeaderRelator[] GetRelators()
        {
            return new[]
            {
                new CollectionHeaderRelator(_request.Data, null, "Данные запроса"),
                new CollectionHeaderRelator(_request.Params, null, "Параметры запроса"),
                new CollectionHeaderRelator(_answer.Data, null, "Данные ответа"),
                new CollectionHeaderRelator(_answer.Params, null, "Параметры ответа"),
            };
        }
    }

    public class View : EditableViewModel
    {
        private int _onHideAction;
        private int _onShowAction;
        private int _spacing;
        private ObservableCollection<Action> _actions;
        private ObservableCollection<Control> _controls;
        private ObservableCollection<IntegerViewModel> _margins;
        private string _caption;

        public View()
        {
            Actions = new ObservableCollection<Action>();
            Controls = new ObservableCollection<Control>();
            Margins = new ObservableCollection<IntegerViewModel>();
        }

        [JsonProperty("actions")]
        public ObservableCollection<Action> Actions
        {
            get => _actions;
            set => SetField(ref _actions, value);
        }

        [DisplayName("Подпись")]
        [JsonProperty("caption")]
        public string Caption
        {
            get => _caption;
            set => SetField(ref _caption, value);
        }

        [JsonProperty("controls")]
        public ObservableCollection<Control> Controls
        {
            get => _controls;
            set => SetField(ref _controls, value);
        }

        [DisplayName("Отступы")]
        [JsonProperty("margins")]
        public ObservableCollection<IntegerViewModel> Margins
        {
            get => _margins;
            set => SetField(ref _margins, value);
        }

        [DisplayName("Действие при скрытии")]
        [JsonProperty("on_hide_action")]
        public int OnHideAction
        {
            get => _onHideAction;
            set => SetField(ref _onHideAction, value);
        }

        [DisplayName("Действии при показе")]
        [JsonProperty("on_show_action")]
        public int OnShowAction
        {
            get => _onShowAction;
            set => SetField(ref _onShowAction, value);
        }

        [DisplayName("Расстояние")]
        [JsonProperty("spacing")]
        public int Spacing
        {
            get => _spacing;
            set => SetField(ref _spacing, value);
        }

        protected override CollectionHeaderRelator[] GetRelators()
        {
            return new[]
            {
                new CollectionHeaderRelator(_actions, null, "Действия"),
                new CollectionHeaderRelator(_controls, StartEditCommand, "Управления"),
                new CollectionHeaderRelator(_margins, null, "Отступы"),
            };
        }
    }
}