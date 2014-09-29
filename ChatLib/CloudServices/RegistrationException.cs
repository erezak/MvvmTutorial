using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatLib.CloudServices {
    public class RegistrationException : Exception {
        public RegistrationException(string message)
            : base(message) {

        }
    }
}
