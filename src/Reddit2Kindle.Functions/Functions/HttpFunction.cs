using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Reddit2Kindle.Contracts;
using static Reddit2Kindle.Functions.Constants;

namespace Reddit2Kindle.Functions.Functions
{
    public class HttpFunction
    {
        private readonly ILogger<HttpFunction> _logger;

        public HttpFunction(ILogger<HttpFunction> logger)
        {
            _logger = logger;
        }

        [Function("Post")]
        public async Task<HttpAndQueuePost> PostAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")]
            HttpRequestData req)
        {
            try
            {
                var postRequest = await req.ReadFromJsonAsync<PostRequest>();
                _logger.LogInformation(JsonSerializer.Serialize(postRequest));
                return new HttpAndQueuePost
                {
                    PostRequest = postRequest,
                    HttpResponseData = req.CreateResponse(HttpStatusCode.Accepted)
                };
            }
            catch (Exception e)
            {
                _logger.LogError(JsonSerializer.Serialize(e));
                return new HttpAndQueuePost
                {
                    HttpResponseData = req.CreateResponse(HttpStatusCode.BadRequest)
                };
            }
        }

        [Function("Subreddit")]
        public async Task<HttpAndQueueSubreddit> SubredditAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")]
            HttpRequestData req)
        {
            try
            {
                var subredditRequest = await req.ReadFromJsonAsync<SubredditRequest>();
                _logger.LogInformation(JsonSerializer.Serialize(subredditRequest));
                return new HttpAndQueueSubreddit
                {
                    SubredditRequest = subredditRequest,
                    HttpResponseData = req.CreateResponse(HttpStatusCode.Accepted)
                };
            }
            catch (Exception e)
            {
                _logger.LogError(JsonSerializer.Serialize(e));
                return new HttpAndQueueSubreddit
                {
                    HttpResponseData = req.CreateResponse(HttpStatusCode.BadRequest)
                };
            }
        }

        public class HttpAndQueuePost
        {
            [QueueOutput(PostQueue)]
            public PostRequest PostRequest { get; init; }

            public HttpResponseData HttpResponseData { get; init; }
        }

        public class HttpAndQueueSubreddit
        {
            [QueueOutput(SubredditQueue)]
            public SubredditRequest SubredditRequest { get; init; }

            public HttpResponseData HttpResponseData { get; init; }
        }
    }
}
