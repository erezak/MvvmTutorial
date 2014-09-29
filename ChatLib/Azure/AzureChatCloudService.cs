using ChatLib.Azure.Models;
using ChatLib.CloudServices;
using ChatLib.Models;
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
        public async Task<bool> DoRegister(string username,
            string password,
            string firstName, 
            string lastName) {
            try {
                var table = _MobileService.GetTable<AzureUser>();
                var normalizedUsername = username.ToLower();
                var existingUser = (await table.Take(1).Where(
                    u => u.Username == normalizedUsername).ToListAsync());
                if (existingUser.Count > 0) {
                    throw new RegistrationException("user already exists");
                }
                //Salt and hash the password
                var salt = _CryptoProvider.GetSecureRandomNumber();
                byte[] hash = _GetHash(password, salt);
                var newUser = new AzureUser() {
                    Username = normalizedUsername,
                    FirstName = firstName,
                    LastName = lastName,
                    Hash = BitConverter.ToString(hash),
                    Salt = BitConverter.ToString(salt)
                };
                await table.InsertAsync(newUser);
                _LoggedInUsername = normalizedUsername;
            } catch (RegistrationException r) {
                throw r;
            } catch (Exception e) {
                Debug.WriteLine(e);
                return false;
            }
            return true;
        }
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


        public async Task<IEnumerable<Friend>> GetFriends() {
            var friendsInCloudSource = _MobileService.GetTable<AzureFriend>();
            var friendsInCloud = await friendsInCloudSource.Where(
                f => f.Username == _LoggedInUsername).ToListAsync();
            var users = _MobileService.GetTable<AzureUser>();
            var friends = new List<Friend>();
            foreach (var friend in friendsInCloud.ToList()) {
                var user = (await users.Where(u => u.Username == friend.FriendUserName).ToListAsync()).FirstOrDefault();
                if (user != null) {
                    friends.Add(new Friend() {
                        Email = friend.FriendUserName,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        NickName = friend.Nickname
                    });
                } else {
                    //Friend is stale - remove it.
                    await friendsInCloudSource.DeleteAsync(friend);
                }
            }
            return friends;
        }
        public async Task<IEnumerable<Contact>> GetContacts() {
            var usersSource = _MobileService.GetTable<AzureUser>();
            var users = await usersSource.ToListAsync();
            var contacts = new List<Contact>();
            foreach (var user in users) {
                //Don't add yourself
                if (!user.Username.Equals(_LoggedInUsername)) {
                    contacts.Add(new Contact() {
                        Email = user.Username,
                        FirstName = user.FirstName,
                        LastName = user.LastName
                    });
                }
            }
            return contacts;
        }

        public async Task AddNewFriend(string friendUsername, string nickname) {
            var friendsSource = _MobileService.GetTable<AzureFriend>();
            var friend = (await friendsSource.Where(
                                        f => f.Username == _LoggedInUsername && f.FriendUserName == friendUsername)
                                        .ToListAsync()).FirstOrDefault();
            if (friend != null) {
                throw new FriendAlreadyExistsException();
            }

            friend = new AzureFriend() {
                Username = _LoggedInUsername,
                FriendUserName = friendUsername,
                Nickname = nickname
            };
            await friendsSource.InsertAsync(friend);
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


        public async Task<IEnumerable<Message>> CollectMessages(bool deleteOnConsume) {
            var messagesSource = _MobileService.GetTable<AzureMessage>();
            var messagesInCloud = await messagesSource.Where(
                                        m => m.ToUsername == _LoggedInUsername)
                                        .ToListAsync();
            List<Task> deletions = null;
            if (deleteOnConsume) {
                deletions = new List<Task>();
            }
            var messages = new List<Message>();
            foreach (var message in messagesInCloud) {
                messages.Add(new Message() {
                    Content = message.Content,
                    DeliveryTime = message.SentAt,
                    IsIncoming = true,
                    OtherPartyUsername = message.FromUsername
                });
                if (deleteOnConsume) {
                    //No await is used - we will wait on all the deletions in the end
                    var deletion = messagesSource.DeleteAsync(message);
                    deletions.Add(deletion);
                }
            }
            if (deleteOnConsume) {
                Task.WaitAll(deletions.ToArray());
            }
            return messages;
        }


        public async Task SendMessage(string toUsername, string content) {
            var messagesSource = _MobileService.GetTable<AzureMessage>();
            var message = new AzureMessage() {
                FromUsername = _LoggedInUsername,
                ToUsername = toUsername,
                Content = content
            };
            await messagesSource.InsertAsync(message);
        }
    }
}
