using ChatLib.CloudServices;
using ChatLib.Common;
using ChatLib.DataServices;
using ChatLib.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using YoctoMvvm.Commands;
using YoctoMvvm.Common;
using YoctoMvvm.Courier;
using YoctoMvvm.Platform;

namespace ChatLib.ViewModels {
    public class MessagesListViewModel : ViewModel {
        #region private members
        private string _OtherPartyUsername;
        private ICourier _Courier;
        private IChatCloudService _ChatCloudService;
        private IMessagesDataService _MessagesDataService;
        private IDispatcher _Dispatcher;
        #endregion private members

        public string OtherPartyUsername {
            get {
                return _OtherPartyUsername;
            }
            set {
                SetProperty(ref _OtherPartyUsername, value);
            }
        }

        private string _OtherPartyNickname;

        public string OtherPartyNickname {
            get {
                return _OtherPartyNickname;
            }
            set {
                SetProperty(ref _OtherPartyNickname, value);
            }
        }

        private ObservableCollection<Message> _Messages;
        public ObservableCollection<Message> Messages {
            get {
                return _Messages;
            }
            set {
                SetProperty(ref _Messages, value);
            }
        }

        private string _OutgoingMessage;

        public string OutgoingMessage {
            get {
                return _OutgoingMessage;
            }
            set {
                SetProperty(ref _OutgoingMessage, value);
            }
        }

        private AsyncYoctoCommand _SendMessageCommand;

        public AsyncYoctoCommand SendMessageCommand {
            get {
                return _SendMessageCommand;
            }
            set {
                SetProperty(ref _SendMessageCommand, value);
            }
        }

        private AsyncYoctoCommand _CollectMessagesCommand;

        public AsyncYoctoCommand CollectMessagesCommand {
            get {
                return _CollectMessagesCommand;
            }
            set {
                SetProperty(ref _CollectMessagesCommand, value);
            }
        }
        #region constructors
        public MessagesListViewModel() {
            _Messages = new ObservableCollection<Message>();
            var messages = MockChatCloudService.MessagesResult;
            foreach (var message in messages) {
                _Messages.Add(message);
            }
            _OtherPartyNickname = "Erez";
            _OtherPartyUsername = "erakorn@outlook.com";
        }
        [IocConstructor]
        public MessagesListViewModel(IChatCloudService chatCloudService,
                                     ICourier courier,
                                     IMessagesDataService messagesDataService,
                                     IDispatcher dispatcher) {
            _Courier = courier;
            _ChatCloudService = chatCloudService;
            _MessagesDataService = messagesDataService;
            _Messages = new ObservableCollection<Message>();
            _Dispatcher = dispatcher;
            _Courier.SubscribeForParcelType<SelectedContactParcel>(_NewContactSelected);
            _SendMessageCommand = new AsyncYoctoCommand(SendMessageAction, (a) => true);
            _CollectMessagesCommand = new AsyncYoctoCommand(CollectMessagesAction, (a) => true);
            _Courier.SubscribeForParcelType<NewMessagesArrivedParcel>(NewMessagesArrived,
                                                        filter: FilterNewMessagesParcels);
        }

        private async void NewMessagesArrived(NewMessagesArrivedParcel parcel) {
            await CollectMessagesAction();
        }

        private bool FilterNewMessagesParcels(NewMessagesArrivedParcel parcel) {
            var senders = parcel.MessagesSenders;
            if (senders.Contains(_OtherPartyUsername)) {
                return true;
            } else {
                return false;
            }
        }

        private async Task CollectMessagesAction() {
            var messages = await _MessagesDataService.LoadMessagesWithOtherParty(_OtherPartyUsername);
            _Dispatcher.RunOnUiThread(() => {
                _Messages.Clear();
                foreach (var message in messages) {
                    _Messages.Add(message);
                }
            });
        }

        private async Task SendMessageAction() {
            var message = new Message() {
                Content = _OutgoingMessage,
                DeliveryTime = DateTime.Now,
                IsIncoming = false,
                OtherPartyUsername = _OtherPartyUsername
            };
            await _ChatCloudService.SendMessage(_OtherPartyUsername, _OutgoingMessage).ConfigureAwait(continueOnCapturedContext: false);
            await _MessagesDataService.SaveMessage(message).ConfigureAwait(continueOnCapturedContext: false);
            _Dispatcher.RunOnUiThread(() => {
                _Messages.Add(message);
                OutgoingMessage = null;
            });
        }

        private void _NewContactSelected(SelectedContactParcel selectedContact) {
            _Dispatcher.RunOnUiThread(() => {
                OtherPartyNickname = selectedContact.Nickname;
                OtherPartyUsername = selectedContact.Username;
            });
        }
        #endregion constructors

    }
}
