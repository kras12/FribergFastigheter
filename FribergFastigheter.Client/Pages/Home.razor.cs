﻿using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using System.Net.Http.Json;
using FribergFastigheter.Shared.Dto;
using FribergFastigheter.Client.Services;

namespace FribergFastigheter.Client.Pages
{
    /// <summary>
    /// The website home page component.
    /// </summary>
    public partial class Home : ComponentBase
    {
        private Stopwatch _stopwatch = new Stopwatch();

		#region Properties

#pragma warning disable CS8618
		/// <summary>
		/// The inject Api service for Friberg Fastigheter.
		/// </summary>
		[Inject] 
		private IFribergFastigheterApiService ApiService { get; set; }
#pragma warning restore CS8618

		/// <summary>
		/// A test property to test the API.
		/// </summary>
		public List<HousingDto> Housings { get; set; } = new();

		#endregion

		#region Methods

		/// <summary>
		/// Method invoked when the component is ready to start, having received its
		/// initial parameters from its parent in the render tree.
		/// </summary>
		/// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
		protected async override Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
            //await EmbeddedDataTest();
            await UrlDataTest();

            _stopwatch.Start();
		}

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
        }

        private async Task EmbeddedDataTest()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            var housings = await ApiService.SearchHousings(limitHousings: 50, limitImageCountPerHousing: 3, includeImageData: true);

            Console.WriteLine(stopwatch.ElapsedMilliseconds);

            if (housings != null)
            {
                var firstImage = housings.First().Images.First().Base64;
               Housings = housings;

#pragma warning disable CS4014
                Task.Run(
                    async () =>
                    {
                        await Task.Delay(2000);
                        foreach (var housing in housings)
                        {
                            foreach (var image in housing.Images)
                            {
                                Console.WriteLine("Switching image.");
                                image.Base64 = firstImage;
                                StateHasChanged();
                                await Task.Delay(100);
                            }
                        }
                    });
#pragma warning restore CS4014
            }
        }

        private async Task UrlDataTest()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            var housings = await ApiService.SearchHousings(limitHousings: 50, limitImageCountPerHousing: 1, includeImageData: false);

            Console.WriteLine(stopwatch.ElapsedMilliseconds);

            if (housings != null)
            {                
                Housings = housings;
            }
        }

        #endregion
    }	
}