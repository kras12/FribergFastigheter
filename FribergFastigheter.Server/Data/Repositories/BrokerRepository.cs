using FribergFastigheter.Data.Entities;
using FribergFastigheter.Server.Data.Interfaces;
using FribergFastigheterApi.Data.DatabaseContexts;
using FribergFastigheterApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;

namespace FribergFastigheter.Server.Data.Repositories
{
	/// <summary>
	/// Repository for Brokers
	/// </summary>
	/// <!-- Author: Marcus -->
	/// <!-- Co Authors: -->
	public class BrokerRepository : IBrokerRepository
    {
        #region Fields

        private readonly ApplicationDbContext applicationDbContext;

        #endregion

        #region Constructors

        public BrokerRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        #endregion

        #region Methods

        public async Task AddAsync(Broker broker)
        {
            applicationDbContext.BrokerFirms.Attach(broker.BrokerFirm);
            await applicationDbContext.Brokers.AddAsync(broker);
            await applicationDbContext.SaveChangesAsync();
        }

        public Task DeleteAsync(int brokerId)
        {
            return DeleteAsync(new Broker() { BrokerId = brokerId });
        }

        public async Task DeleteAsync(Broker broker)
        {
            applicationDbContext.Brokers.Remove(broker);
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Broker broker) 
        {
            applicationDbContext.BrokerFirms.Attach(broker.BrokerFirm);
            applicationDbContext.Update(broker);
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task<Broker?> GetBrokerByIdAsync(int id)
        {
            return await applicationDbContext.Brokers.Include(x => x.BrokerFirm).AsNoTracking().FirstOrDefaultAsync(b => b.BrokerId == id);
        }

        public async Task<List<Broker>> GetAllBrokersAsync()
        {
            return await applicationDbContext.Brokers.Include(x => x.BrokerFirm).AsNoTracking().ToListAsync();
        }

        public Task<List<Broker>> GetAllBrokersByBrokerFirmIdAsync(int brokerFirmId)
        {
            return applicationDbContext.Brokers
                .Include(x => x.BrokerFirm)
                .Where(x => x.BrokerFirm.BrokerFirmId == brokerFirmId)
				.AsNoTracking()
				.ToListAsync();
        }

        /// <summary>
        /// Returns true if the broker belongs to the broker firm.
        /// </summary>
        /// <param name="brokerId">The ID of the broker.</param>
        /// <param name="brokerFirmId">The ID of the broker firm.</param>
        /// <returns></returns>
        /// <!-- Author: Marcus -->
        /// <!-- Co Authors: -->
        public Task<bool> BelongsToBrokerFirm(int brokerId, int brokerFirmId)
        {
            return applicationDbContext.Brokers.Where(x => x.BrokerId == brokerId).AnyAsync(x => x.BrokerFirm.BrokerFirmId == brokerFirmId);
        }

		#endregion
	}
}
