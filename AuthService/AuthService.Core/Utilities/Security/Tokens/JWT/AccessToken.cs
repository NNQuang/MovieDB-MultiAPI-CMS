using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Core.Utilities.Security.Tokens.JWT
{
    public class AccessToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
