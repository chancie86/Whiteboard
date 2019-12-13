using System.Collections.Generic;
using System.ServiceModel;
using chancies.Whiteboard.Network.Client;

namespace chancies.Whiteboard.Network.Interfaces
{
    public interface IWhiteboardServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void UpdateClientsList(List<ClientModel> clients);

        [OperationContract(IsOneWay = true)]
        void UpdateStrokes(ClientModel clientModel, byte[] bytesStroke);

        [OperationContract(IsOneWay = true)]
        void ServerDisconnected();
    }
}
