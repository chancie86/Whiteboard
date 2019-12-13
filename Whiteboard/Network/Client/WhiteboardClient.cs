using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using chancies.Whiteboard.Network.Interfaces;
using chancies.Whiteboard.ViewModels;

namespace chancies.Whiteboard.Network.Client
{
    public class WhiteboardClient
         : DuplexClientBase<IWhiteboardService>, IWhiteboardService
    {
        #region Implement DuplexClientBase
        public WhiteboardClient(object callbackInstance)
            : base(callbackInstance)
        {
        }

        public WhiteboardClient(object callbackInstance, string endpointConfigurationName)
            : base(callbackInstance, endpointConfigurationName)
        {
        }

        public WhiteboardClient(object callbackInstance, string endpointConfigurationName, string remoteAddress)
            : base(callbackInstance, endpointConfigurationName, remoteAddress)
        {
        }

        public WhiteboardClient(object callbackInstance, string endpointConfigurationName, EndpointAddress remoteAddress)
            : base(callbackInstance, endpointConfigurationName, remoteAddress)
        {
        }

        public WhiteboardClient(object callbackInstance, Binding binding, EndpointAddress remoteAddress)
            : base(callbackInstance, binding, remoteAddress)
        {
        }

        public WhiteboardClient(object callbackInstance, ServiceEndpoint endpoint)
            : base(callbackInstance, endpoint)
        {
        }

        public WhiteboardClient(InstanceContext callbackInstance)
            : base(callbackInstance)
        {
        }

        public WhiteboardClient(InstanceContext callbackInstance, string endpointConfigurationName)
            : base(callbackInstance, endpointConfigurationName)
        {
        }

        public WhiteboardClient(InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress)
            : base(callbackInstance, endpointConfigurationName, remoteAddress)
        {
        }

        public WhiteboardClient(InstanceContext callbackInstance, string endpointConfigurationName, EndpointAddress remoteAddress)
            : base(callbackInstance, endpointConfigurationName, remoteAddress)
        {
        }

        public WhiteboardClient(InstanceContext callbackInstance, Binding binding, EndpointAddress remoteAddress)
            : base(callbackInstance, binding, remoteAddress)
        {
        }

        public WhiteboardClient(InstanceContext callbackInstance, ServiceEndpoint endpoint)
            : base(callbackInstance, endpoint)
        {
        }
        #endregion

        #region Implement IWhiteboardService
        public bool Join(ClientModel clientModel)
        {
            return Channel.Join(clientModel);
        }

        public void Leave(ClientModel clientModel, bool isServer)
        {
            Channel.Leave(clientModel, isServer);
        }

        public void SendInkStrokes(MemoryStream memoryStream)
        {
            Channel.SendInkStrokes(memoryStream);
        }
        #endregion
    }
}
