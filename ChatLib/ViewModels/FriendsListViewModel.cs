using ChatLib.CloudServices;
using ChatLib.Common;
using ChatLib.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoctoMvvm.Commands;
using YoctoMvvm.Common;
using YoctoMvvm.Courier;
using YoctoMvvm.Platform;

namespace ChatLib.ViewModels {
    public class FriendsListViewModel : ViewModel {
        #region private members
        private IChatCloudService _ChatCloudService;
        private INavigationService _NavigationService;
        private ICourier _Courier;
        private IDispatcher _Dispatcher;
        #endregion private members
        #region observable properties
        private ObservableCollection<Friend> _Friends = new ObservableCollection<Friend>();

        public ObservableCollection<Friend> Friends {
            get {
                return _Friends;
            }
            set {
                SetProperty(ref _Friends, value);
            }
        }
        private AsyncYoctoCommand _GetFriendsCommand;

        public AsyncYoctoCommand GetFriendsCommand {
            get {
                return _GetFriendsCommand;
            }
            set {
                SetProperty(ref _GetFriendsCommand, value);
            }
        }
        private AsyncYoctoCommand _AddNewFriendCommand;

        public AsyncYoctoCommand AddNewFriendCommand {
            get {
                return _AddNewFriendCommand;
            }
            set {
                SetProperty(ref _AddNewFriendCommand, value);
            }
        }

        private YoctoCommand<Friend> _SelectedFriendCommand;

        public YoctoCommand<Friend> SelectedFriendCommand {
            get {
                return _SelectedFriendCommand;
            }
            set {
                SetProperty(ref _SelectedFriendCommand, value);
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
        
        #endregion observable properties
        public FriendsListViewModel() {
            var friends = MockChatCloudService.FriendsResult;
            foreach (var friend in friends) {
                _Friends.Add(friend);
            }
            _ShouldShowList = true;
        }
        [IocConstructor]
        public FriendsListViewModel(IChatCloudService chatCloudService,
                                    INavigationService navigationService,
                                    ICourier courier,
                                    IDispatcher dispatcher) {
            _ChatCloudService = chatCloudService;
            _NavigationService = navigationService;
            _Dispatcher = dispatcher;
            _GetFriendsCommand = new AsyncYoctoCommand(GetFriendsAction, (a) => true);
            _AddNewFriendCommand = new AsyncYoctoCommand(AddNewFriendAction, (a) => true);
            _ShouldShowList = true;
            _Courier = courier;
            _SelectedFriendCommand = new YoctoCommand<Friend>(SelectedFriendAction, () => true);
        }

        private void SelectedFriendAction(Friend friend) {
            if (friend != null) {
                _NavigationService.Navigate<MessagesListViewModel>();
                var parcel = new SelectedContactParcel(this, friend.Email, friend.NickName);
                _Courier.Publish<SelectedContactParcel>(parcel);
                //Also - in case the consumer was not yet created
                _Courier.DepositInMailbox<SelectedContactParcel>(parcel);
            }
        }

        private async Task GetFriendsAction() {
            var friends = await _ChatCloudService.GetFriends();
            _SelectedFriendCommand.Disable();
            _Friends.Clear();
            foreach (var friend in friends) {
                _Friends.Add(friend);
            }
            _SelectedFriendCommand.Enable();
            if (_Friends.Count > 0) {
                ShouldShowList = true;
            } else {
                ShouldShowList = false;
            }
        }
        private async Task AddNewFriendAction() {
            await Task.Delay(1);
            _NavigationService.Navigate<AddNewFriendViewModel>();
        }

    }
}
