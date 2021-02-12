using System;
using YIF.Core.Domain.DtoModels.IdentityDTO;

namespace YIF.Core.Domain.DtoModels
{
    public class TokenDTO
    {
        public string Id { get; set; }
        public virtual UserDTO User { get; set; }

        /// <summary>
        /// Get or set refresh token
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Get or set refresh token expiry time
        /// </summary>
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
