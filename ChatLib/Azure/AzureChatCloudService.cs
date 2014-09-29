using ChatLib.Azure.Models;
using ChatLib.CloudServices;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoctoMvvm.Platform;

namespace ChatLib.Azure {
    public class AzureChatCloudService : IChatCloudService {
        #region private members
        private static MobileServiceClient _MobileService = new MobileServiceClient(
            "https://chatfortraining.azure-mobile.net/",
            "HDzmxAIibPaouAwBDNdXvNckFhYOBI52"
        );
        private ICryptoProvider _CryptoProvider;
        private string _LoggedInUsername;
        #endregion private members

        #region constructors
        public AzureChatCloudService(ICryptoProvider cryptoProvider) {
            _CryptoProvider = cryptoProvider;
            _LoggedInUsername = null;
        }
        #endregion constructors

        #region IChatCloudService
        public async Task<bool> DoLogin(string username, string password) {
            var table = _MobileService.GetTable<AzureUser>();
            var normalizedUsername = username.ToLower();
            var existingUser = (await table.Take(1).Where(
                u => u.Username == normalizedUsername).ToListAsync()).FirstOrDefault();
            if (existingUser == null) {
                return false;
            }
            var salt = _DecodeSalt(existingUser.Salt);
            var hash = _GetHash(password, salt);
            if (BitConverter.ToString(hash).Equals(existingUser.Hash)) {
                _LoggedInUsername = normalizedUsername;
                return true;
            }
            return false;
        }

        #endregion IChatCloudService

        private byte[] _DecodeSalt(string codedSalt) {


            string[] raw = codedSalt.Split('-');
            byte[] result = new byte[raw.Length];
            for (int i = 0; i < raw.Length; i++) {
                result[i] = Convert.ToByte(raw[i], 16);

            }
            return result;
        }

        private byte[] _GetHash(string preHash, byte[] salt) {
            byte[] passwordData = Encoding.UTF8.GetBytes(preHash);

            byte[] composite = new byte[passwordData.Length + 32];

            Array.Copy(passwordData, composite, passwordData.Length);
            Array.Copy(salt, 0, composite, passwordData.Length, salt.Length);

            byte[] hash = _CryptoProvider.GetHash(composite);
            return hash;
        }

    }
}
