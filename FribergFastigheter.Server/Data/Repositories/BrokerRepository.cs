using FribergFastigheter.Server.Data.Entities;
using FribergFastigheter.Server.Data.Interfaces;
using FribergFastigheterApi.Data.DatabaseContexts;
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
            applicationDbContext.ChangeTracker.Clear();
            applicationDbContext.Users.Entry(broker.User).State = EntityState.Unchanged;
            applicationDbContext.BrokerFirms.Entry(broker.BrokerFirm).State = EntityState.Unchanged;
            applicationDbContext.Brokers.Add(broker);
            await applicationDbContext.SaveChangesAsync();
        }

        /// <!-- Author: Marcus, Jimmie -->
        /// <!-- Co Authors: -->
        public async Task DeleteAsync(int brokerId)
        {
            var fetchedBroker = applicationDbContext.Brokers.Where(x => x.BrokerId == brokerId)
                .FirstOrDefault();

            if (fetchedBroker == null)
            {
                throw new Exception($"The broker object with ID '{brokerId}' was not found.");
            }

            if (fetchedBroker.ProfileImage != null)
            {
                applicationDbContext.Entry(fetchedBroker.ProfileImage!).State = EntityState.Deleted;
            }
            
            applicationDbContext.Brokers.Remove(fetchedBroker);
            await applicationDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Updates a broker and its user entity.
        /// </summary>
        /// <param name="broker"></param>
        /// <returns></returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task UpdateAsync(Broker broker)
        {
            applicationDbContext.BrokerFirms.Entry(broker.BrokerFirm).State = EntityState.Unchanged;
            applicationDbContext.Brokers.Entry(broker).State = EntityState.Modified;
            applicationDbContext.Users.Entry(broker.User).State = EntityState.Modified;
            broker.User.NormalizedEmail = broker.User.Email!.ToUpper();
            broker.User.NormalizedUserName = broker.User.UserName!.ToUpper();
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task<Broker?> GetBrokerByIdAsync(int id)
        {
            return await applicationDbContext.Brokers
                .AsNoTracking().FirstOrDefaultAsync(b => b.BrokerId == id);
        }

        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<Broker?> GetBrokerByUserIdAsync(string id)
        {
            return await applicationDbContext.Brokers
                .AsNoTracking().FirstOrDefaultAsync(b => b.User.Id == id);
        }

        /// <!-- Author: Marcus, Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<List<Broker>> GetBrokersAsync(int? brokerFirmId = null, bool includeDeleted = false)
        {
            var query = applicationDbContext.Brokers
                .AsNoTracking();

            if (!includeDeleted)
            {
                query = query.Where(x => !x.IsDeleted);
            }

            if (brokerFirmId != null)
            {
                query = query.Where(x => x.BrokerFirm.BrokerFirmId == brokerFirmId);
            }

            return await query.ToListAsync();
        }

        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        public Task<bool> Exists(int id)
        {
            return applicationDbContext.Brokers.AnyAsync(x => x.BrokerId == id);
        }

        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        public Task<Image?> GetProfileImage(int brokerId)
        {
          return applicationDbContext
                .Brokers.Where(x => x.BrokerId == brokerId)
                .AsNoTracking()
                .Select(x => x.ProfileImage)
                .SingleOrDefaultAsync();
        }

        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        public async Task AddImage(int brokerId, Image image)
        {
            var broker = applicationDbContext.Brokers.Where(x => x.BrokerId == brokerId)
                .FirstOrDefault();

            if (broker == null)
            {
                throw new Exception($"The broker object with ID '{broker}' was not found.");
            }

            broker.ProfileImage = image;
           

            await applicationDbContext.SaveChangesAsync();
        }

        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        public async Task DeleteProfileImage(int brokerId)
        {
            var broker = applicationDbContext.Brokers.Where(x => x.BrokerId == brokerId)
                .FirstOrDefault();

            if (broker == null)
            {
                throw new Exception($"The broker object with ID '{broker}' was not found.");
            }
            applicationDbContext.Entry(broker.ProfileImage!).State = EntityState.Deleted;
            broker.ProfileImage = null;

            await applicationDbContext.SaveChangesAsync();
        }

        #endregion
    }
}
