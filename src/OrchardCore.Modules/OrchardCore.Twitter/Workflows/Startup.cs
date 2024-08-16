using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using OrchardCore.Twitter.Migrations;
using OrchardCore.Twitter.Workflows.Activities;
using OrchardCore.Twitter.Workflows.Drivers;
using OrchardCore.Workflows.Helpers;

namespace OrchardCore.Twitter.Workflows;

[RequireFeatures("OrchardCore.Workflows")]
public sealed class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddActivity<UpdateXTwitterStatusTask, UpdateTwitterStatusTaskDisplayDriver>();
    }
}
