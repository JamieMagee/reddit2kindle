using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Reddit2Kindle.Contracts;
using Reddit2Kindle.Functions.Services;
using static Reddit2Kindle.Functions.Constants;

namespace Reddit2Kindle.Functions.Functions
{
    public class QueueFunction
    {
        private readonly ILogger<QueueFunction> _logger;
        private readonly Reddit2KindleService _reddit2KindleService;

        public QueueFunction(ILogger<QueueFunction> logger, Reddit2KindleService reddit2KindleService)
        {
            _logger = logger;
            _reddit2KindleService = reddit2KindleService;
        }

        [Function("PostQueue")]
        public async Task PostAsync([QueueTrigger(PostQueue)] PostRequest request)
        {
            _logger.LogInformation(JsonSerializer.Serialize(request));
            await _reddit2KindleService.SendPostAsync(request);
        }

        [Function("SubredditQueue")]
        public async Task SubredditAsync([QueueTrigger(SubredditQueue)] SubredditRequest request)
        {
            _logger.LogInformation(JsonSerializer.Serialize(request));
            await _reddit2KindleService.SendSubredditAsync(request);
        }
    }
}
