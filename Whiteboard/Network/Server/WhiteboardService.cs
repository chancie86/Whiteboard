using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using chancies.Whiteboard.Network.Client;
using chancies.Whiteboard.Network.Interfaces;

namespace chancies.Whiteboard.Network.Server
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single,InstanceContextMode = InstanceContextMode.Single)]
    public class WhiteboardService
        : IWhiteboardService
    {
        #region Singleton stuff
        private static WhiteboardService _service;

        // Enforce singleton
        private WhiteboardService()
        {
        }

        public static WhiteboardService GetService()
        {
            return _service ?? (_service = new WhiteboardService());
        }
        #endregion

        private ServiceHost _host;
        private byte[] _latestStrokes;

        public bool IsRunning { get; private set; }
        public bool PendingShutdown { get; private set; }

        private readonly Dictionary<IWhiteboardServiceCallback, ClientModel> _callbackToUserDictionary = new Dictionary<IWhiteboardServiceCallback, ClientModel>();

        #region Lifecycle management
        public void Start()
        {
            IsRunning = true;
            var thread = new Thread(RunThread);
            thread.Start();
        }

        public void Stop()
        {
            PendingShutdown = true;
        }

        private void RunThread()
        {
            try
            {
                _host = new ServiceHost(typeof (WhiteboardService));
                _host.Open();

                while (true)
                {
                    if (PendingShutdown)
                        break;

                    Thread.Sleep(1000);
                }

                _host.Close();
            }
            catch
            {
                if (_host != null)
                    _host.Close();
                throw;
            }
            finally
            {
                IsRunning = false;
                _host = null;
            }
        }
        #endregion

        #region Implement IWhiteboardService
        public bool Join(ClientModel clientModel)
        {
            // Remember this client and callback channel
            var callbackChannel = OperationContext.Current.GetCallbackChannel<IWhiteboardServiceCallback>();

            if (_callbackToUserDictionary.ContainsValue(clientModel) == false)
            {
                _callbackToUserDictionary.Add(callbackChannel, clientModel);
            }

            // Update list of clients
            foreach (var callbackClient in _callbackToUserDictionary.Keys)
            {
                callbackClient.UpdateClientsList(_callbackToUserDictionary.Values.ToList());
            }

            // Send the latest ink strokes to the newly connected client
            callbackChannel.UpdateStrokes(_callbackToUserDictionary[callbackChannel], _latestStrokes);

            return true;
        }

        public void Leave(ClientModel clientModel, bool isServer)
        {
            var callbackChannel = OperationContext.Current.GetCallbackChannel<IWhiteboardServiceCallback>();
            if (_callbackToUserDictionary.ContainsKey(callbackChannel))
            {
                _callbackToUserDictionary.Remove(callbackChannel);
            }

            // Tell each of the clients about the updated list of clients
            foreach (var callbackClient in _callbackToUserDictionary.Keys)
            {
                if (isServer)
                {
                    //server user logout, disconnect clients
                    callbackClient.ServerDisconnected();
                }
                else
                {
                    //normal user logout
                    callbackClient.UpdateClientsList(_callbackToUserDictionary.Values.ToList());
                }
            }

            if (isServer)
            {
                _callbackToUserDictionary.Clear();
            }
        }

        public void SendInkStrokes(MemoryStream memoryStream)
        {
            var callbackChannel = OperationContext.Current.GetCallbackChannel<IWhiteboardServiceCallback>();

            _latestStrokes = memoryStream.GetBuffer();

            foreach (var callbackClient in _callbackToUserDictionary.Keys)
            {
                if (callbackClient != OperationContext.Current.GetCallbackChannel<IWhiteboardServiceCallback>())
                {
                    callbackClient.UpdateStrokes(_callbackToUserDictionary[callbackChannel], _latestStrokes);
                }
            }
        }
        #endregion
    }
}
