using ChatLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatLib.DataServices {
    public class InMemoryMessagesDataService : IMessagesDataService {
        private List<Message> _Messages = new List<Message>();
        public Task<IEnumerable<Message>> LoadMessagesWithOtherParty(string otherPartyUsername) {
            var messages = _Messages.Where(m => m.OtherPartyUsername == otherPartyUsername);
            return Task<IEnumerable<Message>>.FromResult(messages);
        }
        public Task SaveMessage(Message message) {
            _Messages.Add(message);
            return Task.FromResult(true);
        }
        public Task SaveMessages(IEnumerable<Message> messages) {
            foreach (var message in messages) {
                _Messages.Add(message);
            }
            return Task.FromResult(true);
        }
    }
}
