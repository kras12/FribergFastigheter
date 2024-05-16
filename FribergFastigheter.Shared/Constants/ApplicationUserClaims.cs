namespace FribergFastigheter.Shared.Constants
{
    /// <summary>
    /// A class that contains user claims.
    /// </summary>
    /// <!-- Author: Marcus, Jimmie -->
    /// <!-- Co Authors: -->
    public class ApplicationUserClaims
    {
        #region Constants

        /// <summary>
        /// The broker firm ID claim.
        /// </summary>
        public const string BrokerFirmId = "BrokerFirmId";

        /// <summary>
        /// The broker ID claim.
        /// </summary>
        public const string BrokerId = "BrokerId";

        /// <summary>
        /// The user ID claim.
        /// </summary>
        public const string UserId = "UserId";

        /// <summary>
        /// The user role claim. 
        /// </summary>
        public const string UserRole = "BrokerRole";

        /// <summary>
        /// The user name claim. 
        /// </summary>
        public const string UserName = "sub";

        /// <summary>
        /// The user email claim. 
        /// </summary>
        public const string Email = "email";

        /// <summary>
        /// The user firstname claim. 
        /// </summary>
        public const string FirstName = "given_name";

        /// <summary>
        /// The user lastname claim. 
        /// </summary>
        public const string LastName = "family_name";

        /// <summary>
        /// The user jti claim. 
        /// </summary>
        public const string Jti = "jti";

        #endregion
    }
}
