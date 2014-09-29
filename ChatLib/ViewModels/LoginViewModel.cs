using ChatLib.Azure;
using ChatLib.CloudServices;
using ChatLib.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoctoMvvm.Commands;
using YoctoMvvm.Common;
using YoctoMvvm.Courier;
using YoctoMvvm.Platform;

namespace ChatLib.ViewModels {
    public class LoginViewModel : ViewModel {
        #region private members
        private IChatCloudService _ChatCloudService;
        private INavigationService _NavigationService;
        private ICourier _Courier;
        #endregion private members
        #region observable properties
        private string _Username;
        public string Username {
            get {
                return _Username;
            }
            set {
                SetProperty(ref _Username, value);
            }
        }
        private string _Password;

        public string Password {
            get {
                return _Password;
            }
            set {
                SetProperty(ref _Password, value);
            }
        }

        private string _PasswordVerify;

        public string PasswordVerify {
            get {
                return _PasswordVerify;
            }
            set {
                SetProperty(ref _PasswordVerify, value);
            }
        }

        private string _LoginResult;

        public string LoginResult {
            get {
                return _LoginResult;
            }
            set {
                SetProperty(ref _LoginResult, value);
            }
        }

        private bool _IsLoginResultVisible;

        public bool IsLoginResultVisible {
            get {
                return _IsLoginResultVisible;
            }
            set {
                SetProperty(ref _IsLoginResultVisible, value);
            }
        }

        #region commands
        private AsyncYoctoCommand _DoLoginCommand;
        public AsyncYoctoCommand DoLoginCommand {
            get {
                return _DoLoginCommand;
            }
            protected set {
                SetProperty(ref _DoLoginCommand, value);
            }
        }

        private YoctoCommand _DoRegisterCommand;

        public YoctoCommand DoRegisterCommand {
            get {
                return _DoRegisterCommand;
            }
            set {
                SetProperty(ref _DoRegisterCommand, value);
            }
        }
        #endregion commands
        #endregion observable properties
        #region constructors
        public LoginViewModel() {
            _Username = "erez";
            _Password = "****";
            _LoginResult = "Testing login result";
            _IsLoginResultVisible = true;
        }
        [IocConstructor]
        public LoginViewModel(IChatCloudService chatCloudService, 
                                        INavigationService navigationService,
                                    ICourier courier,
                                    MessageFetcher fetcher // to make sure it's created
                                                            ) {
            _ChatCloudService = chatCloudService;
            _DoLoginCommand = new AsyncYoctoCommand(DoLoginAction, (a) => true);
            _DoRegisterCommand = new YoctoCommand(_DoRegisterAction, () => true);
            _NavigationService = navigationService;
            _Courier = courier;
        }

        #endregion constructors
        private async Task DoLoginAction() {
            _DoLoginCommand.Disable(null);
            if (string.IsNullOrWhiteSpace(_Username)) {
                LoginResult = "Please enter a user name.";
                IsLoginResultVisible = true;
            } else if (string.IsNullOrWhiteSpace(_Password)) {
                LoginResult = "Please enter a password.";
                IsLoginResultVisible = true;
            } else {
                IsLoginResultVisible = false;
                var didLoginSucceed = await _ChatCloudService.DoLogin(_Username, _Password);
                if (didLoginSucceed) {
                    _NavigationService.NavigateReplaceAndKeepCurrent<FriendsListViewModel, LoginViewModel>();
                    var parcel = new LoggedInParcel(this);
                    _Courier.Publish<LoggedInParcel>(parcel);
                } else {
                    LoginResult = "Wrong credentials.";
                    IsLoginResultVisible = true;
                }
            }
            _DoLoginCommand.Enable(null);
        }

        private void _DoRegisterAction() {
            _NavigationService.Navigate<RegisterViewModel>();
        }

    }
}
