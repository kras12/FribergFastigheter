using AutoMapper;
using FribergFastigheter.Client.Components;
using FribergFastigheter.Client.Models.Housing;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using FribergFastigheter.Shared.Dto;
using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Pages.BrokerFirmMember.Housing
{
    /// <summary>
    /// The list housing page in the broker firm back office.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: --> 
    public partial class ListHousingPage : ComponentBase
    {
        #region Fields       

        /// <summary>
        /// A collection of housing objects to show in the listing.
        /// </summary>
        private List<HousingViewModel> _housings = new();

        #endregion

        #region Properties
#pragma warning disable CS8618

        /// <summary>
        /// The injected Auto Mapper service.
        /// </summary>
        [Inject]
        private IMapper AutoMapper { get; set; }

        /// <summary>
        /// The injected broker firm API service. 
        /// </summary>
        [Inject]
        private IBrokerFirmApiService BrokerFirmApiService { get; set; }

#pragma warning restore CS8618
        #endregion

        #region Methods

		/// <summary>
		/// Method invoked when the component is ready to start, having received its initial 
		/// parameters from its parent in the render tree. Override this method if you will 
		/// perform an asynchronous operation and want the component to refresh when that 
		/// operation is completed.
		/// </summary>
		/// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
		protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var response = await BrokerFirmApiService.GetHousings(limitImagesPerHousing: 3);

            if (response.Success)
            {
                _housings = AutoMapper.Map<List<HousingViewModel>>(response.Value);
            }
            else
            {
                // Todo - Handle
            }
        }

		#endregion
	}
}
