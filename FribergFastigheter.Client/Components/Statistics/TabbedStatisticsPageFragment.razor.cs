using Microsoft.AspNetCore.Components;
using System.Diagnostics;

namespace FribergFastigheter.Client.Components.Statistics
{
    // <summary>
    /// A component designed to be used as a render fragment for the <see cref="TabbedStatisticsContainerFragment"/> component. 
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class TabbedStatisticsPageFragment : ComponentBase
    {
        #region Properties

        /// <summary>
        /// The text of the button for the tab.
        /// </summary>
        [Parameter]
        public string ButtonText { get; set; } = "";

        /// <summary>
        /// The Font Awesome class for the icon to be displayed.
        /// </summary>
        [Parameter]
        public string FontAwesomeIconClass { get; set; } = "";

        /// <summary>
        /// Sets to true if the page should be the default (active) page. 
        /// </summary>
        public bool IsDefaultPage { get; private set; } = false;

        /// <summary>
        /// The label to use for the statistic item.
        /// </summary>
        [Parameter]
        public string Label { get; set; } = "";

        /// <summary>
        /// The ID of the page. 
        /// </summary>
        public string PageId { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// The parent tab container. 
        /// </summary>
        [CascadingParameter]
        public TabbedStatisticsContainerFragment? TabContainer { get; set; } = null;

        /// <summary>
        /// The value for the statistic item. 
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

        #region Methods

        /// <summary>
        /// Marks the page as the default page. 
        /// </summary>
        public void MakeDefaultPage()
        {
            IsDefaultPage = true;
        }

        /// <summary>
        /// Method invoked when the component is ready to start, having received its initial 
        /// parameters from its parent in the render tree. Override this method if you will 
        /// perform an asynchronous operation and want the component to refresh when that 
        /// operation is completed.
        /// </summary>
        /// <returns> A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Debug.Assert(TabContainer != null);
            TabContainer.AddPage(this);
        }

        #endregion
    }
}
