using System.Collections.Generic;
using Newtonsoft.Json;

namespace MicrosoftGraphOneDriveTest.Models
{
    public class ShareToEmailPayload
    {
        [JsonProperty("recipients")]
        public List<Recipient> Recipients { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("requireSignIn")]
        public bool RequireSignIn { get; set; }

        [JsonProperty("sendInvitation")]
        public bool SendInvitation { get; set; }

        [JsonProperty("roles")]    
        public List<string> Roles { get; set; }
    }
    
    public class Recipient
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }

}