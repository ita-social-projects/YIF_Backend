﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Others;

namespace YIF.Core.Data
{
    public class SeederDB
    {
        public async static Task SeedRoles(EFDbContext context,RoleManager<IdentityRole> roleManager)
        {
            if(context.Roles.Count() == 0)
            {
                var superAdminRole = new IdentityRole(ProjectRoles.SuperAdmin);
                await roleManager.CreateAsync(superAdminRole);

                var schoolAdminRole = new IdentityRole(ProjectRoles.SchoolAdmin);
                await roleManager.CreateAsync(schoolAdminRole);

                var universityAdminRole = new IdentityRole(ProjectRoles.UniversityAdmin);
                await roleManager.CreateAsync(universityAdminRole);

                var schoolModeratorRole = new IdentityRole(ProjectRoles.SchoolModerator);
                await roleManager.CreateAsync(schoolModeratorRole);

                var universityModeratorRole = new IdentityRole(ProjectRoles.UniversityModerator);
                await roleManager.CreateAsync(universityModeratorRole);

                var lectureRole = new IdentityRole(ProjectRoles.Lecture);
                await roleManager.CreateAsync(lectureRole);

                var graduateRole = new IdentityRole(ProjectRoles.Graduate);
                await roleManager.CreateAsync(graduateRole);
            }
        }

        public async static Task SeedSuperAdmin(EFDbContext context, UserManager<DbUser> userManager)
        {
            if(context.SuperAdmins.Count() == 0)
            {
                var dbUser = new DbUser
                {
                    Email = "superAdmin@gmail.com",
                    UserName = "Super Admin"
                };

                var superAdmin = new SuperAdmin 
                {
                    User = dbUser
                };

                await userManager.CreateAsync(dbUser, "Super");
                await context.SuperAdmins.AddAsync(superAdmin);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.SuperAdmin);
                context.SaveChanges();

            }

        }

        #region School

        public static void SeedSchools(EFDbContext context)
        {
            if(context.Schools.Count() == 0)
            {
                var schools = new List<School>();

                schools.Add(new School 
                {
                    Name = "ЗОШ №3",
                    Description = "Рівненська загальноосвітня школа І-ІІІ ступенів № 3"
                });

                schools.Add(new School
                {
                    Name = "Школа №4",
                    Description = "Рівненська загальноосвітня школа І-ІІІ ступенів №4"
                });

                schools.Add(new School
                {
                    Name = "Рівненська гімназія \"Гармонія\"",
                    Description = "Рівненська гімназія, міської ради"
                });

                schools.Add(new School
                {
                    Name = "Школа №25",
                    Description = "Рівненська загальноосвітня школа І-ІІІ ступенів №25"
                });

                context.Schools.AddRange(schools);
                context.SaveChanges();
            }
        }

        public static void SeedSchoolAdmins(EFDbContext context)
        {
            if(context.SchoolAdmins.Count() == 0)
            {
                var admins = new List<SchoolAdmin>();
                string currentSchool = string.Empty;
                var schools = context.Schools.ToList();

                #region ЗОШ №3
                currentSchool = schools.FirstOrDefault(x => x.Name == "ЗОШ №3").Id;

                admins.Add(new SchoolAdmin
                {
                    SchoolId = currentSchool 
                });
                #endregion

                #region Школа №4
                currentSchool = schools.FirstOrDefault(x => x.Name == "Школа №4").Id;

                admins.Add(new SchoolAdmin
                {
                    SchoolId = currentSchool
                });

                admins.Add(new SchoolAdmin
                {
                    SchoolId = currentSchool
                });
                #endregion

                #region Рівненська гімназія Гармонія
                currentSchool = schools.FirstOrDefault(x => x.Name == "Рівненська гімназія \"Гармонія\"").Id;

                admins.Add(new SchoolAdmin
                {
                    SchoolId = currentSchool
                });
                #endregion

                #region Школа №25
                currentSchool = schools.FirstOrDefault(x => x.Name == "Школа №25").Id;

                admins.Add(new SchoolAdmin
                {
                    SchoolId = currentSchool
                });
                #endregion

                context.SchoolAdmins.AddRange(admins);
                context.SaveChanges();
            }
        }

