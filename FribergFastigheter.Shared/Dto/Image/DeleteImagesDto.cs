using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergFastigheter.Shared.Dto.Image
{
    /// <summary>
    /// A DTO class that stores a collection of IDs for the images to delete. 
    /// </summary>
    public class DeleteImagesDto
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public DeleteImagesDto()
        {

        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="housingId">The housing object that owns the images. </param>
        /// <param name="imageIds">A collection of IDs for the images to delete. </param>
        public DeleteImagesDto(int housingId, List<int>? imageIds = null)
        {
            HousingId = housingId;
            ImageIds = imageIds != null ? imageIds : new();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The housing object that owns the images. 
        /// </summary>
        public int HousingId { get; set; }

        /// <summary>
        /// A collection of IDs for the images to delete. 
        /// </summary>
        public List<int> ImageIds { get; set; } = new();

        #endregion
    }
}
