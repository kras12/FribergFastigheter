using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Components.Accordion
{
    /// <summary>
    /// An accordion container component that together with components like <see cref="AccordionPageFragment"/> creates an accordian styled page toggler.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class AccordionContainer : ComponentBase
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
