using YIF.Core.Data.Entities.IdentityEntities;

namespace YIF.Core.Domain.ServiceInterfaces
{
    public interface IJwtService
    {
        string CreateTokenByUser(DbUser user);
    }
}
