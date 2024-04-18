using System.ComponentModel.DataAnnotations;

namespace FribergFastigheterApi.Data.Entities
{
	/// <summary>
	/// An entity class that represent an image.
	/// </summary>
	/// <!-- Author: Jimmie -->
	/// <!-- Co Authors: -->
	public class Image
	{

        #region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
        public Image()
        {
            
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        public Image(string fileName)
		{
			if (string.IsNullOrEmpty(fileName))
			{
				throw new ArgumentException($"The value of parameter '{nameof(fileName)}' can't be null or empty.", nameof(fileName));
			}

			FileName = fileName;
		}

		#endregion

		#region Properties

		/// <summary>
		/// The ID of the image.
		/// </summary>
		[Key]
		public int ImageId { get; set; }

		/// <summary>
		/// The name of the file.
		/// </summary>
		[Required]
		public string FileName { get; set; } = "";

		#endregion
	}
}
