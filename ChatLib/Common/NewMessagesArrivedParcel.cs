using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoctoMvvm.Courier;

namespace ChatLib.Common {
    public class NewMessagesArrivedParcel : Parcel {
        public IEnumerable<string> MessagesSenders {
            get;
            private set;
        }

        public NewMessagesArrivedParcel(object sender, IEnumerable<string> messagesSenders)
            : base(sender) {
            MessagesSenders = messagesSenders;
        }
    }
}
