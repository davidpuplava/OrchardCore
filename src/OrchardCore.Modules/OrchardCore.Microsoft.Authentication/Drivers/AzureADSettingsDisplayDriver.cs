using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using OrchardCore.DisplayManagement.Entities;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Environment.Shell;
using OrchardCore.Microsoft.Authentication.Settings;
using OrchardCore.Microsoft.Authentication.ViewModels;
using OrchardCore.Settings;

namespace OrchardCore.Microsoft.Authentication.Drivers
{
    public class AzureADSettingsDisplayDriver : SectionDisplayDriver<ISite, AzureADSettings>
    {
        private readonly IShellReleaseManager _shellReleaseManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AzureADSettingsDisplayDriver(
            IShellReleaseManager shellReleaseManager,
            IAuthorizationService authorizationService,
            IHttpContextAccessor httpContextAccessor)
        {
            _shellReleaseManager = shellReleaseManager;
            _authorizationService = authorizationService;
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task<IDisplayResult> EditAsync(AzureADSettings settings, BuildEditorContext context)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (!await _authorizationService.AuthorizeAsync(user, Permissions.ManageMicrosoftAuthentication))
            {
                return null;
            }
            return Initialize<AzureADSettingsViewModel>("MicrosoftEntraIDSettings_Edit", model =>
            {
                model.DisplayName = settings.DisplayName;
                model.AppId = settings.AppId;
                model.TenantId = settings.TenantId;
                model.SaveTokens = settings.SaveTokens;
                if (settings.CallbackPath.HasValue)
                {
                    model.CallbackPath = settings.CallbackPath.Value;
                }
            }).Location("Content:0").OnGroup(MicrosoftAuthenticationConstants.Features.AAD);
        }

        public override async Task<IDisplayResult> UpdateAsync(AzureADSettings settings, UpdateEditorContext context)
        {
            if (context.GroupId == MicrosoftAuthenticationConstants.Features.AAD)
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (!await _authorizationService.AuthorizeAsync(user, Permissions.ManageMicrosoftAuthentication))
                {
                    return null;
                }

                var model = new AzureADSettingsViewModel();

                await context.Updater.TryUpdateModelAsync(model, Prefix);

                settings.DisplayName = model.DisplayName;
                settings.AppId = model.AppId;
                settings.TenantId = model.TenantId;
                settings.CallbackPath = model.CallbackPath;
                settings.SaveTokens = model.SaveTokens;

                _shellReleaseManager.RequestRelease();
            }

            return await EditAsync(settings, context);
        }
    }
}
