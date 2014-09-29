using ChatLib.Azure;
using ChatLib.CloudServices;
using ChatLib.ViewModels;
using YoctoMvvm.Courier;
using YoctoMvvm.Platform;
using YoctoMvvmWpf.Common;
using YoctoMvvmWpf.Platform;

namespace ChatWpf.Common {
    internal class IocApp : IocWpf {
        protected override void RegisterAll() {
            Register<INavigationMappingProvider, NavigationMappingProvider>();
            Register<INavigationService, NavigationServiceWpf>();
            Register<IDispatcher, Dispatcher>();
            if (IsInDesignMode) {
                Register<IChatCloudService, MockChatCloudService>();
            } else {
                Register<ICryptoProvider, CryptoProvider>();
                Register<IChatCloudService, AzureChatCloudService>();
            }
            Register<LoginViewModel>();
        }
        #region singleton
        private IocApp() {
        }

        public static IocApp Instance {
            get {
                return ForSingleton.instance;
            }
        }

        class ForSingleton {
            // Explicit static constructor to tell TClass# compiler
            // not to mark type as beforefieldinit
            static ForSingleton() {
            }

            internal static readonly IocApp instance = new IocApp();
        }
        #endregion singleton
    }
}
