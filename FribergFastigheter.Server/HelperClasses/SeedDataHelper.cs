using FribergFastigheter.Data.Entities;
using FribergFastigheterApi.Data.Entities;
using FribergFastigheterApi.HelperClasses.Data;
using Microsoft.AspNetCore.Components.Web;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO.Pipelines;
using System.Linq;
using System.Text.Json;

namespace FribergFastigheter.HelperClasses
{
	/// <summary>
	/// A helper class for handling seed data.
	/// </summary>
	/// <!-- Author: Jimmie -->
	/// <!-- Co Authors: -->
	public static class SeedDataHelper
    {
		#region Constants

		/// <summary>
		/// The non breaking space string used in HTML documents.
		/// </summary>
		private const string NonBreakingSpace = "\u00A0";

		#endregion

		/// <summary>
		/// Returns a collection of image urls associated with the housing objects from a json seed file.
		/// </summary>
		/// <param name="jsonFilePath"The path of the seed file.></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public static List<string> GetHousingImagePathsFromJsonFile(string jsonFilePath)
		{
			List<string> result = new();
			string json = File.ReadAllText(jsonFilePath);
			var seedDataRows = JsonSerializer.Deserialize<List<HousingSeedDataRow>>(json) ?? throw new Exception("Failed to deserialize housing seed data input");

			foreach (var row in seedDataRows)
			{
				if (!string.IsNullOrEmpty(row.Image) && !row.Image.StartsWith("data:image"))
				{
					result.Add(row.Image);
				}
			}

			return result;
		}

