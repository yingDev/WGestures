using WGestures.Core.Commands;

namespace WGestures.App.Gui.Windows.CommandViews
{
    public partial class GeneralNoParameterCommandView : CommandViewUserControl
    {
        public GeneralNoParameterCommandView()
        {
            InitializeComponent();
        }

        public override AbstractCommand Command { get; set; }
    }
}
