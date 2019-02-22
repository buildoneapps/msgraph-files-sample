using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class MeProxy
{

    [JsonProperty("businessPhones")]
    public object[] BusinessPhones { get; set; }

    [JsonProperty("displayName")]
    public string DisplayName { get; set; }

    [JsonProperty("givenName")]
    public string GivenName { get; set; }

    [JsonProperty("jobTitle")]
    public string JobTitle { get; set; }

    [JsonProperty("mail")]
    public string Mail { get; set; }

    [JsonProperty("mobilePhone")]
    public string MobilePhone { get; set; }

    [JsonProperty("officeLocation")]
    public string OfficeLocation { get; set; }

    [JsonProperty("preferredLanguage")]
    public string PreferredLanguage { get; set; }

    [JsonProperty("surname")]
    public string Surname { get; set; }

    [JsonProperty("userPrincipalName")]
    public string UserPrincipalName { get; set; }

    [JsonProperty("id")]
    public Guid Id { get; set; }
}