		/// <summary>
		/// Creates housing seed data from a json seed file.
		/// </summary>
		/// <param name="jsonFilePath">The path of the seed file.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="Exception"></exception>
		public static HousingSeedData GetHousingSeedDataFromJsonFile(string jsonFilePath)
		{
            #region Checks

            if (string.IsNullOrEmpty(jsonFilePath))
            {
                throw new ArgumentException("The file path can't be empty", nameof(jsonFilePath));
            }

			#endregion

			HousingSeedData result = new HousingSeedData();
            string json = File.ReadAllText(jsonFilePath);
			var seedDataRows = JsonSerializer.Deserialize<List<HousingSeedDataRow>>(json) ?? throw new Exception("Failed to deserialize housing seed data input");

            var pageData = seedDataRows.GroupBy(
               keySelector: row => row.PageURL,
               elementSelector: row => row,
               resultSelector: (PageUrl, Rows) => new
               {
                   PageUrl = PageUrl,
                   Rows = Rows
               })
                .ToList();

			Dictionary<string, HousingCategory> categories = new();
			Dictionary<string, Municipality> municipalities = new();
			Dictionary<string, Image> images = new();
			Dictionary<string, BrokerFirm> brokerFirms = new();

			foreach (var page in pageData)
            {
                Housing newHousing = new Housing();
                bool isFirstIteration = true;
                Dictionary<string, KeyValuePair<string, string>> housingData = new();

				foreach (var row in page.Rows)
                {
                    if (isFirstIteration)
                    {
						// Mixed
                        newHousing.Address = row.Address;
                        newHousing.Description = row.Description;						
						newHousing.Price = int.Parse(row.Price.Replace("Pris saknas", "1000000").Replace(NonBreakingSpace, "").Split("kr")[0].Replace(" ", ""));

						// Municipality
						var parsedMunicipality = ParseMunicipality(row.Location);
						if (!municipalities.ContainsKey(parsedMunicipality.MunicipalityName))
						{
							municipalities.Add(parsedMunicipality.MunicipalityName, parsedMunicipality);
						}
						newHousing.Municipality = municipalities[parsedMunicipality.MunicipalityName];

						// Brokers and broker firms
						if (TryParseBrokerFirm(row.BrokerFirm, out string? parsedBrokerFirmName) && TryParseBroker(row.Broker, out string? parsedBrokerFirstName, out string? parsedBrokerLastName))
						{
							if (!brokerFirms.ContainsKey(parsedBrokerFirmName))
							{
								brokerFirms.Add(parsedBrokerFirmName, new BrokerFirm(parsedBrokerFirmName));
							}

							newHousing.BrokerFirm = brokerFirms[parsedBrokerFirmName];

							if (!newHousing.BrokerFirm.Brokers.Any(x => x.FirstName.Equals(parsedBrokerFirstName, StringComparison.CurrentCultureIgnoreCase)
								&& x.LastName.Equals(parsedBrokerLastName, StringComparison.CurrentCultureIgnoreCase)))
							{
								newHousing.BrokerFirm.Brokers.Add(new Broker(parsedBrokerFirstName, parsedBrokerLastName, newHousing.BrokerFirm));
							}

							newHousing.Broker = brokerFirms[parsedBrokerFirmName].Brokers.First(x => x.FirstName.Equals(parsedBrokerFirstName, StringComparison.CurrentCultureIgnoreCase)
								&& x.LastName.Equals(parsedBrokerLastName, StringComparison.CurrentCultureIgnoreCase));
						}
					}

                    if (!string.IsNullOrEmpty(row.Image) && !row.Image.StartsWith("data:image"))
                    {
						string imageFileName = Path.GetFileName(row.Image);

						if (!images.ContainsKey(imageFileName))
						{
							images.Add(imageFileName, new Image(imageFileName));
						}

						newHousing.Images.Add(images[imageFileName]);
                    }

					if (!string.IsNullOrEmpty(row.Key) && !housingData.ContainsKey(row.Key))
                    {
                        housingData.Add(row.Key, new KeyValuePair<string, string>(row.Key, row.Value));
                    }
                }

                if (TryParseHousingCategory(housingData, out var parsedCategory))
                {
					if (!categories.ContainsKey(parsedCategory.CategoryName))
					{
						categories.Add(parsedCategory.CategoryName, parsedCategory);
					}

                    newHousing.Category = categories[parsedCategory.CategoryName];
                }

				if (TryParseLivingArea(housingData, out double? parsedLivingArea))
				{
					newHousing.LivingArea = parsedLivingArea.Value;
				}

				if (TryParseAncillaryArea(housingData, out double? parsedAncillaryArea))
				{
					newHousing.AncillaryArea = parsedAncillaryArea.Value;
				}

				if (TryParseLandArea(housingData, out double? parsedLandArea))
				{
					newHousing.LandArea = parsedLandArea.Value;
				}

				if (TryParseRoomCount(housingData, out int? parsedRoomCount))
				{
					newHousing.RoomCount = parsedRoomCount.Value;
				}

				if (TryParseRunningCost(housingData, out decimal? parsedYearlyRunningCost))
				{
					newHousing.YearlyRunningCost = parsedYearlyRunningCost.Value;
				}

				if (TryParseMonthlyCost(housingData, out decimal? parsedMonthlyFee))
				{
					newHousing.MonthlyFee = parsedMonthlyFee.Value;
				}

				if (TryParseBuildYear(housingData, out int? parsedBuildYear))
				{
					newHousing.BuildYear = parsedBuildYear.Value;
				}

				if (newHousing.Images.Count > 0 && newHousing.Category != null && newHousing.Broker != null && newHousing.BrokerFirm != null)
                {
					result.Housings.Add(newHousing);
                }
			}

			return result;
        }

		/// <summary>
		/// Attempts to parse the municipality from serialized data.
		/// </summary>
		/// <param name="location">The location string to parse (can contain location and municipality).</param>
		/// <returns>The parsed municipality.</returns>
		private static Municipality ParseMunicipality(string location)
		{
			int dataPoints = location.Count(x => x == ',');

			if (dataPoints == 0)
			{
				return new Municipality(location.Trim());
			}
			else
			{
				var parts = location.Split(",");
				return new Municipality(parts[parts.Length - 1].Trim());
			}
		}

		/// <summary>
		/// Attempts to parse the housing ancillary area from serialized data.
		/// </summary>
		/// <param name="housingData">A dictionary containing the serialized data to parse.</param>
		/// <param name="ancillaryArea">The ancillary area if the operation was successful.</param>
		/// <returns>True if an ancillary area was found.</returns>
		private static bool TryParseAncillaryArea(Dictionary<string, KeyValuePair<string, string>> housingData, [NotNullWhen(returnValue: true)] out double? ancillaryArea)
		{
			if (housingData.ContainsKey("Biarea"))
			{
				ancillaryArea = double.Parse(housingData["Biarea"].Value.Split(NonBreakingSpace)[0]);
				return true;
			}

			ancillaryArea = null;
			return false;
		}

