using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Shared;

namespace YIF.Core.Data.Seaders
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

                var baseUserRole = new IdentityRole(ProjectRoles.BaseUser);
                await roleManager.CreateAsync(baseUserRole);
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

        public async static Task SeedDirections(EFDbContext context)
        {
            if (context.Directions.Count() == 0)
            {
                await context.Directions.AddRangeAsync(new List<Direction> { 
                    new Direction { Name = "Соціальні та поведінкові науки", Code = "05" },
                    new Direction { Name = "Математика та статистика", Code = "11" },
                    new Direction { Name = "Інформаційні технології", Code = "12" },
                    new Direction { Name = "Електрична інженерія", Code = "14" },
                    new Direction { Name = "Автоматизація та приладобудування", Code = "15" }
                });
                await context.SaveChangesAsync();
            }
        }

        public async static Task SeedSpecialities(EFDbContext context)
        {
            if (context.Specialties.Count() == 0)
            {
                string currentDirection = string.Empty;
                var specialities = new List<Specialty>();

                #region Інформаційні технології
                currentDirection = context.Directions.FirstOrDefault(x => x.Name == "Інформаційні технології").Id;

                specialities.Add(new Specialty
                {
                    Name = "Інженерія програмного забезпечення",
                    Code = "121",
                    DirectionId = currentDirection
                });

                specialities.Add(new Specialty
                {
                    Name = "Комп'ютерні науки",
                    Code = "122",
                    DirectionId = currentDirection
                });

                specialities.Add(new Specialty
                {
                    Name = "Комп’ютерна інженерія",
                    Code = "123",
                    DirectionId = currentDirection
                });

                specialities.Add(new Specialty
                {
                    Name = "Системний аналіз",
                    Code = "124",
                    DirectionId = currentDirection
                });

                specialities.Add(new Specialty
                {
                    Name = "Кібербезпека",
                    Code = "125",
                    DirectionId = currentDirection
                });

                specialities.Add(new Specialty
                {
                    Name = "Інформаційні системи та технології",
                    Code = "126",
                    DirectionId = currentDirection
                });
                #endregion

                #region Математика та статистика
                currentDirection = context.Directions.FirstOrDefault(x => x.Name == "Математика та статистика").Id;

                specialities.Add(new Specialty
                {
                    Name = "Математика",
                    Code = "111",
                    DirectionId = currentDirection
                });

                specialities.Add(new Specialty
                {
                    Name = "Статистика",
                    Code = "112",
                    DirectionId = currentDirection
                });

                specialities.Add(new Specialty
                {
                    Name = "Прикладна математика",
                    Code = "113",
                    DirectionId = currentDirection
                });
                #endregion

                #region Соціальні та поведінкові науки
                currentDirection = context.Directions.FirstOrDefault(x => x.Name == "Соціальні та поведінкові науки").Id;

                specialities.Add(new Specialty
                {
                    Name = "Економіка",
                    Code = "051",
                    DirectionId = currentDirection
                });

                specialities.Add(new Specialty
                {
                    Name = "Політологія",
                    Code = "052",
                    DirectionId = currentDirection
                });

                specialities.Add(new Specialty
                {
                    Name = "Психологія",
                    Code = "053",
                    DirectionId = currentDirection
                });

                specialities.Add(new Specialty
                {
                    Name = "Соціологія",
                    Code = "054",
                    DirectionId = currentDirection
                });
                #endregion

                #region Автоматизація та приладобудування
                currentDirection = context.Directions.FirstOrDefault(x => x.Name == "Автоматизація та приладобудування").Id;

                specialities.Add(new Specialty
                {
                    Name = "Автоматизація та комп’ютерно-інтегровані технології",
                    Code = "151",
                    DirectionId = currentDirection
                });

                specialities.Add(new Specialty
                {
                    Name = "Метрологія та інформаційно-вимірювальна техніка",
                    Code = "152",
                    DirectionId = currentDirection
                });

                specialities.Add(new Specialty
                {
                    Name = "Мікро- та наносистемна техніка",
                    Code = "153",
                    DirectionId = currentDirection
                });
                #endregion

                #region Електрична інженерія
                currentDirection = context.Directions.FirstOrDefault(x => x.Name == "Електрична інженерія").Id;

                specialities.Add(new Specialty
                {
                    Name = "Електроенергетика, електротехніка та електромеханіка",
                    Code = "141",
                    DirectionId = currentDirection,
                });

                specialities.Add(new Specialty
                {
                    Name = "Енергетичне машинобудування",
                    Code = "142",
                    DirectionId = currentDirection,
                });

                specialities.Add(new Specialty
                {
                    Name = "Атомна енергетика",
                    Code = "143",
                    DirectionId = currentDirection,
                });

                specialities.Add(new Specialty
                {
                    Name = "Теплоенергетика",
                    Code = "144",
                    DirectionId = currentDirection,
                });

                specialities.Add(new Specialty
                {
                    Name = "Гідроенергетика",
                    Code = "145",
                    DirectionId = currentDirection,
                });
                #endregion

                await context.Specialties.AddRangeAsync(specialities);
                await context.SaveChangesAsync();
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
                    Abbreviation = "НУВГП",
                    Site = "https://nuwm.edu.ua/",
                    Address = "вулиця Соборна, 11, Рівне, Рівненська область, 33000",
                    Phone = "380362633209",
                    Email = "mail@nuwm.edu.ua",
                    Description = "Єдиний в Україні вищий навчальний заклад водогосподарського профілю." +
                    " Заклад є навчально-науковим комплексом, що здійснює підготовку висококваліфікованих фахівців," +
                    " науково-педагогічних кадрів, забезпечує підвищення кваліфікації фахівців та" +
                    " проводить науково-дослідну роботу. ",
                    Lat = 50.61798003111006f,
                    Lon = 26.25865398659906f,
                    StartOfCampaign = new DateTime(2021, 8, 13),
                    EndOfCampaign = new DateTime(2021, 8, 31)
                });
                #endregion

                #region МЕГУ
                univerities.Add(new University 
                {
                    Name = "Міжнародний економіко-гуманітарний університет імені академіка Степана Дем’янчука",
                    Abbreviation = "МЕГУ",
                    Site = "https://www.megu.edu.ua/",
                    Address = "вулиця Степана Дем'янчука, 4, Рівне, Рівненська область, 33000",
                    Phone = "380362637234",
                    Email = "mail@megu.edu.ua",
                    Description = "Міжнародний економіко-гуманітарний університет імені академіка Степана Дем’янчука, " +
                    "навчальний заклад, що акредитований за IV рівнем, пропонує здобути освіту за широким вибором різних престижних спеціальностей, " +
                    "які серйозно знадобляться в житті кожному, хто навчатиметься в нашому університеті.",
                    Lat = 50.6097214170274f,
                    Lon = 26.288458069318498f,
                    StartOfCampaign = new DateTime(2021, 8, 13),
                    EndOfCampaign = new DateTime(2021, 8, 31)
                });
                #endregion

                #region ОА
                univerities.Add(new University 
                {
                    Name = "Національний університет \"Острозька академія\"",
                    Abbreviation = "ОА",
                    Site = "https://www.oa.edu.ua/",
                    Address = "вулиця Семінарська, 2, Острог, Рівненська область, 35800",
                    Phone = "380365422949",
                    Email = "press@oa.edu.ua",
                    Description = "Національний університет «Острозька академія» — наступник першого вищого навчального закладу східнослов’янських народів — " +
                    "Острозької слов’яно-греко-латинської академії. Заснував академію у 1576 році князь Василь-Костянтин Острозький. " +
                    "Велику суму коштів на розбудову академії надала його племінниця — княжна Гальшка Острозька. " +
                    "В основу діяльності Острозької академії було покладено традиційне для середньовічної Європи, " +
                    "однак цілком незвичне для українського шкільництва, вивчення семи вільних наук (граматики, риторики, діалектики, арифметики, геометрії, музики, астрономії), " +
                    "а також вищих наук: філософії, богослів’я, медицини. Спудеї Острозької академії опановували п’ять мов: слов’янську, польську, давньоєврейську, грецьку, латинську. " +
                    "Унікальність та оригінальність цього вищого закладу освіти виявилися й у тому, що тут уперше поєдналися два типи культур: візантійська і західноєвропейська. " +
                    "З Острозькою академією пов’язується ренесанс українського народу.",
                    Lat = 50.329296716686464f,
                    Lon = 26.512545745229293f,
                    StartOfCampaign = new DateTime(2021, 8, 13),
                    EndOfCampaign = new DateTime(2021, 8, 31)
                });
                #endregion

                #region РДГУ
                univerities.Add(new University 
                {
                    Name = "Рівненський державний гуманітарний університет",
                    Abbreviation = "РДГУ",
                    Site = "http://www.rshu.edu.ua/",
                    Address = "вулиця Пластова, 31, Рівне, Рівненська область, 33000",
                    Phone = "380362263715",
                    Email = "info@rshu.edu.ua",
                    Description = "Рівненський державний гуманітарний університет (РДГУ) – багатопрофільний заклад вищої освіти, " +
                    "який здійснює підготовку фахівців з педагогічних, природничих, культурно-мистецьких, економічних спеціальностей. " +
                    "Історія РДГУ розпочинається з відкриття Ровенського вчительського інституту в 1940 р., Ровенського інституту культури в 1979 р. " +
                    "та їх об’єднання в 1998 р. у Рівненський державний гуманітарний університет, що дає закладу змогу поєднати багаторічний досвід з " +
                    "інноваційним потенціалом сучасних технологій навчання студентів.",
                    Lat = 50.62372932193232f,
                    Lon = 26.260765167100683f,
                    StartOfCampaign = new DateTime(2021, 8, 13),
                    EndOfCampaign = new DateTime(2021, 8, 31)
                });
                #endregion

                #region КПІ
                univerities.Add(new University
                {
                    Name = "Київський політехнічний інститут імені Ігоря Сікорського",
                    Abbreviation="КПІ",
                    Site = "https://kpi.ua/",
                    Address = "проспект Перемоги, 37, Київ, 03056",
                    Phone = "380442049100",
                    Description = "Заклад вищої освіти інженерного профілю," +
                    " заснований в Києві у 1898 р., на сьогодні це один із найбільших університетів" +
                    " України за кількістю студентів з широким спектром спеціальностей і освітніх програм " +
                    "для підготовки фахівців з технічних і гуманітарних наук",
                });
                #endregion

                #region НАВС
                univerities.Add(new University
                {
                    Name = "Академія внутрішніх військ МВС України",
                    Abbreviation = "НАВС",
                    Site = "https://www.naiau.kiev.ua/",
                    Address = "Солом'янська площа, 1, Київ, 02000",
                    Phone = "380442469491",
                    Description = "Державний вищий навчальний заклад IV рівня акредитації," +
                    " підпорядкований Міністерству внутрішніх справ України та розташований у Києві",
                    Lat = 50.43278409191516f,
                    Lon = 30.471873937648123f,
                });
                #endregion

                context.Universities.AddRange(univerities);
                context.SaveChanges();
            }
        }

        public static void SeedPaymentForms(EFDbContext context)
        {
            if (context.PaymentForms.Count() == 0)
            {
                var paymentForms = new List<PaymentForm>();

                paymentForms.Add(new PaymentForm
                {
                    Name = "контракт"
                });
                paymentForms.Add(new PaymentForm
                {
                    Name = "бюджет"
                });
                context.PaymentForms.AddRange(paymentForms);
                context.SaveChanges();
            }
        }


        public static void SeedDirectionsAndSpecialitiesToUniversity(EFDbContext context)
        {
            if (context.DirectionsToUniversities.Count() == 0 || context.SpecialtyToUniversities.Count() == 0)
            {
                var directions = context.Directions.ToList();
                var specialities = context.Specialties.ToList();
                var universities = context.Universities.ToList();

                var directionsToUniversities = new List<DirectionToUniversity>();
                var specialitiesToUniversities = new List<SpecialtyToUniversity>();

                var currentUniversityId = string.Empty;

                #region Академія внутрішніх військ МВС України
                currentUniversityId = universities.FirstOrDefault(x => x.Name == "Академія внутрішніх військ МВС України").Id;
                directionsToUniversities.AddRange(new List<DirectionToUniversity>
                {
                    new DirectionToUniversity { DirectionId = directions.FirstOrDefault(x => x.Name == "Соціальні та поведінкові науки").Id,UniversityId = currentUniversityId },
                    new DirectionToUniversity { DirectionId = directions.FirstOrDefault(x => x.Name == "Математика та статистика").Id,UniversityId = currentUniversityId },
                });

                specialitiesToUniversities.AddRange(new List<SpecialtyToUniversity>
                {
                    #region Соціальні та поведінкові науки
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Соціологія").Id,UniversityId = currentUniversityId },
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Політологія").Id,UniversityId = currentUniversityId },
                    #endregion
                    #region Математика та статистика
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Статистика").Id,UniversityId = currentUniversityId }
                    #endregion
                });
                #endregion

                #region Національний університет "Острозька академія"
                currentUniversityId = universities.FirstOrDefault(x => x.Name == $"Національний університет \"Острозька академія\"").Id;
                directionsToUniversities.AddRange(new List<DirectionToUniversity>
                {
                    new DirectionToUniversity { DirectionId = directions.FirstOrDefault(x => x.Name == "Інформаційні технології").Id,UniversityId = currentUniversityId },
                    new DirectionToUniversity { DirectionId = directions.FirstOrDefault(x => x.Name == "Математика та статистика").Id,UniversityId = currentUniversityId },
                    new DirectionToUniversity { DirectionId = directions.FirstOrDefault(x => x.Name == "Електрична інженерія").Id,UniversityId = currentUniversityId },
                });

                specialitiesToUniversities.AddRange(new List<SpecialtyToUniversity>
                {
                    #region Інформаційні технології
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Системний аналіз").Id,UniversityId = currentUniversityId },
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Інженерія програмного забезпечення").Id,UniversityId = currentUniversityId },
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Кібербезпека").Id,UniversityId = currentUniversityId },
                    #endregion
                    #region Математика та статистика
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Математика").Id,UniversityId = currentUniversityId },
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Прикладна математика").Id,UniversityId = currentUniversityId },
                    #endregion
                    #region Електрична інженерія
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Гідроенергетика").Id,UniversityId = currentUniversityId }
                    #endregion
                });
                #endregion

                #region Національний університет водного господарства та природокористування
                currentUniversityId = universities.FirstOrDefault(x => x.Name == $"Національний університет водного господарства та природокористування").Id;
                directionsToUniversities.AddRange(new List<DirectionToUniversity>
                {
                    new DirectionToUniversity { DirectionId = directions.FirstOrDefault(x => x.Name == "Інформаційні технології").Id,UniversityId = currentUniversityId },
                    new DirectionToUniversity { DirectionId = directions.FirstOrDefault(x => x.Name == "Автоматизація та приладобудування").Id,UniversityId = currentUniversityId },
                    new DirectionToUniversity { DirectionId = directions.FirstOrDefault(x => x.Name == "Електрична інженерія").Id,UniversityId = currentUniversityId },
                });

                specialitiesToUniversities.AddRange(new List<SpecialtyToUniversity>
                {
                    #region Інформаційні технології
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Системний аналіз").Id,UniversityId = currentUniversityId },
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Інженерія програмного забезпечення").Id,UniversityId = currentUniversityId },
                    #endregion
                    #region Автоматизація та приладобудування
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Метрологія та інформаційно-вимірювальна техніка").Id,UniversityId = currentUniversityId },
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Автоматизація та комп’ютерно-інтегровані технології").Id,UniversityId = currentUniversityId },
                    #endregion
                    #region Електрична інженерія
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Гідроенергетика").Id,UniversityId = currentUniversityId },
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Енергетичне машинобудування").Id,UniversityId = currentUniversityId }
                    #endregion
                });
                #endregion

                #region Міжнародний економіко-гуманітарний університет імені академіка Степана Дем’янчука
                currentUniversityId = universities.FirstOrDefault(x => x.Name == $"Міжнародний економіко-гуманітарний університет імені академіка Степана Дем’янчука").Id;
                directionsToUniversities.AddRange(new List<DirectionToUniversity>
                {
                    new DirectionToUniversity { DirectionId = directions.FirstOrDefault(x => x.Name == "Математика та статистика").Id,UniversityId = currentUniversityId },
                });

                specialitiesToUniversities.AddRange(new List<SpecialtyToUniversity>
                {
                    #region Математика та статистика
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Математика").Id,UniversityId = currentUniversityId },
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Статистика").Id,UniversityId = currentUniversityId },
                    #endregion                 
                });
                #endregion

                #region Київський політехнічний інститут імені Ігоря Сікорського
                currentUniversityId = universities.FirstOrDefault(x => x.Name == $"Київський політехнічний інститут імені Ігоря Сікорського").Id;
                directionsToUniversities.AddRange(new List<DirectionToUniversity>
                {
                    new DirectionToUniversity { DirectionId = directions.FirstOrDefault(x => x.Name == "Електрична інженерія").Id,UniversityId = currentUniversityId },
                    new DirectionToUniversity { DirectionId = directions.FirstOrDefault(x => x.Name == "Математика та статистика").Id,UniversityId = currentUniversityId },
                    new DirectionToUniversity { DirectionId = directions.FirstOrDefault(x => x.Name == "Інформаційні технології").Id,UniversityId = currentUniversityId },
                    new DirectionToUniversity { DirectionId = directions.FirstOrDefault(x => x.Name == "Автоматизація та приладобудування").Id,UniversityId = currentUniversityId },
                });

                specialitiesToUniversities.AddRange(new List<SpecialtyToUniversity>
                {
                    #region Електрична інженерія
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Атомна енергетика").Id,UniversityId = currentUniversityId },
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Теплоенергетика").Id,UniversityId = currentUniversityId },
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Електроенергетика, електротехніка та електромеханіка").Id,UniversityId = currentUniversityId },
                    #endregion  
                    #region Математика та статистика
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Математика").Id,UniversityId = currentUniversityId },
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Прикладна математика").Id,UniversityId = currentUniversityId },
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Статистика").Id,UniversityId = currentUniversityId },
                    #endregion 
                    #region Інформаційні технології
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Інженерія програмного забезпечення").Id,UniversityId = currentUniversityId },
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Інформаційні системи та технології").Id,UniversityId = currentUniversityId },
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Кібербезпека").Id,UniversityId = currentUniversityId },
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Комп’ютерна інженерія").Id,UniversityId = currentUniversityId },
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Комп'ютерні науки").Id,UniversityId = currentUniversityId },
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Системний аналіз").Id,UniversityId = currentUniversityId },
                    #endregion 
                    #region Автоматизація та приладобудування
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Метрологія та інформаційно-вимірювальна техніка").Id,UniversityId = currentUniversityId },
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Мікро- та наносистемна техніка").Id,UniversityId = currentUniversityId },
                    #endregion 

                });
                #endregion

                #region Рівненський державний гуманітарний університет
                currentUniversityId = universities.FirstOrDefault(x => x.Name == $"Київський політехнічний інститут імені Ігоря Сікорського").Id;
                directionsToUniversities.AddRange(new List<DirectionToUniversity>
                {
                    new DirectionToUniversity { DirectionId = directions.FirstOrDefault(x => x.Name == "Електрична інженерія").Id,UniversityId = currentUniversityId },
                    new DirectionToUniversity { DirectionId = directions.FirstOrDefault(x => x.Name == "Математика та статистика").Id,UniversityId = currentUniversityId },
                });

                specialitiesToUniversities.AddRange(new List<SpecialtyToUniversity>
                {
                    #region Електрична інженерія
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Атомна енергетика").Id,UniversityId = currentUniversityId },
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Теплоенергетика").Id,UniversityId = currentUniversityId },
                    #endregion  
                    #region Математика та статистика
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Математика").Id,UniversityId = currentUniversityId },
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Прикладна математика").Id,UniversityId = currentUniversityId },
                    new SpecialtyToUniversity { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Статистика").Id,UniversityId = currentUniversityId },
                    #endregion                   
                });
                #endregion


                context.DirectionsToUniversities.AddRange(directionsToUniversities);
                context.SpecialtyToUniversities.AddRange(specialitiesToUniversities);
                context.SaveChanges();

            }
        }



        public static void SeedUniversityAdmins(EFDbContext context)
        {
            if(context.UniversityAdmins.Count() == 0)
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
        }

        public static void SeedEducationForms(EFDbContext context)
        {
            if (context.EducationForms.Count() == 0)
            {
                var educationForms = new List<EducationForm>();

                educationForms.Add(new EducationForm
                {
                    Name = "денна"
                });
                educationForms.Add(new EducationForm
                {
                    Name = "заочна"
                });
                educationForms.Add(new EducationForm
                {
                    Name = "вечірня"
                });
                context.EducationForms.AddRange(educationForms);
                context.SaveChanges();
            }
        }

        public static void SeedExams(EFDbContext context)
        {
            if (context.Exams.Count() == 0)
            {
                var exams = new List<Exam>();

                exams.Add(new Exam
                {
                    Name = "Українська мова та література"
                });
                exams.Add(new Exam
                {
                    Name = "Математика"
                });
                exams.Add(new Exam
                {
                    Name = "Історія України"
                });
                exams.Add(new Exam
                {
                    Name = "Біологія"
                });
                exams.Add(new Exam
                {
                    Name = "Хімія"
                });
                exams.Add(new Exam
                {
                    Name = "Фізика"
                });
                exams.Add(new Exam
                {
                    Name = "Географія"
                });
                exams.Add(new Exam
                {
                    Name = "Англійська мова"
                }); 
                exams.Add(new Exam
                {
                    Name = "Французька мова"
                });
                exams.Add(new Exam
                {
                    Name = "Німецька мова"
                });
                context.Exams.AddRange(exams);
                context.SaveChanges();
            }
        }

        //public static void SeedSpecialtyInUniversityDescription(EFDbContext context)
        //{
        //    if (context.SpecialtyInUniversityDescriptions.Count() == 0)
        //    {
        //        var specialityDescriptions = new List<SpecialtyInUniversityDescription>();

        //        specialityDescriptions.Add(new SpecialtyInUniversityDescription
        //        {
        //            EducationalProgramLink = ""

        //        });
        //        specialityDescriptions.Add(new SpecialtyInUniversityDescription
        //        {
        //            Name = "Бюджет"

        //        });
        //        context.EducationForms.AddRange(specialityDescriptions);
        //        context.SaveChanges();
        //    }
        //}

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
                await userManager.AddToRolesAsync(dbUser, new List<string>() { roleName, ProjectRoles.BaseUser });

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
                await SeederDB.SeedDirections(context);
                await SeederDB.SeedSpecialities(context);
                SeederDB.SeedUniversities(context);
                SeederDB.SeedDirectionsAndSpecialitiesToUniversity(context);
                SeederDB.SeedUniversityAdmins(context);
                await SeederDB.SeedUniversityModerators(context, manager);
                await SeederDB.SeedLectures(context, manager);
                #endregion

                Console.WriteLine("Database seeded.");
            }
        }
    }
}