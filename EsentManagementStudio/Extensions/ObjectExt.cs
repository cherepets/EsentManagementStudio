using System.Windows;

namespace EsentManagementStudio.Extensions
{
    public static class ObjectExt
    {
        public static FrameworkElement AsFrameworkElement(this object obj) => obj as FrameworkElement;
        public static object GetDataContext(this object obj) => obj.AsFrameworkElement()?.DataContext;
    }
}
