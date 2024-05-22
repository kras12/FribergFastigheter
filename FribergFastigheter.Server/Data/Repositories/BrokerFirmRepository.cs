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

        /// <summary>
        /// The injected DB context.
        /// </summary>
        private readonly ApplicationDbContext applicationDbContext;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="applicationDbContext">The injected DB context.</param>
        public BrokerFirmRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a broker firm.
        /// </summary>
        /// <param name="brokerFirm">The broker firm.</param>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        public async Task AddAsync(BrokerFirm brokerFirm)
        {
            await applicationDbContext.BrokerFirms.AddAsync(brokerFirm);
            await applicationDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Adds a collection of broker firms.
        /// </summary>
        /// <param name="brokerFirms">The broker firms to add.</param>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task AddAsync(List<BrokerFirm> brokerFirms)
        {
            await applicationDbContext.BrokerFirms.AddRangeAsync(brokerFirms);            
            await applicationDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Gets the broker count for a broker firm.
        /// </summary>
        /// <param name="brokerFirmId">The ID of the broker firm.</param>
        /// <returns>The number of brokers in the broker firm.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors:  -->
        public async Task<int> BrokerCount(int brokerFirmId)
        {
            return await applicationDbContext.BrokerFirms.Where(x => x.BrokerFirmId == brokerFirmId).Select(x => x.Brokers).CountAsync();
        }

        /// <summary>
        /// Deletes a broker firm.
        /// </summary>
        /// <param name="brokerFirm">The broker firm.</param>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        public async Task DeleteAsync(BrokerFirm brokerFirm)
        {
            applicationDbContext.BrokerFirms.Remove(brokerFirm);
            await applicationDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Checks if a broker firm exists.
        /// </summary>
        /// <param name="brokerFirmId">The ID of the broker firm.</param>
        /// <returns>True if the broker firm exists.</returns>
        public async Task<bool> Exists(int brokerFirmId)
        {
            return await applicationDbContext.BrokerFirms
                .AnyAsync(x => x.BrokerFirmId == brokerFirmId);
        }

        /// <summary>
        /// Gets all broker firms.
        /// </summary>
        /// <returns>A collection of broker firms that were found.</returns>
        public async Task<List<BrokerFirm>> GetAllBrokerFirmsAsync()
        {
            return await applicationDbContext.BrokerFirms.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Gets a broker firm by ID.
        /// </summary>
        /// <param name="id">The ID of the broker firm.</param>
        /// <param name="includeDeletedBrokers">True to include deleted brokers.</param>
        /// <returns>A broker firm if found.</returns>
        public async Task<BrokerFirm?> GetBrokerFirmByIdAsync(int id, bool? includeDeletedBrokers = true)
        {
            var firm = await applicationDbContext.BrokerFirms.AsNoTracking().FirstOrDefaultAsync(b => b.BrokerFirmId == id);

            if (includeDeletedBrokers != null && includeDeletedBrokers.Value == false)
            {
                firm!.Brokers.RemoveAll(x => x.IsDeleted);
            }

            return firm;
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
            result.HousingCount = await applicationDbContext.Housings.CountAsync(x => x.Broker.BrokerFirm.BrokerFirmId == brokerFirmId);
            result.BrokerCount = await applicationDbContext.Brokers.CountAsync(x => x.BrokerFirm.BrokerFirmId == brokerFirmId);
            result.HousingCountPerCategory = await applicationDbContext.Housings.Where(x => x.Broker.BrokerFirm.BrokerFirmId == brokerFirmId)
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

        /// <summary>
        /// Checks if a broker firm have a specific broker.
        /// </summary>
        /// <param name="brokerFirmId">The ID of the broker firm.</param>
        /// <param name="brokerId">The ID of the broker.</param>
        /// <returns>True if the broker belongs to the firm.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors:  -->
        public async Task<bool> HaveBroker(int brokerFirmId, int brokerId)
        {
            return await applicationDbContext.BrokerFirms
                .AnyAsync(x => x.BrokerFirmId == brokerFirmId && x.Brokers.Any(x => x.BrokerId == brokerId));
        }

        /// <summary>
        /// Updates a broker firm.
        /// </summary>
        /// <param name="brokerFirm">The broker firm.</param>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        public async Task UpdateAsync(BrokerFirm brokerFirm)
        {
            foreach (var broker in brokerFirm.Brokers)
            {
                applicationDbContext.Brokers.Attach(broker);
            }
            applicationDbContext.Update(brokerFirm);
            await applicationDbContext.SaveChangesAsync();
        }

        #endregion
    }
}
