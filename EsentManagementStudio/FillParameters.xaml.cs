using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using EsentManagementStudio.Extensions;

namespace EsentManagementStudio
{
    public sealed partial class FillParameters : INotifyPropertyChanged
    {
        public DataTable Parameters { get; }

        public string Result { get; private set; }

        private readonly string[] _knownParameters =
        {
            "{table.Name}",
            "{column.Name}",
            "{index.Name}",
            "{datatype}",
            "{key}",
            "{value}",
            "{count}"
        };

        public FillParameters(string input)
        {
            Result = input;
            var parameters = input == null
                ? null
                : _knownParameters.Where(input.Contains).ToArray();
            InitializeComponent();
            ParametersGrid.DataContext = this;
            Parameters = new DataTable("Parameters");
            Parameters.Columns.Add("Parameter");
            Parameters.Columns.Add("Value");
            parameters.ForEach(p => Parameters.Rows.Add(p, string.Empty));
            OnPropertyChanged(nameof(Parameters));
            Loaded += FillParameters_Loaded;
        }

        public new bool? ShowDialog()
        {
            if (Parameters == null || Parameters.Rows.Count < 1)
            {
                Close();
                return false;
            }
            return base.ShowDialog();
        }

        private void FillParameters_Loaded(object sender, RoutedEventArgs e)
        {
            ParametersGrid.Columns[0].IsReadOnly = true;
            ParametersGrid.Columns[0].Width = 100;
            ParametersGrid.Columns[1].Width = 200;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            for (var i = 0; i < Parameters.Rows.Count; i++)
            {
                var parameter = Parameters.Rows[i]["Parameter"].ToString();
                var value = Parameters.Rows[i]["Value"].ToString();
                if (!string.IsNullOrEmpty(value))
                    Result = Result.Replace(parameter,value);
            }
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) => Close();
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