		/// <summary>
		/// Attempts to parse the build year from serialized data.
		/// </summary>
		/// <param name="housingData">A dictionary containing the serialized data to parse.</param>
		/// <param name="buildYear">The build year if the operation was successful.</param>
		/// <returns>True if a build year was found.</returns>
		private static bool TryParseBuildYear(Dictionary<string, KeyValuePair<string, string>> housingData, [NotNullWhen(returnValue: true)] out int? buildYear)
		{
			if (housingData.ContainsKey("Byggår"))
			{
				int dataPoints = housingData["Byggår"].Value.Count(x => x == '-');

				if (dataPoints == 0)
				{
					buildYear = int.Parse(housingData["Byggår"].Value);
					return true;
				}
				else
				{
					buildYear = int.Parse(housingData["Byggår"].Value.Split("-", StringSplitOptions.TrimEntries)[0]);
					return true;
				}
			}

			buildYear = null;
			return false;
		}

		/// <summary>
		/// Attempts to parse the housing category from serialized data.
		/// </summary>
		/// <param name="housingData">A dictionary containing the serialized data to parse.</param>
		/// <param name="category">The category if the operation was successful.</param>
		/// <returns>True if a category was found.</returns>
		private static bool TryParseHousingCategory(Dictionary<string, KeyValuePair<string, string>> housingData, [NotNullWhen(returnValue: true)] out HousingCategory? category)
        {
            // Bostadsrättslägenhet
            if (housingData.ContainsKey("Bostadstyp") && housingData["Bostadstyp"].Value == "Lägenhet"
                && housingData.ContainsKey("Upplåtelseform") && housingData["Upplåtelseform"].Value == "Bostadsrätt")
            {
                category = new HousingCategory("Bostadsrättslägenhet");
                return true;
            }
            // Bostadsrättsradhus
            else if (housingData.ContainsKey("Bostadstyp") && (housingData["Bostadstyp"].Value == "Par-/kedje-/radhus" || housingData["Bostadstyp"].Value == "Kedjehus" || housingData["Bostadstyp"].Value == "Radhus")
                && housingData.ContainsKey("Upplåtelseform") && housingData["Upplåtelseform"].Value == "Bostadsrätt")
            {
                category = new HousingCategory("Bostadsrättsradhus");
				return true;
			}
            // Fritidshus
            else if (housingData.ContainsKey("Bostadstyp") && (housingData["Bostadstyp"].Value == "Fritidshus" || housingData["Bostadstyp"].Value == "Fritidsboende" || housingData["Bostadstyp"].Value == "Vinterbonat fritidshus"))
            {
                category = new HousingCategory("Fritidshus");
				return true;
			}
            // Villa
            else if (housingData.ContainsKey("Bostadstyp") && housingData["Bostadstyp"].Value == "Villa")
            {
                category = new HousingCategory("Villa");
				return true;
			}
            else
            {
                Debug.WriteLine($"Unknown housing type.");
                category = null;
                return false;
            }
        }

		/// <summary>
		/// Attempts to parse the housing land area from serialized data.
		/// </summary>
		/// <param name="housingData">A dictionary containing the serialized data to parse.</param>
		/// <param name="landArea">The land area if the operation was successful.</param>
		/// <returns>True if a land area was found.</returns>
		private static bool TryParseLandArea(Dictionary<string, KeyValuePair<string, string>> housingData, [NotNullWhen(returnValue: true)] out double? landArea)
		{
			if (housingData.ContainsKey("Tomtarea"))
			{
				landArea = double.Parse(housingData["Tomtarea"].Value.Split(NonBreakingSpace)[0]);
				return true;
			}

			landArea = null;
			return false;
		}

		/// <summary>
		/// Attempts to parse the housing living area from serialized data.
		/// </summary>
		/// <param name="housingData">A dictionary containing the serialized data to parse.</param>
		/// <param name="livingArea">The living area if the operation was successful.</param>
		/// <returns>True if a living area was found.</returns>
		private static bool TryParseLivingArea(Dictionary<string, KeyValuePair<string, string>> housingData, [NotNullWhen(returnValue: true)] out double? livingArea)
        {
			if (housingData.ContainsKey("Boarea"))
			{
                livingArea = double.Parse(housingData["Boarea"].Value.Split(NonBreakingSpace)[0]);
				return true;
			}

            livingArea = null;
            return false;
		}

