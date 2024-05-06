using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Components.Statistics
{
    // <summary>
    /// A component designed to be used as a render fragment for the <see cref="AccordionStatisticsContainerFragment"/> component. 
    /// It displays simple statistics in the form of a label and numeric value pair. 
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class SimpleStatisticsCardFragment : ComponentBase
    {
        #region Properties

        /// <summary>
        /// The Font Awesome class for the icon to be displayed.
        /// </summary>
        [Parameter]
        public string FontAwesomeIconClass { get; set; } = "";

        /// <summary>
        /// The label to use for the statistic item.
        /// </summary>
        [Parameter]
        public string Label { get; set; } = "";

        /// <summary>
        /// The value for the statistic item. 
        /// </summary>
        [Parameter]
        public int Value { get; set; }

        #endregion
    }
}
