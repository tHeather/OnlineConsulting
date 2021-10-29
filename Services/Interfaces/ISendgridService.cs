using System.Threading.Tasks;

namespace OnlineConsulting.Services.Interfaces
{
    public interface ISendgridService
    {
        Task SendPasswordResetLink(string recipientMail, string resetLink);
        Task SendConfirmEmailAddressLink(string recipientMail, string confirmationLink);
    }
}
