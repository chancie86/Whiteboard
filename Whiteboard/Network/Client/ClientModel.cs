using System.Runtime.Serialization;

namespace chancies.Whiteboard.Network.Client
{
    [DataContract]
    public class ClientModel
    {
        [DataMember]
        public string NetworkUserName { get; set; }

        [DataMember]
        public string FriendlyName { get; set; }
    }
}
