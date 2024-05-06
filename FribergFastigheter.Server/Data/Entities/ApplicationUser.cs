using Microsoft.AspNetCore.Identity;

namespace FribergFastigheter.Server.Data.Entities
{
    /// <summary>
    /// An entity class that represent a User.
    /// </summary>
    /// <!-- Author: Jimmie, Marcus -->
    /// <!-- Co Authors: -->
    public class ApplicationUser : IdentityUser
    {        
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public ApplicationUser()
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="firstName">The first name of the user.</param>
        /// <param name="lastName">The last name of the user.</param>
        /// <param name="email">The email of the user.</param>
        /// <param name="phoneNumber">The phonenumber of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="emailConfirmed">True to set email as confirmed.</param>param
        /// <exception cref="ArgumentException"></exception>
        public ApplicationUser(string firstName, string lastName, string email, string phoneNumber, string password, bool emailConfirmed = false)
        {
            #region Checks

            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentException($"The value of parameter '{nameof(firstName)}' can't be null or empty.", nameof(firstName));
            }

            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentException($"The value of parameter '{nameof(lastName)}' can't be null or empty.", nameof(lastName));
            }

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException($"The value of parameter '{nameof(email)}' can't be null or empty.", nameof(email));
            }

            if (string.IsNullOrEmpty(phoneNumber))
            {
                throw new ArgumentException($"The value of parameter '{nameof(phoneNumber)}' can't be null or empty.", nameof(phoneNumber));
            }

            #endregion

            UserName = email;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            EmailConfirmed = emailConfirmed;
            NormalizedEmail = email.ToUpper();
            NormalizedUserName = email.ToUpper();
            var hasher = new PasswordHasher<ApplicationUser>();
            PasswordHash = hasher.HashPassword(null!, password);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The first name of the user.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the user.
        /// </summary>
        public string LastName { get; set; }

        #endregion

    }
}
