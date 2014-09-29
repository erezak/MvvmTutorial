using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatLib.Azure.Models {
    [DataTable("Friends")]
    public class AzureFriend {
        public string Id { get; set; }
        public string Username { get; set; }
        public string FriendUserName { get; set; }
        public string Nickname { get; set; }
        [JsonProperty("__createdAt")]
        public DateTime Creation { get; set; }
    }
}
