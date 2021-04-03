using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Reddit2Kindle.Contracts;

namespace Reddit2Kindle.Blazor.Services
{
    public class Reddit2KindleService
    {
        private readonly IHttpClientFactory _clientFactory;

        public Reddit2KindleService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task SubmitPost(PostRequest postRequest)
        {
            using var client = _clientFactory.CreateClient(Constants.HttpClientName);
            var response = await client.SendAsync(GenerateHttpRequestMessage(postRequest));
            response.EnsureSuccessStatusCode();
        }

        public async Task SubmitSubreddit(SubredditRequest subredditRequest)
        {
            using var client = _clientFactory.CreateClient(Constants.HttpClientName);
            var response = await client.SendAsync(GenerateHttpRequestMessage(subredditRequest));
            response.EnsureSuccessStatusCode();
        }

        private static HttpRequestMessage GenerateHttpRequestMessage(Request request)
        {
            var content = request switch
            {
                PostRequest postRequest => JsonSerializer.Serialize(postRequest),
                SubredditRequest subredditRequest => JsonSerializer.Serialize(subredditRequest)
            };
            return new()
            {
                Method = HttpMethod.Post,
                Content = new StringContent(content, Encoding.UTF8, "application/json"),
                RequestUri = request switch
                {
                    PostRequest => new Uri("api/Post", UriKind.Relative),
                    SubredditRequest => new Uri("api/Subreddit", UriKind.Relative)
                }
            };
        }
    }
}
