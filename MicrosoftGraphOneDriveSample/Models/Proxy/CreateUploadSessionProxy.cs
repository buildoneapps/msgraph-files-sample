using System;
using Newtonsoft.Json;

namespace MicrosoftGraphOneDriveSample.Models
{
    public class CreateUploadSessionProxy
    {
        [JsonProperty("uploadUrl")]
        public string UploadUrl { get; set; }

        [JsonProperty("expirationDateTime")]
        public DateTimeOffset ExpirationDateTime { get; set; }
    }
}