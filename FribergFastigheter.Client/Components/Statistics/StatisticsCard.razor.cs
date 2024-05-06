using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Components.Statistics
{
    /// <summary>
    /// A component that creates a statistics card. The body of the content must be provided in the form of a render fragment. 
    /// Components such as <see cref="SimpleStatisticsCardFragment"/>, <see cref="AccordionStatisticsContainerFragment"/> and 
    /// <see cref="TabbedStatisticsContainerFragment"/> can fill this role. 
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class StatisticsCard : ComponentBase
    {
        #region Properties

        /// <summary>
        /// The render fragment for the body of the component. 
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        /// <summary>
        /// The title of the card. 
        /// </summary>
        [Parameter]
        public string Title { get; set; } = "";

        #endregion
    }
}
