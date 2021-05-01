using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Reddit.Controllers;
using Reddit2Kindle.Contracts;
using Reddit2Kindle.Functions.Contracts.Templates;
using static Reddit2Kindle.Functions.Utils.RedditUtils;

namespace Reddit2Kindle.Functions.Services
{
    public class Reddit2KindleService
    {
        private readonly ILogger<Reddit2KindleService> _logger;
        private readonly RazorService _razorService;
        private readonly ReadabilityService _readabilityService;
        private readonly RedditService _redditService;
        private readonly SendGridService _sendGridService;

        public Reddit2KindleService(ILogger<Reddit2KindleService> logger, RazorService razorService, ReadabilityService readabilityService,
            RedditService redditService, SendGridService sendGridService)
        {
            _logger = logger;
            _razorService = razorService;
            _readabilityService = readabilityService;
            _redditService = redditService;
            _sendGridService = sendGridService;
        }

        public async Task SendPost(PostRequest request)
        {
            var post = _redditService.GetPost(request.Post.ToString());
            var template = await GetPostTemplate(post);
            var attachmentContent = await _razorService.RenderTemplateAsync(template);
            await _sendGridService.SendEmailAsync(request.Email, post.Title, attachmentContent);
        }

        public async Task SendSubreddit(SubredditRequest request)
        {
            var posts = _redditService.GetSubredditPosts(request.Subreddit, request.TimePeriod);
            var postTemplates = await Task.WhenAll(posts.Select(GetPostTemplate));
            var subredditTemplate = new SubredditTemplate(postTemplates);
            var attachmentContent = await _razorService.RenderTemplateAsync(subredditTemplate);
            await _sendGridService.SendEmailAsync(request.Email, GenerateTitle(request), attachmentContent);
        }
        
        internal async Task<string> RenderPost(PostRequest request)
        {
            var post = _redditService.GetPost(request.Post.ToString());
            var template = await GetPostTemplate(post);
            return await _razorService.RenderTemplateAsync(template);
        }
        
        internal async Task<string> RenderSubreddit(SubredditRequest request)
        {
            var posts = _redditService.GetSubredditPosts(request.Subreddit, request.TimePeriod);
            var postTemplates = await Task.WhenAll(posts.Select(GetPostTemplate));
            var subredditTemplate = new SubredditTemplate(postTemplates);
            return await _razorService.RenderTemplateAsync(subredditTemplate);
        }
        
        private async Task<PostTemplate> GetPostTemplate(Post post)
        {
            return post switch
            {
                SelfPost selfPost => new SelfPostTemplate(selfPost),
                LinkPost linkPost => new LinkPostTemplate(linkPost, await _readabilityService.GetArticle(linkPost.URL))
            };
        }
    }
}
