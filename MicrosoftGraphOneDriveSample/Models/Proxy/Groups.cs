using System.Collections.Generic;
using Newtonsoft.Json;

namespace MicrosoftGraphOneDriveTest.Models
{
    public class Groups
    {
        [JsonProperty("value")]
        public List<Group> Value { get; set; }
    }
    
    public class Group
    {
        [JsonProperty("@odata.type")]
        public string OdataType { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("createdDateTime")]
        public object CreatedDateTime { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("groupTypes")]
        public object[] GroupTypes { get; set; }

        [JsonProperty("mailEnabled")]
        public bool MailEnabled { get; set; }

        [JsonProperty("securityEnabled")]
        public bool SecurityEnabled { get; set; }
    }
}