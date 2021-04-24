using Reddit.Controllers;

namespace Reddit2Kindle.Functions.Contracts.Templates
{
    public class SelfPostTemplate : PostTemplate<SelfPost>
    {
        public SelfPostTemplate(SelfPost selfPost) : base(selfPost)
        {
        }

        public override string TemplateName => "Templates.SelfPost";
    }
}
