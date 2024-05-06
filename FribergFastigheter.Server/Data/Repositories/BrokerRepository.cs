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
            applicationDbContext.BrokerFirms.Attach(broker.BrokerFirm);
            await applicationDbContext.Brokers.AddAsync(broker);
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

        public async Task UpdateAsync(Broker broker) 
        {
            applicationDbContext.BrokerFirms.Attach(broker.BrokerFirm);
            applicationDbContext.Update(broker);
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

        public async Task<List<Broker>> GetAllBrokersAsync()
        {
            return await applicationDbContext.Brokers.AsNoTracking().ToListAsync();
        }

        public Task<List<Broker>> GetAllBrokersByBrokerFirmIdAsync(int brokerFirmId)
        {
            return applicationDbContext.Brokers
                .Where(x => x.BrokerFirm.BrokerFirmId == brokerFirmId)
				.AsNoTracking()
				.ToListAsync();
        }

        public Task<bool> IsOwnedByBrokerFirm(int brokerId, int BrokerFirmId)
        {
            return applicationDbContext.Brokers.AnyAsync(x => x.BrokerId == brokerId && x.BrokerFirm.BrokerFirmId == BrokerFirmId);
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
        public Task<bool> OwnsImage(int brokerId, int imageId)
        {
            return applicationDbContext.Brokers.Where(x => x.BrokerId == brokerId).AnyAsync(x => x.ProfileImage != null && x.ProfileImage.ImageId == imageId);
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
