using ChatLib.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoctoMvvmWpf.Platform;

namespace ChatWpf.Common {
    class NavigationMappingProvider : INavigationMappingProvider {
        public Dictionary<Type, string> ProvideViewModelToViewMap() {
            var mapping = new Dictionary<Type, string> {
                                                     };
            return mapping;
        }
    }
}
