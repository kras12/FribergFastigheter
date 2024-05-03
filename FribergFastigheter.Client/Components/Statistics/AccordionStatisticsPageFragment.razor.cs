using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Components.Statistics
{
	// <summary>
	/// A component designed to be used as a render fragment for the <see cref="AccordionStatisticsContainerFragment"/> component. 
	/// </summary>
	/// <!-- Author: Jimmie -->
	/// <!-- Co Authors: -->
	public partial class AccordionStatisticsPageFragment : ComponentBase
    {
        #region Properties

        /// <summary>
        /// The element ID of the parent container. 
        /// </summary>
        [CascadingParameter]
        public string AccordionContainerIdString { get; set; } = "";

        /// <summary>
        /// The text of the ribbon button for the page. 
        /// </summary>
        [Parameter]
        public string ButtonText { get; set; } = "";

        /// <summary>
        /// The Font Awesome class for the icon to be displayed.
        /// </summary>
        [Parameter]
        public string FontAwesomeIconClass { get; set; } = "";

        /// <summary>
        /// Set to true if the page should start in an open state. 
        /// </summary>
        [Parameter]
        public bool IsOpen { get; set; } = true;

        /// <summary>
        /// The label to use for the statistic.
        /// </summary>
        [Parameter]
        public string Label { get; set; } = "";

        /// <summary>
        /// The value for the statistics item. 
        /// </summary>
        [Parameter]
        public int Value { get; set; }

        /// <summary>
        /// The percentual value change for a specific period of time. 
        /// </summary>
        [Parameter]
        public double? ValueChangeInPercentage { get; set; }

        /// <summary>
        /// The time period of the percentual value change. 
        /// </summary>
        [Parameter]
        public string? ValueChangeSinceText { get; set; }

        /// <summary>
        /// Returns true if there is trend data to be shown. 
        /// </summary>
        private bool HaveTrendData
        {
            get
            {
                return ValueChangeInPercentage != null && !string.IsNullOrEmpty(ValueChangeSinceText);   
            }
        }

        #endregion
    }
}
