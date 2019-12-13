using chancies.Whiteboard.ViewModels;
using WpfUtils;

namespace chancies.Whiteboard.Views
{
    /// <summary>
    /// Interaction logic for ConnectView.xaml
    /// </summary>
    [DataTemplated(typeof(ConnectViewModel))]
    public partial class ConnectView
    {
        public ConnectView()
        {
            InitializeComponent();
        }
    }
}
