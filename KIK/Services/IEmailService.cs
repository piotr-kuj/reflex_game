using System.Threading.Tasks;

namespace KIK.Services
{
    public interface IEmailService
    {
        Task SendEmail(string emailTo, string subject, string msg);
    }
}