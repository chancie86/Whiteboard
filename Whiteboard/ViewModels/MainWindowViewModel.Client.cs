using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.Threading;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Input;
using chancies.Whiteboard.Network.Client;
using chancies.Whiteboard.Network.Interfaces;
using chancies.Whiteboard.Properties;
using WpfUtils;

namespace chancies.Whiteboard.ViewModels
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Single, UseSynchronizationContext = false)]
    public partial class MainWindowViewModel
        : IWhiteboardServiceCallback
    {
        private WhiteboardClient _client;
        private string _lastUpdatedBy;
        public event StrokesUpdatedHandler StrokesUpdated;

        public string LastUpdatedBy
        {
            get { return _lastUpdatedBy; }
            set { OnPropertyChanged(ref _lastUpdatedBy, value, () => LastUpdatedBy); }
        }

        public ICommand SendStrokesCommand { get; private set; }

        public void Connect()
        {
            var serverAddress = Connection.IsServer
                ? string.Format("net.tcp://localhost:{0}/WhiteboardService/", Settings.Default.Port)
                : string.Format("net.tcp://{0}:{1}/WhiteboardService/", Connection.ServerAddress, Settings.Default.Port);
            var endpoint = new EndpointAddress(serverAddress);

            _client = new WhiteboardClient(
                                            new InstanceContext(this),
                                            "WhiteboardClientTcpBinding",
                                            endpoint
                                        );
            _client.Open();

            _client.Join(ClientViewModel.Current.Model);

            Closing += LeaveServer;
        }

        #region Implement IWhiteboardServiceCallback
        public void UpdateClientsList(List<ClientModel> clients)
        {
            SendOrPostCallback callback =
                delegate(object obj)
                {
                    var newClients = obj as List<ClientModel>;
                    Clients.Clear();

                    if (newClients == null)
                        return;

                    foreach (var c in newClients)
                        Clients.Add(new ClientViewModel(c));
                };

            _uiSynchronizationContext.Post(callback, clients);
        }

        public void UpdateStrokes(ClientModel clientModel, byte[] bytesStroke)
        {
            SendOrPostCallback strokCallback =
                      delegate(object obj)
                      {
                          if (StrokesUpdated == null)
                              return;

                          var data = obj as byte[];
                          if (data == null)
                              return;

                          try
                          {
                              var memoryStream = new MemoryStream(bytesStroke);
                              StrokesUpdated(this, new StrokesUpdatedEventArgs(new StrokeCollection(memoryStream)));
                          }
                          catch (Exception exc)
                          {
                              MessageBox.Show(exc.Message, Title);
                          }
                      };

            _uiSynchronizationContext.Post(strokCallback, bytesStroke);

            SendOrPostCallback lastUpdatedByCallback =
                      delegate(object obj)
                      {
                          var model = obj as ClientModel;
                          if (model == null)
                              return;

                          var clientViewModel = new ClientViewModel(model);

                          LastUpdatedBy = clientViewModel.DisplayUserName;
                      };
            _uiSynchronizationContext.Post(lastUpdatedByCallback, clientModel);
        }

        public void ServerDisconnected()
        {
            Closing -= LeaveServer;

            SendOrPostCallback callback =
                        delegate(object dummy)
                        {
                            Utilities.ShowModal(new ErrorViewModel("Server disconnected"));
                            Close();
                        };

            _uiSynchronizationContext.Post(callback, null);
        }
        #endregion

        private void SendStrokesCommandExecute(object obj)
        {
            var memoryStream = obj as MemoryStream;

            if (memoryStream == null)
                return;

            _client.SendInkStrokes(memoryStream);
        }

        private void LeaveServer(object sender, EventArgs args)
        {
            _client.Leave(ClientViewModel.Current.Model, Connection.IsServer);
        }
    }

    #region StrokesUpdated Event
    public delegate void StrokesUpdatedHandler(object sender, StrokesUpdatedEventArgs args);

    public class StrokesUpdatedEventArgs
        : EventArgs
    {
        public StrokesUpdatedEventArgs(StrokeCollection strokes)
        {
            Strokes = strokes;
        }

        public StrokeCollection Strokes { get; private set; }
    }
    #endregion
}
