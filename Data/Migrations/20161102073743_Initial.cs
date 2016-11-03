using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenGameList.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    display_name = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: false),
                    flags = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    notes = table.Column<string>(nullable: true),
                    type = table.Column<int>(nullable: false),
                    user_name = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "items",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    created_date = table.Column<DateTime>(nullable: false),
                    description = table.Column<string>(nullable: true),
                    flags = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    notes = table.Column<string>(nullable: true),
                    text = table.Column<string>(nullable: true),
                    title = table.Column<string>(nullable: false),
                    type = table.Column<int>(nullable: false),
                    user_id = table.Column<string>(nullable: false),
                    view_count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_items", x => x.id);
                    table.ForeignKey(
                        name: "FK_items_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    created_date = table.Column<DateTime>(nullable: false),
                    flags = table.Column<int>(nullable: false),
                    item_id = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    text = table.Column<string>(nullable: false),
                    type = table.Column<int>(nullable: false),
                    user_id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comments", x => x.id);
                    table.ForeignKey(
                        name: "FK_comments_items_item_id",
                        column: x => x.item_id,
                        principalTable: "items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_comments_comments_ParentId",
                        column: x => x.ParentId,
                        principalTable: "comments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_comments_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_comments_item_id",
                table: "comments",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "IX_comments_ParentId",
                table: "comments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_comments_user_id",
                table: "comments",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_items_user_id",
                table: "items",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.DropTable(
                name: "items");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
