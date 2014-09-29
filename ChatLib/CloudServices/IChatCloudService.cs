using ChatLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatLib.CloudServices {
    public interface IChatCloudService {
        Task<bool> DoRegister(string userName,
            string password,
            string firstName,
            string lastName);
        Task<bool> DoLogin(string userName, string password);
        Task<IEnumerable<Friend>> GetFriends();

        Task<IEnumerable<Contact>> GetContacts();
        Task AddNewFriend(string friendUsername, string nickname);
        Task<IEnumerable<Message>> CollectMessages(bool deleteOnConsume);
        Task SendMessage(string toUsername, string content);
    }
}
