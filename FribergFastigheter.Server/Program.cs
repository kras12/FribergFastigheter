
using FribergFastigheter.HelperClasses;
using FribergFastigheterApi.Data.DatabaseContexts;
using FribergFastigheterApi.HelperClasses;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FribergFastigheter
{
    public class Program
    {
        public static void Main(string[] args)
        {
			var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

			// DB Context
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext")));

			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

			using (var scope = app.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var context = services.GetRequiredService<ApplicationDbContext>();
				context.Database.Migrate();

                if (!context.Housings.Any())
                {
                    SeedMockData(context);

				}		
			}

			//WriteSeedImageUrlsToOutputFolder();

			app.Run();
        }

		[Conditional("DEBUG")]
        private static void WriteSeedImageUrlsToOutputFolder()
        {
			var seedFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "HousingSeedData.json");
			var imageUrls = SeedDataHelper.GetHousingImagePathsFromJsonFile(seedFile);
            string debugFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DebugOutput");

			if (!Directory.Exists(debugFolder))
            {
                Directory.CreateDirectory(debugFolder);
            }
                
            File.WriteAllLines(Path.Combine(debugFolder, "SeedImageUrls.txt"), imageUrls);
		}

		[Conditional("DEBUG")]
		private static void SeedMockData(ApplicationDbContext context)
		{
			var seedFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "HousingSeedData.json");
			var seedData = SeedDataHelper.GetHousingSeedDataFromJsonFile(seedFile);

            var housingCategories = context.HousingCategories.ToDictionary(x => x.CategoryName);
            var municipalities = context.Municipalities.ToDictionary(x => x.MunicipalityName);

            foreach (var housing in seedData.Housings)
            {
                housing.Category = housingCategories[housing.Category.CategoryName];
                housing.Municipality = municipalities[housing.Municipality.MunicipalityName];
                context.Housings.Add(housing);
            }

            context.SaveChanges();
		}
	}    
}
