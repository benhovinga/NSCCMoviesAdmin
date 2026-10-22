using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoviesAdmin.Migrations
{
    /// <inheritdoc />
    public partial class AddGenreRelationshipToMovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GenreId",
                table: "Movie",
                type: "int",
                nullable: true);
            
            // Create the new Genre table
            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    GenreId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.GenreId);
                });

            // Extract the first genre, trim whitespace, and insert unique values.
            migrationBuilder.Sql("""
                INSERT INTO Genre (Title)
                SELECT DISTINCT NormalizedGenre
                FROM (
                    SELECT COALESCE(
                        NULLIF(
                            LTRIM(RTRIM(
                                LEFT(
                                    Genre,
                                    CHARINDEX(',', Genre + ',') - 1
                                )
                            )),
                            ''
                        ),
                        'Unknown'
                    ) AS NormalizedGenre
                    FROM Movie
                ) AS NormalizedGenres;
                """);

            // Assign each movie the ID of its normalized genre.
            migrationBuilder.Sql("""
                UPDATE Movie
                SET GenreId = Genre.GenreId
                FROM Movie
                INNER JOIN Genre
                    ON Genre.Title = COALESCE(
                        NULLIF(
                            LTRIM(RTRIM(
                                LEFT(
                                    Movie.Genre,
                                    CHARINDEX(',', Movie.Genre + ',') - 1
                                )
                            )),
                            ''
                        ),
                        'Unknown'
                    );
                """);

            // Validate genres were assigned
            migrationBuilder.Sql("""
                IF EXISTS (SELECT 1 FROM Movie WHERE GenreId IS NULL)
                    THROW 50001, 'Some movies could not be assigned a genre.', 1;
                """);

            // Make GenreId nullable
            migrationBuilder.AlterColumn<int>(
                name: "GenreId",
                table: "Movie",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            // Create indexes and foreign keys
            migrationBuilder.CreateIndex(
                name: "IX_Movie_GenreId",
                table: "Movie",
                column: "GenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movie_Genre_GenreId",
                table: "Movie",
                column: "GenreId",
                principalTable: "Genre",
                principalColumn: "GenreId",
                onDelete: ReferentialAction.Cascade);

            // Drop old Genre column
            migrationBuilder.DropColumn(
                name: "Genre",
                table: "Movie");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movie_Genre_GenreId",
                table: "Movie");

            migrationBuilder.DropTable(
                name: "Genre");

            migrationBuilder.DropIndex(
                name: "IX_Movie_GenreId",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "GenreId",
                table: "Movie");

            migrationBuilder.AddColumn<string>(
                name: "Genre",
                table: "Movie",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
