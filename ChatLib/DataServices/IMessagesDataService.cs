using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ChatLib.DataServices {
    public interface IMessagesDataService {
        Task<System.Collections.Generic.IEnumerable<Models.Message>> LoadMessagesWithOtherParty(string otherPartyUsername);
        Task SaveMessage(Models.Message message);
        Task SaveMessages(IEnumerable<Models.Message> messages);
    }
}
