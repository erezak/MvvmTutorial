using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoctoMvvm.Courier;

namespace ChatLib.Common {
    public class SelectedContactParcel : Parcel {
        public string Username {
            get;
            private set;
        }
        public string Nickname {
            get;
            private set;
        }

        public SelectedContactParcel(object sender, string username, string nickname)
            : base(sender) {
            Username = username;
            Nickname = nickname;
        }
    }
}
