using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EsentManagementStudio.Controls
{
    public partial class VisualButton
    {
        /// <summary>
        /// Add / Remove ClickEvent handler 
        /// </summary>
        [Category("Behavior")]
        public event RoutedEventHandler Click;

        /// <summary>
        /// Gets or sets the brush's content.
        /// </summary>
        /// 
        /// <returns>
        /// The brush's content. The default is null.
        /// </returns>
        public Visual Visual
        {
            get { return ((Button.Content as Rectangle)?.Fill as VisualBrush)?.Visual; }
            set { Button.Content = new Rectangle {Fill = new VisualBrush(value), Width = 13, Height = 13}; }
        }

        public VisualButton()
        {
            InitializeComponent();
            Button.Click += Button_Click;
        }

        private void Button_Click(object sender, RoutedEventArgs e) => Click?.Invoke(this, e);
    }
}
