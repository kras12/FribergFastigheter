using FribergFastigheter.Client.Services;
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
			builder.Services.AddHttpClient<IFribergFastigheterApiService, FribergFastigheterApiService>(client =>
			{
				client.BaseAddress = new Uri(builder.Configuration["FribergFastigheterApiBaseUrl"]!);
			});

			await builder.Build().RunAsync();
		}
	}
}
