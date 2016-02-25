using System.Windows;
using EsentManagementStudio.Extensions;
using FourToolkit.Esent;

namespace EsentManagementStudio
{
    public partial class ObjectExplorer
    {
        public event RoutedEventHandler NewQuery;

        public event RoutedEventHandler CreateDb;
        public event RoutedEventHandler OpenDb;
        public event RoutedEventHandler CloseDb;
        public event RoutedEventHandler RefreshDb;

        public event RoutedEventHandler NewTable;
        public event RoutedEventHandler ShowTable;
        public event RoutedEventHandler DropTable;

        public event RoutedEventHandler GenerateClass;
        public event RoutedEventHandler SelectTop;
        public event RoutedEventHandler SelectExact;
        public event RoutedEventHandler SelectCondition;
        public event RoutedEventHandler CountRows;
        public event RoutedEventHandler InsertData;
        public event RoutedEventHandler UpdateData;
        public event RoutedEventHandler DeleteData;

        public event RoutedEventHandler NewColumn;
        public event RoutedEventHandler ShowColumn;
        public event RoutedEventHandler DropColumn;

        public event RoutedEventHandler NewIndex;
        public event RoutedEventHandler ShowIndex;
        public event RoutedEventHandler DropIndex;

        public event RoutedEventHandler SelectedItemChanged;

        public ObjectExplorer()
        {
            InitializeComponent();
        }

        private void Menu_NewQuery_Click(object sender, RoutedEventArgs e)
        {
            var context = sender.GetDataContext();
            if (context != null)
                NewQuery?.Invoke(context, e);
        }

        #region Database
        private void CreateDb_Click(object sender, RoutedEventArgs e) => CreateDb?.Invoke(this, e);
        private void OpenDb_Click(object sender, RoutedEventArgs e) => OpenDb?.Invoke(this, e);
        private void CloseDb_Click(object sender, RoutedEventArgs e)
        {
            var context = DatabasesView.SelectedItem as EsentDatabase;
            if (context != null)
                CloseDb?.Invoke(context, e);
        }

        private void Menu_CloseDb_Click(object sender, RoutedEventArgs e)
        {
            var context = sender.GetDataContext();
            if (context != null)
                CloseDb?.Invoke(context, e);
        }

        private void Menu_RefreshDb_Click(object sender, RoutedEventArgs e)
        {
            var context = sender.GetDataContext();
            if (context != null)
                RefreshDb?.Invoke(context, e);
        }
        #endregion

        #region Table
        private void Menu_NewTable_Click(object sender, RoutedEventArgs e)
        {
            var context = sender.GetDataContext();
            if (context != null)
                NewTable?.Invoke(context, e);
        }

        private void Menu_ShowTable_Click(object sender, RoutedEventArgs e)
        {
            var context = sender.GetDataContext();
            if (context != null)
                ShowTable?.Invoke(context, e);
        }

        private void Menu_DropTable_Click(object sender, RoutedEventArgs e)
        {
            var context = sender.GetDataContext();
            if (context != null)
                DropTable?.Invoke(context, e);
        }
        #endregion
        
        private void Menu_GenerateClass_Click(object sender, RoutedEventArgs e)
        {
            var context = sender.GetDataContext();
            if (context != null)
                GenerateClass?.Invoke(context, e);
        }

        #region Data
        private void Menu_SelectTop_Click(object sender, RoutedEventArgs e)
        {
            var context = sender.GetDataContext();
            if (context != null)
                SelectTop?.Invoke(context, e);
        }

        private void Menu_SelectExact_Click(object sender, RoutedEventArgs e)
        {
            var context = sender.GetDataContext();
            if (context != null)
                SelectExact?.Invoke(context, e);
        }

        private void Menu_SelectCondition_Click(object sender, RoutedEventArgs e)
        {
            var context = sender.GetDataContext();
            if (context != null)
                SelectCondition?.Invoke(context, e);
        }

        private void Menu_CountRows_Click(object sender, RoutedEventArgs e)
        {
            var context = sender.GetDataContext();
            if (context != null)
                CountRows?.Invoke(context, e);
        }

        private void Menu_InsertData_Click(object sender, RoutedEventArgs e)
        {
            var context = sender.GetDataContext();
            if (context != null)
                InsertData?.Invoke(context, e);
        }

        private void Menu_UpdateData_Click(object sender, RoutedEventArgs e)
        {
            var context = sender.GetDataContext();
            if (context != null)
                UpdateData?.Invoke(context, e);
        }

        private void Menu_DeleteData_Click(object sender, RoutedEventArgs e)
        {
            var context = sender.GetDataContext();
            if (context != null)
                DeleteData?.Invoke(context, e);
        }
        #endregion

        #region Column
        private void Menu_NewColumn_Click(object sender, RoutedEventArgs e)
        {
            var context = sender.GetDataContext();
            if (context != null)
                NewColumn?.Invoke(context, e);
        }

        private void Menu_ShowColumn_Click(object sender, RoutedEventArgs e)
        {
            var context = sender.GetDataContext();
            if (context != null)
                ShowColumn?.Invoke(context, e);
        }

        private void Menu_DropColumn_Click(object sender, RoutedEventArgs e)
        {
            var context = sender.GetDataContext();
            if (context != null)
                DropColumn?.Invoke(context, e);
        }
        #endregion

        #region Index
        private void Menu_NewIndex_Click(object sender, RoutedEventArgs e)
        {
            var context = sender.GetDataContext();
            if (context != null)
                NewIndex?.Invoke(context, e);
        }

        private void Menu_ShowIndex_Click(object sender, RoutedEventArgs e)
        {
            var context = sender.GetDataContext();
            if (context != null)
                ShowIndex?.Invoke(context, e);
        }

        private void Menu_DropIndex_Click(object sender, RoutedEventArgs e)
        {
            var context = sender.GetDataContext();
            if (context != null)
                DropIndex?.Invoke(context, e);
        }
        #endregion

        private void DatabasesView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var context = DatabasesView.SelectedItem;
            if (context != null)
                SelectedItemChanged?.Invoke(context, e);
        }
    }
}
