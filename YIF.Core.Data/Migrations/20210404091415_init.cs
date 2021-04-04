using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YIF.Core.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Directions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Directions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Exams",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InstitutionOfEducations",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Abbreviation = table.Column<string>(nullable: true),
                    Site = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ImagePath = table.Column<string>(nullable: true),
                    Lat = table.Column<float>(nullable: false),
                    Lon = table.Column<float>(nullable: false),
                    InstitutionOfEducationType = table.Column<string>(nullable: false),
                    StartOfCampaign = table.Column<DateTime>(nullable: false),
                    EndOfCampaign = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionOfEducations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schools",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BaseUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SuperAdmins",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuperAdmins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuperAdmins_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblTokens",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    RefreshToken = table.Column<string>(maxLength: 100, nullable: false),
                    RefreshTokenExpiryTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblTokens_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblUserProfiles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Surname = table.Column<string>(maxLength: 255, nullable: true),
                    MiddleName = table.Column<string>(maxLength: 255, nullable: true),
                    Photo = table.Column<string>(maxLength: 255, nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: true),
                    RegistrationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUserProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblUserProfiles_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Specialties",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    DirectionId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Specialties_Directions_DirectionId",
                        column: x => x.DirectionId,
                        principalTable: "Directions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DirectionsToInstitutionOfEducations",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    DirectionId = table.Column<string>(nullable: false),
                    InstitutionOfEducationId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectionsToInstitutionOfEducations", x => new { x.Id, x.InstitutionOfEducationId, x.DirectionId });
                    table.ForeignKey(
                        name: "FK_DirectionsToInstitutionOfEducations_Directions_DirectionId",
                        column: x => x.DirectionId,
                        principalTable: "Directions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DirectionsToInstitutionOfEducations_InstitutionOfEducations_InstitutionOfEducationId",
                        column: x => x.InstitutionOfEducationId,
                        principalTable: "InstitutionOfEducations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InstitutionOfEducationAdmins",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    IsBanned = table.Column<bool>(nullable: false),
                    InstitutionOfEducationId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionOfEducationAdmins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstitutionOfEducationAdmins_InstitutionOfEducations_InstitutionOfEducationId",
                        column: x => x.InstitutionOfEducationId,
                        principalTable: "InstitutionOfEducations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstitutionOfEducationAdmins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lectures",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    InstitutionOfEducationId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lectures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lectures_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lectures_InstitutionOfEducations_InstitutionOfEducationId",
                        column: x => x.InstitutionOfEducationId,
                        principalTable: "InstitutionOfEducations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Graduates",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    SchoolId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Graduates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Graduates_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Graduates_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolAdmins",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    SchoolId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolAdmins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolAdmins_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SpecialtyToInstitutionOfEducations",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    SpecialtyId = table.Column<string>(nullable: true),
                    InstitutionOfEducationId = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialtyToInstitutionOfEducations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialtyToInstitutionOfEducations_InstitutionOfEducations_InstitutionOfEducationId",
                        column: x => x.InstitutionOfEducationId,
                        principalTable: "InstitutionOfEducations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecialtyToInstitutionOfEducations_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InstitutionOfEducationModerators",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AdminId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionOfEducationModerators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstitutionOfEducationModerators_InstitutionOfEducationAdmins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "InstitutionOfEducationAdmins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstitutionOfEducationModerators_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InstitutionOfEducationsToGraduates",
                columns: table => new
                {
                    GraduateId = table.Column<string>(nullable: false),
                    InstitutionOfEducationId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionOfEducationsToGraduates", x => new { x.InstitutionOfEducationId, x.GraduateId });
                    table.ForeignKey(
                        name: "FK_InstitutionOfEducationsToGraduates_Graduates_GraduateId",
                        column: x => x.GraduateId,
                        principalTable: "Graduates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstitutionOfEducationsToGraduates_InstitutionOfEducations_InstitutionOfEducationId",
                        column: x => x.InstitutionOfEducationId,
                        principalTable: "InstitutionOfEducations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecialtyToGraduates",
                columns: table => new
                {
                    SpecialtyId = table.Column<string>(nullable: false),
                    GraduateId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialtyToGraduates", x => new { x.GraduateId, x.SpecialtyId });
                    table.ForeignKey(
                        name: "FK_SpecialtyToGraduates_Graduates_GraduateId",
                        column: x => x.GraduateId,
                        principalTable: "Graduates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecialtyToGraduates_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecialtyToInstitutionOfEducationToGraduates",
                columns: table => new
                {
                    SpecialtyId = table.Column<string>(nullable: false),
                    InstitutionOfEducationId = table.Column<string>(nullable: false),
                    GraduateId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialtyToInstitutionOfEducationToGraduates", x => new { x.SpecialtyId, x.InstitutionOfEducationId, x.GraduateId });
                    table.ForeignKey(
                        name: "FK_SpecialtyToInstitutionOfEducationToGraduates_Graduates_GraduateId",
                        column: x => x.GraduateId,
                        principalTable: "Graduates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecialtyToInstitutionOfEducationToGraduates_InstitutionOfEducations_InstitutionOfEducationId",
                        column: x => x.InstitutionOfEducationId,
                        principalTable: "InstitutionOfEducations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecialtyToInstitutionOfEducationToGraduates_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolModerators",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    SchoolId = table.Column<string>(nullable: true),
                    AdminId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolModerators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolModerators_SchoolAdmins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "SchoolAdmins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolModerators_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolModerators_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecialtyToIoEDescriptions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    SpecialtyToInstitutionOfEducationId = table.Column<string>(nullable: true),
                    PaymentForm = table.Column<string>(nullable: false),
                    EducationForm = table.Column<string>(nullable: false),
                    EducationalProgramLink = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialtyToIoEDescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialtyToIoEDescriptions_SpecialtyToInstitutionOfEducations_SpecialtyToInstitutionOfEducationId",
                        column: x => x.SpecialtyToInstitutionOfEducationId,
                        principalTable: "SpecialtyToInstitutionOfEducations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamRequirements",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ExamId = table.Column<string>(nullable: false),
                    SpecialtyToIoEDescriptionId = table.Column<string>(nullable: false),
                    MinimumScore = table.Column<double>(nullable: false),
                    Coefficient = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamRequirements", x => new { x.Id, x.ExamId, x.SpecialtyToIoEDescriptionId });
                    table.ForeignKey(
                        name: "FK_ExamRequirements_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamRequirements_SpecialtyToIoEDescriptions_SpecialtyToIoEDescriptionId",
                        column: x => x.SpecialtyToIoEDescriptionId,
                        principalTable: "SpecialtyToIoEDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BaseUsers_UserId",
                table: "BaseUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectionsToInstitutionOfEducations_DirectionId",
                table: "DirectionsToInstitutionOfEducations",
                column: "DirectionId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectionsToInstitutionOfEducations_InstitutionOfEducationId",
                table: "DirectionsToInstitutionOfEducations",
                column: "InstitutionOfEducationId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamRequirements_ExamId",
                table: "ExamRequirements",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamRequirements_SpecialtyToIoEDescriptionId",
                table: "ExamRequirements",
                column: "SpecialtyToIoEDescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Graduates_SchoolId",
                table: "Graduates",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Graduates_UserId",
                table: "Graduates",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionOfEducationAdmins_InstitutionOfEducationId",
                table: "InstitutionOfEducationAdmins",
                column: "InstitutionOfEducationId");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionOfEducationAdmins_UserId",
                table: "InstitutionOfEducationAdmins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionOfEducationModerators_AdminId",
                table: "InstitutionOfEducationModerators",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionOfEducationModerators_UserId",
                table: "InstitutionOfEducationModerators",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionOfEducationsToGraduates_GraduateId",
                table: "InstitutionOfEducationsToGraduates",
                column: "GraduateId");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_InstitutionOfEducationId",
                table: "Lectures",
                column: "InstitutionOfEducationId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolAdmins_SchoolId",
                table: "SchoolAdmins",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolModerators_AdminId",
                table: "SchoolModerators",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolModerators_SchoolId",
                table: "SchoolModerators",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolModerators_UserId",
                table: "SchoolModerators",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_DirectionId",
                table: "Specialties",
                column: "DirectionId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialtyToGraduates_SpecialtyId",
                table: "SpecialtyToGraduates",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialtyToInstitutionOfEducations_InstitutionOfEducationId",
                table: "SpecialtyToInstitutionOfEducations",
                column: "InstitutionOfEducationId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialtyToInstitutionOfEducations_SpecialtyId",
                table: "SpecialtyToInstitutionOfEducations",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialtyToInstitutionOfEducationToGraduates_GraduateId",
                table: "SpecialtyToInstitutionOfEducationToGraduates",
                column: "GraduateId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialtyToInstitutionOfEducationToGraduates_InstitutionOfEducationId",
                table: "SpecialtyToInstitutionOfEducationToGraduates",
                column: "InstitutionOfEducationId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialtyToIoEDescriptions_SpecialtyToInstitutionOfEducationId",
                table: "SpecialtyToIoEDescriptions",
                column: "SpecialtyToInstitutionOfEducationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BaseUsers");

            migrationBuilder.DropTable(
                name: "DirectionsToInstitutionOfEducations");

            migrationBuilder.DropTable(
                name: "ExamRequirements");

            migrationBuilder.DropTable(
                name: "InstitutionOfEducationModerators");

            migrationBuilder.DropTable(
                name: "InstitutionOfEducationsToGraduates");

            migrationBuilder.DropTable(
                name: "Lectures");

            migrationBuilder.DropTable(
                name: "SchoolModerators");

            migrationBuilder.DropTable(
                name: "SpecialtyToGraduates");

            migrationBuilder.DropTable(
                name: "SpecialtyToInstitutionOfEducationToGraduates");

            migrationBuilder.DropTable(
                name: "SuperAdmins");

            migrationBuilder.DropTable(
                name: "tblTokens");

            migrationBuilder.DropTable(
                name: "tblUserProfiles");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Exams");

            migrationBuilder.DropTable(
                name: "SpecialtyToIoEDescriptions");

            migrationBuilder.DropTable(
                name: "InstitutionOfEducationAdmins");

            migrationBuilder.DropTable(
                name: "SchoolAdmins");

            migrationBuilder.DropTable(
                name: "Graduates");

            migrationBuilder.DropTable(
                name: "SpecialtyToInstitutionOfEducations");

            migrationBuilder.DropTable(
                name: "Schools");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "InstitutionOfEducations");

            migrationBuilder.DropTable(
                name: "Specialties");

            migrationBuilder.DropTable(
                name: "Directions");
        }
    }
}
