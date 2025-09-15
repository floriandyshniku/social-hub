using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace newProject.Migrations
{
    /// <inheritdoc />
    public partial class FollowRequestEntitySetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FollowRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: false),
                    IsRejected = table.Column<bool>(type: "bit", nullable: false),
                    UserId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FollowRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FollowRequests_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FollowRequests_FromUserId",
                table: "FollowRequests",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowRequests_RequestedAt",
                table: "FollowRequests",
                column: "RequestedAt");

            migrationBuilder.CreateIndex(
                name: "IX_FollowRequests_UserId",
                table: "FollowRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowRequests_UserId1",
                table: "FollowRequests",
                column: "UserId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FollowRequests");
        }
    }
}
