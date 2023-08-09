using Microsoft.Extensions.Options;
using ReadIt.DAL.Persistance.Settings;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ReadIt.DAL.Persistance.Services
{
    public class EmailSenderService
    {
        private readonly ISendGridClient _sendGridClient;
        private readonly SendGridSettings _sendGridSettings;

        public EmailSenderService(ISendGridClient sendGridClient,
        IOptions<SendGridSettings> sendGridSettings)
        {
            _sendGridClient = sendGridClient;
            _sendGridSettings = sendGridSettings.Value;
        }

        public async Task<bool> SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_sendGridSettings.FromEmail, _sendGridSettings.EmailName),
                Subject = subject,
                PlainTextContent = htmlMessage
            };
            msg.AddTo(email);
            var response = await _sendGridClient.SendEmailAsync(msg);
            return response.IsSuccessStatusCode;
        }
    }

}
