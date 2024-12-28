using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iPlayground.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    ContactInfo = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SyncLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SyncTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    ErrorMessage = table.Column<string>(type: "TEXT", nullable: false),
                    SyncType = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyncLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VoucherValidations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FiscalNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    JIB = table.Column<string>(type: "TEXT", nullable: false),
                    IsValid = table.Column<bool>(type: "INTEGER", nullable: false),
                    ErrorMessage = table.Column<string>(type: "TEXT", nullable: false),
                    ValidationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    QRCode = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherValidations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Children",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Children", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Children_Parents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyPasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ChildId = table.Column<int>(type: "INTEGER", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    QrCode = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyPasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthlyPasses_Children_ChildId",
                        column: x => x.ChildId,
                        principalTable: "Children",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ChildId = table.Column<int>(type: "INTEGER", nullable: false),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    LossAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HasLoss = table.Column<bool>(type: "INTEGER", nullable: false),
                    TotalVaucer = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsFinished = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSynced = table.Column<bool>(type: "INTEGER", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsPaused = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    PauseStartTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsPauseOverdue = table.Column<bool>(type: "INTEGER", nullable: false),
                    ShowEndButton = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanPause = table.Column<bool>(type: "INTEGER", nullable: false),
                    PauseButtonText = table.Column<string>(type: "TEXT", nullable: false),
                    SessionStatus = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessions_Children_ChildId",
                        column: x => x.ChildId,
                        principalTable: "Children",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SessionVouchers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SessionId = table.Column<int>(type: "INTEGER", nullable: false),
                    FiscalNumber = table.Column<string>(type: "TEXT", nullable: false),
                    QRCode = table.Column<string>(type: "TEXT", nullable: false),
                    OriginalAmount = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    ScanTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsValid = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionVouchers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionVouchers_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vouchers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FiscalNumber = table.Column<string>(type: "TEXT", nullable: false),
                    FiscalQRCode = table.Column<string>(type: "TEXT", nullable: false),
                    OriginalAmount = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    IsUsed = table.Column<bool>(type: "INTEGER", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    SessionId = table.Column<int>(type: "INTEGER", nullable: false),
                    JIB = table.Column<string>(type: "TEXT", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ValidationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ValidationMessage = table.Column<string>(type: "TEXT", nullable: false),
                    IsValid = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vouchers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vouchers_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Children_ParentId",
                table: "Children",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyPasses_ChildId",
                table: "MonthlyPasses",
                column: "ChildId");

            migrationBuilder.CreateIndex(
                name: "IX_Parents_Phone",
                table: "Parents",
                column: "Phone");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_ChildId",
                table: "Sessions",
                column: "ChildId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_StartTime",
                table: "Sessions",
                column: "StartTime");

            migrationBuilder.CreateIndex(
                name: "IX_SessionVouchers_FiscalNumber",
                table: "SessionVouchers",
                column: "FiscalNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SessionVouchers_QRCode",
                table: "SessionVouchers",
                column: "QRCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SessionVouchers_SessionId",
                table: "SessionVouchers",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_FiscalNumber",
                table: "Vouchers",
                column: "FiscalNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_FiscalQRCode",
                table: "Vouchers",
                column: "FiscalQRCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_SessionId",
                table: "Vouchers",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherValidations_FiscalNumber",
                table: "VoucherValidations",
                column: "FiscalNumber");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherValidations_QRCode",
                table: "VoucherValidations",
                column: "QRCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonthlyPasses");

            migrationBuilder.DropTable(
                name: "SessionVouchers");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "SyncLogs");

            migrationBuilder.DropTable(
                name: "Vouchers");

            migrationBuilder.DropTable(
                name: "VoucherValidations");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "Children");

            migrationBuilder.DropTable(
                name: "Parents");
        }
    }
}
