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
    public class RegisterViewModel : ViewModel {
        #region private members
        private IChatCloudService _ChatCloudService;
        private INavigationService _NavigationService;
        #endregion private members
        #region observable properties
        private string _FirstName;

        public string FirstName {
            get {
                return _FirstName;
            }
            set {
                SetProperty(ref _FirstName, value);
            }
        }
        private string _LastName;

        public string LastName {
            get {
                return _LastName;
            }
            set {
                SetProperty(ref _LastName, value);
            }
        }
        private string _UserName;

        public string UserName {
            get {
                return _UserName;
            }
            set {
                SetProperty(ref _UserName, value);
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

        private string _RegisterResult;

        public string RegisterResult {
            get {
                return _RegisterResult;
            }
            set {
                SetProperty(ref _RegisterResult, value);
            }
        }

        private bool _IsRegisterResultVisible;

        public bool IsRegisterResultVisible {
            get {
                return _IsRegisterResultVisible;
            }
            set {
                SetProperty(ref _IsRegisterResultVisible, value);
            }
        }

        #region commands
        private AsyncYoctoCommand _DoRegisterCommand;
        public AsyncYoctoCommand DoRegisterCommand {
            get {
                return _DoRegisterCommand;
            }
            protected set {
                SetProperty(ref _DoRegisterCommand, value);
            }
        }
        #endregion commands
        #endregion observable properties
        #region constructors
        public RegisterViewModel() {
            _UserName = "erez";
            _Password = "****";
            _PasswordVerify = "****";
            _RegisterResult = "Testing login result";
            _IsRegisterResultVisible = true;
        }
        [IocConstructor]
        public RegisterViewModel(IChatCloudService chatCloudService,
                                 INavigationService navigationService,
                                 MessageFetcher fetcher // to make sure it's created
                                 ) {
            _ChatCloudService = chatCloudService;
            _DoRegisterCommand = new AsyncYoctoCommand(DoRegisterAction, (a) => true);
            _NavigationService = navigationService;
        }

        #endregion constructors
        private async Task DoRegisterAction() {
            _DoRegisterCommand.Disable(null);
            if (string.IsNullOrWhiteSpace(_FirstName)) {
                RegisterResult = "Please enter your first name.";
                IsRegisterResultVisible = true;
            } else if (string.IsNullOrWhiteSpace(_LastName)) {
                RegisterResult = "Please enter your last name.";
                IsRegisterResultVisible = true;
            } else if (string.IsNullOrWhiteSpace(_UserName)) {
                RegisterResult = "Please enter a user name.";
                IsRegisterResultVisible = true;
            } else if (!_UserName.IsValidEmail()) {
                RegisterResult = "Please enter a valid email address.";
                IsRegisterResultVisible = true;
            } else if (string.IsNullOrWhiteSpace(_Password)) {
                RegisterResult = "Please enter a password.";
            } else if (string.IsNullOrEmpty(_PasswordVerify)){
                RegisterResult = "Please enter password again.";
            } else if (!_Password.Equals(_PasswordVerify)) {
                RegisterResult = "Please enter the same password again.";
            } else {
                IsRegisterResultVisible = false;
                bool didRegisterSucceed = false;
                try {
                    didRegisterSucceed = await _ChatCloudService.DoRegister(_UserName, _Password,
                                                                            _FirstName, _LastName);
                } catch (RegistrationException) {
                    IsRegisterResultVisible = true;
                    RegisterResult = string.Format("User {0} already exists.", _UserName);
                }
                if (didRegisterSucceed) {
                    IsRegisterResultVisible = false;
                    _NavigationService.NavigateReplaceAndKeepCurrent<FriendsListViewModel, LoginViewModel>();
                }
            }
            _DoRegisterCommand.Enable(null);
        }

    }
}
