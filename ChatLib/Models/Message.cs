using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoctoMvvm.Common;

namespace ChatLib.Models {
    public class Message : Bindable {
        private string _Content;

        public string Content {
            get {
                return _Content;
            }
            set {
                SetProperty(ref _Content, value);
            }
        }

        private DateTime _DeliveryTime;

        public DateTime DeliveryTime {
            get {
                return _DeliveryTime;
            }
            set {
                SetProperty(ref _DeliveryTime, value);
            }
        }

        private bool _IsIncoming;

        public bool IsIncoming {
            get {
                return _IsIncoming;
            }
            set {
                SetProperty(ref _IsIncoming, value);
            }
        }

        private string _OtherPartyUsername;

        public string OtherPartyUsername {
            get {
                return _OtherPartyUsername;
            }
            set {
                SetProperty(ref _OtherPartyUsername, value);
            }
        }
    }
}
