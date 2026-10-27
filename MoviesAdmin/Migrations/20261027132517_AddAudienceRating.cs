using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoviesAdmin.Migrations
{
    /// <inheritdoc />
    public partial class AddAudienceRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Migration partially modified with ChatGPT to prevent loss of rating data.

            migrationBuilder.CreateTable(
                name: "AudienceRating",
                columns: table => new
                {
                    AudienceRatingId = table.Column<int>(
                        type: "int",
                        nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),

                    Title = table.Column<string>(
                        type: "nvarchar(20)",
                        maxLength: 20,
                        nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_AudienceRating",
                        x => x.AudienceRatingId);
                });

            // Copy the existing rating values into AudienceRating.
            migrationBuilder.Sql(@"
                INSERT INTO AudienceRating (Title)
                SELECT DISTINCT Rating
                FROM Movie
                WHERE Rating IS NOT NULL;
            ");

            migrationBuilder.AddColumn<int>(
                name: "AudienceRatingId",
                table: "Movie",
                type: "int",
                nullable: true);

            // Associate each movie with its corresponding audience rating.
            migrationBuilder.Sql(@"
                UPDATE Movie
                SET AudienceRatingId = AudienceRating.AudienceRatingId
                FROM Movie
                INNER JOIN AudienceRating
                    ON Movie.Rating = AudienceRating.Title;
            ");

            // Remove the old Rating column after migrating its data.
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Movie");

            // Make the foreign key required after the data has been migrated.
            migrationBuilder.AlterColumn<int>(
                name: "AudienceRatingId",
                table: "Movie",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Movie_AudienceRatingId",
                table: "Movie",
                column: "AudienceRatingId");

            migrationBuilder.CreateIndex(
                name: "IX_AudienceRating_Title",
                table: "AudienceRating",
                column: "Title",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Movie_AudienceRating_AudienceRatingId",
                table: "Movie",
                column: "AudienceRatingId",
                principalTable: "AudienceRating",
                principalColumn: "AudienceRatingId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movie_AudienceRating_AudienceRatingId",
                table: "Movie");

            migrationBuilder.DropTable(
                name: "AudienceRating");

            migrationBuilder.DropIndex(
                name: "IX_Movie_AudienceRatingId",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "AudienceRatingId",
                table: "Movie");

            migrationBuilder.AddColumn<string>(
                name: "Rating",
                table: "Movie",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
