
using FribergFastigheter.HelperClasses;
using FribergFastigheter.Server.AutoMapper;
using FribergFastigheter.Shared.Constants;
using FribergFastigheter.Server.Data.Entities;
using FribergFastigheter.Server.Data.Interfaces;
using FribergFastigheter.Server.Data.Repositories;
using FribergFastigheter.Server.HelperClasses.Data;
using FribergFastigheter.Server.Services;
using FribergFastigheter.Shared.Dto;
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
using FribergFastigheter.Server.Services.AuthorizationHandlers;

namespace FribergFastigheter
{
    /// <!-- Author: Jimmie, Marcus -->
    /// <!-- Co Authors: -->
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
                options.User.AllowedUserNameCharacters += "едц";

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
                        System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]!)),
                    RoleClaimType = ApplicationUserClaims.UserRole
                };
            });

            // Authorization
            /// <!-- Author: Jimmie -->
            /// <!-- Co Authors: -->
            builder.Services.AddAuthorizationCore(options =>
            {
                options.AddPolicy(ApplicationPolicies.Broker, policy =>
                    policy.RequireClaim(ApplicationUserClaims.UserRole, ApplicationUserRoles.Broker, ApplicationUserRoles.BrokerAdmin));

                options.AddPolicy(ApplicationPolicies.BrokerAdmin, policy =>
                    policy.RequireClaim(ApplicationUserClaims.UserRole, ApplicationUserRoles.BrokerAdmin));

                options.AddPolicy(ApplicationPolicies.CanCreateHousing, policy =>
                    policy.Requirements.Add(new ManageHousingAuthorizationHandler(ManageHousingAuthorizationHandler.ActionTypes.CreateHousing)));

                options.AddPolicy(ApplicationPolicies.CanDeleteHousing, policy =>
                    policy.AddRequirements(new ManageHousingAuthorizationHandler(ManageHousingAuthorizationHandler.ActionTypes.DeleteHousing)));

                options.AddPolicy(ApplicationPolicies.CanEditHousing, policy =>
                    policy.AddRequirements(new ManageHousingAuthorizationHandler(ManageHousingAuthorizationHandler.ActionTypes.EditHousing)));
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
                context.Database.Migrate();

#if DEBUG
                if (!context.Housings.Any())
                {
                    var seedFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "MockData", "HousingSeedData.json");
                    var seedHelper = new SeedDataHelper(seedFile);
                    seedHelper.SeedMockData(app).Wait();
				}
#endif
            }        

            app.Run();
        }         
    }
}
