using FribergFastigheter.Server.Data.Interfaces;
using FribergFastigheterApi.Data.DatabaseContexts;
using FribergFastigheterApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

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
            return await applicationDbContext.Brokers.Include(x => x.BrokerFirm).FirstOrDefaultAsync(b => b.BrokerId == id);
        }

        public async Task<List<Broker>> GetAllBrokersAsync()
        {
            return await applicationDbContext.Brokers.Include(x => x.BrokerFirm).ToListAsync();
        }


        #endregion
    }
}
