using FribergFastigheter.Shared.Dto;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace FribergFastigheter.Client.Services
{
	/// <summary>
	/// A service to fetch data from Friberg Fastigheter API endpoints.
	/// </summary>
	/// <!-- Author: Jimmie -->
	/// <!-- Co Authors: -->
	public class FribergFastigheterApiService : IFribergFastigheterApiService
	{
		#region Fields

		/// <summary>
		/// The injected HTTP client.
		/// </summary>
		private readonly HttpClient _httpClient;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="httpClient">The injected HTTP client.</param>
		public FribergFastigheterApiService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		#endregion

		#region OpenAPiMethods

		/// <summary>
		/// Fetches all housings that matches the filters and options.
		/// </summary>
		/// <param name="municipalityId">Sets a filter on municipality.</param>
		/// <param name="limitHousings">Sets the max limit of housing objects to return.</param>
		/// <param name="limitImageCountPerHousing">Sets the max limit of images to return per housing object.</param>
		/// <returns>A <see cref="Task"/> containing a collection of <see cref="HousingDto"/>.</returns>
		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors: -->
		public async Task<List<HousingDto>?> SearchHousings(int? municipalityId = null, int? limitHousings = null, int? limitImageCountPerHousing = null)
		{
			List<KeyValuePair<string, string>> queries = new();

			if (municipalityId != null)
			{
				queries.Add(new KeyValuePair<string, string>("municipalityId", municipalityId.Value.ToString()));
			}

			if (limitHousings != null)
			{
				queries.Add(new KeyValuePair<string, string>("limitHousings", limitHousings.Value.ToString()));
			}

			if (limitImageCountPerHousing != null)
			{
				queries.Add(new KeyValuePair<string, string>("limitImageCountPerHousing", limitImageCountPerHousing.Value.ToString()));
			}

			return await _httpClient.GetFromJsonAsync<List<HousingDto>>($"/api/Housing/Search{BuildQueryString(queries)}");
		}

		#endregion

		#region OtherMethods

		/// <summary>
		/// Builds the query string to send with a request.
		/// </summary>
		/// <param name="queries">A collection of key value pairs for the queries to include.</param>
		/// <returns>A <see cref="string"/></returns>
		/// <exception cref="ArgumentException"></exception>
		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors: -->
		private string BuildQueryString(List<KeyValuePair<string, string>> queries)
		{
			#region Checks

			if (queries.Count == 0)
			{
				throw new ArgumentException("The query collection can't be empty", nameof(queries));
			}

			#endregion

			StringBuilder stringBuilder = new();

			foreach (var query in queries)
			{
				if (stringBuilder.Length == 0)
				{
					stringBuilder.Append($"?");
				}
				else
				{
					stringBuilder.Append($"&");
				}

				stringBuilder.Append($"{query.Key}={query.Value}");
			}

			return stringBuilder.ToString();
		}

		#endregion
	}
}
