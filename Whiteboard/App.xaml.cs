using System.Windows;
using chancies.Whiteboard.ViewModels;
using WpfUtils;

namespace chancies.Whiteboard
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            AutoDataTemplateResourceDictionary.LoadAutoDataTemplates();

            Utilities.Show(new ConnectViewModel());
        }
    }

}
