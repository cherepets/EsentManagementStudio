using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using EsentManagementStudio.Extensions;
using FourToolkit.Esent;
using Microsoft.Win32;
using System.Text;

namespace EsentManagementStudio
{
    public partial class MainWindow : INotifyPropertyChanged
    {
        public static MainWindow Current { get; private set; }

        public EsentInstance Instance { get; }
        public ObservableCollection<EsentDatabase> Databases { get; }
        public ObservableCollection<string> DatabaseNames { get { return Databases.Select(d => d.Name).ToObservableCollection(); } }

        public EsentDatabase GetDatabase(string name) => Databases.FirstOrDefault(d => d.Name == name);

        public MainWindow()
        {
            InitializeComponent();
            Current = this;
            DataContext = this;
            Instance = new EsentInstance("EsentManagementStudio");
            Databases = new ObservableCollection<EsentDatabase>();
            Databases.CollectionChanged += (sender, e) => OnPropertyChanged(nameof(DatabaseNames));
        }

        private void ObjectExplorer_OnNewQuery(object sender, RoutedEventArgs e)
        {
            var db = sender as EsentDatabase;
            if (db == null) return;
            QueryEditor.NewSession(db.Name, CodeConstants.NewQueryScript);
        }

        #region Database
        private void ObjectExplorer_OnCreateDb(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new SaveFileDialog { Filter = "ESENT Database (*.db)|*.db|All files(*.*)|*.*" };
                if (dialog.ShowDialog(GetWindow(this)) != true) return;
                var path = dialog.FileName;
                if (string.IsNullOrEmpty(path)) return;
                if (Databases.Any(d => d.FilePath == path)) return;
                var db = Instance.BeginSession().CreateDatabase(path, true);
                Databases.Add(db);
                SetStatus("Created database: " + db.Name, Colors.LightSlateGray);
            }
            catch (Exception exception)
            {
                SetStatus(exception.Message, Colors.IndianRed);
            }
        }

