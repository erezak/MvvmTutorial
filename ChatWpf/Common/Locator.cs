using ChatLib.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatWpf.Common {
    public class Locator {
        public LoginViewModel LoginViewModel {
            get {
                return IocApp.Instance.Resolve<LoginViewModel>();
            }
        }
    }
}
