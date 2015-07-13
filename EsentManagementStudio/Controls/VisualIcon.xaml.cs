using System.Windows.Media;

namespace EsentManagementStudio.Controls
{
    public partial class VisualIcon
    {
        /// <summary>
        /// Gets or sets the brush's content.
        /// </summary>
        /// 
        /// <returns>
        /// The brush's content. The default is null.
        /// </returns>
        public Visual Visual
        {
            get { return (Rectangle.Fill as VisualBrush)?.Visual; }
            set { Rectangle.Fill = new VisualBrush(value); }
        }

        public VisualIcon()
        {
            InitializeComponent();
        }
    }
}
