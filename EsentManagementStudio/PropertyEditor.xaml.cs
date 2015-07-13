using System.ComponentModel;
using System.Data;
using FourToolkit.Esent;

namespace EsentManagementStudio
{
    public partial class PropertyEditor : INotifyPropertyChanged
    {
        public PropertyEditor()
        {
            InitializeComponent();
            PropertiesGrid.DataContext = this;
        }

        public DataTable Properties { get; private set; }

        internal void Display(object sender)
        {
            Properties = new DataTable("Properties");
            Properties.Columns.Add("Name");
            Properties.Columns.Add("Value");
            var database = sender as EsentDatabase;
            if (database != null) FillDatabase(database);
            var table = sender as EsentTable;
            if (table != null) FillTable(table);
            var column = sender as EsentColumn;
            if (column != null) FillColumn(column);
            OnPropertyChanged(nameof(Properties));
        }

        private void FillDatabase(EsentDatabase db)
        {
            Properties.Rows.Add(nameof(db.Name), db.Name);
            Properties.Rows.Add(nameof(db.Opened), db.Opened);
            Properties.Rows.Add(nameof(db.FilePath), db.FilePath);
            Properties.Rows.Add(nameof(db.Tables), db.Tables.Count);
        }

        private void FillTable(EsentTable table)
        {
            Properties.Rows.Add(nameof(table.Name), table.Name);
            Properties.Rows.Add(nameof(table.Opened), table.Opened);
            Properties.Rows.Add(nameof(table.Database), table.Database.Name);
            Properties.Rows.Add("Rows", table.Count);
            Properties.Rows.Add(nameof(table.Columns), table.Columns.Count);
            Properties.Rows.Add(nameof(table.Indexes), table.Indexes.Count);
        }

        private void FillColumn(EsentColumn column)
        {
            Properties.Rows.Add(nameof(column.Name), column.Name);
            Properties.Rows.Add(nameof(column.Table), column.Table.Name);
            Properties.Rows.Add(nameof(column.ColumnType), column.ColumnType.Name);
            Properties.Rows.Add(nameof(column.Max), column.Max);
            Properties.Rows.Add(nameof(column.Encoding), column.Encoding);
            Properties.Rows.Add(nameof(column.Options), column.Options);
            Properties.Rows.Add(nameof(column.DefaultValue), column.DefaultValue);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
