using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Components.Links
{
    /// <summary>
    /// Component that can dynamically embedd content inside a link element. 
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class DynamicLink : ComponentBase
    {
        #region Properties

        /// <summary>
        /// The body of the component. 
        /// </summary>
        [Parameter]
#pragma warning disable CS8618 
        public RenderFragment ChildContent { get; set; }
#pragma warning restore CS8618 

        /// <summary>
        /// Set to true to enable the link set in <see cref="Url"/>.
        /// </summary>
        [Parameter]
        public bool ShowLink { get; set; }

        /// <summary>
        /// The url of the link to embedd the content within.
        /// </summary>
        [Parameter]
        public string? Url { get; set; } = null;

        #endregion
    }
}
