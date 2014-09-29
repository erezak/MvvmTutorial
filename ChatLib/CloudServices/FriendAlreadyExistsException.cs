using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatLib.CloudServices {
    class FriendAlreadyExistsException : Exception {
        public FriendAlreadyExistsException()
            : base() {
        }
        public FriendAlreadyExistsException(string message)
            : base(message) {
        }
    }
}
