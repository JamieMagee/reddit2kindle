using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Reddit2Kindle.Contracts;
using Reddit2Kindle.Functions.Services;

namespace Reddit2Kindle.Functions.Functions
{
    public class DebugFunction
    {
        private readonly ILogger<DebugFunction> _logger;
        private readonly Reddit2KindleService _reddit2KindleService;

        public DebugFunction(ILogger<DebugFunction> logger, Reddit2KindleService reddit2KindleService)
        {
            _logger = logger;
            _reddit2KindleService = reddit2KindleService;
        }

        [Function("PostDebug")]
        public async Task<HttpResponseData> PostDebugAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")]
            HttpRequestData req)
        {
            var postRequest = await req.ReadFromJsonAsync<PostRequest>();
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/html; charset=utf-8");
            await response.WriteStringAsync(await _reddit2KindleService.RenderPostAsync(postRequest));
            return response;
        }

        [Function("SubredditDebug")]
        public async Task<HttpResponseData> SubredditDebugAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")]
            HttpRequestData req, FunctionContext context)
        {
            var subredditRequest = await req.ReadFromJsonAsync<SubredditRequest>();
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/html; charset=utf-8");
            await response.WriteStringAsync(await _reddit2KindleService.RenderSubredditAsync(subredditRequest));
            return response;
        }
    }
}
