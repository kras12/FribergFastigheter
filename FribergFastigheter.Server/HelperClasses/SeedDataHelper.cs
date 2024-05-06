using FribergFastigheter.Server.Data.Entities;
using FribergFastigheter.Server.HelperClasses.Data;
using FribergFastigheter.Shared.Dto;
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
    public class SeedDataHelper
    {
		#region Constants

		/// <summary>
		/// The non breaking space string used in HTML documents.
		/// </summary>
		private const string NonBreakingSpace = "\u00A0";

        #endregion

        #region Fields

        /// <summary>
        /// A lookup collection for broker firm images.
        /// </summary>
        /// <remarks>Key: Full URL.</remarks>
        private Dictionary<string, Image> _brokerFirmImages = new();

        /// <summary>
        /// A lookup collection for broker firms
        /// </summary>
        private Dictionary<string, BrokerFirm> _brokerFirms = new();

        /// <summary>
        /// A lookup collection for broker images.
        /// </summary>
        /// <remarks>Key: Full URL.</remarks>
        private Dictionary<string, Image> _brokerImages = new();

        /// <summary>
        /// A lookup collection for housing categories.
        /// </summary>
        private Dictionary<string, HousingCategory> _categories = new();

        /// <summary>
        /// A collection of grouped seed housing data.
        /// </summary>
        private List<GroupedHousingSeedData> _groupedHousingSeeds = new();

        /// <summary>
        /// A lookup collection for housing images.
        /// </summary>
        /// <remarks>Key: Full URL.</remarks>
        private Dictionary<string, Image> _housingImages = new();

        /// <summary>
        /// The path of the seed file.
        /// </summary>
        private string _jsonSeedFilePath = "";

        /// <summary>
        /// A lookup collection for municipalities.
        /// </summary>
        private Dictionary<string, Municipality> _municipalities = new();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
		/// <param name="jsonSeedFile">The path of the seed file.</param>
        public SeedDataHelper(string jsonSeedFile)
        {
            #region Checks

            if (string.IsNullOrEmpty(jsonSeedFile))
            {
                throw new ArgumentException("The file path can't be empty", nameof(jsonSeedFile));
            }


            #endregion
            this._jsonSeedFilePath = jsonSeedFile;
        }

        #endregion

        #region PublicMethods

        /// <summary>
        /// Creates housing seed data from a json seed file.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        public HousingSeedData GetSeedData()
        {
            ResetLookupData();
            HousingSeedData result = new HousingSeedData();
            string json = File.ReadAllText(_jsonSeedFilePath);
            var seedDataRows = JsonSerializer.Deserialize<List<SeedFileDataRow>>(json) ?? throw new Exception("Failed to deserialize housing seed data input");

            _groupedHousingSeeds = seedDataRows.GroupBy(
               keySelector: row => row.PageURL,
               elementSelector: row => row,
               resultSelector: (PageUrl, Rows) => new GroupedHousingSeedData()
               {
                   PageUrl = PageUrl,
                   MainDataRow = Rows.Last(),
                   ExtraData = Rows.GroupBy(x => x.Key).Select(x => new KeyValuePair<string, string>(x.Key, x.First().Value)).ToDictionary(x => x.Key, x => x.Value),
                   Images = Rows.Select(x => x.Image).Where(x => !string.IsNullOrEmpty(x) && !x.StartsWith("data:image")).ToList()
               })
                .ToList();

            CreateLookupTables();

            foreach (var groupedData in _groupedHousingSeeds)
            {
                Housing newHousing = new();
                ParseHousingData(newHousing, groupedData);

                if (newHousing.Images.Count > 0 && newHousing.Category != null && newHousing.Broker != null && newHousing.BrokerFirm != null)
                {
                    result.Housings.Add(newHousing);
                }
            }

            result.SeedImageUrls.HousingImageUrls = _housingImages.Keys.ToList();
            result.SeedImageUrls.BrokerImages = _brokerImages.Keys.ToList();
            result.SeedImageUrls.BrokerFirmImages = _brokerFirmImages.Keys.ToList();
            result.BrokerFirms = _brokerFirms.Values.ToList();

            result.BrokerFirms.ForEach(brokerFirm =>
                brokerFirm.Brokers.RemoveAll(broker =>
                    !result.Housings.Any(housing => housing.Broker == broker)));

            result.BrokerFirms.RemoveAll(x => x.Brokers.Count == 0);

            ResetLookupData();
            return result;
        }

        #endregion

        #region PrivateMethods        

        /// <summary>
        /// Creates the lookup tables
        /// </summary>
		private void CreateLookupTables()
        {
            foreach (var groupedData in _groupedHousingSeeds)
            {
                var parsedMunicipality = new Municipality(ParseMunicipality(groupedData));
                _municipalities.TryAdd(parsedMunicipality.MunicipalityName, parsedMunicipality);
                groupedData.Images.ForEach(x => _housingImages.TryAdd(x, new Image(Path.GetFileName(x))));

                if (TryParseHousingCategory(groupedData, out var parsedCategory))
                {
                    _categories.TryAdd(parsedCategory.CategoryName, parsedCategory);
                }

                if (TryParseBrokerFirm(groupedData, out string? parsedBrokerFirmName, out string? parsedBrokerFirmImage)
                    && TryParseBroker(groupedData, out string? parsedBrokerFirstName, out string? parsedBrokerLastName,
                        out string? parsedBrokerImage, out string? parsedBrokerDescription))
                {
                    _brokerImages.TryAdd(parsedBrokerImage, new Image(Path.GetFileName(parsedBrokerImage)));
                    _brokerFirmImages.TryAdd(parsedBrokerFirmImage, new Image(Path.GetFileName(parsedBrokerFirmImage)));
                    _brokerFirms.TryAdd(parsedBrokerFirmName, new BrokerFirm(parsedBrokerFirmName, logotype: _brokerFirmImages[parsedBrokerFirmImage]));

                    if (!_brokerFirms[parsedBrokerFirmName].Brokers.Any(x => x.User.FirstName.Equals(parsedBrokerFirstName, StringComparison.CurrentCultureIgnoreCase)
                        && x.User.LastName.Equals(parsedBrokerLastName, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        string email = $"{parsedBrokerFirstName.ToLower()}.{parsedBrokerLastName.ToLower()}@{parsedBrokerFirmName.ToLower().Replace(" ", "")}.se";
                        string password = $"A{Guid.NewGuid()}-{Guid.NewGuid()}!";
                        string phoneNumber = $"070-{new Random().Next(1_000_000, 2_000_000)}";
                        _brokerFirms[parsedBrokerFirmName].Brokers.Add(new Broker(_brokerFirms[parsedBrokerFirmName], parsedBrokerDescription, _brokerImages[parsedBrokerImage])
                        { User = new ApplicationUser(parsedBrokerFirstName, parsedBrokerLastName, email, phoneNumber, password) });
                    }
                }
            }
        }

        /// <summary>
        /// Parses housing data and fills a housing object with it.
        /// </summary>
        /// <param name="housing">The housing object that receives the data</param>
        /// <param name="groupedData">The grouped seed data for the housing object.</param>
		private void ParseHousingData(Housing housing, GroupedHousingSeedData groupedData)
        {
            housing.Address = groupedData.MainDataRow.Address;
            housing.Description = groupedData.MainDataRow.Description;
            housing.Price = int.Parse(groupedData.MainDataRow.Price.Replace("Pris saknas", "1000000").Replace(NonBreakingSpace, "").Split("kr")[0].Replace(" ", ""));
            housing.Municipality = _municipalities[ParseMunicipality(groupedData)];
            housing.Images = groupedData.Images.Select(x => _housingImages[x]).ToList();

            if (TryParseHousingCategory(groupedData, out var parsedCategory))
            {
                housing.Category = _categories[parsedCategory.CategoryName];
            }

            if (TryParseBrokerFirm(groupedData, out string? parsedBrokerFirmName, out string? parsedBrokerFirmImage)
                && TryParseBroker(groupedData, out string? parsedBrokerFirstName, out string? parsedBrokerLastName,
                        out string? _, out string? _))
            {
                housing.BrokerFirm = _brokerFirms[parsedBrokerFirmName];
                housing.Broker = _brokerFirms[parsedBrokerFirmName].Brokers.Single(x => x.User.FirstName.Equals(parsedBrokerFirstName, StringComparison.CurrentCultureIgnoreCase)
                        && x.User.LastName.Equals(parsedBrokerLastName, StringComparison.CurrentCultureIgnoreCase));
            }

            if (TryParseLivingArea(groupedData, out double? parsedLivingArea))
            {
                housing.LivingArea = parsedLivingArea.Value;
            }

            if (TryParseAncillaryArea(groupedData, out double? parsedAncillaryArea))
            {
                housing.AncillaryArea = parsedAncillaryArea.Value;
            }

            if (TryParseLandArea(groupedData, out double? parsedLandArea))
            {
                housing.LandArea = parsedLandArea.Value;
            }

            if (TryParseRoomCount(groupedData, out int? parsedRoomCount))
            {
                housing.RoomCount = parsedRoomCount.Value;
            }

            if (TryParseRunningCost(groupedData, out decimal? parsedYearlyRunningCost))
            {
                housing.YearlyRunningCost = parsedYearlyRunningCost.Value;
            }

            if (TryParseMonthlyCost(groupedData, out decimal? parsedMonthlyFee))
            {
                housing.MonthlyFee = parsedMonthlyFee.Value;
            }

            if (TryParseBuildYear(groupedData, out int? parsedBuildYear))
            {
                housing.BuildYear = parsedBuildYear.Value;
            }
        }

        /// <summary>
        /// Attempts to parse the municipality from serialized data.
        /// </summary>
        /// <param name="groupedData">The grouped seed data for the housing object.</param>
        /// <returns>The parsed municipality.</returns>
        private string ParseMunicipality(GroupedHousingSeedData groupedData)
        {
            int dataPoints = groupedData.MainDataRow.Location.Count(x => x == ',');

            if (dataPoints == 0)
            {
                return groupedData.MainDataRow.Location.Trim();
            }
            else
            {
                var parts = groupedData.MainDataRow.Location.Split(",");
                return parts[parts.Length - 1].Trim();
            }
        }

        /// <summary>
        /// Resets lookup data.
        /// </summary>
        private void ResetLookupData()
        {
            _categories = new();
            _municipalities = new();
            _housingImages = new();
            _brokerFirms = new();
            _groupedHousingSeeds = new();
        }
        /// <summary>
        /// Attempts to parse the housing ancillary area from serialized data.
        /// </summary>
        /// <param name="groupedData">The grouped seed data for the housing object.</param>
        /// <param name="ancillaryArea">The ancillary area if the operation was successful.</param>
        /// <returns>True if an ancillary area was found.</returns>
        private bool TryParseAncillaryArea(GroupedHousingSeedData groupedData, [NotNullWhen(returnValue: true)] out double? ancillaryArea)
		{
			if (groupedData.ExtraData.ContainsKey("Biarea"))
			{
				ancillaryArea = double.Parse(groupedData.ExtraData["Biarea"].Split(NonBreakingSpace)[0]);
				return true;
			}

			ancillaryArea = null;
			return false;
		}

        /// <summary>
        /// Attempts to parse the broker from serialized data.
        /// </summary>
        /// <param name="groupedData">The grouped seed data for the housing object.</param>
        /// <param name="brokerFirstName">The first name of the broker if the operation was successful.</param>
        /// <param name="brokerLastName">The first name of the broker if the operation was successful.</param>
        /// <returns>True if a broker was found.</returns>
        private bool TryParseBroker(GroupedHousingSeedData groupedData,
            [NotNullWhen(returnValue: true)] out string? brokerFirstName, [NotNullWhen(returnValue: true)] out string? brokerLastName,
            [NotNullWhen(returnValue: true)] out string? brokerImage,
            out string brokerDescription)
        {
            string[] brokerNameParts = groupedData.MainDataRow.Broker.Split(" ");
            brokerFirstName = string.Join(" ", brokerNameParts.Take(brokerNameParts.Length - 1));
            brokerLastName = brokerNameParts[brokerNameParts.Length - 1];
            brokerDescription = groupedData.MainDataRow.BrokerDescription;
            brokerImage = groupedData.MainDataRow.BrokerImage;

            return !string.IsNullOrEmpty(brokerFirstName) && !string.IsNullOrEmpty(brokerLastName)
               && !string.IsNullOrEmpty(brokerImage);

        }

        /// <summary>
        /// Attempts to parse the broker firm from serialized data.
        /// </summary>
        /// <param name="groupedData">The grouped seed data for the housing object.</param>
        /// <param name="brokerFirm">The parsed broker firm name if the operation was successful.</param>
        /// <param name="brokerFirmImage">The parsed broker firm image if the operation was successful.</param>
        /// <returns>True if a broker firm name was found.</returns>
        private bool TryParseBrokerFirm(GroupedHousingSeedData groupedData, [NotNullWhen(returnValue: true)] out string? brokerFirm,
            [NotNullWhen(returnValue: true)] out string? brokerFirmImage)
        {
            // Code from old seed structure
            //string[] parts = brokerFirm.Split("</div>");
            //brokerFirmName = parts[parts.Length - 1];

            brokerFirm = groupedData.MainDataRow.BrokerFirm;
            brokerFirmImage = groupedData.MainDataRow.BrokerFirmImage;
            return !string.IsNullOrEmpty(brokerFirm) && !string.IsNullOrEmpty(brokerFirmImage);
        }

        /// <summary>
        /// Attempts to parse the build year from serialized data.
        /// </summary>
        /// <param name="groupedData">The grouped seed data for the housing object.</param>
        /// <param name="buildYear">The build year if the operation was successful.</param>
        /// <returns>True if a build year was found.</returns>
        private bool TryParseBuildYear(GroupedHousingSeedData groupedData, [NotNullWhen(returnValue: true)] out int? buildYear)
		{
			if (groupedData.ExtraData.ContainsKey("Byggår"))
			{
				int dataPoints = groupedData.ExtraData["Byggår"].Count(x => x == '-');

				if (dataPoints == 0)
				{
					buildYear = int.Parse(groupedData.ExtraData["Byggår"]);
					return true;
				}
				else
				{
					buildYear = int.Parse(groupedData.ExtraData["Byggår"].Split("-", StringSplitOptions.TrimEntries)[0]);
					return true;
				}
			}

			buildYear = null;
			return false;
		}

        /// <summary>
        /// Attempts to parse the housing category from serialized data.
        /// </summary>
        /// <param name="groupedData">The grouped seed data for the housing object.</param>
        /// <param name="category">The category if the operation was successful.</param>
        /// <returns>True if a category was found.</returns>
        private bool TryParseHousingCategory(GroupedHousingSeedData groupedData, [NotNullWhen(returnValue: true)] out HousingCategory? category)
        {
            if (groupedData.ExtraData.ContainsKey("Bostadstyp"))
			{
				string housingType = groupedData.ExtraData["Bostadstyp"];
				string? housingForm = null;

				if (groupedData.ExtraData.ContainsKey("Upplåtelseform"))
				{
					housingForm = groupedData.ExtraData["Upplåtelseform"];
                }

                // Bostadsrättslägenhet
                if (housingType == "Lägenhet" && housingForm != null && housingForm == "Bostadsrätt")
                {
                    category = new HousingCategory("Bostadsrättslägenhet");
                    return true;
                }
                // Bostadsrättsradhus
                else if ((housingType == "Par-/kedje-/radhus" || housingType == "Kedjehus" || housingType == "Radhus")
                    && housingForm != null && housingForm == "Bostadsrätt")
                {
                    category = new HousingCategory("Bostadsrättsradhus");
                    return true;
                }
                // Fritidshus
                else if (housingType == "Fritidshus" || housingType == "Fritidsboende" || housingType == "Vinterbonat fritidshus")
                {
                    category = new HousingCategory("Fritidshus");
                    return true;
                }
                // Villa
                else if (housingType == "Villa")
                {
                    category = new HousingCategory("Villa");
                    return true;
                }
                else
                {
                    Debug.WriteLine($"Unknown housing type.");
                }
            }

            category = null;
            return false;
        }

        /// <summary>
        /// Attempts to parse the housing land area from serialized data.
        /// </summary>
        /// <param name="groupedData">The grouped seed data for the housing object.</param>
        /// <param name="landArea">The land area if the operation was successful.</param>
        /// <returns>True if a land area was found.</returns>
        private bool TryParseLandArea(GroupedHousingSeedData groupedData, [NotNullWhen(returnValue: true)] out double? landArea)
		{
			if (groupedData.ExtraData.ContainsKey("Tomtarea"))
			{
				landArea = double.Parse(groupedData.ExtraData["Tomtarea"].Split(NonBreakingSpace)[0]);
				return true;
			}

			landArea = null;
			return false;
		}

        /// <summary>
        /// Attempts to parse the housing living area from serialized data.
        /// </summary>
        /// <param name="groupedData">The grouped seed data for the housing object.</param>
        /// <param name="livingArea">The living area if the operation was successful.</param>
        /// <returns>True if a living area was found.</returns>
        private bool TryParseLivingArea(GroupedHousingSeedData groupedData, [NotNullWhen(returnValue: true)] out double? livingArea)
        {
			if (groupedData.ExtraData.ContainsKey("Boarea"))
			{
                livingArea = double.Parse(groupedData.ExtraData["Boarea"].Split(NonBreakingSpace)[0]);
				return true;
			}

            livingArea = null;
            return false;
		}

        /// <summary>
        /// Attempts to parse the monthly cost from serialized data.
        /// </summary>
        /// <param name="groupedData">The grouped seed data for the housing object.</param>
        /// <param name="monthlyFee">The monthly fee if the operation was successful.</param>
        /// <returns>True if a monthly fee was found.</returns>
        private bool TryParseMonthlyCost(GroupedHousingSeedData groupedData, [NotNullWhen(returnValue: true)] out decimal? monthlyFee)
		{
			if (groupedData.ExtraData.ContainsKey("Avgift"))
			{
				monthlyFee = decimal.Parse(groupedData.ExtraData["Avgift"].Replace("kr/mån", "").Trim());
				return true;
			}

			monthlyFee = null;
			return false;
		}

        /// <summary>
        /// Attempts to parse the number of rooms from serialized data.
        /// </summary>
        /// <param name="groupedData">The grouped seed data for the housing object.</param>
        /// <param name="roomCount">The room count if the operation was successful.</param>
        /// <returns>True if the room count was found.</returns>
        private bool TryParseRoomCount(GroupedHousingSeedData groupedData, [NotNullWhen(returnValue: true)] out int? roomCount)
		{
			if (groupedData.ExtraData.ContainsKey("Antal rum"))
			{
				// Some ads have rooms with decimal points
				roomCount = int.Parse(groupedData.ExtraData["Antal rum"].Split(" ")[0].Replace(".5", ""));
				return true;
			}

			roomCount = null;
			return false;
		}

        /// <summary>
        /// Attempts to parse the running cost from serialized data.
        /// </summary>
        /// <param name="groupedData">The grouped seed data for the housing object.</param>
        /// <param name="yearlyRunningCost">The yearly running cost if the operation was successful.</param>
        /// <returns>True if a running cost was found.</returns>
        private bool TryParseRunningCost(GroupedHousingSeedData groupedData, [NotNullWhen(returnValue: true)] out decimal? yearlyRunningCost)
		{
			if (groupedData.ExtraData.ContainsKey("Driftkostnad"))
			{
				yearlyRunningCost = decimal.Parse(groupedData.ExtraData["Driftkostnad"].Replace("kr/år", "").Trim());
				return true;
			}

			yearlyRunningCost = null;
			return false;
		}

        #endregion
    }
}
