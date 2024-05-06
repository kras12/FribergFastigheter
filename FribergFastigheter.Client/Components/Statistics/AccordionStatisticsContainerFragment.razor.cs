using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Components.Statistics
{
    /// <summary>
    /// A component designed to be used as a render fragment for the <see cref="StatisticsCard"/> component. 
    /// The <see cref="AccordionStatisticsPageFragment"/> should be used to create the pages within the component. 
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class AccordionStatisticsContainerFragment : ComponentBase
    {
        #region Properties

        /// <summary>
        /// The render fragment for the body of the component. 
        /// The <see cref="AccordionStatisticsPageFragment"/> should be used to create the pages within the component. 
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        #endregion
    }
}
