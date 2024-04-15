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
	/// <!-- Co Authors: -->

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

        public async Task<Housing?> GetHousingByIdAsync(int id)
        {
            return await applicationDbContext.Housings
                .Include(x => x.Broker)
                .Include(x => x.BrokerFirm)
                .Include(x => x.Category)
                .Include(x => x.Municipality)
                .FirstOrDefaultAsync(b => b.HousingId == id);
        }

        public async Task<List<Housing>> GetAllHousingAsync()
        {
            return await applicationDbContext.Housings
                .Include(x => x.Broker)
                .Include(x => x.BrokerFirm)
                .Include(x => x.Category)
                .Include(x => x.Municipality)
                .ToListAsync();
        }

        #endregion
    }
}
