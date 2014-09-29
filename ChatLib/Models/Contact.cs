using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoctoMvvm.Common;

namespace ChatLib.Models {
    public class Contact : Bindable {
        private string _Email;

        public string Email {
            get {
                return _Email;
            }
            set {
                SetProperty(ref _Email, value);
            }
        }

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

    }
}
