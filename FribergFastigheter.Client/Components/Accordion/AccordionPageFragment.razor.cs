using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Components.Accordion
{
	/// <summary>
	/// A component designed to be used as a render fragment for the <see cref="AccordionContainer"/> component to create an accordian styled page toggler.
	/// </summary>
	/// <!-- Author: Jimmie -->
	/// <!-- Co Authors: -->
	public partial class AccordionPageFragment : ComponentBase
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
        /// The body of the component. 
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

		/// <summary>
		/// Set to true if the page should start in an open state. 
		/// </summary>
		[Parameter]
		public bool StartOpen { get; set; } = true;

		#endregion
	}
}
