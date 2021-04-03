using Reddit.Controllers;

namespace Reddit2Kindle.Functions.Contracts.Templates
{
    public class LinkPostTemplate : PostTemplate<LinkPost>
    {
        public LinkPostTemplate(LinkPost linkPost, string article) : base(linkPost)
        {
            Article = article;
        }

        public override string TemplateName => "Templates.LinkPost";

        public string Article { get; }
    }
}
