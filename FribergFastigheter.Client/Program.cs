using Blazored.LocalStorage;
using FribergFastigheter.Client.AutoMapper;
using FribergFastigheter.Client.Services;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using FribergFastigheter.Shared.Constants;
using FribergFastigheter.Shared.Services.AuthorizationHandlers.Broker;
using FribergFastigheter.Client.Services.AuthorizationHandlers.Housing;
using FribergFastigheter.Shared.Services.AuthorizationHandlers.Housing;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FribergFastigheter.Client.Services.AuthorizationHandlers.Broker;
using Blazored.SessionStorage;

namespace FribergFastigheter.Client
{
    /// <!-- Author: Jimmie, Marcus -->
    /// <!-- Co Authors: -->
    public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");
			builder.RootComponents.Add<HeadOutlet>("head::after");

			builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddAutoMapper(typeof(ViewModelToDtoAutoMapperProfile), typeof(DtoToViewModelAutoMapperProfile));
            builder.Services.AddScoped<AuthenticationStateProvider, BrokerFirmAuthenticationStateProvider>();
            builder.Services.AddScoped<HousingSearchService, HousingSearchService>();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddBlazoredSessionStorage();  

            builder.Services.AddOidcAuthentication(options =>
			{
				// Configure your authentication provider options here.
				// For more information, see https://aka.ms/blazor-standalone-auth
				builder.Configuration.Bind("Local", options.ProviderOptions);
			});

            /// <!-- Author: Jimmie -->
            /// <!-- Co Authors: -->
            builder.Services.AddAuthorizationCore(options =>
            {
                options.AddPolicy(ApplicationPolicies.Broker, policy => 
                    policy.RequireClaim(ApplicationUserClaims.UserRole, ApplicationUserRoles.Broker, ApplicationUserRoles.BrokerAdmin));

                options.AddPolicy(ApplicationPolicies.BrokerAdmin, policy => 
                    policy.RequireClaim(ApplicationUserClaims.UserRole, ApplicationUserRoles.BrokerAdmin));

                options.AddPolicy(ApplicationPolicies.CanCreateHousing, policy => 
                    policy.Requirements.Add(new ManageHousingPreAuthorizationHandler(ManageHousingPreAuthorizationHandler.ActionTypes.CreateHousing)));

                options.AddPolicy(ApplicationPolicies.CanCreateHousingImageResource, policy =>
                    policy.Requirements.Add(new ManageHousingAuthorizationHandler(ManageHousingAuthorizationHandler.ActionTypes.CreateHousingImage)));

                options.AddPolicy(ApplicationPolicies.CanDeleteHousing, policy => 
                    policy.AddRequirements(new ManageHousingPreAuthorizationHandler(ManageHousingPreAuthorizationHandler.ActionTypes.DeleteHousing)));

                options.AddPolicy(ApplicationPolicies.CanEditHousing, policy => 
                    policy.AddRequirements(new ManageHousingPreAuthorizationHandler(ManageHousingPreAuthorizationHandler.ActionTypes.EditHousing)));

                options.AddPolicy(ApplicationPolicies.CanEditFullHousing, policy =>
                    policy.AddRequirements(new ManageHousingPreAuthorizationHandler(ManageHousingPreAuthorizationHandler.ActionTypes.EditHousing)));

                options.AddPolicy(ApplicationPolicies.CanCreateHousingResource, policy =>
                    policy.Requirements.Add(new ManageHousingAuthorizationHandler(ManageHousingAuthorizationHandler.ActionTypes.CreateHousing)));

                options.AddPolicy(ApplicationPolicies.CanDeleteHousingResource, policy =>
                    policy.AddRequirements(new ManageHousingAuthorizationHandler(ManageHousingAuthorizationHandler.ActionTypes.DeleteHousing)));

                options.AddPolicy(ApplicationPolicies.CanDeleteHousingImageResource, policy =>
                    policy.AddRequirements(new ManageHousingAuthorizationHandler(ManageHousingAuthorizationHandler.ActionTypes.DeleteHousingImage)));

                options.AddPolicy(ApplicationPolicies.CanEditHousingResource, policy =>
                    policy.AddRequirements(new ManageHousingAuthorizationHandler(ManageHousingAuthorizationHandler.ActionTypes.EditHousing)));

                options.AddPolicy(ApplicationPolicies.CanCreateBrokerResource, policy =>
                    policy.AddRequirements(new ManageBrokerAuthorizationHandler(ManageBrokerAuthorizationHandler.ActionTypes.CreateBroker)));

                options.AddPolicy(ApplicationPolicies.CanEditBrokerResource, policy =>
                    policy.AddRequirements(new ManageBrokerAuthorizationHandler(ManageBrokerAuthorizationHandler.ActionTypes.EditBroker)));

                options.AddPolicy(ApplicationPolicies.CanDeleteBrokerResource, policy =>
                    policy.AddRequirements(new ManageBrokerAuthorizationHandler(ManageBrokerAuthorizationHandler.ActionTypes.DeleteBroker)));

                options.AddPolicy(ApplicationPolicies.CanCreateBroker, policy =>
                policy.AddRequirements(new ManageBrokerPreAuthorizationHandler(ManageBrokerPreAuthorizationHandler.ActionTypes.CreateBroker)));

                options.AddPolicy(ApplicationPolicies.CanEditBroker, policy =>
                policy.AddRequirements(new ManageBrokerPreAuthorizationHandler(ManageBrokerPreAuthorizationHandler.ActionTypes.EditBroker)));

                options.AddPolicy(ApplicationPolicies.CanEditFullBroker, policy =>
                policy.AddRequirements(new ManageBrokerPreAuthorizationHandler(ManageBrokerPreAuthorizationHandler.ActionTypes.EditFullBroker)));

                options.AddPolicy(ApplicationPolicies.CanDeleteBroker, policy =>
                policy.AddRequirements(new ManageBrokerPreAuthorizationHandler(ManageBrokerPreAuthorizationHandler.ActionTypes.DeleteBroker)));
            });

            // Add API services with typed http clients
            /// <!-- Author: Jimmie -->
            /// <!-- Co Authors: -->
            builder.Services.AddHttpClient<IHousingApiService, HousingApiService>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["FribergFastigheterApiBaseUrl"]!);
            });

            /// <!-- Author: Jimmie -->
            /// <!-- Co Authors: -->
            builder.Services.AddHttpClient<IBrokerFirmApiService, BrokerFirmApiService>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["FribergFastigheterApiBaseUrl"]!);
            });            

            await builder.Build().RunAsync();
		}
	}
}
