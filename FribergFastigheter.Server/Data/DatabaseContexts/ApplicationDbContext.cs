using FribergFastigheter.Server.Data.Entities;
using FribergFastigheter.HelperClasses;
using FribergFastigheter.Server.Data.Constants;
using FribergFastigheterApi.HelperClasses.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Emit;

namespace FribergFastigheterApi.Data.DatabaseContexts
{
	/// <summary>
	/// The database context for the application.
	/// </summary>
	/// <!-- Author: Jimmie -->
	/// <!-- Co Authors: Marcus -->
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		#region Constructors

		/// <summary>
		/// A constructor.
		/// </summary>
		/// <param name="options">The options to use.</param>
		public ApplicationDbContext(DbContextOptions options) : base(options)
		{

		}

		#endregion

		#region Properties

		/// <summary>
		/// The database set for broker firms.
		/// </summary>
		public DbSet<BrokerFirm> BrokerFirms { get; set; }

		/// <summary>
		/// The database set for brokers.
		/// </summary>
		public DbSet<Broker> Brokers { get; set; }

		/// <summary>
		/// The database set for housings.
		/// </summary>
		public DbSet<Housing> Housings { get; set; }

		/// <summary>
		/// The database set for housing categories.
		/// </summary>
		public DbSet<HousingCategory> HousingCategories { get; set; }

		/// <summary>
		/// The database set for municipalities.
		/// </summary>
		public DbSet<Municipality> Municipalities { get; set; }		

		#endregion

		#region Methods

		/// <summary>
		/// Configures the conventions.
		/// </summary>
		/// <param name="configurationBuilder">The configuration builder.</param>
		protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
		{
			base.ConfigureConventions(configurationBuilder);

			// Expliclity set the precision. The default is 18,2 but we choose to give a little more precision on the decimal side. 
			// We will still be able to handle huge numbers. 
			configurationBuilder.Properties<decimal>()
				.HavePrecision(18, 4);
		}

		/// <summary>
		/// Configures the options.
		/// </summary>
		/// <param name="optionsBuilder"></param>
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);

			if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
			{
				optionsBuilder.EnableSensitiveDataLogging(sensitiveDataLoggingEnabled: true);
			}
		}

		/// <summary>
		/// Configures the database model.
		/// </summary>
		/// <param name="modelBuilder">The model builder.</param>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Housing>()
				.HasOne(x => x.Broker)
				.WithMany()
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Housing>()
				.Navigation(x => x.Broker)
				.AutoInclude();

            modelBuilder.Entity<Housing>()
                .Navigation(x => x.BrokerFirm)
                .AutoInclude();

            modelBuilder.Entity<Housing>()
                .Navigation(x => x.Images)
                .AutoInclude();

            modelBuilder.Entity<Housing>()
                .Navigation(x => x.Municipality)
                .AutoInclude();

            modelBuilder.Entity<Housing>()
                .Navigation(x => x.Category)
                .AutoInclude();

            AddEntitySeeds(modelBuilder);

			modelBuilder.Entity<BrokerFirm>()
				.HasMany(x => x.Brokers)
				.WithOne(x => x.BrokerFirm)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<BrokerFirm>()
				.Navigation(x => x.Brokers)
				.AutoInclude();

            modelBuilder.Entity<BrokerFirm>()
                .Navigation(x => x.Logotype)
                .AutoInclude();

            modelBuilder.Entity<Broker>()
                .Navigation(x => x.BrokerFirm)
                .AutoInclude();

            modelBuilder.Entity<Broker>()
                .Navigation(x => x.ProfileImage)
                .AutoInclude();

			modelBuilder.Entity<Broker>()
				.Navigation(x => x.User)
				.AutoInclude();

			modelBuilder.Entity<IdentityRole>()
			.HasData(
				new IdentityRole
				{
					Id = "7e648d4e-a530-4cd4-b8d7-8be891780f71",
					Name = ApplicationUserRoles.Broker,
					NormalizedName = ApplicationUserRoles.Broker.ToUpper(),
				},
				new IdentityRole
				{
					Id = "bcd2b11c-e243-4310-a9c3-3180c1b743ea",
					Name = ApplicationUserRoles.BrokerAdmin,
					NormalizedName = ApplicationUserRoles.BrokerAdmin.ToUpper(),
				});


			modelBuilder.Entity<BrokerFirm>()
				.HasData(
					new BrokerFirm()
					{
						Name = "Ankeborg",
						BrokerFirmId = 1,
					}
				);

            var hasher = new PasswordHasher<ApplicationUser>();
            modelBuilder.Entity<ApplicationUser>()
                .HasData(
                    new ApplicationUser()
                    {
                        Id = "cda42790-efce-43b0-b569-41648d6c8e82",
                        Email = "kalle@ankeborg.com",
                        NormalizedEmail = "kalle@ankeborg.com",
                        UserName = "kalle@ankeborg.com",
                        NormalizedUserName = "kalle@ankeborg.com",
                        EmailConfirmed = true,
                        PasswordHash = hasher.HashPassword(null, "123456789Ab!"),
						FirstName = "Kalle",
						LastName= "Anka"
                    }
                );

   //         
			//modelBuilder.Entity<Broker>()
			//	.HasData(
			//		new Broker()
			//		{
			//			BrokerFirm = new BrokerFirm() { BrokerFirmId = 1, Name = "Ankeborg" }, 
			//			BrokerId = 1,
			//			User = new ApplicationUser() { 
							
   //                     }
			//		}
			//	);

        }

		/// <summary>
		/// Adds entity seeds to the model creation
		/// </summary>
		/// <param name="modelBuilder"></param>
		private void AddEntitySeeds(ModelBuilder modelBuilder)
		{
			// =====================================
			// HousingCategories
			// =====================================
			int categoryId = 1;
			var categorySeedFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "HousingCategoryNames.txt");
			var seedCategories = File.ReadAllLines(categorySeedFile).Select(x => new HousingCategory(x) { HousingCategoryId = categoryId++}).ToList();
			modelBuilder.Entity<HousingCategory>()
				.HasData(seedCategories);

			// =====================================
			// Municipalities
			// =====================================
			int municipalityId = 1;
			var municipalitiesSeedFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "MunicipalitiesNames.txt");
			var seedMunicipalities = File.ReadAllLines(municipalitiesSeedFile).Select(x => new Municipality(x) { MunicipalityId = municipalityId++}).ToList();
			modelBuilder.Entity<Municipality>()
				.HasData(seedMunicipalities);
		}



		#endregion
	}
}
