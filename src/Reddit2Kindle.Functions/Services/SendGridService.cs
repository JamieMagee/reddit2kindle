using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using Microsoft.Extensions.Logging;

namespace Reddit2Kindle.Functions.Services
{
    public class SendGridService
    {
        private readonly IFluentEmailFactory _emailFactory;
        private readonly ILogger<SendGridService> _logger;

        public SendGridService(ILogger<SendGridService> logger, IFluentEmailFactory emailFactory)
        {
            _logger = logger;
            _emailFactory = emailFactory;
        }

        public async Task SendEmailAsync(string toAddress, string title, string attachmentContent)
        {
            var email = _emailFactory
                .Create()
                .To(toAddress)
                .Subject(title)
                .Body(" ")
                .Attach(new Attachment
                {
                    Filename = $"{title}.html",
                    Data = new MemoryStream(Encoding.Latin1.GetBytes(attachmentContent ?? "")),
                    ContentType = "text/html"
                });
            var response = await email.SendAsync();
            _logger.LogInformation(JsonSerializer.Serialize(response));
        }
    }
}
