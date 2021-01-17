using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        public async static Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (roleManager.Roles.Count() == 0)
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
            if (context.SuperAdmins.Count() == 0)
            {
                var dbUser = new DbUser
                {
                    Email = "superAdmin@gmail.com",
                    UserName = "SuperAdmin"
                };

                var superAdmin = new SuperAdmin
                {
                    User = dbUser
                };

                await SeederDB.CreateUser(context, userManager, dbUser, ProjectRoles.SuperAdmin, superAdmin);
            }

        }

        #region School

        public static void SeedSchools(EFDbContext context)
        {
            if (context.Schools.Count() == 0)
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
            if (context.SchoolAdmins.Count() == 0)
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
            if (context.SchoolModerators.Count() == 0)
            {
                var schools = context.Schools.ToList();
                var schoolAdmins = context.SchoolAdmins.ToList();

                #region ЗОШ №3
                {
                    var dbUser = new DbUser
                    {
                        Email = "anataly@gmail.com",
                        UserName = "Moderator228"
                    };
                    var schoolModerator = new SchoolModerator
                    {
                        AdminId = schoolAdmins.FirstOrDefault(x => x.School.Name == "ЗОШ №3").Id,
                        SchoolId = schools.FirstOrDefault(x => x.Name == "ЗОШ №3").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.SchoolAdmin, schoolModerator);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "vasiliy@gmail.com",
                        UserName = "KitVasil"
                    };
                    var schoolModerator = new SchoolModerator
                    {
                        SchoolId = schools.FirstOrDefault(x => x.Name == "ЗОШ №3").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.SchoolModerator, schoolModerator);
                }
                #endregion

                #region Школа №4  
                {
                    var dbUser = new DbUser
                    {
                        Email = "snakSmash@gmail.com",
                        UserName = "SuperSmash"
                    };
                    var schoolModerator = new SchoolModerator
                    {
                        AdminId = schoolAdmins.Where(x => x.School.Name == "Школа №4").First().Id,
                        SchoolId = schools.FirstOrDefault(x => x.Name == "Школа №4").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.SchoolAdmin, schoolModerator);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "8edua@factwalk.com",
                        UserName = "DidPanas"
                    };
                    var schoolModerator = new SchoolModerator
                    {
                        AdminId = schoolAdmins.Where(x => x.School.Name == "Школа №4").ToList()[1].Id,
                        SchoolId = schools.FirstOrDefault(x => x.Name == "Школа №4").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.SchoolAdmin, schoolModerator);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "bromSava@gmail.com",
                        UserName = "BezymniyMax" // :)
                    };
                    var schoolModerator = new SchoolModerator
                    {
                        SchoolId = schools.FirstOrDefault(x => x.Name == "Школа №4").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.SchoolModerator, schoolModerator);
                }
                #endregion

                #region Рівненська гімназія Гармонія
                {
                    var dbUser = new DbUser
                    {
                        Email = "counterDown23@gmail.com",
                        UserName = "AdminKachalki"
                    };
                    var schoolModerator = new SchoolModerator
                    {
                        AdminId = schoolAdmins.Where(x => x.School.Name == "Рівненська гімназія \"Гармонія\"").First().Id,
                        SchoolId = schools.FirstOrDefault(x => x.Name == "Рівненська гімназія \"Гармонія\"").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.SchoolAdmin, schoolModerator);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "supremeSavage@gmail.com",
                        UserName = "MinorSouce"
                    };
                    var schoolModerator = new SchoolModerator
                    {
                        SchoolId = schools.FirstOrDefault(x => x.Name == "Рівненська гімназія \"Гармонія\"").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.SchoolModerator, schoolModerator);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "vorobSetin@gmail.com",
                        UserName = "AndriiMetov"
                    };
                    var schoolModerator = new SchoolModerator
                    {
                        SchoolId = schools.FirstOrDefault(x => x.Name == "Рівненська гімназія \"Гармонія\"").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.SchoolModerator, schoolModerator);
                }
                #endregion

                #region Школа №25
                {
                    var dbUser = new DbUser
                    {
                        Email = "brownWizzard09@gmail.com",
                        UserName = "TomHalson"
                    };
                    var schoolModerator = new SchoolModerator
                    {
                        AdminId = schoolAdmins.Where(x => x.School.Name == "Школа №25").First().Id,
                        SchoolId = schools.FirstOrDefault(x => x.Name == "Школа №25").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.SchoolAdmin, schoolModerator);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "intelIdea@gmail.com",
                        UserName = "HowardTzirdzia"
                    };
                    var schoolModerator = new SchoolModerator
                    {
                        SchoolId = schools.FirstOrDefault(x => x.Name == "Школа №25").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.SchoolModerator, schoolModerator);
                }
                #endregion
            }
        }

        public async static Task SeedGraduates(EFDbContext context, UserManager<DbUser> userManager)
        {
            if (context.Graduates.Count() == 0)
            {
                string currentSchool = string.Empty;
                var schools = context.Schools.ToList();

                #region ЗОШ №3
                currentSchool = schools.FirstOrDefault(x => x.Name == "ЗОШ №3").Id;

                {
                    var dbUser = new DbUser
                    {
                        Email = "cabbas21212121b@banot.net",
                        UserName = "ChristopherGraves"
                    };
                    var graduate = new Graduate
                    {
                        User = dbUser,
                        SchoolId = currentSchool
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.Graduate, graduate);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "6necum.how@silentsuite.com",
                        UserName = "IsaakSeymour"
                    };
                    var graduate = new Graduate
                    {
                        User = dbUser,
                        SchoolId = currentSchool
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.Graduate, graduate);
                }
                #endregion

                #region Школа №4
                currentSchool = schools.FirstOrDefault(x => x.Name == "Школа №4").Id;

                {
                    var dbUser = new DbUser
                    {
                        Email = "rarige@thegrovebnb.com",
                        UserName = "MikhailRahman"
                    };
                    var graduate = new Graduate
                    {
                        User = dbUser,
                        SchoolId = currentSchool
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.Graduate, graduate);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "umanogabida@partajona.com",
                        UserName = "ClaudiaAdam"
                    };
                    var graduate = new Graduate
                    {
                        User = dbUser,
                        SchoolId = currentSchool
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.Graduate, graduate);
                }
                #endregion

                #region Рівненська гімназія Гармонія
                currentSchool = schools.FirstOrDefault(x => x.Name == "Рівненська гімназія \"Гармонія\"").Id;

                {
                    var dbUser = new DbUser
                    {
                        Email = "wshubcnbn@memprof.com",
                        UserName = "GaryFerguson"
                    };
                    var graduate = new Graduate
                    {
                        User = dbUser,
                        SchoolId = currentSchool
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.Graduate, graduate);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "fbrylle.bhousz7@iklankeren.pw",
                        UserName = "SamsonBannister"
                    };
                    var graduate = new Graduate
                    {
                        User = dbUser,
                        SchoolId = currentSchool
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.Graduate, graduate);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "43azzyr@chrisjoyce.net",
                        UserName = "EmilisRandall"
                    };
                    var graduate = new Graduate
                    {
                        User = dbUser,
                        SchoolId = currentSchool
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.Graduate, graduate);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "tshadyalex1a@azel.xyz",
                        UserName = "JoshuaMacfarlane"
                    };
                    var graduate = new Graduate
                    {
                        User = dbUser,
                        SchoolId = currentSchool
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.Graduate, graduate);
                }
                #endregion

                #region Школа №25
                currentSchool = schools.FirstOrDefault(x => x.Name == "Школа №25").Id;

                {
                    var dbUser = new DbUser
                    {
                        Email = "jyass@bomtool.com",
                        UserName = "MihaiHanna"
                    };
                    var graduate = new Graduate
                    {
                        User = dbUser,
                        SchoolId = currentSchool
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.Graduate, graduate);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "shadj_hadjf@maliberty.com",
                        UserName = "JeremiahGibson"
                    };
                    var graduate = new Graduate
                    {
                        User = dbUser,
                        SchoolId = currentSchool
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.Graduate, graduate);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "tzezogamil673@subrolive.com",
                        UserName = "MontelJeffery"
                    };
                    var graduate = new Graduate
                    {
                        User = dbUser,
                        SchoolId = currentSchool
                    };

                    await CreateUser(context, userManager, dbUser, ProjectRoles.Graduate, graduate);
                }
                #endregion
            }
        }

        #endregion

        #region University

        public static void SeedDirections(EFDbContext context)
        {
            if (context.Directions.Count() == 0)
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
            if (context.Specialities.Count() == 0)
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
            if (context.Universities.Count() == 0)
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
            if (context.DirectionsToUniversities.Count() == 0 || context.SpecialityToUniversities.Count() == 0)
            {
                var directions = context.Directions.ToList();
                var specialities = context.Specialities.ToList();
                var universities = context.Universities.ToList();

                var directionsToUniversities = new List<DirectionToUniversity>();
                var specialitiesToUniversities = new List<SpecialityToUniversity>();

                // Random seeding
                universities.ForEach(x =>
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

                        for (int j = 0; j < new Random().Next(specialities.Where(x => x.DirectionId == directions[i].Id).Count() - 3,
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
            if (context.Lectures.Count() == 0)
            {
                var universities = context.Universities.ToList();

                #region НУВГП
                {
                    var dbUser = new DbUser
                    {
                        Email = "wformservices@triumphlotto.com",
                        UserName = "RayyanFord"
                    };
                    var lecture = new Lecture
                    {

                        UniversityId = universities.FirstOrDefault(x => x.Name == "Національний університет водного господарства та природокористування").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.Lecture, lecture);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "8wesclei_rjf@guestify.com",
                        UserName = "FedericoPierce"
                    };
                    var lecture = new Lecture
                    {

                        UniversityId = universities.FirstOrDefault(x => x.Name == "Національний університет водного господарства та природокористування").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.Lecture, lecture);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "imali@tuneintogiving.com",
                        UserName = "HamzahNeville"
                    };
                    var lecture = new Lecture
                    {

                        UniversityId = universities.FirstOrDefault(x => x.Name == "Національний університет водного господарства та природокористування").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.Lecture, lecture);
                }
                #endregion

                #region КПІ
                {
                    var dbUser = new DbUser
                    {
                        Email = "xsdhafer1@usayummy.com",
                        UserName = "StephanPlummer"
                    };
                    var lecture = new Lecture
                    {

                        UniversityId = universities.FirstOrDefault(x => x.Name == "Київський політехнічний інститут імені Ігоря Сікорського").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.Lecture, lecture);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "vweslaine.pg@ericreyess.com",
                        UserName = "AbrahamStafford"
                    };
                    var lecture = new Lecture
                    {

                        UniversityId = universities.FirstOrDefault(x => x.Name == "Київський політехнічний інститут імені Ігоря Сікорського").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.Lecture, lecture);
                }
                #endregion

                #region АВВУ
                {
                    var dbUser = new DbUser
                    {
                        Email = "7wave@rose2.ga",
                        UserName = "BradleighHagan"
                    };
                    var lecture = new Lecture
                    {

                        UniversityId = universities.FirstOrDefault(x => x.Name == "Академія внутрішніх військ МВС України").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.Lecture, lecture);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "fmcaagent908@rose2.ga",
                        UserName = "GarinBurrows"
                    };
                    var lecture = new Lecture
                    {

                        UniversityId = universities.FirstOrDefault(x => x.Name == "Академія внутрішніх військ МВС України").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.Lecture, lecture);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "gjuninho.silva.3s@jbnasfjhas96637.ml",
                        UserName = "KaciePalmer"
                    };
                    var lecture = new Lecture
                    {

                        UniversityId = universities.FirstOrDefault(x => x.Name == "Академія внутрішніх військ МВС України").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.Lecture, lecture);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "6amine.kikira2@nx-mail.com",
                        UserName = "KadeWood"
                    };
                    var lecture = new Lecture
                    {

                        UniversityId = universities.FirstOrDefault(x => x.Name == "Академія внутрішніх військ МВС України").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.Lecture, lecture);
                }
                #endregion
            }
        }

        public async static Task SeedUniversityModerators(EFDbContext context, UserManager<DbUser> userManager)
        {
            if (context.UniversityModerators.Count() == 0)
            {
                var universities = context.Universities.ToList();
                var admins = context.UniversityAdmins.ToList();

                #region НУВГП
                {
                    var dbUser = new DbUser
                    {
                        Email = "qtoni6@gmail.com",
                        UserName = "ArnoldBeasley"
                    };
                    var universityModerator = new UniversityModerator
                    {
                        AdminId = admins.FirstOrDefault(x => x.University.Name == "Національний університет водного господарства та природокористування").Id,
                        UniversityId = universities.FirstOrDefault(x => x.Name == "Національний університет водного господарства та природокористування").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.UniversityAdmin, universityModerator);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "cfarid.nadji2r@devist.com",
                        UserName = "SafwanWickens"
                    };
                    var universityModerator = new UniversityModerator
                    {
                        UniversityId = universities.FirstOrDefault(x => x.Name == "Національний університет водного господарства та природокористування").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.UniversityModerator, universityModerator);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "hselma_kra@jomcs.com",
                        UserName = "DanieleHicks"
                    };
                    var universityModerator = new UniversityModerator
                    {
                        UniversityId = universities.FirstOrDefault(x => x.Name == "Національний університет водного господарства та природокористування").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.UniversityModerator, universityModerator);
                }
                #endregion

                #region КПІ
                {
                    var dbUser = new DbUser
                    {
                        Email = "dill.pazee@azel.xyz",
                        UserName = "GiaVang"
                    };
                    var universityModerator = new UniversityModerator
                    {
                        AdminId = admins.FirstOrDefault(x => x.University.Name == "Київський політехнічний інститут імені Ігоря Сікорського").Id,
                        UniversityId = universities.FirstOrDefault(x => x.Name == "Київський політехнічний інститут імені Ігоря Сікорського").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.UniversityAdmin, universityModerator);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "2soso@hustletussle.com",
                        UserName = "SuhailMcloughlin"
                    };
                    var universityModerator = new UniversityModerator
                    {
                        UniversityId = universities.FirstOrDefault(x => x.Name == "Київський політехнічний інститут імені Ігоря Сікорського").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.UniversityModerator, universityModerator);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "vhamimi.salah@nanbianshan.com",
                        UserName = "MirzaReed"
                    };
                    var universityModerator = new UniversityModerator
                    {
                        UniversityId = universities.FirstOrDefault(x => x.Name == "Київський політехнічний інститут імені Ігоря Сікорського").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.UniversityModerator, universityModerator);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "7chhavi.s@cagi.ru",
                        UserName = "AntonRoberts"
                    };
                    var universityModerator = new UniversityModerator
                    {
                        UniversityId = universities.FirstOrDefault(x => x.Name == "Київський політехнічний інститут імені Ігоря Сікорського").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.UniversityModerator, universityModerator);
                }
                #endregion

                #region АВВУ
                {
                    var dbUser = new DbUser
                    {
                        Email = "fmourad_v7w@hotmail-s.com",
                        UserName = "AlexisHolding"
                    };
                    var universityModerator = new UniversityModerator
                    {
                        AdminId = admins.FirstOrDefault(x => x.University.Name == "Академія внутрішніх військ МВС України").Id,
                        UniversityId = universities.FirstOrDefault(x => x.Name == "Академія внутрішніх військ МВС України").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.UniversityAdmin, universityModerator);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "jleart.blakaj2n@twseel.com",
                        UserName = "ConorBloggs"
                    };
                    var universityModerator = new UniversityModerator
                    {
                        UniversityId = universities.FirstOrDefault(x => x.Name == "Академія внутрішніх військ МВС України").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.UniversityModerator, universityModerator);
                }
                #endregion
            }
        }

        #endregion

        static async Task CreateUser(EFDbContext context,
           UserManager<DbUser> userManager,
           DbUser dbUser,
           string roleName,
           Object entityUser)
        {
            var result = await userManager.CreateAsync(dbUser, "QWerty-1");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(dbUser, roleName);

                await context.AddAsync(entityUser);
                await context.SaveChangesAsync();
            }
        }

        public async static void SeedData(IServiceProvider services)
        {
            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var manager = scope.ServiceProvider.GetRequiredService<UserManager<DbUser>>();
                var managerRole = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var context = scope.ServiceProvider.GetRequiredService<EFDbContext>();

                Console.WriteLine("Database migrating ... ");
                context.Database.Migrate();
                Console.WriteLine("Database migrated");

                if (context.Users.Count() == 0)
                {
                    Console.WriteLine("Database seeding ... ");

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

                    await SeederDB.SeedRoles(managerRole);
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
                Console.WriteLine("Database seeded.");
            }
        }
    }
}