using System.Windows.Controls;
using chancies.Whiteboard.ViewModels;
using WpfUtils;

namespace chancies.Whiteboard.Views
{
    /// <summary>
    /// Interaction logic for ErrorView.xaml
    /// </summary>
    [DataTemplated(typeof(ErrorViewModel))]
    public partial class ErrorView
    {
        public ErrorView()
        {
            InitializeComponent();
        }
    }
}
