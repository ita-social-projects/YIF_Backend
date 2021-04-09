﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using YIF.Core.Data;

namespace YIF.Core.Data.Migrations
{
    [DbContext(typeof(EFDbContext))]
    [Migration("20210409175025_AddedIoEToGraduate")]
    partial class AddedIoEToGraduate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.BaseUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("BaseUsers");
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.Direction", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Directions");
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.DirectionToInstitutionOfEducation", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("InstitutionOfEducationId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DirectionId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id", "InstitutionOfEducationId", "DirectionId");

                    b.HasIndex("DirectionId");

                    b.HasIndex("InstitutionOfEducationId");

                    b.ToTable("DirectionsToInstitutionOfEducations");
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.Exam", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Exams");
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.ExamRequirement", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ExamId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SpecialtyToIoEDescriptionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Coefficient")
                        .HasColumnType("float");

                    b.Property<double>("MinimumScore")
                        .HasColumnType("float");

                    b.HasKey("Id", "ExamId", "SpecialtyToIoEDescriptionId");

                    b.HasIndex("ExamId");

                    b.HasIndex("SpecialtyToIoEDescriptionId");

                    b.ToTable("ExamRequirements");
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.Graduate", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SchoolId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("SchoolId");

                    b.HasIndex("UserId");

                    b.ToTable("Graduates");
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.IdentityEntities.DbUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.InstitutionOfEducation", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Abbreviation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndOfCampaign")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InstitutionOfEducationType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Lat")
                        .HasColumnType("real");

                    b.Property<float>("Lon")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Site")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartOfCampaign")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("InstitutionOfEducations");
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.InstitutionOfEducationAdmin", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("InstitutionOfEducationId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsBanned")
                        .HasColumnType("bit");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("InstitutionOfEducationId");

                    b.HasIndex("UserId");

                    b.ToTable("InstitutionOfEducationAdmins");
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.InstitutionOfEducationModerator", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AdminId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.HasIndex("UserId");

                    b.ToTable("InstitutionOfEducationModerators");
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.InstitutionOfEducationToGraduate", b =>
                {
                    b.Property<string>("InstitutionOfEducationId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("GraduateId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("InstitutionOfEducationId", "GraduateId");

                    b.HasIndex("GraduateId");

                    b.ToTable("InstitutionOfEducationsToGraduates");
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.Lecture", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("InstitutionOfEducationId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("InstitutionOfEducationId");

                    b.ToTable("Lectures");
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.School", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Schools");
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.SchoolAdmin", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SchoolId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("SchoolId");

                    b.ToTable("SchoolAdmins");
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.SchoolModerator", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AdminId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SchoolId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.HasIndex("SchoolId");

                    b.HasIndex("UserId");

                    b.ToTable("SchoolModerators");
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.Specialty", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DirectionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DirectionId");

                    b.ToTable("Specialties");
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.SpecialtyToGraduate", b =>
                {
                    b.Property<string>("GraduateId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SpecialtyId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("GraduateId", "SpecialtyId");

                    b.HasIndex("SpecialtyId");

                    b.ToTable("SpecialtyToGraduates");
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.SpecialtyToInstitutionOfEducation", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("InstitutionOfEducationId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("SpecialtyId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("InstitutionOfEducationId");

                    b.HasIndex("SpecialtyId");

                    b.ToTable("SpecialtyToInstitutionOfEducations");
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.SpecialtyToInstitutionOfEducationToGraduate", b =>
                {
                    b.Property<string>("SpecialtyId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("InstitutionOfEducationId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("GraduateId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("SpecialtyId", "InstitutionOfEducationId", "GraduateId");

                    b.HasIndex("GraduateId");

                    b.HasIndex("InstitutionOfEducationId");

                    b.ToTable("SpecialtyToInstitutionOfEducationToGraduates");
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.SpecialtyToIoEDescription", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EducationForm")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EducationalProgramLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentForm")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SpecialtyToInstitutionOfEducationId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("SpecialtyToInstitutionOfEducationId");

                    b.ToTable("SpecialtyToIoEDescriptions");
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.SuperAdmin", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.ToTable("SuperAdmins");
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.Token", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<DateTime>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("tblTokens");
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.UserProfile", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("MiddleName")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("Photo")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("tblUserProfiles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("YIF.Core.Data.Entities.IdentityEntities.DbUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("YIF.Core.Data.Entities.IdentityEntities.DbUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YIF.Core.Data.Entities.IdentityEntities.DbUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("YIF.Core.Data.Entities.IdentityEntities.DbUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.BaseUser", b =>
                {
                    b.HasOne("YIF.Core.Data.Entities.IdentityEntities.DbUser", "User")
                        .WithMany("BaseUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.DirectionToInstitutionOfEducation", b =>
                {
                    b.HasOne("YIF.Core.Data.Entities.Direction", "Direction")
                        .WithMany("DirectionToInstitutionOfEducations")
                        .HasForeignKey("DirectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YIF.Core.Data.Entities.InstitutionOfEducation", "InstitutionOfEducation")
                        .WithMany("DirectionToInstitutionOfEducation")
                        .HasForeignKey("InstitutionOfEducationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.ExamRequirement", b =>
                {
                    b.HasOne("YIF.Core.Data.Entities.Exam", "Exam")
                        .WithMany("ExamRequirements")
                        .HasForeignKey("ExamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YIF.Core.Data.Entities.SpecialtyToIoEDescription", "SpecialtyToIoEDescription")
                        .WithMany("ExamRequirements")
                        .HasForeignKey("SpecialtyToIoEDescriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.Graduate", b =>
                {
                    b.HasOne("YIF.Core.Data.Entities.School", "School")
                        .WithMany("Graduates")
                        .HasForeignKey("SchoolId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("YIF.Core.Data.Entities.IdentityEntities.DbUser", "User")
                        .WithMany("Graduates")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.InstitutionOfEducationAdmin", b =>
                {
                    b.HasOne("YIF.Core.Data.Entities.InstitutionOfEducation", "InstitutionOfEducation")
                        .WithMany("Admins")
                        .HasForeignKey("InstitutionOfEducationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("YIF.Core.Data.Entities.IdentityEntities.DbUser", "User")
                        .WithMany("InstitutionOfEducationAdmins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.InstitutionOfEducationModerator", b =>
                {
                    b.HasOne("YIF.Core.Data.Entities.InstitutionOfEducationAdmin", "Admin")
                        .WithMany("Moderators")
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("YIF.Core.Data.Entities.IdentityEntities.DbUser", "User")
                        .WithMany("InstitutionOfEducationModerators")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.InstitutionOfEducationToGraduate", b =>
                {
                    b.HasOne("YIF.Core.Data.Entities.Graduate", "Graduate")
                        .WithMany("InstitutionOfEducationGraduates")
                        .HasForeignKey("GraduateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YIF.Core.Data.Entities.InstitutionOfEducation", "InstitutionOfEducation")
                        .WithMany("InstitutionOfEducationGraduates")
                        .HasForeignKey("InstitutionOfEducationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.Lecture", b =>
                {
                    b.HasOne("YIF.Core.Data.Entities.IdentityEntities.DbUser", "User")
                        .WithMany("Lectures")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YIF.Core.Data.Entities.InstitutionOfEducation", "InstitutionOfEducation")
                        .WithMany("Lectures")
                        .HasForeignKey("InstitutionOfEducationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.SchoolAdmin", b =>
                {
                    b.HasOne("YIF.Core.Data.Entities.School", "School")
                        .WithMany("Admins")
                        .HasForeignKey("SchoolId");
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.SchoolModerator", b =>
                {
                    b.HasOne("YIF.Core.Data.Entities.SchoolAdmin", "Admin")
                        .WithMany("SchoolModerators")
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("YIF.Core.Data.Entities.School", "School")
                        .WithMany("Moderators")
                        .HasForeignKey("SchoolId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("YIF.Core.Data.Entities.IdentityEntities.DbUser", "User")
                        .WithMany("SchoolModerators")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.Specialty", b =>
                {
                    b.HasOne("YIF.Core.Data.Entities.Direction", "Direction")
                        .WithMany("Specialties")
                        .HasForeignKey("DirectionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.SpecialtyToGraduate", b =>
                {
                    b.HasOne("YIF.Core.Data.Entities.Graduate", "Graduate")
                        .WithMany("SpecialtyToGraduates")
                        .HasForeignKey("GraduateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YIF.Core.Data.Entities.Specialty", "Specialty")
                        .WithMany("SpecialtyToGraduates")
                        .HasForeignKey("SpecialtyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.SpecialtyToInstitutionOfEducation", b =>
                {
                    b.HasOne("YIF.Core.Data.Entities.InstitutionOfEducation", "InstitutionOfEducation")
                        .WithMany("SpecialtyToInstitutionOfEducations")
                        .HasForeignKey("InstitutionOfEducationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("YIF.Core.Data.Entities.Specialty", "Specialty")
                        .WithMany("SpecialtyToInstitutionOfEducations")
                        .HasForeignKey("SpecialtyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.SpecialtyToInstitutionOfEducationToGraduate", b =>
                {
                    b.HasOne("YIF.Core.Data.Entities.Graduate", "Graduate")
                        .WithMany("SpecialtyToInstitutionOfEducationToGraduates")
                        .HasForeignKey("GraduateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YIF.Core.Data.Entities.InstitutionOfEducation", "InstitutionOfEducation")
                        .WithMany("SpecialtyToInstitutionOfEducationToGraduates")
                        .HasForeignKey("InstitutionOfEducationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YIF.Core.Data.Entities.Specialty", "Specialty")
                        .WithMany("SpecialtyToInstitutionOfEducationToGraduates")
                        .HasForeignKey("SpecialtyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.SpecialtyToIoEDescription", b =>
                {
                    b.HasOne("YIF.Core.Data.Entities.SpecialtyToInstitutionOfEducation", "SpecialtyToInstitutionOfEducation")
                        .WithMany("SpecialtyToIoEDescriptions")
                        .HasForeignKey("SpecialtyToInstitutionOfEducationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.SuperAdmin", b =>
                {
                    b.HasOne("YIF.Core.Data.Entities.IdentityEntities.DbUser", "User")
                        .WithMany("SuperAdmins")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.Token", b =>
                {
                    b.HasOne("YIF.Core.Data.Entities.IdentityEntities.DbUser", "User")
                        .WithOne("Token")
                        .HasForeignKey("YIF.Core.Data.Entities.Token", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("YIF.Core.Data.Entities.UserProfile", b =>
                {
                    b.HasOne("YIF.Core.Data.Entities.IdentityEntities.DbUser", "User")
                        .WithOne("UserProfile")
                        .HasForeignKey("YIF.Core.Data.Entities.UserProfile", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
