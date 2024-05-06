
using FribergFastigheter.HelperClasses;
using FribergFastigheter.Server.AutoMapper;
using FribergFastigheter.Server.Data.Constants;
using FribergFastigheter.Server.Data.Entities;
using FribergFastigheter.Server.Data.Interfaces;
using FribergFastigheter.Server.Data.Repositories;
using FribergFastigheter.Server.HelperClasses.Data;
using FribergFastigheter.Server.Services;
using FribergFastigheterApi.Data.DatabaseContexts;
using FribergFastigheterApi.HelperClasses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace FribergFastigheter
{
    public class Program
    {
        public static void Main(string[] args)
        {
			var builder = WebApplication.CreateBuilder(args);


			// Add Cors policy for our debug build
#if DEBUG
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("LocalHostingCorsPolicy", builder => builder.WithOrigins("https://localhost:7038")
					 .AllowAnyMethod()
					 .AllowAnyHeader()
					 .AllowCredentials());
			});
#endif

			// Add services to the container.
			builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            
            // Swagger
            builder.Services.AddSwaggerGen();

            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Friberg Fastigheter API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            // DB Context
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext")));

			// Repositories
			builder.Services.AddTransient<IHousingRepository, HousingRepository>();
            builder.Services.AddTransient<IBrokerRepository, BrokerRepository>();
            builder.Services.AddTransient<IBrokerFirmRepository, BrokerFirmRepository>();

            // Auto Mapper
            builder.Services.AddAutoMapper(typeof(EntityToDtoAutoMapperProfile), typeof(DtoToEntityAutoMapperProfile));

            // Custom Services
            builder.Services.AddTransient<IImageService, ImageService>();

            // Add serialization converters
            builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            // Token service
            builder.Services.AddTransient<ITokenService, TokenService>();

            // Identity Services

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequiredLength = 10;
                options.User.AllowedUserNameCharacters += "åäö";

            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                options.DefaultChallengeScheme =
                options.DefaultForbidScheme =
                options.DefaultScheme =
                options.DefaultSignInScheme =
                options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]!))
                };
            });

            // Authorization
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(ApplicationPolicies.BrokerAdmin, policy => policy.RequireClaim(BrokerUserClaims.BrokerRole, ApplicationUserRoles.BrokerAdmin));
            });


            // Compression test			
            //builder.Services.AddResponseCompression(options =>
            //{
            //	options.EnableForHttps = true;
            //    options.Providers.Add<BrotliCompressionProvider>();
            //	options.Providers.Add<GzipCompressionProvider>();
            //});
            //builder.Services.Configure<BrotliCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Fastest);
            //builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);

            var app = builder.Build();

#if DEBUG
			app.UseCors("LocalHostingCorsPolicy");
#endif

			// Compression test
			//app.UseResponseCompression();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

			app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

			app.MapControllers();

			using (var scope = app.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var context = services.GetRequiredService<ApplicationDbContext>();
                var configuration = services.GetRequiredService<IConfiguration>();
                context.Database.Migrate();

                if (!context.Housings.Any())
                {					
					//SeedMockData(context, configuration);
				}

                //CopyMockDataImagesToUploadFolder(configuration);
            }        

            app.Run();
        }

        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        [Conditional("DEBUG")]
		private static void SeedMockData(ApplicationDbContext context, IConfiguration configuration)
		{
            // Database
			var seedFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "MockData", "HousingSeedData.json");
            var seedHelper = new SeedDataHelper(seedFile);
			var seedData = seedHelper.GetSeedData();

            var housingCategories = context.HousingCategories.ToDictionary(x => x.CategoryName);
            var municipalities = context.Municipalities.ToDictionary(x => x.MunicipalityName);

            foreach (var housing in seedData.Housings)
            {
                housing.Category = housingCategories[housing.Category.CategoryName];
                housing.Municipality = municipalities[housing.Municipality.MunicipalityName];
                context.Housings.Add(housing);
            }

            context.SaveChanges();

            var roles = context.Roles.ToList();

            foreach (var brokerFirm in seedData.BrokerFirms)
            {
                var superBroker = brokerFirm.Brokers.First();
                var brokerId = context.Brokers.Where(x => x.User.UserName == superBroker.User.UserName).Select(x => x.User.Id).Single();
                var roleId = roles.Single(x => x.Name == ApplicationUserRoles.BrokerAdmin).Id;
                context.UserRoles.Add(new IdentityUserRole<string>() { UserId = brokerId, RoleId = roleId });

                foreach (var broker in brokerFirm.Brokers.Skip(1))
                {
                    brokerId = context.Brokers.Where(x => x.User.UserName == broker.User.UserName).Select(x => x.User.Id).Single();
                    roleId = roles.Single(x => x.Name == ApplicationUserRoles.Broker).Id;
                    context.UserRoles.Add(new IdentityUserRole<string>() { UserId = brokerId, RoleId = roleId });
                }
            }

            context.SaveChanges();

            // Save urls for images to download
            string outputFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DebugSeedImageUrls");
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            File.WriteAllLines(Path.Combine(outputFolder, "SeedImageUrls.txt"), seedData.SeedImageUrls.AllImageUrls);
        }

        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        [Conditional("DEBUG")]
        private static void CopyMockDataImagesToUploadFolder(IConfiguration configuration)
        {
            // Copy images
            foreach (var file in Directory.EnumerateFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "MockData", "Images")))
            {
                string destinationFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configuration.GetSection("FileStorage").GetSection("UploadFolderName").Value!);

                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                File.Copy(file, Path.Combine(destinationFolder, Path.GetFileName(file)), overwrite: true);
            }
        }
    }
}
