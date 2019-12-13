using System;
using System.DirectoryServices.AccountManagement;
using System.Runtime.Serialization;
using chancies.Whiteboard.Network.Client;
using WpfUtils;

namespace chancies.Whiteboard.ViewModels
{
    public class ClientViewModel
        : BaseViewModel
    {
        public static ClientViewModel _current;

        public ClientViewModel(ClientModel model)
        {
            Model = model;
        }

        public ClientModel Model { get; private set; }

        public string NetworkUserName
        {
            get { return Model.NetworkUserName; }
            set { Model.NetworkUserName = value; }
        }

        public string FriendlyName
        {
            get { return Model.FriendlyName; }
            set { Model.FriendlyName = value; }
        }

        public string DisplayUserName
        {
            get
            {
                return string.Format("{0} ({1})", FriendlyName, NetworkUserName);
            }
        }

        public static ClientViewModel Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new ClientViewModel(
                        new ClientModel
                        {
                            FriendlyName = UserPrincipal.Current.DisplayName,
                            NetworkUserName = Environment.UserDomainName + "\\" + Environment.UserName
                        });
                }

                return _current;
            }
        }
    }
}
