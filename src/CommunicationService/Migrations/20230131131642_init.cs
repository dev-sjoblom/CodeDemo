using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommunicationService.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Classification",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MetadataType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetadataType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Receiver",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UniqueName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receiver", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MetadataTypeClassification",
                columns: table => new
                {
                    MetadataTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassificationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetadataTypeClassification", x => new { x.ClassificationId, x.MetadataTypeId });
                    table.ForeignKey(
                        name: "FK_MetadataTypeClassification_Classification_ClassificationId",
                        column: x => x.ClassificationId,
                        principalTable: "Classification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MetadataTypeClassification_MetadataType_MetadataTypeId",
                        column: x => x.MetadataTypeId,
                        principalTable: "MetadataType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReceiverClassification",
                columns: table => new
                {
                    ReceiverId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassificationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiverClassification", x => new { x.ClassificationId, x.ReceiverId });
                    table.ForeignKey(
                        name: "FK_ReceiverClassification_Classification_ClassificationId",
                        column: x => x.ClassificationId,
                        principalTable: "Classification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceiverClassification_Receiver_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Receiver",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReceiverMetadata",
                columns: table => new
                {
                    ReceiverId = table.Column<Guid>(type: "uuid", nullable: false),
                    MetadataTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Data = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiverMetadata", x => new { x.ReceiverId, x.MetadataTypeId });
                    table.ForeignKey(
                        name: "FK_ReceiverMetadata_MetadataType_MetadataTypeId",
                        column: x => x.MetadataTypeId,
                        principalTable: "MetadataType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceiverMetadata_Receiver_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Receiver",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classification_Name",
                table: "Classification",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MetadataType_Name",
                table: "MetadataType",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MetadataTypeClassification_MetadataTypeId",
                table: "MetadataTypeClassification",
                column: "MetadataTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Receiver_UniqueName",
                table: "Receiver",
                column: "UniqueName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReceiverClassification_ReceiverId",
                table: "ReceiverClassification",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiverMetadata_MetadataTypeId",
                table: "ReceiverMetadata",
                column: "MetadataTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MetadataTypeClassification");

            migrationBuilder.DropTable(
                name: "ReceiverClassification");

            migrationBuilder.DropTable(
                name: "ReceiverMetadata");

            migrationBuilder.DropTable(
                name: "Classification");

            migrationBuilder.DropTable(
                name: "MetadataType");

            migrationBuilder.DropTable(
                name: "Receiver");
        }
    }
}
