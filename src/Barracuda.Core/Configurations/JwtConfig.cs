namespace Barracuda.Core.Configurations
{
    public class JwtConfig
    {
        /// <summary>
        /// Who has issued the token
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Who is the token intended for
        /// </summary>
        public string Audience { get; set; }

        public string SecureKey { get; set; }

        /// <summary>
        /// The token expiry time
        /// </summary>
        public int ExpiryInMinutes { get; set; }
    }
}