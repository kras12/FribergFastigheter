using FribergFastigheter.Data.Entities;
using FribergFastigheter.HelperClasses;
using FribergFastigheterApi.Data.Entities;
using FribergFastigheterApi.HelperClasses.Data;
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
	public class ApplicationDbContext : DbContext
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

			AddEntitySeeds(modelBuilder);

			modelBuilder.Entity<BrokerFirm>()
				.HasMany(x => x.Brokers)
				.WithOne(x => x.BrokerFirm)
				.OnDelete(DeleteBehavior.Cascade);	
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