        private void ObjectExplorer_OnOpenDb(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new OpenFileDialog { Filter = "ESENT Database (*.db)|*.db|All files(*.*)|*.*" };
                if (dialog.ShowDialog(GetWindow(this)) != true) return;
                var path = dialog.FileName;
                if (string.IsNullOrEmpty(path)) return;
                if (Databases.Any(d => d.FilePath == path)) return;
                var db = Instance.BeginSession().OpenDatabase(path);
                db.Tables.Values.ForEach(t => t.Open());
                Databases.Add(db);
                SetStatus("Opened database: " + db.Name, Colors.LightSlateGray);
            }
            catch (Exception exception)
            {
                SetStatus(exception.Message, Colors.IndianRed);
            }
        }

        private void ObjectExplorer_OnCloseDb(object sender, RoutedEventArgs e)
        {
            var db = sender as EsentDatabase;
            if (db == null) return;
            db.Close();
            Databases.Remove(db);
        }

        private void QueryEditor_OnRefreshDb(object sender, RoutedEventArgs e)
        {
            var db = sender as EsentDatabase;
            if (db == null) return;
            db.Close();
            Databases.Remove(db);
            db = Instance.BeginSession().OpenDatabase(db.FilePath);
            db.Tables.Values.ForEach(t => t.Open());
            Databases.Add(db);
        }
        #endregion

        #region Table
        private void ObjectExplorer_OnNewTable(object sender, RoutedEventArgs e)
        {
            var db = sender as EsentDatabase;
            if (db == null) return;
            QueryEditor.NewSession(db.Name, CodeConstants.NewTableScript);
        }

        private void ObjectExplorer_OnShowTable(object sender, RoutedEventArgs e)
        {
            var db = sender as EsentDatabase;
            if (db == null) return;
            QueryEditor.NewSession(db.Name, CodeConstants.ShowTableScript);
        }

        private void ObjectExplorer_OnDropTable(object sender, RoutedEventArgs e)
        {
            var table = sender as EsentTable;
            var db = table?.Database ?? sender as EsentDatabase;
            if (db == null) return;
            QueryEditor.NewSession(db.Name, CodeConstants.DropTableScript
                .With("table.Name", table?.Name));
        }
        #endregion
        
        private void ObjectExplorer_OnGenerateClass(object sender, RoutedEventArgs e)
        {
            var table = sender as EsentTable;
            if (table == null) return;
            var codeBuilder = new StringBuilder();
            foreach (var col in table.Columns.Values)
                codeBuilder.AppendLine(CodeConstants.Property
                    .With("type", col.ColumnType.Name)
                    .With("column.Name", col.Name));
            QueryEditor.NewSession(table.Database.Name, CodeConstants.ClassHeader
                .With("namespace", table.Database.Name)
                .With("table.Name", table.Name)
                .With("code", codeBuilder.ToString()));
        }

        #region Data
        private void ObjectExplorer_OnSelectTop(object sender, RoutedEventArgs e)
        {
            var table = sender as EsentTable;
            if (table == null) return;
            QueryEditor.NewSession(table.Database.Name, CodeConstants.SelectTopScript
                .With("table.Name", table.Name));
        }

        private void ObjectExplorer_OnSelectExact(object sender, RoutedEventArgs e)
        {
            var table = sender as EsentTable;
            if (table == null) return;
            QueryEditor.NewSession(table.Database.Name, CodeConstants.SelectExactScript
                .With("table.Name", table.Name));
        }

        private void ObjectExplorer_OnSelectCondition(object sender, RoutedEventArgs e)
        {
            var table = sender as EsentTable;
            if (table == null) return;
            QueryEditor.NewSession(table.Database.Name, CodeConstants.SelectConditionScript
                .With("table.Name", table.Name));
        }

        private void ObjectExplorer_OnCountRows(object sender, RoutedEventArgs e)
        {
            var table = sender as EsentTable;
            if (table == null) return;
            QueryEditor.NewSession(table.Database.Name, CodeConstants.CountRowsScript
                .With("table.Name", table.Name));
        }

        private void ObjectExplorer_OnInsertData(object sender, RoutedEventArgs e)
        {
            var table = sender as EsentTable;
            if (table == null) return;
            QueryEditor.NewSession(table.Database.Name, CodeConstants.InsertDataScript
                .With("table.Name", table.Name));
        }

        private void ObjectExplorer_OnUpdateData(object sender, RoutedEventArgs e)
        {
            var table = sender as EsentTable;
            if (table == null) return;
            QueryEditor.NewSession(table.Database.Name, CodeConstants.UpdateDataScript
                .With("table.Name", table.Name));
        }

        private void ObjectExplorer_OnDeleteData(object sender, RoutedEventArgs e)
        {
            var table = sender as EsentTable;
            if (table == null) return;
            QueryEditor.NewSession(table.Database.Name, CodeConstants.DeleteDataScript
                .With("table.Name", table.Name));
        }
        #endregion

        #region Column
        private void ObjectExplorer_OnNewColumn(object sender, RoutedEventArgs e)
        {
            var table = sender as EsentTable;
            if (table == null) return;
            QueryEditor.NewSession(table.Database.Name, CodeConstants.NewColumnScript
                .With("table.Name", table.Name));
        }

        private void ObjectExplorer_OnShowColumn(object sender, RoutedEventArgs e)
        {
            var table = sender as EsentTable;
            if (table == null) return;
            QueryEditor.NewSession(table.Database.Name, CodeConstants.ShowColumnScript
                .With("table.Name", table.Name));
        }

        private void ObjectExplorer_OnDropColumn(object sender, RoutedEventArgs e)
        {
            var column = sender as EsentColumn;
            var table = column?.Table ?? sender as EsentTable;
            if (table == null) return;
            QueryEditor.NewSession(table.Database.Name, CodeConstants.DropColumnScript
                .With("table.Name", table.Name)
                .With("column.Name", column?.Name));
        }
        #endregion

        #region Index
        private void ObjectExplorer_OnNewIndex(object sender, RoutedEventArgs e)
        {
            var table = sender as EsentTable;
            if (table == null) return;
            QueryEditor.NewSession(table.Database.Name, CodeConstants.NewIndexScript
                .With("table.Name", table.Name));
        }

        private void ObjectExplorer_OnShowIndex(object sender, RoutedEventArgs e)
        {
            var table = sender as EsentTable;
            if (table == null) return;
            QueryEditor.NewSession(table.Database.Name, CodeConstants.ShowIndexScript
                .With("table.Name", table.Name));
        }

        private void ObjectExplorer_OnDropIndex(object sender, RoutedEventArgs e)
        {
            var table = sender as EsentTable;
            if (table == null) return;
            QueryEditor.NewSession(table.Database.Name, CodeConstants.DropIndexScript
                .With("table.Name", table.Name));
        }
        #endregion

        private void ObjectExplorer_OnSelectedItemChanged(object sender, RoutedEventArgs e) => PropertyEditor.Display(sender);

        private void MetroWindow_Closed(object sender, EventArgs e) => Instance?.Close();
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static void SetStatus(string status, Color color)
        {
            Current.Status.Items.Clear();
            Current.Status.Background = new SolidColorBrush(color);
            Current.Status.Items.Add(status);
        }
    }
}
