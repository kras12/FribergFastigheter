using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FribergFastigheterApi.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BrokerFirms",
                columns: table => new
                {
                    BrokerFirmId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrokerFirms", x => x.BrokerFirmId);
                });

            migrationBuilder.CreateTable(
                name: "HousingCategories",
                columns: table => new
                {
                    HousingCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HousingCategories", x => x.HousingCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Municipalities",
                columns: table => new
                {
                    MunicipalityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MunicipalityName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipalities", x => x.MunicipalityId);
                });

            migrationBuilder.CreateTable(
                name: "Brokers",
                columns: table => new
                {
                    BrokerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrokerFirmId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brokers", x => x.BrokerId);
                    table.ForeignKey(
                        name: "FK_Brokers_BrokerFirms_BrokerFirmId",
                        column: x => x.BrokerFirmId,
                        principalTable: "BrokerFirms",
                        principalColumn: "BrokerFirmId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Housings",
                columns: table => new
                {
                    HousingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AncillaryArea = table.Column<double>(type: "float", nullable: true),
                    BrokerId = table.Column<int>(type: "int", nullable: false),
                    BrokerFirmId = table.Column<int>(type: "int", nullable: false),
                    BuildYear = table.Column<int>(type: "int", nullable: true),
                    CategoryHousingCategoryId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LandArea = table.Column<double>(type: "float", nullable: true),
                    LivingArea = table.Column<double>(type: "float", nullable: false),
                    MonthlyFee = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    MunicipalityId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    RoomCount = table.Column<int>(type: "int", nullable: true),
                    YearlyRunningCost = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Housings", x => x.HousingId);
                    table.ForeignKey(
                        name: "FK_Housings_BrokerFirms_BrokerFirmId",
                        column: x => x.BrokerFirmId,
                        principalTable: "BrokerFirms",
                        principalColumn: "BrokerFirmId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Housings_Brokers_BrokerId",
                        column: x => x.BrokerId,
                        principalTable: "Brokers",
                        principalColumn: "BrokerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Housings_HousingCategories_CategoryHousingCategoryId",
                        column: x => x.CategoryHousingCategoryId,
                        principalTable: "HousingCategories",
                        principalColumn: "HousingCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Housings_Municipalities_MunicipalityId",
                        column: x => x.MunicipalityId,
                        principalTable: "Municipalities",
                        principalColumn: "MunicipalityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    ImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HousingId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_Image_Housings_HousingId",
                        column: x => x.HousingId,
                        principalTable: "Housings",
                        principalColumn: "HousingId");
                });

            migrationBuilder.InsertData(
                table: "HousingCategories",
                columns: new[] { "HousingCategoryId", "CategoryName" },
                values: new object[,]
                {
                    { 1, "Bostadsrättslägenhet" },
                    { 2, "Bostadsrättsradhus" },
                    { 3, "Fritidshus" },
                    { 4, "Villa" }
                });

            migrationBuilder.InsertData(
                table: "Municipalities",
                columns: new[] { "MunicipalityId", "MunicipalityName" },
                values: new object[,]
                {
                    { 1, "Ale kommun" },
                    { 2, "Alingsås kommun" },
                    { 3, "Alvesta kommun" },
                    { 4, "Aneby kommun" },
                    { 5, "Arboga kommun" },
                    { 6, "Arjeplogs kommun" },
                    { 7, "Arvidsjaurs kommun" },
                    { 8, "Arvika kommun" },
                    { 9, "Askersunds kommun" },
                    { 10, "Avesta kommun" },
                    { 11, "Bengtsfors kommun" },
                    { 12, "Bergs kommun" },
                    { 13, "Bjurholms kommun" },
                    { 14, "Bjuvs kommun" },
                    { 15, "Bodens kommun" },
                    { 16, "Bollebygds kommun" },
                    { 17, "Bollnäs kommun" },
                    { 18, "Borgholms kommun" },
                    { 19, "Borlänge kommun" },
                    { 20, "Borås kommun" },
                    { 21, "Botkyrka kommun" },
                    { 22, "Boxholms kommun" },
                    { 23, "Bromölla kommun" },
                    { 24, "Bräcke kommun" },
                    { 25, "Burlövs kommun" },
                    { 26, "Båstads kommun" },
                    { 27, "Dals-Eds kommun" },
                    { 28, "Danderyds kommun" },
                    { 29, "Degerfors kommun" },
                    { 30, "Dorotea kommun" },
                    { 31, "Eda kommun" },
                    { 32, "Ekerö kommun" },
                    { 33, "Eksjö kommun" },
                    { 34, "Emmaboda kommun" },
                    { 35, "Enköpings kommun" },
                    { 36, "Eskilstuna kommun" },
                    { 37, "Eslövs kommun" },
                    { 38, "Essunga kommun" },
                    { 39, "Fagersta kommun" },
                    { 40, "Falkenbergs kommun" },
                    { 41, "Falköpings kommun" },
                    { 42, "Falu kommun" },
                    { 43, "Filipstads kommun" },
                    { 44, "Finspångs kommun" },
                    { 45, "Flens kommun" },
                    { 46, "Forshaga kommun" },
                    { 47, "Färgelanda kommun" },
                    { 48, "Gagnefs kommun" },
                    { 49, "Gislaveds kommun" },
                    { 50, "Gnesta kommun" },
                    { 51, "Gnosjö kommun" },
                    { 52, "Region Gotland" },
                    { 53, "Grums kommun" },
                    { 54, "Grästorps kommun" },
                    { 55, "Gullspångs kommun" },
                    { 56, "Gällivare kommun" },
                    { 57, "Gävle kommun" },
                    { 58, "Göteborgs kommun" },
                    { 59, "Götene kommun" },
                    { 60, "Habo kommun" },
                    { 61, "Hagfors kommun" },
                    { 62, "Hallsbergs kommun" },
                    { 63, "Hallstahammars kommun" },
                    { 64, "Halmstads kommun" },
                    { 65, "Hammarö kommun" },
                    { 66, "Haninge kommun" },
                    { 67, "Haparanda kommun" },
                    { 68, "Heby kommun" },
                    { 69, "Hedemora kommun" },
                    { 70, "Helsingborgs kommun" },
                    { 71, "Herrljunga kommun" },
                    { 72, "Hjo kommun" },
                    { 73, "Hofors kommun" },
                    { 74, "Huddinge kommun" },
                    { 75, "Hudiksvalls kommun" },
                    { 76, "Hultsfreds kommun" },
                    { 77, "Hylte kommun" },
                    { 78, "Håbo kommun" },
                    { 79, "Hällefors kommun" },
                    { 80, "Härjedalens kommun" },
                    { 81, "Härnösands kommun" },
                    { 82, "Härryda kommun" },
                    { 83, "Hässleholms kommun" },
                    { 84, "Höganäs kommun" },
                    { 85, "Högsby kommun" },
                    { 86, "Hörby kommun" },
                    { 87, "Höörs kommun" },
                    { 88, "Jokkmokks kommun" },
                    { 89, "Järfälla kommun" },
                    { 90, "Jönköpings kommun" },
                    { 91, "Kalix kommun" },
                    { 92, "Kalmar kommun" },
                    { 93, "Karlsborgs kommun" },
                    { 94, "Karlshamns kommun" },
                    { 95, "Karlskoga kommun" },
                    { 96, "Karlskrona kommun" },
                    { 97, "Karlstads kommun" },
                    { 98, "Katrineholms kommun" },
                    { 99, "Kils kommun" },
                    { 100, "Kinda kommun" },
                    { 101, "Kiruna kommun" },
                    { 102, "Klippans kommun" },
                    { 103, "Knivsta kommun" },
                    { 104, "Kramfors kommun" },
                    { 105, "Kristianstads kommun" },
                    { 106, "Kristinehamns kommun" },
                    { 107, "Krokoms kommun" },
                    { 108, "Kumla kommun" },
                    { 109, "Kungsbacka kommun" },
                    { 110, "Kungsörs kommun" },
                    { 111, "Kungälvs kommun" },
                    { 112, "Kävlinge kommun" },
                    { 113, "Köpings kommun" },
                    { 114, "Laholms kommun" },
                    { 115, "Landskrona kommun" },
                    { 116, "Laxå kommun" },
                    { 117, "Lekebergs kommun" },
                    { 118, "Leksands kommun" },
                    { 119, "Lerums kommun" },
                    { 120, "Lessebo kommun" },
                    { 121, "Lidingö kommun" },
                    { 122, "Lidköpings kommun" },
                    { 123, "Lilla Edets kommun" },
                    { 124, "Lindesbergs kommun" },
                    { 125, "Linköpings kommun" },
                    { 126, "Ljungby kommun" },
                    { 127, "Ljusdals kommun" },
                    { 128, "Ljusnarsbergs kommun" },
                    { 129, "Lomma kommun" },
                    { 130, "Ludvika kommun" },
                    { 131, "Luleå kommun" },
                    { 132, "Lunds kommun" },
                    { 133, "Lycksele kommun" },
                    { 134, "Lysekils kommun" },
                    { 135, "Malmö kommun" },
                    { 136, "Malung-Sälens kommun" },
                    { 137, "Malå kommun" },
                    { 138, "Mariestads kommun" },
                    { 139, "Markaryds kommun" },
                    { 140, "Marks kommun" },
                    { 141, "Melleruds kommun" },
                    { 142, "Mjölby kommun" },
                    { 143, "Mora kommun" },
                    { 144, "Motala kommun" },
                    { 145, "Mullsjö kommun" },
                    { 146, "Munkedals kommun" },
                    { 147, "Munkfors kommun" },
                    { 148, "Mölndals kommun" },
                    { 149, "Mönsterås kommun" },
                    { 150, "Mörbylånga kommun" },
                    { 151, "Nacka kommun" },
                    { 152, "Nora kommun" },
                    { 153, "Norbergs kommun" },
                    { 154, "Nordanstigs kommun" },
                    { 155, "Nordmalings kommun" },
                    { 156, "Norrköpings kommun" },
                    { 157, "Norrtälje kommun" },
                    { 158, "Norsjö kommun" },
                    { 159, "Nybro kommun" },
                    { 160, "Nykvarns kommun" },
                    { 161, "Nyköpings kommun" },
                    { 162, "Nynäshamns kommun" },
                    { 163, "Nässjö kommun" },
                    { 164, "Ockelbo kommun" },
                    { 165, "Olofströms kommun" },
                    { 166, "Orsa kommun" },
                    { 167, "Orust kommun" },
                    { 168, "Osby kommun" },
                    { 169, "Oskarshamns kommun" },
                    { 170, "Ovanåkers kommun" },
                    { 171, "Oxelösunds kommun" },
                    { 172, "Pajala kommun" },
                    { 173, "Partille kommun" },
                    { 174, "Perstorps kommun" },
                    { 175, "Piteå kommun" },
                    { 176, "Ragunda kommun" },
                    { 177, "Robertsfors kommun" },
                    { 178, "Ronneby kommun" },
                    { 179, "Rättviks kommun" },
                    { 180, "Sala kommun" },
                    { 181, "Salems kommun" },
                    { 182, "Sandvikens kommun" },
                    { 183, "Sigtuna kommun" },
                    { 184, "Simrishamns kommun" },
                    { 185, "Sjöbo kommun" },
                    { 186, "Skara kommun" },
                    { 187, "Skellefteå kommun" },
                    { 188, "Skinnskattebergs kommun" },
                    { 189, "Skurups kommun" },
                    { 190, "Skövde kommun" },
                    { 191, "Smedjebackens kommun" },
                    { 192, "Sollefteå kommun" },
                    { 193, "Sollentuna kommun" },
                    { 194, "Solna kommun" },
                    { 195, "Sorsele kommun" },
                    { 196, "Sotenäs kommun" },
                    { 197, "Staffanstorps kommun" },
                    { 198, "Stenungsunds kommun" },
                    { 199, "Stockholms kommun" },
                    { 200, "Storfors kommun" },
                    { 201, "Storumans kommun" },
                    { 202, "Strängnäs kommun" },
                    { 203, "Strömstads kommun" },
                    { 204, "Strömsunds kommun" },
                    { 205, "Sundbybergs kommun" },
                    { 206, "Sundsvalls kommun" },
                    { 207, "Sunne kommun" },
                    { 208, "Surahammars kommun" },
                    { 209, "Svalövs kommun" },
                    { 210, "Svedala kommun" },
                    { 211, "Svenljunga kommun" },
                    { 212, "Säffle kommun" },
                    { 213, "Säters kommun" },
                    { 214, "Sävsjö kommun" },
                    { 215, "Söderhamns kommun" },
                    { 216, "Söderköpings kommun" },
                    { 217, "Södertälje kommun" },
                    { 218, "Sölvesborgs kommun" },
                    { 219, "Tanums kommun" },
                    { 220, "Tibro kommun" },
                    { 221, "Tidaholms kommun" },
                    { 222, "Tierps kommun" },
                    { 223, "Timrå kommun" },
                    { 224, "Tingsryds kommun" },
                    { 225, "Tjörns kommun" },
                    { 226, "Tomelilla kommun" },
                    { 227, "Torsby kommun" },
                    { 228, "Torsås kommun" },
                    { 229, "Tranemo kommun" },
                    { 230, "Tranås kommun" },
                    { 231, "Trelleborgs kommun" },
                    { 232, "Trollhättans kommun" },
                    { 233, "Trosa kommun" },
                    { 234, "Tyresö kommun" },
                    { 235, "Täby kommun" },
                    { 236, "Töreboda kommun" },
                    { 237, "Uddevalla kommun" },
                    { 238, "Ulricehamns kommun" },
                    { 239, "Umeå kommun" },
                    { 240, "Upplands Väsby kommun" },
                    { 241, "Upplands-Bro kommun" },
                    { 242, "Uppsala kommun" },
                    { 243, "Uppvidinge kommun" },
                    { 244, "Vadstena kommun" },
                    { 245, "Vaggeryds kommun" },
                    { 246, "Valdemarsviks kommun" },
                    { 247, "Vallentuna kommun" },
                    { 248, "Vansbro kommun" },
                    { 249, "Vara kommun" },
                    { 250, "Varbergs kommun" },
                    { 251, "Vaxholms kommun" },
                    { 252, "Vellinge kommun" },
                    { 253, "Vetlanda kommun" },
                    { 254, "Vilhelmina kommun" },
                    { 255, "Vimmerby kommun" },
                    { 256, "Vindelns kommun" },
                    { 257, "Vingåkers kommun" },
                    { 258, "Vårgårda kommun" },
                    { 259, "Vänersborgs kommun" },
                    { 260, "Vännäs kommun" },
                    { 261, "Värmdö kommun" },
                    { 262, "Värnamo kommun" },
                    { 263, "Västerviks kommun" },
                    { 264, "Västerås kommun" },
                    { 265, "Växjö kommun" },
                    { 266, "Ydre kommun" },
                    { 267, "Ystads kommun" },
                    { 268, "Åmåls kommun" },
                    { 269, "Ånge kommun" },
                    { 270, "Åre kommun" },
                    { 271, "Årjängs kommun" },
                    { 272, "Åsele kommun" },
                    { 273, "Åstorps kommun" },
                    { 274, "Åtvidabergs kommun" },
                    { 275, "Älmhults kommun" },
                    { 276, "Älvdalens kommun" },
                    { 277, "Älvkarleby kommun" },
                    { 278, "Älvsbyns kommun" },
                    { 279, "Ängelholms kommun" },
                    { 280, "Öckerö kommun" },
                    { 281, "Ödeshögs kommun" },
                    { 282, "Örebro kommun" },
                    { 283, "Örkelljunga kommun" },
                    { 284, "Örnsköldsviks kommun" },
                    { 285, "Östersunds kommun" },
                    { 286, "Österåkers kommun" },
                    { 287, "Östhammars kommun" },
                    { 288, "Östra Göinge kommun" },
                    { 289, "Överkalix kommun" },
                    { 290, "Övertorneå kommun" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Brokers_BrokerFirmId",
                table: "Brokers",
                column: "BrokerFirmId");

            migrationBuilder.CreateIndex(
                name: "IX_Housings_BrokerFirmId",
                table: "Housings",
                column: "BrokerFirmId");

            migrationBuilder.CreateIndex(
                name: "IX_Housings_BrokerId",
                table: "Housings",
                column: "BrokerId");

            migrationBuilder.CreateIndex(
                name: "IX_Housings_CategoryHousingCategoryId",
                table: "Housings",
                column: "CategoryHousingCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Housings_MunicipalityId",
                table: "Housings",
                column: "MunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_HousingId",
                table: "Image",
                column: "HousingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "Housings");

            migrationBuilder.DropTable(
                name: "Brokers");

            migrationBuilder.DropTable(
                name: "HousingCategories");

            migrationBuilder.DropTable(
                name: "Municipalities");

            migrationBuilder.DropTable(
                name: "BrokerFirms");
        }
    }
}
