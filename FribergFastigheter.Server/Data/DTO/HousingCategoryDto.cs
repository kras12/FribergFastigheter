using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Server.Data.DTO
{
	/// <summary>
	/// A DTO class that represents a housing category.
	/// </summary>
	/// <!-- Author: Jimmie -->
	/// <!-- Co Authors: -->
	public class HousingCategoryDto
	{
		#region Properties

		/// <summary>
		/// The name of the category.
		/// </summary>
		public string CategoryName { get; private set; } = string.Empty;

		/// <summary>
		/// The ID of the category.
		/// </summary>
		public int HousingCategoryId { get; set; }

		#endregion
	}
}
