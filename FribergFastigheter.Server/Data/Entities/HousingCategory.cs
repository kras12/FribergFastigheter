using System.ComponentModel.DataAnnotations;

namespace FribergFastigheterApi.Data.Entities
{
	/// <summary>
	/// An entity class that represents a housing object category.
	/// </summary>
	/// <!-- Author: Jimmie -->
	/// <!-- Co Authors: -->
	public class HousingCategory
	{
        #region Constructors

		/// <summary>
		/// Constructor
		/// </summary>
        public HousingCategory()
        {
            
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="categoryName">The name of the category.</param>
        public HousingCategory(string categoryName)
		{
			if (string.IsNullOrEmpty(categoryName)) 
			{ 
				throw new ArgumentException($"The value of parameter '{nameof(categoryName)}' can't be null or empty.", nameof(categoryName));
			}

			CategoryName = categoryName;
		}

		#endregion

		#region Properties

		/// <summary>
		/// The name of the category.
		/// </summary>
		[Required]
		public string CategoryName { get; private set; } = string.Empty;

		/// <summary>
		/// The ID of the category.
		/// </summary>
		[Key]
		public int HousingCategoryId { get; set; }

		#endregion
	}
}
