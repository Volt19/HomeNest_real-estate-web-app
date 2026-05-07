using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HomeNest.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsRead = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NewsletterSubscribers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    SubscribedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsletterSubscribers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    IsAdmin = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<int>(type: "INTEGER", nullable: false),
                    Area = table.Column<int>(type: "INTEGER", nullable: false),
                    Rooms = table.Column<int>(type: "INTEGER", nullable: false),
                    District = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Furnished = table.Column<bool>(type: "INTEGER", nullable: false),
                    Floor = table.Column<int>(type: "INTEGER", nullable: false),
                    Image = table.Column<string>(type: "TEXT", nullable: false),
                    Features = table.Column<string>(type: "TEXT", nullable: false),
                    PriceUnit = table.Column<string>(type: "TEXT", nullable: false),
                    OwnerId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Properties_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    PropertyId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favorites_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favorites_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Id", "Area", "CreatedAt", "Description", "District", "Features", "Floor", "Furnished", "Image", "OwnerId", "Price", "PriceUnit", "Rooms", "Title", "Type" },
                values: new object[,]
                {
                    { 1, 80, new DateTime(2026, 5, 5, 9, 43, 34, 735, DateTimeKind.Utc).AddTicks(7469), "Отдава се апартамент под наем в район Приморски, гр.Варна", "Приморски", "3 стаен,Обзаведен,80 кв.м", 3, true, "images/property-offer-1.jpg", null, 550, "/ месец", 3, "Апартамент", "3-стаен" },
                    { 2, 45, new DateTime(2026, 5, 5, 9, 43, 34, 735, DateTimeKind.Utc).AddTicks(9113), "Уютно студио в центъра на Варна, близо до Морската градина", "Център", "Студио,Обзаведен,45 кв.м", 2, true, "images/property-offer-2.jpg", null, 350, "/ месец", 1, "Студио", "Студио" },
                    { 3, 180, new DateTime(2026, 5, 5, 9, 43, 34, 735, DateTimeKind.Utc).AddTicks(9118), "Просторна семейна къща в тих квартал с градина", "Владиславово", "5 стаен,Необзаведен,180 кв.м", 2, false, "images/property-offer-3.jpg", null, 1200, "/ месец", 5, "Къща", "Къща" },
                    { 4, 140, new DateTime(2026, 5, 5, 9, 43, 34, 735, DateTimeKind.Utc).AddTicks(9122), "Луксозен мезонет с панорамна гледка към морето", "Приморски", "4 стаен,Обзаведен,140 кв.м", 6, true, "images/property-1.jpg", null, 950, "/ месец", 4, "Мезонет", "Мезонет" },
                    { 5, 65, new DateTime(2026, 5, 5, 9, 43, 34, 735, DateTimeKind.Utc).AddTicks(9126), "Приятен двустаен апартамент в квартал Левски", "Левски", "2 стаен,Обзаведен,65 кв.м", 4, true, "images/property-2.jpg", null, 400, "/ месец", 2, "Двустаен", "2-стаен" },
                    { 6, 95, new DateTime(2026, 5, 5, 9, 43, 34, 735, DateTimeKind.Utc).AddTicks(9130), "Нов тристаен апартамент в модерен комплекс в Одесос", "Одесос", "3 стаен,Необзаведен,95 кв.м", 5, false, "images/property-3.jpg", null, 600, "/ месец", 3, "Тристаен", "3-стаен" },
                    { 7, 110, new DateTime(2026, 5, 5, 9, 43, 34, 735, DateTimeKind.Utc).AddTicks(9134), "Офис площ в бизнес сграда в центъра на града", "Център", "3 помещения,Обзаведен,110 кв.м", 3, true, "images/property-1.jpg", null, 800, "/ месец", 3, "Офис", "Офис" },
                    { 8, 55, new DateTime(2026, 5, 5, 9, 43, 34, 735, DateTimeKind.Utc).AddTicks(9138), "Творческо ателие в арт зона на града", "Чайка", "Отворен план,Необзаведен,55 кв.м", 1, false, "images/property-2.jpg", null, 300, "/ месец", 1, "Ателие", "Ателие" },
                    { 9, 160, new DateTime(2026, 5, 5, 9, 43, 34, 735, DateTimeKind.Utc).AddTicks(9143), "Роскошен пентхаус с тераса и барбекю зона", "Приморски", "4 стаен,Обзаведен,160 кв.м", 8, true, "images/property-3.jpg", null, 1500, "/ месец", 4, "Пентхаус", "Пентхаус" },
                    { 10, 58, new DateTime(2026, 5, 5, 9, 43, 34, 735, DateTimeKind.Utc).AddTicks(9147), "Компактен двустаен в близост до университета", "Одесос", "2 стаен,Обзаведен,58 кв.м", 2, true, "images/property-1.jpg", null, 380, "/ месец", 2, "Двустаен", "2-стаен" },
                    { 11, 105, new DateTime(2026, 5, 5, 9, 43, 34, 735, DateTimeKind.Utc).AddTicks(9151), "Панорамен тристаен с изглед към Варненското езеро", "Аспарухово", "3 стаен,Обзаведен,105 кв.м", 7, true, "images/property-2.jpg", null, 700, "/ месец", 3, "Тристаен", "3-стаен" },
                    { 12, 220, new DateTime(2026, 5, 5, 9, 43, 34, 735, DateTimeKind.Utc).AddTicks(9154), "Нова къща в затворен комплекс с охрана", "Владиславово", "6 стаен,Необзаведен,220 кв.м", 2, false, "images/property-3.jpg", null, 1800, "/ месец", 6, "Къща", "Къща" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_PropertyId",
                table: "Favorites",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserId_PropertyId",
                table: "Favorites",
                columns: new[] { "UserId", "PropertyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NewsletterSubscribers_Email",
                table: "NewsletterSubscribers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_OwnerId",
                table: "Properties",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactMessages");

            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "NewsletterSubscribers");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
