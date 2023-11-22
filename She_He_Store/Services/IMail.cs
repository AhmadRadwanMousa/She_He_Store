using She_He_Store.Models;

namespace She_He_Store.Services
{
    public interface IMail
    {
       void  SendEmail(MailData mailData);
    }

}
