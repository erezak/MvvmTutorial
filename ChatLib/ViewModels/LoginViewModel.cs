using ChatLib.Common;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatLib.ViewModels {
    public class LoginViewModel : Bindable {
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

        private Command _LoginCommand;

        public Command LoginCommand {
            get {
                return _LoginCommand;
            }
            set {
                SetProperty(ref _LoginCommand, value);
            }
        }
        public LoginViewModel() {
            _IsLoginEnabled = false;
            _LoginCommand = new Command(_LoginAction, () => {
                return _IsLoginEnabled;
            });
        }

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

        public class Command : ICommand {

            Func<bool> _CanExecute;
            Func<Task> _ActionToExecute;

            public Command(Func<Task> action, Func<bool> canExecute) {
                _CanExecute = canExecute;
                _ActionToExecute = action;
            }
            public bool CanExecute(object parameter) {
                return _CanExecute();
            }

            public event EventHandler CanExecuteChanged;

            public void RaiseCanExecuteChanged() {
                if (CanExecuteChanged != null) {
                    CanExecuteChanged(this, null);
                }
            }

            public void Execute(object parameter) {
                if (_CanExecute()) {
                    _ActionToExecute();
                }
            }

        }
    }
}
