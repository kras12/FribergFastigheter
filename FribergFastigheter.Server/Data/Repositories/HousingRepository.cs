using FribergFastigheter.Data.Entities;
using FribergFastigheter.Server.Data.Interfaces;
using FribergFastigheterApi.Data.DatabaseContexts;
using FribergFastigheterApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

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

        public async Task UpdateAsync(Housing housing)
        {
            applicationDbContext.Brokers.Attach(housing.Broker);
            applicationDbContext.BrokerFirms.Attach(housing.BrokerFirm);
            applicationDbContext.HousingCategories.Attach(housing.Category);
            applicationDbContext.Municipalities.Attach(housing.Municipality);
            applicationDbContext.Update(housing);
            await applicationDbContext.SaveChangesAsync();
        }

		/// <!-- Author: Marcus, Jimmie -->
		/// <!-- Co Authors: -->
		public async Task<Housing?> GetHousingByIdAsync(int id)
        {
           return await applicationDbContext.Housings
                .Include(x => x.Broker)
                .Include(x => x.BrokerFirm)
                .Include(x => x.Category)
                .Include(x => x.Municipality)
                .Include(x => x.Images)
                .Where(x => x.HousingId == id)
                .FirstOrDefaultAsync();
		}

		/// <!-- Author: Marcus, Jimmie -->
		/// <!-- Co Authors: -->
		public async Task<List<Housing>> GetAllHousingAsync(int? municipalityId = null, int? brokerId = null, int? brokerFirm = null)
        {
            var query = applicationDbContext.Housings
                .Include(x => x.Broker)
                .Include(x => x.BrokerFirm)
                .Include(x => x.Category)
                .Include(x => x.Municipality)
                .Include(x => x.Images)
                .AsQueryable();

            if (municipalityId != null)
            {
				query = query.Where(x => x.Municipality.MunicipalityId == municipalityId);
            }

			if (brokerId != null)
			{
				query = query.Where(x => x.Broker.BrokerId == brokerId);
			}

			if (brokerFirm != null)
			{
				query = query.Where(x => x.BrokerFirm.BrokerFirmId == brokerFirm);
			}

			return await query.ToListAsync();
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

		#endregion
	}
}
