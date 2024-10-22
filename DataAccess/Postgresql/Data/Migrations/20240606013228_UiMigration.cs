using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class UiMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aggregated",
                columns: table => new
                {
                    imageid = table.Column<int>(type: "integer", nullable: false),
                    labelid = table.Column<int>(type: "integer", nullable: false),
                    x1 = table.Column<int>(type: "integer", nullable: false),
                    y1 = table.Column<int>(type: "integer", nullable: false),
                    x2 = table.Column<int>(type: "integer", nullable: false),
                    y2 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aggregated", x => new { x.imageid, x.labelid });
                });

            migrationBuilder.CreateTable(
                name: "Labels",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Labels", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    salt = table.Column<string>(type: "text", nullable: true),
                    refreshToken = table.Column<string>(type: "text", nullable: true),
                    isAdmin = table.Column<bool>(type: "boolean", nullable: false),
                    blockMarks = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    labelID = table.Column<int>(type: "integer", nullable: false),
                    x1 = table.Column<int>(type: "integer", nullable: false),
                    y1 = table.Column<int>(type: "integer", nullable: false),
                    x2 = table.Column<int>(type: "integer", nullable: false),
                    y2 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.id);
                    table.ForeignKey(
                        name: "FK_Areas_Labels_labelID",
                        column: x => x.labelID,
                        principalTable: "Labels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Banned",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userID = table.Column<int>(type: "integer", nullable: false),
                    adminID = table.Column<int>(type: "integer", nullable: false),
                    reason = table.Column<string>(type: "text", nullable: true),
                    banDatetime = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banned", x => x.id);
                    table.ForeignKey(
                        name: "FK_Banned_Users_adminID",
                        column: x => x.adminID,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Banned_Users_userID",
                        column: x => x.userID,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Datasets",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    creatorID = table.Column<int>(type: "integer", nullable: false),
                    loadDatetime = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Datasets", x => x.id);
                    table.ForeignKey(
                        name: "FK_Datasets_Users_creatorID",
                        column: x => x.creatorID,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schemes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    creatorID = table.Column<int>(type: "integer", nullable: false),
                    createDatetime = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schemes", x => x.id);
                    table.ForeignKey(
                        name: "FK_Schemes_Users_creatorID",
                        column: x => x.creatorID,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    datasetID = table.Column<int>(type: "integer", nullable: false),
                    path = table.Column<string>(type: "text", nullable: true),
                    width = table.Column<int>(type: "integer", nullable: false),
                    height = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.id);
                    table.ForeignKey(
                        name: "FK_Images_Datasets_datasetID",
                        column: x => x.datasetID,
                        principalTable: "Datasets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LabelsSchemes",
                columns: table => new
                {
                    labelID = table.Column<int>(type: "integer", nullable: false),
                    schemeID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabelsSchemes", x => new { x.labelID, x.schemeID });
                    table.ForeignKey(
                        name: "FK_LabelsSchemes_Labels_labelID",
                        column: x => x.labelID,
                        principalTable: "Labels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabelsSchemes_Schemes_schemeID",
                        column: x => x.schemeID,
                        principalTable: "Schemes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Marks",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    schemeID = table.Column<int>(type: "integer", nullable: false),
                    imageID = table.Column<int>(type: "integer", nullable: false),
                    creatorID = table.Column<int>(type: "integer", nullable: false),
                    isBlocked = table.Column<bool>(type: "boolean", nullable: false),
                    loadDatetime = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marks", x => x.id);
                    table.ForeignKey(
                        name: "FK_Marks_Images_imageID",
                        column: x => x.imageID,
                        principalTable: "Images",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Marks_Schemes_schemeID",
                        column: x => x.schemeID,
                        principalTable: "Schemes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Marks_Users_creatorID",
                        column: x => x.creatorID,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MarksAreas",
                columns: table => new
                {
                    markedID = table.Column<int>(type: "integer", nullable: false),
                    areaID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarksAreas", x => new { x.areaID, x.markedID });
                    table.ForeignKey(
                        name: "FK_MarksAreas_Areas_areaID",
                        column: x => x.areaID,
                        principalTable: "Areas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MarksAreas_Marks_markedID",
                        column: x => x.markedID,
                        principalTable: "Marks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    markedID = table.Column<int>(type: "integer", nullable: false),
                    creatorID = table.Column<int>(type: "integer", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: true),
                    loadDatetime = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.id);
                    table.ForeignKey(
                        name: "FK_Reports_Marks_markedID",
                        column: x => x.markedID,
                        principalTable: "Marks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reports_Users_creatorID",
                        column: x => x.creatorID,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Areas_labelID",
                table: "Areas",
                column: "labelID");

            migrationBuilder.CreateIndex(
                name: "IX_Banned_adminID",
                table: "Banned",
                column: "adminID");

            migrationBuilder.CreateIndex(
                name: "IX_Banned_userID",
                table: "Banned",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_Datasets_creatorID",
                table: "Datasets",
                column: "creatorID");

            migrationBuilder.CreateIndex(
                name: "IX_Images_datasetID",
                table: "Images",
                column: "datasetID");

            migrationBuilder.CreateIndex(
                name: "IX_LabelsSchemes_schemeID",
                table: "LabelsSchemes",
                column: "schemeID");

            migrationBuilder.CreateIndex(
                name: "IX_Marks_creatorID",
                table: "Marks",
                column: "creatorID");

            migrationBuilder.CreateIndex(
                name: "IX_Marks_imageID",
                table: "Marks",
                column: "imageID");

            migrationBuilder.CreateIndex(
                name: "IX_Marks_schemeID",
                table: "Marks",
                column: "schemeID");

            migrationBuilder.CreateIndex(
                name: "IX_MarksAreas_markedID",
                table: "MarksAreas",
                column: "markedID");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_creatorID",
                table: "Reports",
                column: "creatorID");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_markedID",
                table: "Reports",
                column: "markedID");

            migrationBuilder.CreateIndex(
                name: "IX_Schemes_creatorID",
                table: "Schemes",
                column: "creatorID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aggregated");

            migrationBuilder.DropTable(
                name: "Banned");

            migrationBuilder.DropTable(
                name: "LabelsSchemes");

            migrationBuilder.DropTable(
                name: "MarksAreas");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "Marks");

            migrationBuilder.DropTable(
                name: "Labels");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Schemes");

            migrationBuilder.DropTable(
                name: "Datasets");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
