using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reservation.Repository.Data.Migrations
{
    public partial class fillData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insert 246 ParentSeats
            var p = 1;
            for (int Theatre = 0; Theatre < 2; Theatre++)
            {
                for (int Row = 0; Row < 14; Row++)
                {
                    int MaxSeats = Row == 13 ? 6 : 9;
                    for (int Seat = 1; Seat <= MaxSeats; Seat++)
                    {
                        var TheatreNumber = Theatre == 0 ? "Left" : "Right";
                        var RowNumber = Row + 1;
                        var SeatNumber = Theatre * MaxSeats + Row * 18 + Seat;

                        migrationBuilder.InsertData(
                            table: "ParentsSeats",
                            columns: new[] { "Id", "Name", "UserId", "Status" },
                            values: new object[] { p, $"{TheatreNumber} Theatre:R-{RowNumber}:S-{SeatNumber}", null, 0 }
                        );
                        p++;
                    }
                }
            }

            var k = 1;
            for (int Theatre = 0; Theatre < 2; Theatre++)
            {
                int PreviousSeats = 0;
                for (int Row = 0; Row < 7; Row++)
                {
                    int MaxSeats = Row == 0 ? 11 : Row < 3 ? 10 : 9;
                    for (int Seat = 1; Seat <= MaxSeats; Seat++)
                    {
                        var TheatreNumber = Theatre == 0 ? "Left" : "Right";
                        var RowNumber = Row + 1;
                        var SeatNumber = Theatre * MaxSeats + PreviousSeats + Seat;

                        migrationBuilder.InsertData(
                            table: "KidsSeats",
                            columns: new[] { "Id", "Name", "UserId", "Status" },
                            values: new object[] { k, $"{TheatreNumber} Theatre:R-{RowNumber}:S-{SeatNumber}", null, 0 }
                            );
                        k++;
                    }
                    PreviousSeats += MaxSeats;
                }

            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Delete the inserted ParentSeats
            for (int i = 1; i <= 246; i++)
            {
                migrationBuilder.DeleteData(
                    table: "ParentsSeats",
                    keyColumn: "Id",
                    keyValue: i
                );
            }

            // Delete the inserted KidsSeats
            for (int i = 1; i <= 136; i++)
            {
                migrationBuilder.DeleteData(
                    table: "KidsSeats",
                    keyColumn: "Id",
                    keyValue: i
                );
            }
        }
    }
}
