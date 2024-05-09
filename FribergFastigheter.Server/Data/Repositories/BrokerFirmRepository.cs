using FribergFastigheter.Server.Data.Interfaces;
using FribergFastigheter.Shared.Dto.Statistics;
using FribergFastigheterApi.Data.DatabaseContexts;
using FribergFastigheter.Server.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace FribergFastigheter.Server.Data.Repositories
{
    /// <summary>
	/// Repository for BrokerFirms
	/// </summary>
	/// <!-- Author: Marcus -->
	/// <!-- Co Authors: Jimmie -->

    public class BrokerFirmRepository : IBrokerFirmRepository
    {
        #region Fields

        private readonly ApplicationDbContext applicationDbContext;

        #endregion

        #region Constructors

        public BrokerFirmRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        #endregion

        #region Methods

        public async Task AddAsync(BrokerFirm brokerFirm)
        {
            await applicationDbContext.BrokerFirms.AddAsync(brokerFirm);
            await applicationDbContext.SaveChangesAsync();
        }

        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task AddAsync(List<BrokerFirm> brokerFirms)
        {
            await applicationDbContext.BrokerFirms.AddRangeAsync(brokerFirms);            
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(BrokerFirm brokerFirm)
        {
            applicationDbContext.BrokerFirms.Remove(brokerFirm);
            await applicationDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Returns statistics for a broker firm. 
        /// </summary>
        /// <param name="brokerFirmId">The ID of the broker firm to fetch statistics for.</param>
        /// <returns>A <see cref="Task"/> containining a <see cref="BrokerFirmStatisticsDto"/> object with the statistics.</returns>
        /// <exception cref="Exception"></exception>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<BrokerFirmStatisticsDto?> GetStatistics(int brokerFirmId)
        {
            var brokerFirm = await applicationDbContext.BrokerFirms.Where(x => x.BrokerFirmId == brokerFirmId).FirstOrDefaultAsync();

            if (brokerFirm == null)
            {
                return null;
            }            

            BrokerFirmStatisticsDto result = new();
            result.BrokerFirmId = brokerFirm.BrokerFirmId;
            result.BrokerFirmName = brokerFirm.Name;
            result.HousingCount = await applicationDbContext.Housings.CountAsync(x => x.BrokerFirm.BrokerFirmId == brokerFirmId);
            result.BrokerCount = await applicationDbContext.Brokers.CountAsync(x => x.BrokerFirm.BrokerFirmId == brokerFirmId);
            result.HousingCountPerCategory = await applicationDbContext.Housings.Where(x => x.BrokerFirm.BrokerFirmId == brokerFirmId)
                .GroupBy(                
                    key => key.Category,
                    (category, housing) => new StatisticItemDto()
                    {
                        Key = category.CategoryName,
                        Value = housing.Count()
                })
                .ToListAsync();

            return result;
        }           

        public async Task UpdateAsync(BrokerFirm brokerFirm)
        {
            foreach (var broker in brokerFirm.Brokers)
            {
                applicationDbContext.Brokers.Attach(broker);
            }
            applicationDbContext.Update(brokerFirm);
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task<BrokerFirm?> GetBrokerFirmByIdAsync(int id)
        {
            return await applicationDbContext.BrokerFirms.AsNoTracking().FirstOrDefaultAsync(b => b.BrokerFirmId == id);
        }

        public async Task<List<BrokerFirm>> GetAllBrokerFirmsAsync()
        {
            return await applicationDbContext.BrokerFirms.AsNoTracking().ToListAsync();
        }

		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors:  -->
		public async Task<int> BrokerCount(int brokerFirmId)
        {
            return await applicationDbContext.BrokerFirms.Where(x => x.BrokerFirmId == brokerFirmId).Select(x => x.Brokers).CountAsync();
        }

		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors:  -->
		public async Task<bool> HaveBroker(int brokerFirmId, int brokerId)
        {
            return await applicationDbContext.BrokerFirms
                .AnyAsync(x => x.BrokerFirmId == brokerFirmId && x.Brokers.Any(x => x.BrokerId == brokerId));
        }

        public async Task<bool> Exists(int brokerFirmId)
        {
            return await applicationDbContext.BrokerFirms
                .AnyAsync(x => x.BrokerFirmId == brokerFirmId);
        }

        #endregion
    }
}
