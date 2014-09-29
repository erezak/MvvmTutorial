using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatLib.CloudServices {
    class AuthenticationException : Exception {
        public AuthenticationException()
            : base() {
        }
        public AuthenticationException(string message)
            : base(message) {
        }
    }
}
