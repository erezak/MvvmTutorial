using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using YoctoMvvm.Common;

namespace ChatWpf.ViewModels {
    class LoginViewModel : Bindable {
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

        private void _LoginAction() {
            Username = "Command executed";
        }

        public class Command : ICommand {

            Func<bool> _CanExecute;
            Action _ActionToExecute;

            public Command(Action action, Func<bool> canExecute) {
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
