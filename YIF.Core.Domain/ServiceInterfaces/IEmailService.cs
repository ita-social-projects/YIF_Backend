using System.Threading.Tasks;

namespace YIF.Core.Domain.ServiceInterfaces
{
    public interface IEmailService
    {
        Task<bool> SendAsync(string email, string subject, string content);
    }
}
