using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatLib.Azure.Models {
    [DataTable("Messages")]
    public class AzureMessage {
        public string Id { get; set; }
        public string FromUsername { get; set; }
        public string ToUsername { get; set; }
        public string Content { get; set; }
        [JsonProperty("__createdAt")]
        public DateTime SentAt { get; set; }

    }
}
