using FribergFastigheter.Server.Data.Interfaces;
using FribergFastigheterApi.Data.DatabaseContexts;
using FribergFastigheterApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

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

		public async Task DeleteAsync(BrokerFirm brokerFirm)
        {
            applicationDbContext.BrokerFirms.Remove(brokerFirm);
            await applicationDbContext.SaveChangesAsync();
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
