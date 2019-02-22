using System;
using Microsoft.Extensions.Primitives;

namespace MicrosoftGraphOneDriveSample.Models
{
    public class UserInfoViewModel
    {
        public Guid Id { get; set; }
        public string AccessToken { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}