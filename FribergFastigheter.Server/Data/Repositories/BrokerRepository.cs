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
            return await applicationDbContext.Brokers
                .Include(x => x.BrokerFirm).ThenInclude(x => x.Logotype)
                .Include(x => x.ProfileImage)
                .AsNoTracking().FirstOrDefaultAsync(b => b.BrokerId == id);

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
        public Task<Image?> GetImagebyBrokerId(int brokerId)
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
            var broker = await GetBrokerByIdAsync(brokerId);

            if (broker == null)
            {
                throw new Exception($"The broker object with ID '{broker}' was not found.");
            }

            // We return entities as no tracking.
            applicationDbContext.Brokers.Attach(broker);
            broker.ProfileImage = image;
           

            await applicationDbContext.SaveChangesAsync();
        }

        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        public async Task<int> DeleteImage(int brokerId)
        {
            var broker = await GetBrokerByIdAsync(brokerId);

            if (broker == null)
            {
                throw new Exception($"The broker object with ID '{broker}' was not found.");
            }
            applicationDbContext.Brokers.Attach(broker);
            applicationDbContext.Entry(broker.ProfileImage).State = EntityState.Deleted;
            broker.ProfileImage = null;

            return await applicationDbContext.SaveChangesAsync();
        }

        #endregion
    }
}
