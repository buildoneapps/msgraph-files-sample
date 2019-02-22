using System.IO;
using Newtonsoft.Json;

namespace MicrosoftGraphOneDriveSample.Models.Payload
{
    public class CreateUploadSession
    {
        [JsonProperty("@microsoft.graph.conflictBehavior")]
        public string MicrosoftGraphConflictBehavior { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("fileSystemInfo")]
        public FileSystem FileSystemInfo { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class FileSystem
    {
        [JsonProperty("@odata.type")]
        public string DataType { get; set; }
    }
}