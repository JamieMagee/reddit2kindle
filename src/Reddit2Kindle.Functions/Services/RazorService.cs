using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RazorLight;
using Reddit2Kindle.Functions.Contracts.Templates;

namespace Reddit2Kindle.Functions.Services
{
    public class RazorService
    {
        private readonly RazorLightEngine _engine;
        private readonly ILogger<RazorService> _logger;

        public RazorService(ILogger<RazorService> logger)
        {
            _logger = logger;
            _engine = new RazorLightEngineBuilder()
                .SetOperatingAssembly(Assembly.GetExecutingAssembly())
                .UseEmbeddedResourcesProject(typeof(Program))
                .UseMemoryCachingProvider()
                .Build();
        }

        public async Task<string> RenderTemplateAsync(ITemplate template)
        {
            return await _engine.CompileRenderAsync(template.TemplateName, template);
        }
    }
}
