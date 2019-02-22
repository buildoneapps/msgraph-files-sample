using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class RootFileProxy
{

    [JsonProperty("value")]
    public FileViewModel[] Files { get; set; }
}

public class FileViewModel
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("size")]
    public long Size { get; set; }

    public string WebUrl { get; set; }
    public string Id { get; set; }
}