        public async static Task SeedSchoolModerators(EFDbContext context, UserManager<DbUser> userManager)
        {
            if(context.SchoolModerators.Count() == 0)
            {
                #region ЗОШ №3
                var dbUser = new DbUser
                {
                    Email = "anataly@gmail.com",
                    UserName = "Moderator228"                 
                };
                var schoolModerator = new SchoolModerator
                {
                    AdminId = context.SchoolAdmins.FirstOrDefault(x => x.School.Name == "ЗОШ №3").Id,
                    SchoolId = context.Schools.FirstOrDefault(x => x.Name == "ЗОШ №3").Id,
                    User = dbUser
                };
                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.SchoolModerators.AddAsync(schoolModerator);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.SchoolAdmin);


                dbUser = new DbUser
                {
                    Email = "vasiliy@gmail.com",
                    UserName = "KitVasil"
                };
                schoolModerator = new SchoolModerator
                {
                    SchoolId = context.Schools.FirstOrDefault(x => x.Name == "ЗОШ №3").Id,
                    User = dbUser
                };
                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.SchoolModerators.AddAsync(schoolModerator);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.SchoolModerator);
                #endregion

                #region Школа №4
                dbUser = new DbUser
                {
                    Email = "snakSmash@gmail.com",
                    UserName = "Super Smash"
                };
                schoolModerator = new SchoolModerator
                {
                    AdminId = context.SchoolAdmins.Where(x => x.School.Name == "Школа №4").First().Id,
                    SchoolId = context.Schools.FirstOrDefault(x => x.Name == "Школа №4").Id,
                    User = dbUser
                };
                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.SchoolModerators.AddAsync(schoolModerator);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.SchoolAdmin);


                dbUser = new DbUser
                {
                    Email = "vasiliy@gmail.com",
                    UserName = "Kit Vasil"
                };
                schoolModerator = new SchoolModerator
                {
                    AdminId = context.SchoolAdmins.Where(x => x.School.Name == "Школа №4").ToList()[1].Id,
                    SchoolId = context.Schools.FirstOrDefault(x => x.Name == "Школа №4").Id,
                    User = dbUser
                };
                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.SchoolModerators.AddAsync(schoolModerator);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.SchoolAdmin);


                dbUser = new DbUser
                {
                    Email = "bromSava@gmail.com",
                    UserName = "Bezymniy Max" // :)
                };
                schoolModerator = new SchoolModerator
                {
                    SchoolId = context.Schools.FirstOrDefault(x => x.Name == "Школа №4").Id,
                    User = dbUser
                };
                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.SchoolModerators.AddAsync(schoolModerator);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.SchoolModerator);
                #endregion

                #region Рівненська гімназія Гармонія
                dbUser = new DbUser
                {
                    Email = "counterDown23@gmail.com",
                    UserName = "Admin Kachalki"
                };
                schoolModerator = new SchoolModerator
                {
                    AdminId = context.SchoolAdmins.Where(x => x.School.Name == "Рівненська гімназія \"Гармонія\"").First().Id,
                    SchoolId = context.Schools.FirstOrDefault(x => x.Name == "Рівненська гімназія \"Гармонія\"").Id,
                    User = dbUser
                };
                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.SchoolModerators.AddAsync(schoolModerator); 
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.SchoolAdmin);


                dbUser = new DbUser
                {
                    Email = "supremeSavage@gmail.com",
                    UserName = "Minor Souce"
                };
                schoolModerator = new SchoolModerator
                {
                    SchoolId = context.Schools.FirstOrDefault(x => x.Name == "Рівненська гімназія \"Гармонія\"").Id,
                    User = dbUser
                };
                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.SchoolModerators.AddAsync(schoolModerator);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.SchoolModerator);


                dbUser = new DbUser
                {
                    Email = "vorobSetin@gmail.com",
                    UserName = "Andrii Metov"
                };
                schoolModerator = new SchoolModerator
                {
                    SchoolId = context.Schools.FirstOrDefault(x => x.Name == "Рівненська гімназія \"Гармонія\"").Id,
                    User = dbUser
                };
                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.SchoolModerators.AddAsync(schoolModerator);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.SchoolModerator);
                #endregion

                #region Школа №25
                dbUser = new DbUser
                {
                    Email = "brownWizzard09@gmail.com",
                    UserName = "Tom Halson"
                };
                schoolModerator = new SchoolModerator
                {
                    AdminId = context.SchoolAdmins.Where(x => x.School.Name == "Школа №25").First().Id,
                    SchoolId = context.Schools.FirstOrDefault(x => x.Name == "Школа №25").Id,
                    User = dbUser
                };
                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.SchoolModerators.AddAsync(schoolModerator);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.SchoolAdmin);


                dbUser = new DbUser
                {
                    Email = "intelIdea@gmail.com",
                    UserName = "Howard Tzirdzia"
                };
                schoolModerator = new SchoolModerator
                {
                    SchoolId = context.Schools.FirstOrDefault(x => x.Name == "Школа №25").Id,
                    User = dbUser
                };
                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.SchoolModerators.AddAsync(schoolModerator);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.SchoolModerator);
                #endregion

                context.SaveChanges();
            }
        }

        public async static Task SeedGraduates(EFDbContext context, UserManager<DbUser> userManager)
        {
            if(context.Graduates.Count() == 0)
            {
                string currentSchool = string.Empty;
                var schools = context.Schools.ToList();

                #region ЗОШ №3
                currentSchool = schools.FirstOrDefault(x => x.Name == "ЗОШ №3").Id;

                var dbUser = new DbUser
                {
                    Email = "cabbas21212121b@banot.net",
                    UserName = "Christopher Graves"
                };
                var graduate = new Graduate
                {
                    User = dbUser,
                    SchoolId = currentSchool
                };

                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.Graduates.AddAsync(graduate);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.Graduate);


                dbUser = new DbUser
                {
                    Email = "6necum.how@silentsuite.com",
                    UserName = "Isaak Seymour"
                };
                graduate = new Graduate
                {
                    User = dbUser,
                    SchoolId = currentSchool
                };

                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.Graduates.AddAsync(graduate);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.Graduate);
                #endregion

                #region Школа №4
                currentSchool = schools.FirstOrDefault(x => x.Name == "Школа №4").Id;

                dbUser = new DbUser
                {
                    Email = "rarige@thegrovebnb.com",
                    UserName = "Mikhail Rahman"
                };
                graduate = new Graduate
                {
                    User = dbUser,
                    SchoolId = currentSchool
                };

                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.Graduates.AddAsync(graduate);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.Graduate);


                dbUser = new DbUser
                {
                    Email = "umanogabida@partajona.com",
                    UserName = "Claudia Adam"
                };
                graduate = new Graduate
                {
                    User = dbUser,
                    SchoolId = currentSchool
                };

                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.Graduates.AddAsync(graduate);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.Graduate);
                #endregion

                #region Рівненська гімназія Гармонія
                currentSchool = schools.FirstOrDefault(x => x.Name == "Рівненська гімназія \"Гармонія\"").Id;

                dbUser = new DbUser
                {
                    Email = "wshubcnbn@memprof.com",
                    UserName = "Gary Ferguson"
                };
                graduate = new Graduate
                {
                    User = dbUser,
                    SchoolId = currentSchool
                };

                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.Graduates.AddAsync(graduate);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.Graduate);


                dbUser = new DbUser
                {
                    Email = "fbrylle.bhousz7@iklankeren.pw",
                    UserName = "Samson Bannister"
                };
                graduate = new Graduate
                {
                    User = dbUser,
                    SchoolId = currentSchool
                };

                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.Graduates.AddAsync(graduate);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.Graduate);


                dbUser = new DbUser
                {
                    Email = "43azzyr@chrisjoyce.net",
                    UserName = "Emilis Randall"
                };
                graduate = new Graduate
                {
                    User = dbUser,
                    SchoolId = currentSchool
                };

                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.Graduates.AddAsync(graduate);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.Graduate);


                dbUser = new DbUser
                {
                    Email = "tshadyalex1a@azel.xyz",
                    UserName = "Joshua Macfarlane"
                };
                graduate = new Graduate
                {
                    User = dbUser,
                    SchoolId = currentSchool
                };

                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.Graduates.AddAsync(graduate);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.Graduate);
                #endregion

                #region Школа №25
                currentSchool = schools.FirstOrDefault(x => x.Name == "Школа №25").Id;

                dbUser = new DbUser
                {
                    Email = "jyass@bomtool.com",
                    UserName = "Mihai Hanna"
                };
                graduate = new Graduate
                {
                    User = dbUser,
                    SchoolId = currentSchool
                };

                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.Graduates.AddAsync(graduate);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.Graduate);


                dbUser = new DbUser
                {
                    Email = "shadj_hadjf@maliberty.com",
                    UserName = "Jeremiah Gibson"
                };
                graduate = new Graduate
                {
                    User = dbUser,
                    SchoolId = currentSchool
                };

                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.Graduates.AddAsync(graduate);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.Graduate);


                dbUser = new DbUser
                {
                    Email = "tzezogamil673@subrolive.com",
                    UserName = "Montel Jeffery"
                };
                graduate = new Graduate
                {
                    User = dbUser,
                    SchoolId = currentSchool
                };

                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.Graduates.AddAsync(graduate);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.Graduate);
                #endregion

                context.SaveChanges();
            }
        }

        #endregion

        #region University

        public static void SeedDirections(EFDbContext context)
        {
            if(context.Directions.Count() == 0)
            {
                var directions = new List<Direction>();

                directions.Add(new Direction 
                {
                    Name = "Інформаційні технології"
                });

                context.Directions.AddRange(directions);
                context.SaveChanges();
            }
        }

        public static void SeedSpecialities(EFDbContext context)
        {
            if(context.Specialities.Count() == 0)
            {
                string currentDirection = string.Empty;
                var specialities = new List<Speciality>();

                #region Інформаційні технології
                currentDirection = context.Directions.FirstOrDefault(x => x.Name == "Інформаційні технології").Id;

                specialities.Add(new Speciality 
                { 
                    Name = "Комп'ютерні науки",
                    DirectionId = currentDirection
                });

                specialities.Add(new Speciality
                {
                    Name = "Інженерія програмного забезпечення",
                    DirectionId = currentDirection
                });

                specialities.Add(new Speciality
                {
                    Name = "Комп’ютерна інженерія",
                    DirectionId = currentDirection
                });

                specialities.Add(new Speciality
                {
                    Name = "Кібербезпека",
                    DirectionId = currentDirection
                });

                specialities.Add(new Speciality
                {
                    Name = "Інформаційні системи та технології",
                    DirectionId = currentDirection
                });

                specialities.Add(new Speciality
                {
                    Name = "Системний аналіз",
                    DirectionId = currentDirection
                });
                #endregion

                context.Specialities.AddRange(specialities);
                context.SaveChanges(); 
            }
        }

        public static void SeedUniversities(EFDbContext context)
        {
            if(context.Universities.Count() == 0)
            {
                var univerities = new List<University>();

                #region НУВГП
                univerities.Add(new University
                {
                    Name = "Національний університет водного господарства та природокористування",
                    Description = "Єдиний в Україні вищий навчальний заклад водогосподарського профілю." +
                    " Заклад є навчально-науковим комплексом, що здійснює підготовку висококваліфікованих фахівців," +
                    " науково-педагогічних кадрів, забезпечує підвищення кваліфікації фахівців та" +
                    " проводить науково-дослідну роботу. ",                  
                });
                #endregion

                #region КПІ
                univerities.Add(new University 
                {
                    Name = "Київський політехнічний інститут імені Ігоря Сікорського",
                    Description = "Заклад вищої освіти інженерного профілю," +
                    " заснований в Києві у 1898 р., на сьогодні це один із найбільших університетів" +
                    " України за кількістю студентів з широким спектром спеціальностей і освітніх програм " +
                    "для підготовки фахівців з технічних і гуманітарних наук"
                });
                #endregion

                #region АВВУ
                univerities.Add(new University
                {
                    Name = "Академія внутрішніх військ МВС України",
                    Description = "Державний вищий навчальний заклад IV рівня акредитації," +
                    " підпорядкований Міністерству внутрішніх справ України та розташований у Харкові"
                });
                #endregion

                context.Universities.AddRange(univerities);
                context.SaveChanges();
            }
        }

        public static void SeedDirectionsAndSpecialitiesToUniversity(EFDbContext context)
        {
            if(context.DirectionsToUniversities.Count() == 0 || context.SpecialityToUniversities.Count() == 0)
            {
                var directions = context.Directions.ToList();
                var specialities = context.Specialities.ToList();
                var universities = context.Universities.ToList();

                var directionsToUniversities = new List<DirectionToUniversity>();
                var specialitiesToUniversities = new List<SpecialityToUniversity>();

                // Random seeding
                universities.ForEach (x =>
                 {
                     int rand = new Random().Next(0, directions.Count());
                     if (rand <= 0) rand = 1;

                     for (int i = 0; i < rand; i++)
                     {
                         directionsToUniversities.Add(new DirectionToUniversity
                         {
                             UniversityId = x.Id,
                             DirectionId = directions[i].Id
                         });

                         for (int j = 0; j < new Random().Next(1,
                              specialities.Where(x => x.DirectionId == directions[i].Id).Count());
                              j++)
                         {
                             specialitiesToUniversities.Add(new SpecialityToUniversity 
                             {
                                 SpecialityId = specialities[j].Id,
                                 UniversityId = universities[i].Id
                             });
                         }
                     }
                 });


                context.DirectionsToUniversities.AddRange(directionsToUniversities);
                context.SpecialityToUniversities.AddRange(specialitiesToUniversities);
                context.SaveChanges();

            }
        }

        public static void SeedUniversityAdmins(EFDbContext context)
        {
            var admins = new List<UniversityAdmin>();
            string currentUniversity = string.Empty;
            var universities = context.Universities.ToList();

            #region НУВГП
            currentUniversity = universities.FirstOrDefault(x => x.Name == "Національний університет водного господарства та природокористування").Id;

            admins.Add(new UniversityAdmin 
            {
                UniversityId = currentUniversity
            });
            #endregion

            #region КПІ
            currentUniversity = universities.FirstOrDefault(x => x.Name == "Київський політехнічний інститут імені Ігоря Сікорського").Id;

            admins.Add(new UniversityAdmin
            {
                UniversityId = currentUniversity
            });
            #endregion

            #region АВВУ
            currentUniversity = universities.FirstOrDefault(x => x.Name == "Академія внутрішніх військ МВС України").Id;

            admins.Add(new UniversityAdmin
            {
                UniversityId = currentUniversity
            });
            #endregion

            context.UniversityAdmins.AddRange(admins);
            context.SaveChanges();
        }

        public async static Task SeedLectures(EFDbContext context, UserManager<DbUser> userManager)
        {
            if(context.Lectures.Count() == 0)
            {
                var universities = context.Universities.ToList();

                #region НУВГП
                var dbUser = new DbUser
                {
                    Email = "wformservices@triumphlotto.com",
                    UserName = "Rayyan Ford"
                };
                var lecture = new Lecture
                {
                   
                    UniversityId = universities.FirstOrDefault(x => x.Name == "Національний університет водного господарства та природокористування").Id,
                    User = dbUser
                };
                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.Lectures.AddAsync(lecture);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.Lecture);


                dbUser = new DbUser
                {
                    Email = "8wesclei_rjf@guestify.com",
                    UserName = "Federico Pierce"
                };
                lecture = new Lecture
                {

                    UniversityId = universities.FirstOrDefault(x => x.Name == "Національний університет водного господарства та природокористування").Id,
                    User = dbUser
                };
                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.Lectures.AddAsync(lecture);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.Lecture);


                dbUser = new DbUser
                {
                    Email = "imali@tuneintogiving.com",
                    UserName = "Hamzah Neville"
                };
                lecture = new Lecture
                {

                    UniversityId = universities.FirstOrDefault(x => x.Name == "Національний університет водного господарства та природокористування").Id,
                    User = dbUser
                };
                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.Lectures.AddAsync(lecture);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.Lecture);
                #endregion

                #region КПІ
                dbUser = new DbUser
                {
                    Email = "xsdhafer1@usayummy.com",
                    UserName = "Stephan Plummer"
                };
                lecture = new Lecture
                {

                    UniversityId = universities.FirstOrDefault(x => x.Name == "Київський політехнічний інститут імені Ігоря Сікорського").Id,
                    User = dbUser
                };
                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.Lectures.AddAsync(lecture);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.Lecture);


                dbUser = new DbUser
                {
                    Email = "vweslaine.pg@ericreyess.com",
                    UserName = "Abraham Stafford"
                };
                lecture = new Lecture
                {

                    UniversityId = universities.FirstOrDefault(x => x.Name == "Київський політехнічний інститут імені Ігоря Сікорського").Id,
                    User = dbUser
                };
                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.Lectures.AddAsync(lecture);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.Lecture);
                #endregion

                #region АВВУ
                dbUser = new DbUser
                {
                    Email = "7wave@rose2.ga",
                    UserName = "Bradleigh Hagan"
                };
                lecture = new Lecture
                {

                    UniversityId = universities.FirstOrDefault(x => x.Name == "Академія внутрішніх військ МВС України").Id,
                    User = dbUser
                };
                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.Lectures.AddAsync(lecture);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.Lecture);


                dbUser = new DbUser
                {
                    Email = "fmcaagent908@rose2.ga",
                    UserName = "Garin Burrows"
                };
                lecture = new Lecture
                {

                    UniversityId = universities.FirstOrDefault(x => x.Name == "Академія внутрішніх військ МВС України").Id,
                    User = dbUser
                };
                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.Lectures.AddAsync(lecture);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.Lecture);


                dbUser = new DbUser
                {
                    Email = "gjuninho.silva.3s@jbnasfjhas96637.ml",
                    UserName = "Kacie Palmer"
                };
                lecture = new Lecture
                {

                    UniversityId = universities.FirstOrDefault(x => x.Name == "Академія внутрішніх військ МВС України").Id,
                    User = dbUser
                };
                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.Lectures.AddAsync(lecture);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.Lecture);


                dbUser = new DbUser
                {
                    Email = "6amine.kikira2@nx-mail.com",
                    UserName = "Kade Wood"
                };
                lecture = new Lecture
                {

                    UniversityId = universities.FirstOrDefault(x => x.Name == "Академія внутрішніх військ МВС України").Id,
                    User = dbUser
                };
                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.Lectures.AddAsync(lecture);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.Lecture);
                #endregion

                context.SaveChanges();
            }
        }

        public async static Task SeedUniversityModerators(EFDbContext context, UserManager<DbUser> userManager)
        {
            if(context.UniversityModerators.Count() == 0)
            {
                var universities = context.Universities.ToList();
                var admins = context.UniversityAdmins.ToList();

                #region НУВГП
                var dbUser = new DbUser
                {
                    Email = "qtoni6@gmail.com",
                    UserName = "Arnold Beasley"
                };
                var universityModerator = new UniversityModerator
                {
                    AdminId = admins.FirstOrDefault(x => x.University.Name == "Національний університет водного господарства та природокористування").Id,
                    UniversityId = universities.FirstOrDefault(x => x.Name == "Національний університет водного господарства та природокористування").Id,
                    User = dbUser
                };
                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.UniversityModerators.AddAsync(universityModerator);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.UniversityAdmin);


                dbUser = new DbUser
                {
                    Email = "cfarid.nadji2r@devist.com",
                    UserName = "Safwan Wickens"
                };
                universityModerator = new UniversityModerator
                {
                    UniversityId = universities.FirstOrDefault(x => x.Name == "Національний університет водного господарства та природокористування").Id,
                    User = dbUser
                };
                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.UniversityModerators.AddAsync(universityModerator);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.UniversityModerator);


                dbUser = new DbUser
                {
                    Email = "hselma_kra@jomcs.com",
                    UserName = "Daniele Hicks"
                };
                universityModerator = new UniversityModerator
                {
                    UniversityId = universities.FirstOrDefault(x => x.Name == "Національний університет водного господарства та природокористування").Id,
                    User = dbUser
                };
                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.UniversityModerators.AddAsync(universityModerator);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.UniversityModerator);
                #endregion

                #region КПІ
                dbUser = new DbUser
                {
                    Email = "dill.pazee@azel.xyz",
                    UserName = "Gia Vang"
                };
                universityModerator = new UniversityModerator
                {
                    AdminId = admins.FirstOrDefault(x => x.University.Name == "Київський політехнічний інститут імені Ігоря Сікорського").Id,
                    UniversityId = universities.FirstOrDefault(x => x.Name == "Київський політехнічний інститут імені Ігоря Сікорського").Id,
                    User = dbUser
                };
                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.UniversityModerators.AddAsync(universityModerator);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.UniversityAdmin);


                dbUser = new DbUser
                {
                    Email = "2soso@hustletussle.com",
                    UserName = "Suhail Mcloughlin"
                };
                universityModerator = new UniversityModerator
                {
                    UniversityId = universities.FirstOrDefault(x => x.Name == "Київський політехнічний інститут імені Ігоря Сікорського").Id,
                    User = dbUser
                };
                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.UniversityModerators.AddAsync(universityModerator);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.UniversityModerator);


                dbUser = new DbUser
                {
                    Email = "vhamimi.salah@nanbianshan.com",
                    UserName = "Mirza Reed"
                };
                universityModerator = new UniversityModerator
                {
                    UniversityId = universities.FirstOrDefault(x => x.Name == "Київський політехнічний інститут імені Ігоря Сікорського").Id,
                    User = dbUser
                };
                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.UniversityModerators.AddAsync(universityModerator);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.UniversityModerator);


                dbUser = new DbUser
                {
                    Email = "7chhavi.s@cagi.ru",
                    UserName = "Anton Roberts"
                };
                universityModerator = new UniversityModerator
                {
                    UniversityId = universities.FirstOrDefault(x => x.Name == "Київський політехнічний інститут імені Ігоря Сікорського").Id,
                    User = dbUser
                };
                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.UniversityModerators.AddAsync(universityModerator);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.UniversityModerator);
                #endregion

                #region АВВУ
                dbUser = new DbUser
                {
                    Email = "fmourad_v7w@hotmail-s.com",
                    UserName = "Alexis Holding"
                };
                universityModerator = new UniversityModerator
                {
                    AdminId = admins.FirstOrDefault(x => x.University.Name == "Академія внутрішніх військ МВС України").Id,
                    UniversityId = universities.FirstOrDefault(x => x.Name == "Академія внутрішніх військ МВС України").Id,
                    User = dbUser
                };
                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.UniversityModerators.AddAsync(universityModerator);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.UniversityAdmin);


                dbUser = new DbUser
                {
                    Email = "jleart.blakaj2n@twseel.com",
                    UserName = "Conor Bloggs"
                };
                universityModerator = new UniversityModerator
                {
                    UniversityId = universities.FirstOrDefault(x => x.Name == "Академія внутрішніх військ МВС України").Id,
                    User = dbUser
                };
                await userManager.CreateAsync(dbUser, "QWerty-1");
                await context.UniversityModerators.AddAsync(universityModerator);
                await userManager.AddToRoleAsync(dbUser, ProjectRoles.UniversityModerator);

                #endregion

                context.SaveChanges();
            }
        } 

        #endregion


        public async static void SeedData(IServiceProvider services)
        {
            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var manager = scope.ServiceProvider.GetRequiredService<UserManager<DbUser>>();
                var managerRole = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var context = scope.ServiceProvider.GetRequiredService<EFDbContext>();


                // Roles: 7
                // SuperAdmins: 1

                // Schools: 4
                // SchoolAdmins: 5
                // SchoolModerators: 10
                // Graduates: 11

                // Directions: 1
                // Specialities: 6
                // Universities: 3
                // UniversityAdmins: 3
                // UniversityModerators: 9
                // Lectures: 9

                await SeederDB.SeedRoles(context, managerRole);
                await SeederDB.SeedSuperAdmin(context, manager);

                #region School
                SeederDB.SeedSchools(context);
                SeederDB.SeedSchoolAdmins(context);
                await SeederDB.SeedSchoolModerators(context, manager);
                await SeederDB.SeedGraduates(context, manager);
                #endregion

                #region University
                SeederDB.SeedDirections(context);
                SeederDB.SeedSpecialities(context);
                SeederDB.SeedDirectionsAndSpecialitiesToUniversity(context);
                SeederDB.SeedUniversities(context);
                SeederDB.SeedUniversityAdmins(context);
                await SeederDB.SeedUniversityModerators(context, manager);
                await SeederDB.SeedLectures(context, manager);
                #endregion


            }

        }
    }
}
