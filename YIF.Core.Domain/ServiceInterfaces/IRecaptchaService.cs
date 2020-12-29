namespace YIF.Core.Domain.ServiceInterfaces
{
    public interface IRecaptchaService
    {
        bool IsValid(string recaptchaToken);
    }
}
