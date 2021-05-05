using Reddit.Controllers;

namespace Reddit2Kindle.Functions.Contracts.Templates
{
    public abstract class PostTemplate<T> : PostTemplate, IPostTemplate<T> where T : Post
    {
        protected PostTemplate(T post)
        {
            Post = post;
        }

        public override T Post { get; }
    }

    public abstract class PostTemplate : ITemplate
    {
        public abstract Post Post { get; }

        public abstract string TemplateName { get; }
        
        public string? Index { get; set; }
    }
}
