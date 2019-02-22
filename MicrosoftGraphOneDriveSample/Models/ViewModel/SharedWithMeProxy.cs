using Newtonsoft.Json;

namespace MicrosoftGraphOneDriveSample.Models
{
    public class SharedWithMeProxy
    {
        [JsonProperty("value")]
        public SharedFileViewModel[] Files { get; set; }
    }
    
    public class SharedFileViewModel
    {
        public string Id { get; set; }
    
        [JsonProperty("remoteItem")]
        public FileViewModel File { get; set; }
    }
}

