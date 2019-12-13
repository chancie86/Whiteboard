using System.IO;
using System.ServiceModel;
using chancies.Whiteboard.Network.Client;

namespace chancies.Whiteboard.Network.Interfaces
{
    [ServiceContract(Name = "WhiteboardService", Namespace = "http://localhost/WhiteboardService/",
        SessionMode = SessionMode.Required, CallbackContract = typeof(IWhiteboardServiceCallback))]
    public interface IWhiteboardService
    {
        [OperationContract]
        bool Join(ClientModel clientModel);

        [OperationContract]
        void Leave(ClientModel clientModel, bool isServer);

        [OperationContract]
        void SendInkStrokes(MemoryStream memoryStream);
    }
}
