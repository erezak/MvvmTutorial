using ChatLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatLib.CloudServices {
    public class MockChatCloudService : IChatCloudService {
        #region mock data
        #region raw data
        #region Friends List
        private static List<Friend> _FriendsResult = new List<Friend>
            {
                new Friend {
                    Email = "friend7@example.com",
                    FirstName = "Gordon",
                    LastName = "Gurizky",
                    NickName = "Gordi"
                },
                new Friend {
                    Email = "friend2@example.com",
                    FirstName = "Robert",
                    LastName = "Bonanza",
                    NickName = "Bob"
                },
                new Friend{
                    Email = "friend3@example.com",
                    FirstName = "Caroline",
                    LastName = "Caruso",
                    NickName = "Carol"
                }
            };
        #endregion Friends List
        #region Users List
        private static List<Contact> _UsersResult = new List<Contact>
            {
                new Contact {
                    Email = "friend1@example.com",
                    FirstName = "Alice",
                    LastName = "Alonso"
                },
                new Contact {
                    Email = "friend2@example.com",
                    FirstName = "Robert",
                    LastName = "Bonanza"
                },
                new Contact{
                    Email = "friend3@example.com",
                    FirstName = "Caroline",
                    LastName = "Caruso"
                },
                new Contact {
                    Email = "friend4@example.com",
                    FirstName = "David",
                    LastName = "Derry"
                },
                new Contact {
                    Email = "friend5@example.com",
                    FirstName = "Erin",
                    LastName = "Etching"
                },
                new Contact{
                    Email = "friend6@example.com",
                    FirstName = "Francis",
                    LastName = "Ford"
                },
                new Contact {
                    Email = "friend7@example.com",
                    FirstName = "Gordon",
                    LastName = "Gurizky"
                },
                new Contact {
                    Email = "friend8@example.com",
                    FirstName = "Howard",
                    LastName = "Hones"
                },
                new Contact{
                    Email = "friend9@example.com",
                    FirstName = "Ines",
                    LastName = "Ire"
                },
                new Contact {
                    Email = "friend10@example.com",
                    FirstName = "James",
                    LastName = "Jordan"
                },
                new Contact{
                    Email = "friend11@example.com",
                    FirstName = "Kim",
                    LastName = "Kord"
                }
            };
        #endregion Users List
        #region Messages List
        private static List<Message> _MessagesResult = new List<Message> {
            new Message() {
                Content = "Hi, what's up? I'll be on my way soon.",
                DeliveryTime = DateTime.Now.AddSeconds(-400),
                IsIncoming = true
            },
            new Message() {
                Content = "All's well. I've already started dinner. A sec, please. brb.",
                DeliveryTime = DateTime.Now.AddSeconds(-340),
                IsIncoming = false
            },
            new Message() {
                Content = "I'm back. Just had to flip the pancake. It should be ready by the time you're in.",
                DeliveryTime = DateTime.Now.AddSeconds(-300),
                IsIncoming = false
            },
            new Message() {
                Content = "Great, can't wait. Thanks.",
                DeliveryTime = DateTime.Now.AddSeconds(-30),
                IsIncoming = true
            }
        };
        #endregion Messages List
        #endregion raw data
        public static List<Friend> FriendsResult {
            get {
                return _FriendsResult;
            }
        }
        public static List<Contact> UsersResult {
            get {
                return _UsersResult;
            }
        }
        public static List<Message> MessagesResult {
            get {
                return _MessagesResult;
            }
        }
        #endregion mock data
        private bool _IsLoggedIn;
        public Task<bool> DoRegister(string userName,
            string password,
            string firstName,
            string lastName) {
            return Task.FromResult(true);
        }
        public Task<bool> DoLogin(string userName, string password) {
            _IsLoggedIn = true;
            return Task.FromResult(true);
        }

        public Task<IEnumerable<Models.Friend>> GetFriends() {
            if (!_IsLoggedIn) {
                throw new AuthenticationException("Not logged in");
            }
            return Task.FromResult<IEnumerable<Models.Friend>>(_FriendsResult);
        }
        public Task<IEnumerable<Models.Contact>> GetContacts() {
            if (!_IsLoggedIn) {
                throw new AuthenticationException("Not logged in");
            }
            return Task.FromResult<IEnumerable<Models.Contact>>(_UsersResult);
        }

        public Task AddNewFriend(string friendUsername, string nickname) {
            return Task.FromResult(true);
        }

        public Task<IEnumerable<Models.Message>> CollectMessages(bool deleteOnConsume) {
            return Task.FromResult<IEnumerable<Models.Message>>(_MessagesResult);
        }


        public Task SendMessage(string toUsername, string content) {
            return Task.FromResult(true);
        }
    }
}
