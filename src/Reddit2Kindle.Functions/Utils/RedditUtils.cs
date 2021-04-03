using System.Text.RegularExpressions;
using Reddit2Kindle.Contracts;

namespace Reddit2Kindle.Functions.Utils
{
    public static class RedditUtils
    {
        private static readonly Regex SubredditRegex = new("^https://www.reddit.com/r/(?<subreddit>\\w+)/comments/(?<name>\\w+)/.*$");

        public static string ExtractPostId(string url)
        {
            var groups = SubredditRegex.Match(url).Groups;
            return groups["name"].Value;
        }

        public static string GenerateTitle(SubredditRequest request)
        {
            return request.TimePeriod switch
            {
                TimePeriod.All => $"Top posts from {request.Subreddit} of all time",
                _ => $"Top posts from {request.Subreddit} over the past {request.TimePeriod.ToString().ToLowerInvariant()}"
            };
        }
    }
}
