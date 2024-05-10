using FribergFastigheter.Client.Models.Housing;
using FribergFastigheter.Client.Models.Image;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace FribergFastigheter.Client.Components.Housing
{
    /// <summary>
    /// Component that shows housing details.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class HousingDetails
    {
        #region Properties

        /// <summary>
        /// The housing object to show details for. 
        /// </summary>
        [Parameter]
#pragma warning disable CS8618 
        public HousingViewModel Housing { get; set; } = new();

#pragma warning restore CS8618

        /// <summary>
        /// Returns image slides to show in the image slide show. 
        /// </summary>
        public List<ImageSlideViewModel> ImageSlides
        {
            get
            {
                return Housing.Images.Select(x => new ImageSlideViewModel(x)).ToList();
            }
        }

        /// <summary>
        /// Returns true if the image gallery has been opened. 
        /// </summary>
        public bool IsImageGalleryActive { get; set; } = false;

        #endregion

        #region Methods

        /// <summary>
        /// Closes the image gallery and returns to the housing details.
        /// </summary>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public void CloseImageGallery()
        {
            IsImageGalleryActive = false;
        }

        /// <summary>
        /// Opens the image gallery.
        /// </summary>
        public void OpenImageGallery()
        {
            IsImageGalleryActive = true;
        }

        #endregion
    }
}
