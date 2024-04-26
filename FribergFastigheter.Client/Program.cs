using FribergFastigheter.Client.AutoMapper;
using FribergFastigheter.Client.Services;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
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
            builder.Services.AddAutoMapper(typeof(ViewModelToDtoAutoMapperProfile), typeof(DtoToViewModelAutoMapperProfile));

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

            // Auto Mapper
            builder.Services.AddAutoMapper(typeof(ViewModelToDtoAutoMapperProfile), typeof(DtoToViewModelAutoMapperProfile));            

            await builder.Build().RunAsync();
		}
	}
}
