using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reddit;
using Reddit.Controllers;
using Reddit2Kindle.Contracts;
using Reddit2Kindle.Functions.Contracts.Options;
using Reddit2Kindle.Functions.Utils;

namespace Reddit2Kindle.Functions.Services
{
    public class RedditService
    {
        private readonly ILogger<RedditService> _logger;
        private readonly RedditClient _redditClient;

        public RedditService(ILogger<RedditService> logger, IOptions<RedditOptions> redditOptions)
        {
            _logger = logger;
            var options = redditOptions.Value;
            _redditClient = new RedditClient(options.AppId, appSecret: options.AppSecret, refreshToken: options.RefreshToken);
        }

        public Post GetPost(string uri)
        {
            var id = RedditUtils.ExtractPostId(uri);
            return _redditClient.Post($"t3_{id}").About();
        }

        public IEnumerable<Post> GetSubredditPosts(string subreddit, TimePeriod timePeriod)
        {
            return _redditClient.Subreddit(subreddit).Posts.GetTop(timePeriod.ToString(), limit: 25).OrderByDescending(post => post.Score);
        }
    }
}
