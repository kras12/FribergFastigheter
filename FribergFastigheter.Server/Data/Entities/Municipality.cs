using System.ComponentModel.DataAnnotations;

namespace FribergFastigheterApi.Data.Entities
{
	/// <summary>
	/// Entity class that represents a municipality.
	/// </summary>
	/// <!-- Author: Jimmie -->
	/// <!-- Co Authors: -->
	public class Municipality
	{
        #region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
        public Municipality()
        {
				
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="municipalityName">The name of the municipality.</param>
        /// <exception cref="ArgumentException"></exception>
        public Municipality(string municipalityName)
		{
			if (string.IsNullOrEmpty(municipalityName))
			{
				throw new ArgumentException($"The value of parameter '{nameof(municipalityName)}' can't be null or empty.", nameof(municipalityName));
			}

			MunicipalityName = municipalityName;
		}

		#endregion

		#region Properties

		/// <summary>
		/// The ID of the municipality.
		/// </summary>
		[Key]
		public int MunicipalityId { get; set; }

		/// <summary>
		/// The name of the municipality.
		/// </summary>
		public string MunicipalityName { get; set; } = "";
		
		#endregion
	}
}
