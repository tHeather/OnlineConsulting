using Microsoft.Extensions.Configuration;
using OnlineConsulting.Constants;
using OnlineConsulting.Services.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace OnlineConsulting.Services
{
    public class SendgridService : ISendgridService
    {
        private readonly string supportMailAddress;
        private readonly ISendGridClient sendGridClient;

        public SendgridService(IConfiguration configuration, ISendGridClient sendGridClient)
        {
            supportMailAddress = configuration[Parameters.SUPPORT_MAIL_ADDRESS] ?? throw new ArgumentNullException("No support mail address provided");
            this.sendGridClient = sendGridClient ?? throw new ArgumentNullException(nameof(sendGridClient));
        }

        public async Task SendPasswordResetLink(string recipientMail, string resetLink)
        {
            var result = await SendMailTextAsync(supportMailAddress, recipientMail, "Reset Password", resetLink);
            if (!result)
                throw new Exception("Error occured while trying to send email");
        }

        public async Task SendConfirmEmailAddressLink(string recipientMail, string confirmationLink)
        {
            var result = await SendMailTextAsync(supportMailAddress, recipientMail, "Confirm Email Address", confirmationLink);
            if (!result)
                throw new Exception("Error occured while trying to send email");
        }

        private async Task<bool> SendMailTextAsync(string senderEmail, string recipientMail, string subject, string message)
        {
            var msg = new SendGridMessage();
            msg.SetFrom(senderEmail);
            msg.AddTo(recipientMail);
            msg.Subject = HttpUtility.HtmlEncode(subject);
            msg.PlainTextContent = message;
            var response = await sendGridClient.SendEmailAsync(msg);

            return response.StatusCode == HttpStatusCode.Accepted;
        }

        private async Task<bool> SendMailTemplateAsync(string senderEmail, string recipientMail, string templateId, dynamic templateData)
        {
            var msg = new SendGridMessage();
            msg.SetFrom(senderEmail);
            msg.AddTo(recipientMail);
            msg.SetTemplateId(templateId);
            msg.SetTemplateData(templateData);
            var response = await sendGridClient.SendEmailAsync(msg);

            return response.StatusCode == HttpStatusCode.Accepted;
        }
    }
}
