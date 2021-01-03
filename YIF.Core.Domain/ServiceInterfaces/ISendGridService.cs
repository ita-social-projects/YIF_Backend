using System.Threading.Tasks;

namespace YIF.Core.Domain.ServiceInterfaces
{
    public interface IEmailService
    {
        Task<bool> Send(string email, string subject, string plainTextContent);
    }
}
