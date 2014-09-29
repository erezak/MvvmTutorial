using ChatLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ChatWpf.Views {
    public class MessagesTemplateSelector : DataTemplateSelector {
        public DataTemplate IncomingMessageTemplate { get; set; }
        public DataTemplate OutgoingMessageCaptureTemplate { get; set; }
        public DataTemplate EmptyTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container) {
            var message = item as Message;
            if (message != null) {
                return (message.IsIncoming) ? IncomingMessageTemplate : OutgoingMessageCaptureTemplate;
            }

            return EmptyTemplate;
        }
    }
}
