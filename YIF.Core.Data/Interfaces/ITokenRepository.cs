using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YIF.Core.Data.Entities.IdentityEntities;

namespace YIF.Core.Data.Interfaces
{
    public interface ITokenRepository
    {
        Task<bool> UpdateUserToken(DbUser user, string refreshToken);
    }
}
