using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using EsentManagementStudio.Extensions;
using FourToolkit.Esent;

namespace EsentManagementStudio
{
    public partial class QueryEditor : INotifyPropertyChanged
    {

        public QueryEditor()
        {
            InitializeComponent();
            ResultsGrid.DataContext = this;
        }

        public DataTable Results { get; private set; }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            var context = DataContext as MainWindow;
            var db = context?.GetDatabase(ConnectionBox.Text);
            var code = CodeEditor.Text;
            bool success;
            var result = QueryExecutor.Execute(code, db, out success);
            if (success)
                MainWindow.SetStatus("Query executed successfully", Colors.MediumSeaGreen);
            else
                MainWindow.SetStatus("Query completed with errors", Colors.IndianRed);
            ConnectionBox.Text = db?.Name;
            var row = result as EsentRow;
            if (row != null)
            {
                ClearResults();
                FillResults(row);
                MessagesBox.Text = string.Empty;
                ShowResults();
                return;
            }
            var rows = result as IEnumerable<EsentRow>;
            if (rows != null)
            {
                ClearResults();
                FillResults(rows);
                MessagesBox.Text = string.Empty;
                ShowResults();
                return;
            }
            MessagesBox.Text = result?.ToString();
            MessagesBox.Foreground = new SolidColorBrush(success ? Colors.Black : Colors.Red);
            ClearResults();
            ShowMessages();
        }

        private void ClearResults() => Results = new DataTable("Results");

        private void FillResults(EsentRow row)
        {
            if (Results.Columns.Count < 1)
                row.ForEach(c => Results.Columns.Add(c.ColumnName));
            var values = row.Select(c => c.Value).ToArray();
            Results.Rows.Add(values);
        }

        private void FillResults(IEnumerable<EsentRow> rows) => rows.ForEach(FillResults);

        private void ShowResults()
        {
            ResultsButton.IsEnabled = false;
            ResultsColumn.Width = new GridLength(1, GridUnitType.Star);
            MessagesButton.IsEnabled = true;
            MessagesColumn.Width = new GridLength(0, GridUnitType.Star);
            OnPropertyChanged(nameof(Results));
        }

        private void ShowMessages()
        {
            ResultsButton.IsEnabled = true;
            ResultsColumn.Width = new GridLength(0, GridUnitType.Star);
            MessagesButton.IsEnabled = false;
            MessagesColumn.Width = new GridLength(1, GridUnitType.Star);
        }

        private void Results_Click(object sender, RoutedEventArgs e) => ShowResults();
        private void Button_Click(object sender, RoutedEventArgs e) => ShowMessages();

        public void NewSession(string connection, string code)
        {
            var fillWindow = new FillParameters(code);
            fillWindow.ShowDialog();
            code = fillWindow.Result;
            ConnectionBox.Text = connection;
            CodeEditor.Text = code;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CodeEditor_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
                Run_Click(sender, new RoutedEventArgs());
        }
    }
}
