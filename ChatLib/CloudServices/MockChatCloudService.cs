using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatLib.CloudServices {
    public class MockChatCloudService : IChatCloudService {
        public Task<bool> DoLogin(string userName, string password) {
            return Task.FromResult(true);
        }

    }
}
