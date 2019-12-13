using System;
using System.Windows.Input;
using chancies.Whiteboard.Network.Server;
using WpfUtils;
using WpfUtils.ViewModels;

namespace chancies.Whiteboard.ViewModels
{
    public class ConnectViewModel
        : WindowBaseViewModel
    {
        private bool _isServer;
        private string _serverAddress;

        public ConnectViewModel()
        {
            IsServer = true;
            Title = "Whiteboard";

            StartCommand = new SimpleParameterCommand(StartCommandExecute);
        }

        public bool IsServer
        {
            get { return _isServer; }
            set { OnPropertyChanged(ref _isServer, value, () => IsServer); }
        }

        public string ServerAddress
        {
            get { return _serverAddress; }
            set { OnPropertyChanged(ref _serverAddress, value, () => ServerAddress); }
        }

        #region Start Command
        public ICommand StartCommand { get; set; }

        private void StartCommandExecute(object obj)
        {
            MainWindowViewModel mainWindowViewModel = null;

            try
            {
                mainWindowViewModel = new MainWindowViewModel(this);

                if (IsServer)
                {
                    StartServer(mainWindowViewModel);
                }

                // Note that the server connects to itself
                mainWindowViewModel.Connect();

                Utilities.Show(mainWindowViewModel);
                Close();
            }
            catch (Exception ex)
            {
                Utilities.ShowModal(new ErrorViewModel(ex.Message));

                if (mainWindowViewModel != null)
                    mainWindowViewModel.Close();
            }
        }
        #endregion

        #region Networking
        private void StartServer(MainWindowViewModel mainWindowViewModel)
        {
            var service = WhiteboardService.GetService();
            mainWindowViewModel.Closing += (sender, args) => WhiteboardService.GetService().Stop();
            service.Start();
        }
        #endregion
    }
}
