using ChatLib.CloudServices;
using ChatLib.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoctoMvvm.Commands;
using YoctoMvvm.Common;
using YoctoMvvm.Platform;

namespace ChatLib.ViewModels {
    public class AddNewFriendViewModel : ViewModel {
        #region private members
        private IChatCloudService _ChatCloudService;
        private INavigationService _NavigationService;
        #endregion private members
        #region observable properties
        private ObservableCollection<Contact> _Contacts = new ObservableCollection<Contact>();

        public ObservableCollection<Contact> Contacts {
            get {
                return _Contacts;
            }
            set {
                SetProperty(ref _Contacts, value);
            }
        }
        private AsyncYoctoCommand _GetContactsCommand;

        public AsyncYoctoCommand GetContactsCommand {
            get {
                return _GetContactsCommand;
            }
            set {
                SetProperty(ref _GetContactsCommand, value);
            }
        }

        private AsyncYoctoCommand<Contact> _AddFriendCommand;

        public AsyncYoctoCommand<Contact> AddFriendCommand {
            get {
                return _AddFriendCommand;
            }
            set {
                SetProperty(ref _AddFriendCommand, value);
            }
        }

        private bool _ShouldShowList;

        public bool ShouldShowList {
            get {
                return _ShouldShowList;
            }
            set {
                SetProperty(ref _ShouldShowList, value);
            }
        }

        private string _AddFriendError;

        public string AddFriendError {
            get {
                return _AddFriendError;
            }
            set {
                SetProperty(ref _AddFriendError, value);
            }
        }

        private bool _ShouldShowAddFriendError;

        public bool ShouldShowAddFriendError {
            get {
                return _ShouldShowAddFriendError;
            }
            set {
                SetProperty(ref _ShouldShowAddFriendError, value);
            }
        }
        #endregion observable properties
        public AddNewFriendViewModel() {
            _ShouldShowList = true;
            var contacts = MockChatCloudService.UsersResult;
            foreach (var contact in contacts) {
                _Contacts.Add(contact);
            }
        }
        [IocConstructor]
        public AddNewFriendViewModel(IChatCloudService chatCloudService, INavigationService navigationService) {
            _ChatCloudService = chatCloudService;
            _NavigationService = navigationService;
            _GetContactsCommand = new AsyncYoctoCommand(GetContactsAction, (a) => true);
            _AddFriendCommand = new AsyncYoctoCommand<Contact>(AddFriendAction, (a) => true);
            _ShouldShowList = true;
        }

        private async Task GetContactsAction() {
            _AddFriendCommand.Disable(null);
            ShouldShowAddFriendError = false;
            var contacts = await _ChatCloudService.GetContacts();
            _Contacts.Clear();
            foreach (var contact in contacts) {
                _Contacts.Add(contact);
            }
            if (_Contacts.Count > 0) {
                ShouldShowList = true;
            } else {
                ShouldShowList = false;
            }
            _AddFriendCommand.Enable(null);
        }

        private async Task AddFriendAction(Contact contact) {
            if (contact != null) {
                try {
                    _AddFriendCommand.Disable(null);
                    ShouldShowAddFriendError = false;
                    await _ChatCloudService.AddNewFriend(contact.Email, contact.FirstName);
                    _NavigationService.Navigate<FriendsListViewModel>();
                } catch (FriendAlreadyExistsException) {
                    AddFriendError = string.Format("{0} is already your friend.", contact.FirstName);
                    ShouldShowAddFriendError = true;
                } catch (Exception e) {
                    Debug.WriteLine(e);
                }
                _AddFriendCommand.Enable(null);
            }
        }

    }
}
