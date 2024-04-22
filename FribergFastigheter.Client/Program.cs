using FribergFastigheter.Client.Services;
using FribergFastigheter.Client.Services.HousingApi;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace FribergFastigheter.Client
{
    public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");
			builder.RootComponents.Add<HeadOutlet>("head::after");

			builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

			builder.Services.AddOidcAuthentication(options =>
			{
				// Configure your authentication provider options here.
				// For more information, see https://aka.ms/blazor-standalone-auth
				builder.Configuration.Bind("Local", options.ProviderOptions);
			});

            // Add http clients
            /// <!-- Author: Jimmie -->
            /// <!-- Co Authors: -->
            builder.Services.AddHttpClient<FribergApiHttpClientService>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["FribergFastigheterApiBaseUrl"]!);
            });

            // API Services
            builder.Services.AddTransient<IHousingApiService, HousingApiService>();
            builder.Services.AddTransient<IHousingBrokerApiService, HousingBrokerApiService>();
            builder.Services.AddTransient<IHousingBrokerFirmApiService, HousingBrokerFirmApiService>();
            builder.Services.AddTransient<IBrokerApiService, BrokerApiService>();
            builder.Services.AddTransient<IBrokerFirmApiService, BrokerFirmApiService>();
            builder.Services.AddTransient<IBrokerHousingApiService, BrokerHousingApiService>();
            builder.Services.AddTransient<IBrokerHousingImageApiService, BrokerHousingImageApiService>();

            await builder.Build().RunAsync();
		}
	}
}
