using ChatLib.Common;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using YoctoMvvm.Commands;

namespace ChatLib.ViewModels {
    public class LoginViewModel : Bindable {
        #region properties
        private String _Username;

        public String Username {
            get {
                return _Username;
            }
            set {
                SetProperty(ref _Username, value);
                //Also update the IsLoginEnabled flag, which affects the command CanExecute value
                if (string.IsNullOrEmpty(value)) {
                    IsLoginEnabled = false;
                } else {
                    IsLoginEnabled = true;
                }
                //Alert the UI systen that the command changed
                _LoginCommand.RaiseCanExecuteChanged();
            }
        }

        private String _Password;

        public String Password {
            get {
                return _Password;
            }
            set {
                SetProperty(ref _Password, value);
            }
        }

        private String _LoginResult;

        public String LoginResult {
            get {
                return _LoginResult;
            }
            set {
                SetProperty(ref _LoginResult, value);
            }
        }

        private bool _IsLoginEnabled;

        public bool IsLoginEnabled {
            get {
                return _IsLoginEnabled;
            }
            set {
                SetProperty(ref _IsLoginEnabled, value);
            }
        }

        private AsyncYoctoCommand _LoginCommand;

        public AsyncYoctoCommand LoginCommand {
            get {
                return _LoginCommand;
            }
            set {
                SetProperty(ref _LoginCommand, value);
            }
        }

        private AsyncYoctoCommand _RegisterCommand;

        public AsyncYoctoCommand RegisterCommand {
            get {
                return _RegisterCommand;
            }
            set {
                SetProperty(ref _RegisterCommand, value);
            }
        }

        #endregion properties

        public LoginViewModel() {
            _IsLoginEnabled = false;
            _LoginCommand = new AsyncYoctoCommand(_LoginAction, (a) => {
                return _IsLoginEnabled;
            });
            _RegisterCommand = new AsyncYoctoCommand(_RegisterAction, (a) => {
                return true;
            });

        }
        #region actions for commands

        private async Task _LoginAction() {
            if (string.IsNullOrWhiteSpace(_Username)) {
                LoginResult = "Please enter a user name.";
            } else if (string.IsNullOrWhiteSpace(_Password)) {
                LoginResult = "Please enter a password.";
            } else {
                LoginResult = "Login executed";
                await Task.Delay(2000);
                LoginResult = "";
            }
        }

        private async Task _RegisterAction() {
            LoginResult = "Register called";
            await Task.Delay(2000);
            LoginResult = "";
        }

        #endregion actions for commands
    }
}