		/// <summary>
		/// Attempts to parse the monthly cost from serialized data.
		/// </summary>
		/// <param name="housingData">A dictionary containing the serialized data to parse.</param>
		/// <param name="monthlyFee">The monthly fee if the operation was successful.</param>
		/// <returns>True if a monthly fee was found.</returns>
		private static bool TryParseMonthlyCost(Dictionary<string, KeyValuePair<string, string>> housingData, [NotNullWhen(returnValue: true)] out decimal? monthlyFee)
		{
			if (housingData.ContainsKey("Avgift"))
			{
				monthlyFee = decimal.Parse(housingData["Avgift"].Value.Replace("kr/mån", "").Trim());
				return true;
			}

			monthlyFee = null;
			return false;
		}

		/// <summary>
		/// Attempts to parse the number of rooms from serialized data.
		/// </summary>
		/// <param name="housingData">A dictionary containing the serialized data to parse.</param>
		/// <param name="roomCount">The room count if the operation was successful.</param>
		/// <returns>True if the room count was found.</returns>
		private static bool TryParseRoomCount(Dictionary<string, KeyValuePair<string, string>> housingData, [NotNullWhen(returnValue: true)] out int? roomCount)
		{
			if (housingData.ContainsKey("Antal rum"))
			{
				// Some ads have rooms with decimal points
				roomCount = int.Parse(housingData["Antal rum"].Value.Split(" ")[0].Replace(".5", ""));
				return true;
			}

			roomCount = null;
			return false;
		}

		/// <summary>
		/// Attempts to parse the running cost from serialized data.
		/// </summary>
		/// <param name="housingData">A dictionary containing the serialized data to parse.</param>
		/// <param name="yearlyRunningCost">The yearly running cost if the operation was successful.</param>
		/// <returns>True if a running cost was found.</returns>
		private static bool TryParseRunningCost(Dictionary<string, KeyValuePair<string, string>> housingData, [NotNullWhen(returnValue: true)] out decimal? yearlyRunningCost)
		{
			if (housingData.ContainsKey("Driftkostnad"))
			{
				yearlyRunningCost = decimal.Parse(housingData["Driftkostnad"].Value.Replace("kr/år", "").Trim());
				return true;
			}

			yearlyRunningCost = null;
			return false;
		}

		/// <summary>
		/// Attempts to parse the broker from serialized data.
		/// </summary>
		/// <param name="broker">The broker string to parse.</param>
		/// <param name="brokerFirstName">The first name of the broker if the operation was successful.</param>
		/// <param name="brokerLastName">The first name of the broker if the operation was successful.</param>
		/// <returns>True if a broker was found.</returns>
		private static bool TryParseBroker(string broker,
			[NotNullWhen(returnValue: true)] out string? brokerFirstName, [NotNullWhen(returnValue: true)] out string? brokerLastName)
		{
			string[] brokerNameParts = broker.Split(" ");
			brokerFirstName = string.Join(" ", brokerNameParts.Take(brokerNameParts.Length - 1));
			brokerLastName = brokerNameParts[brokerNameParts.Length - 1];
			return !string.IsNullOrEmpty(brokerFirstName) && !string.IsNullOrEmpty(brokerLastName);
		}

		/// <summary>
		/// Attempts to parse the broker firm from serialized data.
		/// </summary>
		/// <param name="brokerFirm">The broker firm string to parse.</param>
		/// <param name="brokerFirmName">The parsed broker firm name if the operation was successful.</param>
		/// <returns>True if a broker firm name was found.</returns>
		private static bool TryParseBrokerFirm(string brokerFirm, [NotNullWhen(returnValue: true)] out string? brokerFirmName)
		{
			string[] parts = brokerFirm.Split("</div>");
			brokerFirmName = parts[parts.Length - 1];
			return !string.IsNullOrEmpty(brokerFirmName);
		}
	}

	/// <summary>
	/// Stores seed data for housings.
	/// </summary>
	/// <!-- Author: Jimmie -->
	/// <!-- Co Authors: -->
	public class HousingSeedData
	{
		#region Properties

		/// <summary>
		/// Returns a list of categories associated with the housings.
		/// </summary>
		public List<HousingCategory> HousingCategories
		{
			get
			{
				return Housings.Select(x => x.Category).ToHashSet().ToList();
			}
		}

		/// <summary>
		/// A collection of housing objects.
		/// </summary>
		public List<Housing> Housings { get; set; } = new();
		/// <summary>
		/// Returns a list of municipalities associated with the housings.
		/// </summary>
		public List<Municipality> Municipalities
		{
			get
			{
				return Housings.Select(x => x.Municipality).ToHashSet().ToList();
			}
		}

		#endregion
	}
}
