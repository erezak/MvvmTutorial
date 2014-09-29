using ChatLib.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatWpf.Common {
    public class Locator {
        public LoginViewModel LoginViewModel {
            get {
                return IocApp.Instance.Resolve<LoginViewModel>();
            }
        }

        public RegisterViewModel RegisterViewModel {
            get {
                return IocApp.Instance.Resolve<RegisterViewModel>();
            }
        }

        public FriendsListViewModel FriendsListViewModel {
            get {
                return IocApp.Instance.Resolve<FriendsListViewModel>();
            }
        }

        public AddNewFriendViewModel AddNewFriendViewModel {
            get {
                return IocApp.Instance.Resolve<AddNewFriendViewModel>();
            }
        }
        public MessagesListViewModel MessagesListViewModel {
            get {
                return IocApp.Instance.Resolve<MessagesListViewModel>();
            }
        }
    }
}
