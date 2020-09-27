using System;

namespace Barracuda.Core.Authorization
{
    public class TokenResponseModel
    {
        public string Token { get; set; }

        public DateTime ValidTo { get; set; }
    }
}