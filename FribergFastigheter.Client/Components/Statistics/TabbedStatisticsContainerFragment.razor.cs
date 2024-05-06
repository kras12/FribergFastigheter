using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Components.Statistics
{
	//// <summary>
	/// A component designed to be used as a render fragment for the <see cref="StatisticsCard"/> component. 
	/// The <see cref="TabbedStatisticsPageFragment"/> must be used to create the pages within the component. 
	/// </summary>
	/// <!-- Author: Jimmie -->
	/// <!-- Co Authors: -->
	public partial class TabbedStatisticsContainerFragment : ComponentBase
    {
        #region Fields

        /// <summary>
        /// A collection of the tab pages that have been registered with the component. 
        /// </summary>
        private List<TabbedStatisticsPageFragment> _pages = new();

        #endregion

        #region Properties

        /// <summary>
        /// The render fragment for the body of the component. 
        /// The <see cref="TabbedStatisticsPageFragment"/> must be used to create the pages within the component. 
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        #endregion

        #region Methods

        /// <summary>
        /// Registers a tab page with the component. 
        /// </summary>
        /// <param name="page"></param>
        public void AddPage(TabbedStatisticsPageFragment page)
        {
            if (!_pages.Contains(page))
            {
                _pages.Add(page);

                if (_pages.Count == 1)
                {
                    _pages.First().MakeDefaultPage();
                }

                StateHasChanged();
            }
        }

        #endregion
    }
}
