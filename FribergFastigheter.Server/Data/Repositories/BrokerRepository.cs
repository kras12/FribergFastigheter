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
        public BrokerRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a broker.
        /// </summary>
        /// <param name="broker">The broker to add.</param>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        public async Task AddAsync(Broker broker)
        {
            applicationDbContext.ChangeTracker.Clear();
            applicationDbContext.Users.Entry(broker.User).State = EntityState.Unchanged;
            applicationDbContext.BrokerFirms.Entry(broker.BrokerFirm).State = EntityState.Unchanged;
            applicationDbContext.Brokers.Add(broker);
            await applicationDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Adds a profile image to the broker.
        /// </summary>
        /// <param name="brokerId">The ID of the broker.</param>
        /// <param name="image">The image to add.</param>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        /// <exception cref="Exception"></exception>
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

        /// <summary>
        /// Deletes a broker.
        /// </summary>
        /// <param name="brokerId">The ID of the broker.</param>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        /// <exception cref="Exception"></exception>
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
        /// Deletes the profile image of a broker.
        /// </summary>
        /// <param name="brokerId">The ID of the broker.</param>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        /// <exception cref="Exception"></exception>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        public async Task DeleteProfileImage(int brokerId)
        {
            var broker = applicationDbContext.Brokers
                .Where(x => x.BrokerId == brokerId)
                .FirstOrDefault();

            if (broker == null)
            {
                throw new Exception($"The broker object with ID '{broker}' was not found.");
            }
            applicationDbContext.Entry(broker.ProfileImage!).State = EntityState.Deleted;
            broker.ProfileImage = null;

            await applicationDbContext.SaveChangesAsync();
            applicationDbContext.ChangeTracker.Clear();
        }

        /// <summary>
        /// Checks whether a broker exists.
        /// </summary>
        /// <param name="id">The ID of the broker.</param>
        /// <returns>True if the broker exists.</returns>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        public Task<bool> Exists(int id)
        {
            return applicationDbContext.Brokers.AnyAsync(x => x.BrokerId == id);
        }

        /// <summary>
        /// Gets a broker by ID.
        /// </summary>
        /// <param name="id">The ID of the broker.</param>
        /// <returns>The broker if found.</returns>
        public async Task<Broker?> GetBrokerByIdAsync(int id)
        {
            return await applicationDbContext.Brokers
                .AsNoTracking().FirstOrDefaultAsync(b => b.BrokerId == id);
        }

        /// <summary>
        /// Gets a broker by the user ID.
        /// </summary>
        /// <param name="id">The user ID of the broker.</param>
        /// <returns>The broker if found.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<Broker?> GetBrokerByUserIdAsync(string id)
        {
            return await applicationDbContext.Brokers
                .AsNoTracking().FirstOrDefaultAsync(b => b.User.Id == id);
        }

        /// <summary>
        /// Gets all brokers that matches the criterias.
        /// </summary>
        /// <param name="brokerFirmId">Sets the broker firm ID as a filter.</param>
        /// <param name="includeDeleted">True to include deleted brokers.</param>
        /// <returns>A collection of the brokers found.</returns>
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

        /// <summary>
        /// Gets all brokers with an aggregated housing count.
        /// </summary>
        /// <param name="brokerFirmId">Sets the broker firm ID as a filter.</param>
        /// <param name="includeDeleted">True to include deleted brokers.</param>
        /// <returns>A collection of the brokers found.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<List<BrokerWithHousingCount>> GetBrokersWithHousingCountAsync(int? brokerFirmId = null, bool includeDeleted = false)
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

            return await query
                .GroupJoin(applicationDbContext.Housings,
                    broker => broker.BrokerId,
                    housing => housing.Broker.BrokerId,
                    (broker, housings) => new BrokerWithHousingCount(broker, housings.Count())
                )
                .ToListAsync();
        }

        /// <summary>
        /// Gets the profile image of the broker.
        /// </summary>
        /// <param name="brokerId">The ID of the broker.</param>
        /// <returns>The profile image if found.</returns>
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

        /// <summary>
        /// Updates a broker and its user entity.
        /// </summary>
        /// <param name="broker"></param>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task UpdateAsync(Broker broker)
        {
			applicationDbContext.ChangeTracker.Clear();
			applicationDbContext.BrokerFirms.Entry(broker.BrokerFirm).State = EntityState.Unchanged;
            applicationDbContext.Brokers.Entry(broker).State = EntityState.Modified;
            applicationDbContext.Users.Entry(broker.User).State = EntityState.Modified;
            broker.User.NormalizedEmail = broker.User.Email!.ToUpper();
            broker.User.NormalizedUserName = broker.User.UserName!.ToUpper();
            await applicationDbContext.SaveChangesAsync();
        }

        #endregion
    }
}
