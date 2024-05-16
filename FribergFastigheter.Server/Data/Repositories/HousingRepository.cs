using FribergFastigheter.Server.Data.Entities;
using FribergFastigheter.Server.Data.Interfaces;
using FribergFastigheterApi.Data.DatabaseContexts;
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

        public Task AddAsync(Housing housing)
        {
            return AddAsync(new List<Housing>() { housing });
        }

        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task AddAsync(List<Housing> housings)
        {
            foreach (var housing in housings)
            {
                applicationDbContext.Brokers.Attach(housing.Broker);
                applicationDbContext.HousingCategories.Attach(housing.Category);
                applicationDbContext.Municipalities.Attach(housing.Municipality);
                await applicationDbContext.Housings.AddAsync(housing);
            }
            
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
            applicationDbContext.HousingCategories.Entry(housing.Category).State = EntityState.Unchanged;
            applicationDbContext.Municipalities.Entry(housing.Municipality).State = EntityState.Unchanged;
            applicationDbContext.Brokers.Entry(housing.Broker).State = EntityState.Unchanged;            
            applicationDbContext.Housings.Entry(housing).State = EntityState.Modified;
            await applicationDbContext.SaveChangesAsync();
        }

        /// <!-- Author: Marcus, Jimmie -->
        /// <!-- Co Authors: -->
        public Task<List<Housing>> GetHousingsAsync(int? brokerId = null, int? brokerFirmId = null, int? municipalityId = null,
            int? housingCategoryId = null, int? limitHousings = null, int? limitImagesPerHousing = null, decimal? minPrice = null, decimal? maxPrice = null,
            double? minLivingArea = null, double? maxLivingArea = null, int? offsetRows = null, bool includeDeleted = false)
        {
            return GetHousingsInternalAsync(brokerId: brokerId, brokerFirmId: brokerFirmId, municipalityId: municipalityId, housingCategoryId: housingCategoryId, 
                limitHousings: limitHousings, limitImagesPerHousing: limitImagesPerHousing, minPrice: minPrice, maxPrice: maxPrice,
                    minLivingArea: minLivingArea, maxLivingArea: maxLivingArea, offsetRows: offsetRows, includeDeleted: includeDeleted).ToListAsync();
        }

        /// <!-- Author: Marcus, Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<Housing?> GetHousingByIdAsync(int housingId, int? brokerFirmId = null)
        {
           return await GetHousingsInternalAsync(housingId: housingId, brokerFirmId: brokerFirmId, includeDeleted: true)
				.FirstOrDefaultAsync();
		}

        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        private IQueryable<Housing> GetHousingsInternalAsync(int? brokerId = null, int? brokerFirmId = null, int? municipalityId = null,
            int? housingCategoryId = null, int? limitHousings = null, int? limitImagesPerHousing = null, decimal? minPrice = null,
            decimal? maxPrice = null, double? minLivingArea = null, double? maxLivingArea = null, int? offsetRows = null, int? housingId = null, bool includeDeleted = false)
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

            if (!includeDeleted)
            {
                query = query.Where(x => !x.IsDeleted);
            }

            if (housingId != null)
            {
                query = query.Where(x => x.HousingId == housingId);
            }

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

            if (brokerFirmId != null)
            {
                query = query.Where(x => x.Broker.BrokerFirm.BrokerFirmId == brokerFirmId);
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

            // Letting EF Core use auto include was 8-9 time slower with the old database design.
            // Now the difference is much smaller. As the performance is more acceptable now we can just let EF Core handle this. 
            // EF Core would need tracking enabled to handle the new circular references resulting from the manual include route and this new database design anyway, 
            // so the performance gain could be small and the risk for future bugs would increase. 
            // 
            //query = query
            //    .IgnoreAutoIncludes()
            //    .Include(x => x.Images)
            //    .Include(x => x.Category)
            //    .Include(x => x.Municipality)
            //    .Include(x => x.Broker).ThenInclude(x => x.ProfileImage)
            //    .Include(x => x.Broker).ThenInclude(x => x.BrokerFirm).ThenInclude(x => x.Logotype)
            //    .Include(x => x.Broker).ThenInclude(x => x.BrokerFirm).ThenInclude(x => x.Brokers)
            //    .Include(x => x.Broker).ThenInclude(x => x.User);

            return query;
        }        

        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<int> GetHousingsCountAsync(int? brokerId = null, int? brokerFirmId = null, int? municipalityId = null,
            int? housingCategoryId = null, decimal? minPrice = null, decimal? maxPrice = null, double? minLivingArea = null, double? maxLivingArea = null)
        {
            return GetHousingsInternalAsync(brokerId: brokerId, brokerFirmId: brokerFirmId, municipalityId: municipalityId, housingCategoryId: housingCategoryId, 
                minPrice: minPrice, maxPrice: maxPrice, minLivingArea: minLivingArea, maxLivingArea: maxLivingArea).CountAsync();
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
        public Task<bool> HousingExists(int housingId)
		{
			return applicationDbContext.Housings.AnyAsync(x => x.HousingId == housingId);
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

            housing.Images.AddRange(images);
			await applicationDbContext.SaveChangesAsync();
		}

        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<int> DeleteImages(int housingId, List<int>? imageIds = null)
		{
			var housing = await GetHousingByIdAsync(housingId);

            if (housing == null)
            {
                throw new Exception($"The housing object with ID '{housing}' was not found.");
            }

            var imagesToDelete = imageIds != null ? housing.Images.Where(x => imageIds.Contains(x.ImageId)).ToList() : housing.Images;
            imagesToDelete.ForEach(x => applicationDbContext.Entry(x).State = EntityState.Deleted);
            
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
