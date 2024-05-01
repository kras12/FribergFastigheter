using FribergFastigheter.Data.Entities;
using FribergFastigheter.Server.Data.Interfaces;
using FribergFastigheterApi.Data.DatabaseContexts;
using FribergFastigheterApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FribergFastigheter.Server.Data.Repositories
{
    /// <summary>
	/// Repository for Housings
	/// </summary>
	/// <!-- Author: Marcus -->
	/// <!-- Co Authors: Jimmie -->

    public class HousingRepository : IHousingRepository
    {
        #region Fields

    private readonly ApplicationDbContext applicationDbContext;

        #endregion

        #region Constructors

        public HousingRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        #endregion

        #region Methods

        public async Task AddAsync(Housing housing)
        {
            applicationDbContext.Brokers.Attach(housing.Broker);
            applicationDbContext.BrokerFirms.Attach(housing.BrokerFirm);
            applicationDbContext.HousingCategories.Attach(housing.Category);
            applicationDbContext.Municipalities.Attach(housing.Municipality);
            await applicationDbContext.Housings.AddAsync(housing);
            await applicationDbContext.SaveChangesAsync();
        }

		public Task DeleteAsync(int housingId)
		{
            return DeleteAsync(new Housing() { HousingId = housingId });
		}

		public async Task DeleteAsync(Housing housing)
        {
            applicationDbContext.Housings.Remove(housing);
            await applicationDbContext.SaveChangesAsync();
        }

		/// <!-- Author: Marcus, Jimmie -->
		/// <!-- Co Authors: -->
		public async Task UpdateAsync(Housing housing)
        {
			// We must store the final images outside of the entity for our comparisions,
			// since EF Core will add tracked images to the entity if they don't exist.
			var targetImages = housing.Images.ToList();
			applicationDbContext.Update(housing);

			applicationDbContext.Brokers.Attach(housing.Broker);
            applicationDbContext.BrokerFirms.Attach(housing.BrokerFirm);
            applicationDbContext.HousingCategories.Attach(housing.Category);
            applicationDbContext.Municipalities.Attach(housing.Municipality);

			// EF Core will add missing images to the entity here.
			var databaseImages = await applicationDbContext.Housings.Where(x => x.HousingId == housing.HousingId).SelectMany(x => x.Images).ToListAsync();

			// Modify status for deleted images
			databaseImages.ExceptBy(targetImages.Select(x => x.ImageId), y => y.ImageId).ToList()
				.ForEach(deletedImage =>
				{
					applicationDbContext.Entry(deletedImage).State = EntityState.Deleted;
				});

			await applicationDbContext.SaveChangesAsync();
        }

		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors: -->
		public async Task<Broker> GetBroker(int housingId)
		{
			return await applicationDbContext.Housings
				 .Where(x => x.HousingId == housingId)
				 .AsNoTracking()
                 .Select(x => x.Broker)
				 .FirstAsync();
		}

		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors: -->
		public Task<List<Housing>> GetHousingsByBrokerId(int brokerId, int? limitImagesPerHousing = null)
		{
            return GetHousingsInternalAsync(brokerId, limitImagesPerHousing: limitImagesPerHousing).ToListAsync();
		}

        /// <!-- Author: Marcus, Jimmie -->
        /// <!-- Co Authors: -->
        public Task<List<Housing>> GetHousingsByBrokerFirmId(int brokerFirmId, int? limitImagesPerHousing = null)
        {
            return GetHousingsInternalAsync(brokerFirm: brokerFirmId, limitImagesPerHousing: limitImagesPerHousing).ToListAsync();
        }

        /// <!-- Author: Marcus, Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<Housing?> GetHousingByIdAsync(int id)
        {
           return await applicationDbContext.Housings
                .Where(x => x.HousingId == id)
				.AsNoTracking()
				.FirstOrDefaultAsync();
		}

        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        private IQueryable<Housing> GetHousingsInternalAsync(int? brokerId = null, int? brokerFirm = null, int? municipalityId = null,
            int? housingCategoryId = null, int? limitHousings = null, int? limitImagesPerHousing = null, decimal? minPrice = null,
            decimal? maxPrice = null, double? minLivingArea = null, double? maxLivingArea = null, int? offsetRows = null)
        {
            #region Checks

            if (minPrice != null && minPrice < 0)
            {
                throw new ArgumentException("The min price can't be negative.", nameof(minPrice));
            }

            if (maxPrice != null && (maxPrice < 0) || (minPrice != null && maxPrice < minPrice))
            {
                throw new ArgumentException("The max price can't be negative or less than the min price.", nameof(maxPrice));
            }

            if (minLivingArea != null && minLivingArea < 0)
            {
                throw new ArgumentException("The min living area can't be negative.", nameof(minPrice));
            }

            if (maxLivingArea != null && (maxLivingArea < 0) || (minLivingArea != null && maxLivingArea < minLivingArea))
            {
                throw new ArgumentException("The max living area can't be negative or less than the min living area.", nameof(maxLivingArea));
            }

            if (offsetRows < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offsetRows), "The offset rows value can't be negative.");
            }

            #endregion

            var query = applicationDbContext.Housings
                .AsNoTracking()
                .AsQueryable();

            if (municipalityId != null)
            {
                query = query.Where(x => x.Municipality.MunicipalityId == municipalityId);
            }

            if (housingCategoryId != null)
            {
                query = query.Where(x => x.Category.HousingCategoryId == housingCategoryId);
            }

            if (brokerId != null)
            {
                query = query.Where(x => x.Broker.BrokerId == brokerId);
            }

            if (brokerFirm != null)
            {
                query = query.Where(x => x.BrokerFirm.BrokerFirmId == brokerFirm);
            }

            if (minPrice != null)
            {
                query = query.Where(x => x.Price >= minPrice.Value);
            }

            if (maxPrice != null)
            {
                query = query.Where(x => x.Price <= maxPrice.Value);
            }

            if (minLivingArea != null)
            {
                query = query.Where(x => x.LivingArea >= minLivingArea.Value);
            }

            if (maxLivingArea != null)
            {
                query = query.Where(x => x.LivingArea <= maxLivingArea.Value);
            }
            
            if (offsetRows != null)
            {
                query = query.Skip(offsetRows.Value);
            }

            if (limitHousings != null && limitHousings.Value > 0)
            {
                query = query.Take(limitHousings.Value);
            }

            if (limitImagesPerHousing != null && limitImagesPerHousing.Value > 0)
            {
                query = query.Include(x => x.Images
                .OrderBy(x => x.ImageId)
                .Take(3));
            }

            // Letting EF Core use auto include in this instance will be 8-9 time slower.
            query = query
                .IgnoreAutoIncludes()
                .Include(x => x.Broker).ThenInclude(x => x.ProfileImage)
                .Include(x => x.Broker).ThenInclude(x => x.BrokerFirm)
                .Include(x => x.BrokerFirm).ThenInclude(x => x.Logotype)
                .Include(x => x.BrokerFirm).ThenInclude(x => x.Brokers).ThenInclude(x => x.ProfileImage)
                .Include(x => x.Images)
                .Include(x => x.Category)
                .Include(x => x.Municipality);

            return query;
        }

        /// <!-- Author: Marcus, Jimmie -->
        /// <!-- Co Authors: -->
        public Task<List<Housing>> GetHousingsAsync(int? brokerId = null, int? brokerFirm = null, int? municipalityId = null,
            int? housingCategoryId = null, int? limitHousings = null, int? limitImagesPerHousing = null, decimal? minPrice = null, decimal? maxPrice = null,
			double? minLivingArea = null, double? maxLivingArea = null, int? offsetRows = null)
        {
            return GetHousingsInternalAsync(brokerId, brokerFirm, municipalityId, housingCategoryId, limitHousings, limitImagesPerHousing, minPrice, maxPrice,
                    minLivingArea, maxLivingArea, offsetRows).ToListAsync();
		}

        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<int> GetHousingsCountAsync(int? brokerId = null, int? brokerFirm = null, int? municipalityId = null,
            int? housingCategoryId = null, decimal? minPrice = null, decimal? maxPrice = null, double? minLivingArea = null, double? maxLivingArea = null)
        {
            return GetHousingsInternalAsync(brokerId, brokerFirm, municipalityId, housingCategoryId, limitHousings: null, limitImagesPerHousing: null, minPrice: minPrice, maxPrice: maxPrice,
                minLivingArea: minLivingArea, maxLivingArea: maxLivingArea).CountAsync();
        }

        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<bool> IsOwnedByBrokerFirm(int id, int BrokerFirmId)
        {
            return applicationDbContext.Housings.AnyAsync(x => x.HousingId == id && x.BrokerFirm.BrokerFirmId == BrokerFirmId);
		}

		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors: -->
		public Task<bool> Exists(int id)
		{
			return applicationDbContext.Housings.AnyAsync(x => x.HousingId == id);
		}

		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors: -->
		public Task<List<Image>> GetImages(int housingId, List<int>? imageIds = null)
        {
			var query = applicationDbContext
				.Housings.Where(x => x.HousingId == housingId)
				.AsNoTracking()
				.SelectMany(x => x.Images);
				
			if (imageIds != null)
			{
				query = query.Where(x => imageIds.Contains(x.ImageId));
			}

			return query.ToListAsync();
        }

        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<Image?> GetImagebyId(int housingId, int imageId)
		{
			return (await GetImages(housingId, new List<int>() { imageId })).SingleOrDefault();
        }

        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<bool> HousingExists(int housingId)
		{
			return applicationDbContext.Housings.AnyAsync(x => x.HousingId == housingId);
		}

        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<bool> OwnsImage(int housingId, int imageId)
		{
            return applicationDbContext.Housings.Where(x => x.HousingId == housingId).AnyAsync(x => x.Images.Any(x => x.ImageId == imageId));
		}

		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors: -->
        public async Task<bool> OwnsImages(int housingId, List<int> imageIds)
        {
            return await applicationDbContext.Housings
                .Where(x => x.HousingId == housingId)
                .AnyAsync(x => x.Images.Count(x => imageIds.Contains(x.ImageId)) == imageIds.Count);
        }

        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
		public async Task AddImages(int housingId, List<Image> images)
		{
			var housing = await applicationDbContext.Housings
                .Where(x => x.HousingId == housingId)
                .IgnoreAutoIncludes()
                .FirstOrDefaultAsync();

            if (housing == null)
			{
				throw new Exception($"The housing object with ID '{housing}' was not found.");
			}

            // We return entities as no tracking.
            housing.Images.AddRange(images);
			await applicationDbContext.SaveChangesAsync();
		}

        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<int> DeleteImages(int housingId, List<int> imageIds)
		{
			var housing = await GetHousingByIdAsync(housingId);

            if (housing == null)
            {
                throw new Exception($"The housing object with ID '{housing}' was not found.");
            }

			housing.Images.Where(x => imageIds.Contains(x.ImageId)).ToList().ForEach(x => applicationDbContext.Entry(x).State = EntityState.Deleted);
            return await applicationDbContext.SaveChangesAsync();
        }

        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
		public Task<int> DeleteImage(int housingId, int imageId)
		{
			return DeleteImages(housingId, new List<int> { imageId });       
		}

		
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
		public Task<List<HousingCategory>> GetHousingCategories()
		{
			return applicationDbContext.HousingCategories.ToListAsync();
        }

        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<List<Municipality>> GetMunicipalities()
        {
            return applicationDbContext.Municipalities.ToListAsync();
        }

        #endregion
    }
}
