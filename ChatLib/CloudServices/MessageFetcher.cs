using ChatLib.Common;
using ChatLib.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoctoMvvm.Courier;

namespace ChatLib.CloudServices {
    public class MessageFetcher {
        #region private members
        private IChatCloudService _ChatCloudService;
        private IMessagesDataService _MessagesDataService;
        private ICourier _Courier;
        #endregion private members
        #region constructors
        public MessageFetcher(IChatCloudService chatCloudService,
                              IMessagesDataService messagesDataService,
                              ICourier courier) {
            _ChatCloudService = chatCloudService;
            _MessagesDataService = messagesDataService;
            _Courier = courier;
            _Courier.SubscribeForParcelType<LoggedInParcel>(PollForMessages);
        }
        #endregion constructors

        private void PollForMessages(LoggedInParcel parcel) {
            var repeat = RepeatingTaskFactory.Start(
                action: async () => {
                    await FetchMessages();
                },
                intervalInMilliseconds: 1000 * 15,
                maxIterations: 4 * 10
            );
        }

        public async Task FetchMessages() {
            var messages = await _ChatCloudService.CollectMessages(deleteOnConsume: true);
            if (messages != null && messages.Count() > 0) {
                var savingDone = _MessagesDataService.SaveMessages(messages);
                var senders = new List<string>();
                foreach (var message in messages) {
                    if (!(senders.Contains(message.OtherPartyUsername))) {
                        senders.Add(message.OtherPartyUsername);
                    }
                }
                var parcel = new NewMessagesArrivedParcel(this, senders);
                _Courier.Publish<NewMessagesArrivedParcel>(parcel);
                await savingDone;
            }
        }
    }
}
