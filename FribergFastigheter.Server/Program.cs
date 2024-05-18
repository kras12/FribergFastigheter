
using FribergFastigheter.HelperClasses;
using FribergFastigheter.Server.AutoMapper;
using FribergFastigheter.Shared.Constants;
using FribergFastigheter.Server.Data.Entities;
using FribergFastigheter.Server.Data.Interfaces;
using FribergFastigheter.Server.Data.Repositories;
using FribergFastigheter.Server.Services;
using FribergFastigheterApi.Data.DatabaseContexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using FribergFastigheter.Shared.Services.AuthorizationHandlers.Housing;
using FribergFastigheter.Shared.Services.AuthorizationHandlers.Broker;
using FribergFastigheter.Server.Filters;
using System.Text;
using FribergFastigheter.Shared.Services.AuthorizationHandlers;

namespace FribergFastigheter
{
    /// <!-- Author: Jimmie, Marcus -->
    /// <!-- Co Authors: -->
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ==================================================================================================================
            // Data
            // ==================================================================================================================

            // DB Context
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext")));

            // Repositories
            builder.Services.AddTransient<IHousingRepository, HousingRepository>();
            builder.Services.AddTransient<IBrokerRepository, BrokerRepository>();
            builder.Services.AddTransient<IBrokerFirmRepository, BrokerFirmRepository>();

            // ==================================================================================================================
            // Documentation
            // ==================================================================================================================

            // Swagger
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
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

            // ==================================================================================================================
            // Mapping
            // ==================================================================================================================

            // Auto Mapper
            builder.Services.AddAutoMapper(typeof(EntityToDtoAutoMapperProfile), typeof(DtoToEntityAutoMapperProfile));

            // ==================================================================================================================
            // Network (converters, cors, data transfers, filters)
            // ==================================================================================================================

            // Add serialization converters
            builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            // Cors policy
#if DEBUG
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("LocalHostingCorsPolicy", builder => builder.WithOrigins("https://localhost:7038")
                     .AllowAnyMethod()
                     .AllowAnyHeader()
                     .AllowCredentials());
            });
#endif

            /// Reformats validation problems details from bad requests into an ApiErrorResponseDto object.
            /// <!-- Author: Jimmie -->
            /// <!-- Co Authors: -->
            builder.Services.AddControllers(options => options.Filters.Add(typeof(ReformatValidationProblemAttribute)));

            // ==================================================================================================================
            // Security (authentication, authorization, identity)
            // ==================================================================================================================

            // Identity Services
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequiredLength = 10;
                options.User.AllowedUserNameCharacters += "едц";

            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

            // Authentication
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
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]!)),
                    RoleClaimType = ApplicationUserClaims.UserRole
                };
            });

            // Authorization
            /// <!-- Author: Jimmie, Marcus -->
            /// <!-- Co Authors: -->
            builder.Services.AddAuthorizationCore(options =>
            {
                options.AddPolicy(ApplicationPolicies.Broker, policy =>
                    policy.RequireClaim(ApplicationUserClaims.UserRole, ApplicationUserRoles.Broker, ApplicationUserRoles.BrokerAdmin));

                options.AddPolicy(ApplicationPolicies.BrokerAdmin, policy =>
                    policy.RequireClaim(ApplicationUserClaims.UserRole, ApplicationUserRoles.BrokerAdmin));

                options.AddPolicy(ApplicationPolicies.CanCreateHousingResource, policy =>
                    policy.Requirements.Add(new ManageHousingAuthorizationHandler(ManageHousingAuthorizationHandler.ActionTypes.CreateHousing)));

                options.AddPolicy(ApplicationPolicies.CanCreateHousingImageResource, policy =>
                    policy.Requirements.Add(new ManageHousingAuthorizationHandler(ManageHousingAuthorizationHandler.ActionTypes.CreateHousingImage)));

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

                options.AddPolicy(ApplicationPolicies.BrokerFirmAssociation, policy =>
               policy.AddRequirements(new CheckAssociationAuthorizationHandler(CheckAssociationAuthorizationHandler.ActionTypes.CheckBrokerFirmAssociation)));
            });

            // Token service
            builder.Services.AddTransient<ITokenService, TokenService>();

            // ==================================================================================================================
            // Uncategorized
            // ==================================================================================================================

            // Image Services
            builder.Services.AddTransient<IImageService, ImageService>();

            var app = builder.Build();

#if DEBUG
            app.UseCors("LocalHostingCorsPolicy");
#endif

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

            // ==================================================================================================================
            // Migration and seeding
            // ==================================================================================================================
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
