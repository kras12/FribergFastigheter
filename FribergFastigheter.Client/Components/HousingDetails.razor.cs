using FribergFastigheter.Client.Models;
using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Components
{
    /// <summary>
    /// Component that shows housing details.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class HousingDetails
    {
        #region Properties

        [Parameter]
#pragma warning disable CS8618 
        public HousingViewModel Housing { get; set; } = new();
#pragma warning restore CS8618

        public List<ImageSlideViewModel> ImageSlides
        {
            get
            {
                return Housing.Images.Select(x => new ImageSlideViewModel(x)).ToList();
            }
        }

        #endregion
    }
}
