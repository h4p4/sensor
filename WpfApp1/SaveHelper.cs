namespace WpfApp1
{
    using System.Windows.Forms;

    internal class SaveHelper
    {
        public static string GetSaveFolder()
        {
            var browserDialog = new FolderBrowserDialog();
            var result = browserDialog.ShowDialog();
            if (result == DialogResult.Abort)
                return string.Empty;
            return browserDialog.SelectedPath;
        }
    }
}