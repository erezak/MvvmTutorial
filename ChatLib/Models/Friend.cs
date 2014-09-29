using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoctoMvvm.Common;

namespace ChatLib.Models {
    public class Friend : Contact {
        private string _NickName;

        public string NickName {
            get {
                return _NickName;
            }
            set {
                SetProperty(ref _NickName, value);
            }
        }
    }
}
