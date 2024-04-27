using FribergFastigheter.Client.Models;
using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Components
{
    /// <summary>
    /// Component for displaying image slide shows.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class ImageSlideShow
	{
		#region Fields

		/// <summary>
		/// The current image slide index.
		/// </summary>
		private int _currentSlideIndex = 0;

		/// <summary>
		/// The previous image slide index. 
		/// </summary>
		private int _previousSlideIndex = 0;

		#endregion

		#region Properties

		/// <summary>
		/// The images to show in the slide show.
		/// </summary>
		[Parameter]
		public List<ImageSlideViewModel> Images { get; set; } = new();

		/// <summary>
		/// True to show navigational dots under the images.
		/// </summary>
		[Parameter]
		public bool ShowNavigationalDots { get; set; } = true;

        #endregion

        #region Methods

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in
        /// the render tree, and the incoming values have been assigned to properties.
        /// </summary>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        protected override void OnParametersSet()
		{
			base.OnParametersSet();

			if (Images.Count > 0)
			{
				// This is needed every time the component's visibility is changed.
				// Like when the parent toggles the visibility of the slideshow. 
				SetSlide(0);
			}
		}

        /// <summary>
        /// Sets the next slide to show.
        /// </summary>
        /// <param name="index">The index of the image slide to show.</param>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        private void SetSlide(int index)
        {
            _previousSlideIndex = _currentSlideIndex;

            if (index >= Images.Count)
            {
                _currentSlideIndex = 0;
            }
            else if (index < 0)
            {
                _currentSlideIndex = Images.Count - 1;
            }
            else
            {
                _currentSlideIndex = index;
            }

            Images[_previousSlideIndex].IsActive = false;
            Images[_currentSlideIndex].IsActive = true;
        }

        /// <summary>
        /// Shows the next image slide. 
        /// </summary>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        private void ShowNextSlide()
		{
			SetSlide(_currentSlideIndex + 1);
		}

        /// <summary>
        /// Shows the previous image slide. 
        /// </summary>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        private void ShowPreviousSlide()
		{
			SetSlide(_currentSlideIndex - 1);
		}

		#endregion
	}
}
