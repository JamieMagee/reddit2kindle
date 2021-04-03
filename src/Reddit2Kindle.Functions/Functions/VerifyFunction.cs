using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using StrongGrid;

namespace Reddit2Kindle.Functions.Functions
{
    public class VerifyFunction
    {
        private readonly ILogger<VerifyFunction> _logger;
        private readonly WebhookParser _parser;

        public VerifyFunction(ILogger<VerifyFunction> logger, WebhookParser parser)
        {
            _logger = logger;
            _parser = parser;
        }

        [Function("Verify")]
        public async Task VerifyAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")]
            HttpRequestData req)
        {
            var inboundEmail = await _parser.ParseInboundEmailWebhookAsync(req.Body);
            var content = inboundEmail.Text;
            _logger.LogInformation(content);
        }
    }
}
