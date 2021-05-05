using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SmartReader;

namespace Reddit2Kindle.Functions.Services
{
    public class ReadabilityService
    {
        private readonly ILogger<ReadabilityService> _logger;

        public ReadabilityService(ILogger<ReadabilityService> logger)
        {
            _logger = logger;
        }

        public async Task<string> GetArticleAsync(string uri)
        {
            var reader = new Reader(uri);
            try
            {
                var article = await reader.GetArticleAsync();

                if (article.IsReadable)
                {
                    return article.TextContent;
                }

                _logger.LogWarning($"Unable to get content for {uri}");
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message);
            }

            return string.Empty;
        }
    }
}
