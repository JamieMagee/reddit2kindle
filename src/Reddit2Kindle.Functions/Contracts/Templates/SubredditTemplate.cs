using System.Collections.Generic;

namespace Reddit2Kindle.Functions.Contracts.Templates
{
    public class SubredditTemplate : ITemplate
    {
        public SubredditTemplate(IList<PostTemplate> templates)
        {
            Templates = templates;
        }

        public IList<PostTemplate> Templates { get; }

        public string TemplateName => "Templates.Subreddit";
    }
}
