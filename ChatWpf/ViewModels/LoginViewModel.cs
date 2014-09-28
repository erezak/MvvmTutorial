using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                if (string.IsNullOrEmpty(value)) {
                    IsLoginEnabled = false;
                } else {
                    IsLoginEnabled = true;
                }
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

        public LoginViewModel() {
            _Username = "Test username";
            _IsLoginEnabled = true;
        }
    }
}
