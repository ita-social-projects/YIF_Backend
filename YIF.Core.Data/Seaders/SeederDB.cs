using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

                var institutionOfEducationAdminRole = new IdentityRole(ProjectRoles.InstitutionOfEducationAdmin);
                await roleManager.CreateAsync(institutionOfEducationAdminRole);

                var schoolModeratorRole = new IdentityRole(ProjectRoles.SchoolModerator);
                await roleManager.CreateAsync(schoolModeratorRole);

                var institutionOfEducationModeratorRole = new IdentityRole(ProjectRoles.InstitutionOfEducationModerator);
                await roleManager.CreateAsync(institutionOfEducationModeratorRole);

                var lectorRole = new IdentityRole(ProjectRoles.Lector);
                await roleManager.CreateAsync(lectorRole);

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

        #region InstitutionOfEducation

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
                    DirectionId = currentDirection,
                    Description = "Це базовий опис спеціальності. Ця спеціальність підійде для тих хто хоче реалізувати себе у майбутньому у даній галузі." +
                       " Для здобувачів вищої освіти вона буде цікавою тому що вони зможуть розкрити себе у даному напрямку за рахунок актуальної інформації, яку будуть доносити ним професіонали своєї справи, які є майстрами у своїй галузі."
                });

                specialities.Add(new Specialty
                {
                    Name = "Комп'ютерні науки",
                    Code = "122",
                    DirectionId = currentDirection,
                    Description = "Це базовий опис спеціальності. Ця спеціальність підійде для тих хто хоче реалізувати себе у майбутньому у даній галузі." +
                       " Для здобувачів вищої освіти вона буде цікавою тому що вони зможуть розкрити себе у даному напрямку за рахунок актуальної інформації, яку будуть доносити ним професіонали своєї справи, які є майстрами у своїй галузі."
                });

                specialities.Add(new Specialty
                {
                    Name = "Комп’ютерна інженерія",
                    Code = "123",
                    DirectionId = currentDirection,
                    Description = "Це базовий опис спеціальності. Ця спеціальність підійде для тих хто хоче реалізувати себе у майбутньому у даній галузі." +
                       " Для здобувачів вищої освіти вона буде цікавою тому що вони зможуть розкрити себе у даному напрямку за рахунок актуальної інформації, яку будуть доносити ним професіонали своєї справи, які є майстрами у своїй галузі."
                });

                specialities.Add(new Specialty
                {
                    Name = "Системний аналіз",
                    Code = "124",
                    DirectionId = currentDirection,
                    Description = "Це базовий опис спеціальності. Ця спеціальність підійде для тих хто хоче реалізувати себе у майбутньому у даній галузі." +
                       " Для здобувачів вищої освіти вона буде цікавою тому що вони зможуть розкрити себе у даному напрямку за рахунок актуальної інформації, яку будуть доносити ним професіонали своєї справи, які є майстрами у своїй галузі."
                });

                specialities.Add(new Specialty
                {
                    Name = "Кібербезпека",
                    Code = "125",
                    DirectionId = currentDirection,
                    Description = "Це базовий опис спеціальності. Ця спеціальність підійде для тих хто хоче реалізувати себе у майбутньому у даній галузі." +
                       " Для здобувачів вищої освіти вона буде цікавою тому що вони зможуть розкрити себе у даному напрямку за рахунок актуальної інформації, яку будуть доносити ним професіонали своєї справи, які є майстрами у своїй галузі."
                });

                specialities.Add(new Specialty
                {
                    Name = "Інформаційні системи та технології",
                    Code = "126",
                    DirectionId = currentDirection,
                    Description = "Це базовий опис спеціальності. Ця спеціальність підійде для тих хто хоче реалізувати себе у майбутньому у даній галузі." +
                       " Для здобувачів вищої освіти вона буде цікавою тому що вони зможуть розкрити себе у даному напрямку за рахунок актуальної інформації, яку будуть доносити ним професіонали своєї справи, які є майстрами у своїй галузі."
                });
                #endregion

                #region Математика та статистика
                currentDirection = context.Directions.FirstOrDefault(x => x.Name == "Математика та статистика").Id;

                specialities.Add(new Specialty
                {
                    Name = "Математика",
                    Code = "111",
                    DirectionId = currentDirection,
                    Description = "Це базовий опис спеціальності. Ця спеціальність підійде для тих хто хоче реалізувати себе у майбутньому у даній галузі." +
                       " Для здобувачів вищої освіти вона буде цікавою тому що вони зможуть розкрити себе у даному напрямку за рахунок актуальної інформації, яку будуть доносити ним професіонали своєї справи, які є майстрами у своїй галузі."
                });

                specialities.Add(new Specialty
                {
                    Name = "Статистика",
                    Code = "112",
                    DirectionId = currentDirection,
                    Description = "Це базовий опис спеціальності. Ця спеціальність підійде для тих хто хоче реалізувати себе у майбутньому у даній галузі." +
                       " Для здобувачів вищої освіти вона буде цікавою тому що вони зможуть розкрити себе у даному напрямку за рахунок актуальної інформації, яку будуть доносити ним професіонали своєї справи, які є майстрами у своїй галузі."
                });

                specialities.Add(new Specialty
                {
                    Name = "Прикладна математика",
                    Code = "113",
                    DirectionId = currentDirection,
                    Description = "Це базовий опис спеціальності. Ця спеціальність підійде для тих хто хоче реалізувати себе у майбутньому у даній галузі." +
                       " Для здобувачів вищої освіти вона буде цікавою тому що вони зможуть розкрити себе у даному напрямку за рахунок актуальної інформації, яку будуть доносити ним професіонали своєї справи, які є майстрами у своїй галузі."
                });
                #endregion

                #region Соціальні та поведінкові науки
                currentDirection = context.Directions.FirstOrDefault(x => x.Name == "Соціальні та поведінкові науки").Id;

                specialities.Add(new Specialty
                {
                    Name = "Економіка",
                    Code = "051",
                    DirectionId = currentDirection,
                    Description = "Це базовий опис спеціальності. Ця спеціальність підійде для тих хто хоче реалізувати себе у майбутньому у даній галузі." +
                       " Для здобувачів вищої освіти вона буде цікавою тому що вони зможуть розкрити себе у даному напрямку за рахунок актуальної інформації, яку будуть доносити ним професіонали своєї справи, які є майстрами у своїй галузі."
                });

                specialities.Add(new Specialty
                {
                    Name = "Політологія",
                    Code = "052",
                    DirectionId = currentDirection,
                    Description = "Це базовий опис спеціальності. Ця спеціальність підійде для тих хто хоче реалізувати себе у майбутньому у даній галузі." +
                       " Для здобувачів вищої освіти вона буде цікавою тому що вони зможуть розкрити себе у даному напрямку за рахунок актуальної інформації, яку будуть доносити ним професіонали своєї справи, які є майстрами у своїй галузі."
                });

                specialities.Add(new Specialty
                {
                    Name = "Психологія",
                    Code = "053",
                    DirectionId = currentDirection,
                    Description = "Це базовий опис спеціальності. Ця спеціальність підійде для тих хто хоче реалізувати себе у майбутньому у даній галузі." +
                       " Для здобувачів вищої освіти вона буде цікавою тому що вони зможуть розкрити себе у даному напрямку за рахунок актуальної інформації, яку будуть доносити ним професіонали своєї справи, які є майстрами у своїй галузі."
                });

                specialities.Add(new Specialty
                {
                    Name = "Соціологія",
                    Code = "054",
                    DirectionId = currentDirection,
                    Description = "Це базовий опис спеціальності. Ця спеціальність підійде для тих хто хоче реалізувати себе у майбутньому у даній галузі." +
                       " Для здобувачів вищої освіти вона буде цікавою тому що вони зможуть розкрити себе у даному напрямку за рахунок актуальної інформації, яку будуть доносити ним професіонали своєї справи, які є майстрами у своїй галузі."
                });
                #endregion

                #region Автоматизація та приладобудування
                currentDirection = context.Directions.FirstOrDefault(x => x.Name == "Автоматизація та приладобудування").Id;

                specialities.Add(new Specialty
                {
                    Name = "Автоматизація та комп’ютерно-інтегровані технології",
                    Code = "151",
                    DirectionId = currentDirection,
                    Description = "Це базовий опис спеціальності. Ця спеціальність підійде для тих хто хоче реалізувати себе у майбутньому у даній галузі." +
                       " Для здобувачів вищої освіти вона буде цікавою тому що вони зможуть розкрити себе у даному напрямку за рахунок актуальної інформації, яку будуть доносити ним професіонали своєї справи, які є майстрами у своїй галузі."
                });

                specialities.Add(new Specialty
                {
                    Name = "Метрологія та інформаційно-вимірювальна техніка",
                    Code = "152",
                    DirectionId = currentDirection,
                    Description = "Це базовий опис спеціальності. Ця спеціальність підійде для тих хто хоче реалізувати себе у майбутньому у даній галузі." +
                       " Для здобувачів вищої освіти вона буде цікавою тому що вони зможуть розкрити себе у даному напрямку за рахунок актуальної інформації, яку будуть доносити ним професіонали своєї справи, які є майстрами у своїй галузі."
                });

                specialities.Add(new Specialty
                {
                    Name = "Мікро- та наносистемна техніка",
                    Code = "153",
                    DirectionId = currentDirection,
                    Description = "Це базовий опис спеціальності. Ця спеціальність підійде для тих хто хоче реалізувати себе у майбутньому у даній галузі." +
                       " Для здобувачів вищої освіти вона буде цікавою тому що вони зможуть розкрити себе у даному напрямку за рахунок актуальної інформації, яку будуть доносити ним професіонали своєї справи, які є майстрами у своїй галузі."
                });
                #endregion

                #region Електрична інженерія
                currentDirection = context.Directions.FirstOrDefault(x => x.Name == "Електрична інженерія").Id;

                specialities.Add(new Specialty
                {
                    Name = "Електроенергетика, електротехніка та електромеханіка",
                    Code = "141",
                    DirectionId = currentDirection,
                    Description = "Це базовий опис спеціальності. Ця спеціальність підійде для тих хто хоче реалізувати себе у майбутньому у даній галузі." +
                       " Для здобувачів вищої освіти вона буде цікавою тому що вони зможуть розкрити себе у даному напрямку за рахунок актуальної інформації, яку будуть доносити ним професіонали своєї справи, які є майстрами у своїй галузі."
                });

                specialities.Add(new Specialty
                {
                    Name = "Енергетичне машинобудування",
                    Code = "142",
                    DirectionId = currentDirection,
                    Description = "Це базовий опис спеціальності. Ця спеціальність підійде для тих хто хоче реалізувати себе у майбутньому у даній галузі." +
                       " Для здобувачів вищої освіти вона буде цікавою тому що вони зможуть розкрити себе у даному напрямку за рахунок актуальної інформації, яку будуть доносити ним професіонали своєї справи, які є майстрами у своїй галузі."
                });

                specialities.Add(new Specialty
                {
                    Name = "Атомна енергетика",
                    Code = "143",
                    DirectionId = currentDirection,
                    Description = "Це базовий опис спеціальності. Ця спеціальність підійде для тих хто хоче реалізувати себе у майбутньому у даній галузі." +
                       " Для здобувачів вищої освіти вона буде цікавою тому що вони зможуть розкрити себе у даному напрямку за рахунок актуальної інформації, яку будуть доносити ним професіонали своєї справи, які є майстрами у своїй галузі."
                });

                specialities.Add(new Specialty
                {
                    Name = "Теплоенергетика",
                    Code = "144",
                    DirectionId = currentDirection,
                    Description = "Це базовий опис спеціальності. Ця спеціальність підійде для тих хто хоче реалізувати себе у майбутньому у даній галузі." +
                       " Для здобувачів вищої освіти вона буде цікавою тому що вони зможуть розкрити себе у даному напрямку за рахунок актуальної інформації, яку будуть доносити ним професіонали своєї справи, які є майстрами у своїй галузі."
                });

                specialities.Add(new Specialty
                {
                    Name = "Гідроенергетика",
                    Code = "145",
                    DirectionId = currentDirection,
                    Description = "Це базовий опис спеціальності. Ця спеціальність підійде для тих хто хоче реалізувати себе у майбутньому у даній галузі." +
                       " Для здобувачів вищої освіти вона буде цікавою тому що вони зможуть розкрити себе у даному напрямку за рахунок актуальної інформації, яку будуть доносити ним професіонали своєї справи, які є майстрами у своїй галузі."
                });
                #endregion

                await context.Specialties.AddRangeAsync(specialities);
                await context.SaveChangesAsync();
            }
        }

        public static void SeedInstitutionOfEducations(EFDbContext context)
        {
            if (context.InstitutionOfEducations.Count() == 0)
            {
                var univerities = new List<InstitutionOfEducation>();

                #region НУВГП
                univerities.Add(new InstitutionOfEducation
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
                    InstitutionOfEducationType = InstitutionOfEducationType.University,
                    StartOfCampaign = new DateTime(2021, 8, 13),
                    EndOfCampaign = new DateTime(2021, 8, 31)
                });
                #endregion

                #region МЕГУ
                univerities.Add(new InstitutionOfEducation
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
                    InstitutionOfEducationType = InstitutionOfEducationType.University,
                    StartOfCampaign = new DateTime(2021, 8, 13),
                    EndOfCampaign = new DateTime(2021, 8, 31)
                });
                #endregion

                #region ОА
                univerities.Add(new InstitutionOfEducation
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
                    InstitutionOfEducationType = InstitutionOfEducationType.University,
                    StartOfCampaign = new DateTime(2021, 8, 13),
                    EndOfCampaign = new DateTime(2021, 8, 31)
                });
                #endregion

                #region РДГУ
                univerities.Add(new InstitutionOfEducation
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
                    InstitutionOfEducationType = InstitutionOfEducationType.University,
                    StartOfCampaign = new DateTime(2021, 8, 13),
                    EndOfCampaign = new DateTime(2021, 8, 31)
                });
                #endregion

                #region КПІ
                univerities.Add(new InstitutionOfEducation
                {
                    Name = "Київський політехнічний інститут імені Ігоря Сікорського",
                    Abbreviation = "КПІ",
                    Site = "https://kpi.ua/",
                    Address = "проспект Перемоги, 37, Київ, 03056",
                    Phone = "380442049100",
                    Description = "Заклад вищої освіти інженерного профілю," +
                    " заснований в Києві у 1898 р., на сьогодні це один із найбільших університетів" +
                    " України за кількістю студентів з широким спектром спеціальностей і освітніх програм " +
                    "для підготовки фахівців з технічних і гуманітарних наук",
                    Lat = 50.4491699f,
                    Lon = 30.4561487f,
                    InstitutionOfEducationType = InstitutionOfEducationType.University,
                    StartOfCampaign = new DateTime(2021, 8, 13),
                    EndOfCampaign = new DateTime(2021, 8, 31)
                });
                #endregion

                #region НАВС
                univerities.Add(new InstitutionOfEducation
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
                    InstitutionOfEducationType = InstitutionOfEducationType.University,
                    StartOfCampaign = new DateTime(2021, 8, 13),
                    EndOfCampaign = new DateTime(2021, 8, 31)
                });
                #endregion

                context.InstitutionOfEducations.AddRange(univerities);
                context.SaveChanges();
            }
        }


        public static void SeedDirectionsAndSpecialitiesToInstitutionOfEducation(EFDbContext context)
        {
            if (context.DirectionsToInstitutionOfEducations.Count() == 0 || context.SpecialtyToInstitutionOfEducations.Count() == 0)
            {
                var directions = context.Directions.ToList();
                var specialities = context.Specialties.ToList();
                var institutionOfEducations = context.InstitutionOfEducations.ToList();

                var directionsToInstitutionOfEducations = new List<DirectionToInstitutionOfEducation>();
                var specialitiesToInstitutionOfEducations = new List<SpecialtyToInstitutionOfEducation>();

                var currentInstitutionOfEducationId = string.Empty;

                #region Академія внутрішніх військ МВС України
                currentInstitutionOfEducationId = institutionOfEducations.FirstOrDefault(x => x.Name == "Академія внутрішніх військ МВС України").Id;
                directionsToInstitutionOfEducations.AddRange(new List<DirectionToInstitutionOfEducation>
                {
                    new DirectionToInstitutionOfEducation { DirectionId = directions.FirstOrDefault(x => x.Name == "Соціальні та поведінкові науки").Id,InstitutionOfEducationId = currentInstitutionOfEducationId },
                    new DirectionToInstitutionOfEducation { DirectionId = directions.FirstOrDefault(x => x.Name == "Математика та статистика").Id,InstitutionOfEducationId = currentInstitutionOfEducationId },
                });

                specialitiesToInstitutionOfEducations.AddRange(new List<SpecialtyToInstitutionOfEducation>
                {
                    #region Соціальні та поведінкові науки
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Соціологія").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Політологія").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    #endregion
                    #region Математика та статистика
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Статистика").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false }
                    #endregion
                });
                #endregion

                #region Національний університет "Острозька академія"
                currentInstitutionOfEducationId = institutionOfEducations.FirstOrDefault(x => x.Name == $"Національний університет \"Острозька академія\"").Id;
                directionsToInstitutionOfEducations.AddRange(new List<DirectionToInstitutionOfEducation>
                {
                    new DirectionToInstitutionOfEducation { DirectionId = directions.FirstOrDefault(x => x.Name == "Інформаційні технології").Id,InstitutionOfEducationId = currentInstitutionOfEducationId },
                    new DirectionToInstitutionOfEducation { DirectionId = directions.FirstOrDefault(x => x.Name == "Математика та статистика").Id,InstitutionOfEducationId = currentInstitutionOfEducationId },
                    new DirectionToInstitutionOfEducation { DirectionId = directions.FirstOrDefault(x => x.Name == "Електрична інженерія").Id,InstitutionOfEducationId = currentInstitutionOfEducationId },
                });

                specialitiesToInstitutionOfEducations.AddRange(new List<SpecialtyToInstitutionOfEducation>
                {
                    #region Інформаційні технології
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Системний аналіз").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Інженерія програмного забезпечення").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Кібербезпека").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    #endregion
                    #region Математика та статистика
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Математика").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Прикладна математика").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    #endregion
                    #region Електрична інженерія
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Гідроенергетика").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false }
                    #endregion
                });
                #endregion

                #region Національний університет водного господарства та природокористування
                currentInstitutionOfEducationId = institutionOfEducations.FirstOrDefault(x => x.Name == $"Національний університет водного господарства та природокористування").Id;
                directionsToInstitutionOfEducations.AddRange(new List<DirectionToInstitutionOfEducation>
                {
                    new DirectionToInstitutionOfEducation { DirectionId = directions.FirstOrDefault(x => x.Name == "Інформаційні технології").Id,InstitutionOfEducationId = currentInstitutionOfEducationId },
                    new DirectionToInstitutionOfEducation { DirectionId = directions.FirstOrDefault(x => x.Name == "Автоматизація та приладобудування").Id,InstitutionOfEducationId = currentInstitutionOfEducationId },
                    new DirectionToInstitutionOfEducation { DirectionId = directions.FirstOrDefault(x => x.Name == "Електрична інженерія").Id,InstitutionOfEducationId = currentInstitutionOfEducationId },
                });

                specialitiesToInstitutionOfEducations.AddRange(new List<SpecialtyToInstitutionOfEducation>
                {
                    #region Інформаційні технології
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Системний аналіз").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Інженерія програмного забезпечення").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    #endregion
                    #region Автоматизація та приладобудування
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Метрологія та інформаційно-вимірювальна техніка").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Автоматизація та комп’ютерно-інтегровані технології").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    #endregion
                    #region Електрична інженерія
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Гідроенергетика").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Енергетичне машинобудування").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false }
                    #endregion
                });
                #endregion

                #region Міжнародний економіко-гуманітарний університет імені академіка Степана Дем’янчука
                currentInstitutionOfEducationId = institutionOfEducations.FirstOrDefault(x => x.Name == $"Міжнародний економіко-гуманітарний університет імені академіка Степана Дем’янчука").Id;
                directionsToInstitutionOfEducations.AddRange(new List<DirectionToInstitutionOfEducation>
                {
                    new DirectionToInstitutionOfEducation { DirectionId = directions.FirstOrDefault(x => x.Name == "Математика та статистика").Id,InstitutionOfEducationId = currentInstitutionOfEducationId },
                });

                specialitiesToInstitutionOfEducations.AddRange(new List<SpecialtyToInstitutionOfEducation>
                {
                    #region Математика та статистика
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Математика").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Статистика").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    #endregion                 
                });
                #endregion

                #region Київський політехнічний інститут імені Ігоря Сікорського
                currentInstitutionOfEducationId = institutionOfEducations.FirstOrDefault(x => x.Name == $"Київський політехнічний інститут імені Ігоря Сікорського").Id;
                directionsToInstitutionOfEducations.AddRange(new List<DirectionToInstitutionOfEducation>
                {
                    new DirectionToInstitutionOfEducation { DirectionId = directions.FirstOrDefault(x => x.Name == "Електрична інженерія").Id,InstitutionOfEducationId = currentInstitutionOfEducationId },
                    new DirectionToInstitutionOfEducation { DirectionId = directions.FirstOrDefault(x => x.Name == "Математика та статистика").Id,InstitutionOfEducationId = currentInstitutionOfEducationId },
                    new DirectionToInstitutionOfEducation { DirectionId = directions.FirstOrDefault(x => x.Name == "Інформаційні технології").Id,InstitutionOfEducationId = currentInstitutionOfEducationId },
                    new DirectionToInstitutionOfEducation { DirectionId = directions.FirstOrDefault(x => x.Name == "Автоматизація та приладобудування").Id,InstitutionOfEducationId = currentInstitutionOfEducationId },
                });

                specialitiesToInstitutionOfEducations.AddRange(new List<SpecialtyToInstitutionOfEducation>
                {
                    #region Електрична інженерія
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Атомна енергетика").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Теплоенергетика").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Електроенергетика, електротехніка та електромеханіка").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    #endregion  
                    #region Математика та статистика
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Математика").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Прикладна математика").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Статистика").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    #endregion 
                    #region Інформаційні технології
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Інженерія програмного забезпечення").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Інформаційні системи та технології").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Кібербезпека").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Комп’ютерна інженерія").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Комп'ютерні науки").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Системний аналіз").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    #endregion 
                    #region Автоматизація та приладобудування
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Метрологія та інформаційно-вимірювальна техніка").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Мікро- та наносистемна техніка").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    #endregion 

                });
                #endregion

                #region Рівненський державний гуманітарний університет
                currentInstitutionOfEducationId = institutionOfEducations.FirstOrDefault(x => x.Name == $"Рівненський державний гуманітарний університет").Id;
                directionsToInstitutionOfEducations.AddRange(new List<DirectionToInstitutionOfEducation>
                {
                    new DirectionToInstitutionOfEducation { DirectionId = directions.FirstOrDefault(x => x.Name == "Соціальні та поведінкові науки").Id,InstitutionOfEducationId = currentInstitutionOfEducationId },
                    new DirectionToInstitutionOfEducation { DirectionId = directions.FirstOrDefault(x => x.Name == "Електрична інженерія").Id,InstitutionOfEducationId = currentInstitutionOfEducationId },
                    new DirectionToInstitutionOfEducation { DirectionId = directions.FirstOrDefault(x => x.Name == "Математика та статистика").Id,InstitutionOfEducationId = currentInstitutionOfEducationId },
                });

                specialitiesToInstitutionOfEducations.AddRange(new List<SpecialtyToInstitutionOfEducation>
                {
                    #region Соціальні та поведінкові науки
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Економіка").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Психологія").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    #endregion
                    #region Електрична інженерія
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Атомна енергетика").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Теплоенергетика").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    #endregion  
                    #region Математика та статистика
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Математика").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Прикладна математика").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    new SpecialtyToInstitutionOfEducation { SpecialtyId = specialities.FirstOrDefault(x => x.Name == "Статистика").Id,InstitutionOfEducationId = currentInstitutionOfEducationId, IsDeleted = false },
                    #endregion                   
                });
                #endregion


                context.DirectionsToInstitutionOfEducations.AddRange(directionsToInstitutionOfEducations);
                context.SpecialtyToInstitutionOfEducations.AddRange(specialitiesToInstitutionOfEducations);
                context.SaveChanges();

            }
        }

        public static void SeedSpecialtyToInstitutionOfEducationToGraduate(EFDbContext context)
        {
            if (context.SpecialtyToInstitutionOfEducationToGraduates.Count() == 0)
            {
                var graduate = context.Graduates.ToList();
                var specialtyToUniversity = context.SpecialtyToInstitutionOfEducations.ToList();

                context.SpecialtyToInstitutionOfEducationToGraduates.AddRange(new List<SpecialtyToInstitutionOfEducationToGraduate>
                {
                    new SpecialtyToInstitutionOfEducationToGraduate
                    {
                        GraduateId = graduate.FirstOrDefault().Id,
                        SpecialtyId = specialtyToUniversity.FirstOrDefault().SpecialtyId,
                        InstitutionOfEducationId = specialtyToUniversity.FirstOrDefault().InstitutionOfEducationId
                    }
                });
                context.SaveChanges();
            }
        }

        public static void SeedSpecialtyToGraduate(EFDbContext context)
        {
            if (context.SpecialtyToGraduates.Count() == 0)
            {
                var graduate = context.Graduates.ToList();
                var specialty = context.Specialties.ToList();

                context.SpecialtyToGraduates.AddRange(new List<SpecialtyToGraduate>
                {
                    new SpecialtyToGraduate
                    {
                        GraduateId = graduate.FirstOrDefault().Id,
                        SpecialtyId = specialty.FirstOrDefault().Id
                    }
                });
                context.SaveChanges();
            }
        }

        public async static Task SeedInstitutionOfEducationAdmins(EFDbContext context, UserManager<DbUser> userManager)
        {
            if (context.InstitutionOfEducationAdmins.Count() == 0)
            {
                var admins = new List<InstitutionOfEducationAdmin>();
                string currentInstitutionOfEducation = string.Empty;
                var institutionOfEducations = context.InstitutionOfEducations.ToList();

                #region НУВГП
                {
                    currentInstitutionOfEducation = institutionOfEducations.FirstOrDefault(x => x.Name == "Національний університет водного господарства та природокористування").Id;
                    var dbUser = new DbUser
                    {
                        Email = "nuweeAdmin@gmail.com",
                        UserName = "NuweeAdmin",
                        PhoneNumber = "+380-31-415-9265"
                    };
                    var institutionOfEducationAdmin = new InstitutionOfEducationAdmin
                    {
                        InstitutionOfEducationId = currentInstitutionOfEducation,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.InstitutionOfEducationAdmin, institutionOfEducationAdmin);
                }
                #endregion

                #region КПІ
                currentInstitutionOfEducation = institutionOfEducations.FirstOrDefault(x => x.Name == "Київський політехнічний інститут імені Ігоря Сікорського").Id;
                {
                    currentInstitutionOfEducation = institutionOfEducations.FirstOrDefault(x => x.Name == "Київський політехнічний інститут імені Ігоря Сікорського").Id;
                    var dbUser = new DbUser
                    {
                        Email = "kpiAdmin@gmail.com",
                        UserName = "kpiAdmin",
                        PhoneNumber = "+380-31-415-9266"
                    };
                    var institutionOfEducationAdmin = new InstitutionOfEducationAdmin
                    {
                        InstitutionOfEducationId = currentInstitutionOfEducation,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.InstitutionOfEducationAdmin, institutionOfEducationAdmin);
                }
                #endregion

                #region АВВУ
                currentInstitutionOfEducation = institutionOfEducations.FirstOrDefault(x => x.Name == "Академія внутрішніх військ МВС України").Id;
                {
                    currentInstitutionOfEducation = institutionOfEducations.FirstOrDefault(x => x.Name == "Академія внутрішніх військ МВС України").Id;
                    var dbUser = new DbUser
                    {
                        Email = "naifAdmin@gmail.com",
                        UserName = "naifAdmin",
                        PhoneNumber = "+380-31-415-9267"
                    };
                    var institutionOfEducationAdmin = new InstitutionOfEducationAdmin
                    {
                        InstitutionOfEducationId = currentInstitutionOfEducation,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.InstitutionOfEducationAdmin, institutionOfEducationAdmin);
                }
                #endregion
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

        public static void SeedSpecialtyToIoEDescription(EFDbContext context)
        {
            if (context.SpecialtyToIoEDescriptions.Count() == 0)
            {
                var specialtyToInstitutionOfEducations = context.SpecialtyToInstitutionOfEducations.ToList();
                var exams = context.Exams.ToList();
                var institutionOfEducations = context.InstitutionOfEducations.ToList();

                var currentInstitutionOfEducationId = string.Empty;
                string specialtyToIoEId = "";

                #region Академія внутрішніх військ МВС України
                currentInstitutionOfEducationId = institutionOfEducations.FirstOrDefault(x => x.Name == $"Академія внутрішніх військ МВС України").Id;
                #region Соціологія
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Соціологія" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        Description = "Це кастомний опис спеціальності від університету. Ця спеціальність підійде для тих хто хоче реалізувати себе у майбутньому у даній галузі." +
                       " Для здобувачів вищої освіти вона буде цікавою тому що вони зможуть розкрити себе у даному напрямку за рахунок актуальної інформації, яку будуть доносити ним професіонали своєї справи, які є майстрами у своїй галузі.",
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.2},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Історія України").Id, MinimumScore = 100, Coefficient = 0.25},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.2}
                        }
                    });
                #endregion
                #region Політологія   
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Політологія" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        Description = "Це кастомний опис спеціальності від університету. Ця спеціальність підійде для тих хто хоче реалізувати себе у майбутньому у даній галузі." +
                       " Для здобувачів вищої освіти вона буде цікавою тому що вони зможуть розкрити себе у даному напрямку за рахунок актуальної інформації, яку будуть доносити ним професіонали своєї справи, які є майстрами у своїй галузі.",
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.2},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Історія України").Id, MinimumScore = 100, Coefficient = 0.25},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.2}
                        }
                    });
                #endregion
                #region Статистика
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Статистика" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        Description = "Це кастомний опис спеціальності від університету. Ця спеціальність підійде для тих хто хоче реалізувати себе у майбутньому у даній галузі." +
                       " Для здобувачів вищої освіти вона буде цікавою тому що вони зможуть розкрити себе у даному напрямку за рахунок актуальної інформації, яку будуть доносити ним професіонали своєї справи, які є майстрами у своїй галузі.",
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.2},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.25},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.2}
                        }
                    });
                #endregion
                #endregion

                #region Національний університет "Острозька академія"
                currentInstitutionOfEducationId = institutionOfEducations.FirstOrDefault(x => x.Name == $"Національний університет \"Острозька академія\"").Id;

                #region Системний аналіз
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Системний аналіз" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        Description = "Це кастомний опис спеціальності від університету. Ця спеціальність підійде для тих хто хоче реалізувати себе у майбутньому у даній галузі." +
                       " Для здобувачів вищої освіти вона буде цікавою тому що вони зможуть розкрити себе у даному напрямку за рахунок актуальної інформації, яку будуть доносити ним професіонали своєї справи, які є майстрами у своїй галузі.",
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.3},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.4},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.2}
                        }
                    });
                #endregion
                #region Інженерія програмного забезпечення
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Інженерія програмного забезпечення" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.3},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.4},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.2}
                        }
                    });
                #endregion
                #region Кібербезпека
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Кібербезпека" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.3},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.4},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.2}
                        }
                    });
                #endregion
                #region Математика
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Математика" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.3},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.4},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.2}
                        }
                    });
                #endregion
                #region Прикладна математика
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Прикладна математика" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.3},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.4},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.2}
                        }
                    });
                #endregion
                #region Гідроенергетика
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Гідроенергетика" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.3},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.4},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Географія").Id, MinimumScore = 100, Coefficient = 0.2}
                        }
                    });
                #endregion
                #endregion

                #region Національний університет водного господарства та природокористування
                currentInstitutionOfEducationId = institutionOfEducations.FirstOrDefault(x => x.Name == $"Національний університет водного господарства та природокористування").Id;

                #region Системний аналіз
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Системний аналіз" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.25},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.4},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.25}
                        }
                    });
                #endregion
                #region Інженерія програмного забезпечення
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Інженерія програмного забезпечення" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.25},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.35},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.30}
                        }
                    });
                #endregion
                #region Метрологія та інформаційно-вимірювальна техніка
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Метрологія та інформаційно-вимірювальна техніка" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.30},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.35},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.25}
                        }
                    });
                #endregion
                #region Автоматизація та комп’ютерно-інтегровані технології
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Автоматизація та комп’ютерно-інтегровані технології" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.25},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.4},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.25}
                        }
                    });
                #endregion
                #region Гідроенергетика
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Гідроенергетика" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.3},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.3},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Фізика").Id, MinimumScore = 100, Coefficient = 0.3}
                        }
                    });
                #endregion
                #region Енергетичне машинобудування
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Енергетичне машинобудування" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.25},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.40},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.25}
                        }
                    });
                #endregion
                #endregion

                #region Міжнародний економіко-гуманітарний університет імені академіка Степана Дем’янчука
                currentInstitutionOfEducationId = institutionOfEducations.FirstOrDefault(x => x.Name == $"Міжнародний економіко-гуманітарний університет імені академіка Степана Дем’янчука").Id;

                #region Математика
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Математика" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.3},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.4},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.2}
                        }
                    });
                #endregion
                #region Статистика
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Статистика" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.2},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.5},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.2}
                        }
                    });
                #endregion
                #endregion

                #region Київський політехнічний інститут імені Ігоря Сікорського
                currentInstitutionOfEducationId = institutionOfEducations.FirstOrDefault(x => x.Name == $"Київський політехнічний інститут імені Ігоря Сікорського").Id;

                #region Атомна енергетика
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Атомна енергетика" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        Description = "Це кастомний опис спеціальності від університету. Ця спеціальність підійде для тих хто хоче реалізувати себе у майбутньому у даній галузі." +
                       " Для здобувачів вищої освіти вона буде цікавою тому що вони зможуть розкрити себе у даному напрямку за рахунок актуальної інформації, яку будуть доносити ним професіонали своєї справи, які є майстрами у своїй галузі.",
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.28},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.5},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Фізика").Id, MinimumScore = 100, Coefficient = 0.20}
                        }
                    });
                #endregion
                #region Теплоенергетика
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Теплоенергетика" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.28},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.5},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Фізика").Id, MinimumScore = 100, Coefficient = 0.20}
                        }
                    });
                #endregion
                #region Електроенергетика, електротехніка та електромеханіка
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Електроенергетика, електротехніка та електромеханіка" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.25},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.50},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.20}
                        }
                    });
                #endregion
                #region Математика
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Математика" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.25},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.5},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.2}
                        }
                    });
                #endregion
                #region Прикладна математика
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Прикладна математика" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.3},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.5},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.2}
                        }
                    });
                #endregion
                #region Статистика
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Статистика" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.25},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.5},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.2}
                        }
                    });
                #endregion
                #region Інженерія програмного забезпечення
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Інженерія програмного забезпечення" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.3},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.5},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.2}
                        }
                    });
                #endregion
                #region Інформаційні системи та технології
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Інформаційні системи та технології" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.3},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.5},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.2}
                        }
                    });
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Contract,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.3},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.5},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.2}
                        }
                    });
                #endregion
                #region Кібербезпека
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Кібербезпека" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.3},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.5},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.2}
                        }
                    });
                #endregion
                #region Комп’ютерна інженерія
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Комп’ютерна інженерія" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.3},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.5},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.2}
                        }
                    });
                #endregion
                #region Комп'ютерні науки
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Комп'ютерні науки" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.3},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.5},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.2}
                        }
                    });
                #endregion
                #region Системний аналіз
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Системний аналіз" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.25},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.5},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.2}
                        }
                    });
                #endregion
                #region Метрологія та інформаційно-вимірювальна техніка
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Метрологія та інформаційно-вимірювальна техніка" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.25},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.5},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.2}
                        }
                    });
                #endregion
                #region Мікро- та наносистемна техніка
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Мікро- та наносистемна техніка" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.25},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.5},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.2}
                        }
                    });
                #endregion
                #endregion

                #region Рівненський державний гуманітарний університет
                currentInstitutionOfEducationId = institutionOfEducations.FirstOrDefault(x => x.Name == $"Рівненський державний гуманітарний університет").Id;

                #region Атомна енергетика
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Атомна енергетика" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.3},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.35},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.3}
                        }
                    });
                #endregion
                #region Теплоенергетика
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Теплоенергетика" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.3},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.35},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.25}
                        }
                    });
                #endregion
                #region Математика
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Математика" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.3},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.35},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.3}
                        }
                    });
                #endregion
                #region Прикладна математика
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Прикладна математика" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.3},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.35},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.3}
                        }
                    });
                #endregion
                #region Статистика
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Статистика" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.3},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.35},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Географія").Id, MinimumScore = 100, Coefficient = 0.3}
                        }
                    });
                #endregion
                #region Економіка
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Економіка" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.3},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.35},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.3}
                        }
                    });
                #endregion
                #region Психологія
                specialtyToIoEId = context.SpecialtyToInstitutionOfEducations.FirstOrDefault(x => x.Specialty.Name == "Психологія" && x.InstitutionOfEducationId == currentInstitutionOfEducationId).Id;
                context.SpecialtyToIoEDescriptions.Add(
                    new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyToIoEId,
                        EducationForm = EducationForm.Daily,
                        PaymentForm = PaymentForm.Governmental,
                        EducationalProgramLink = "example.com",
                        ExamRequirements = new List<ExamRequirement>
                        {
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Українська мова та література").Id, MinimumScore = 100, Coefficient = 0.3},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Математика").Id, MinimumScore = 100, Coefficient = 0.35},
                            new ExamRequirement{ ExamId = exams.FirstOrDefault(x => x.Name == "Англійська мова").Id, MinimumScore = 100, Coefficient = 0.3}
                        }
                    });
                #endregion
                #endregion
                context.SaveChanges();
            }
        }

        public async static Task SeedLectures(EFDbContext context, UserManager<DbUser> userManager)
        {
            if (context.Lectors.Count() == 0)
            {
                var institutionOfEducations = context.InstitutionOfEducations.ToList();
                var departments = context.Departments.ToList();

                #region НУВГП
                {
                    var dbUser = new DbUser
                    {
                        Email = "wformservices@triumphlotto.com",
                        UserName = "RayyanFord"
                    };
                    var lector = new Lector
                    {
                        DepartmentId = departments.FirstOrDefault(x => x.Name == "Професійний та кар'єрний розвиток").Id,
                        InstitutionOfEducationId = institutionOfEducations.FirstOrDefault(x => x.Name == "Національний університет водного господарства та природокористування").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.Lector, lector);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "8wesclei_rjf@guestify.com",
                        UserName = "FedericoPierce"
                    };
                    var lector = new Lector
                    {
                        DepartmentId = departments.FirstOrDefault(x => x.Name == "Якість освіти").Id,
                        InstitutionOfEducationId = institutionOfEducations.FirstOrDefault(x => x.Name == "Національний університет водного господарства та природокористування").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.Lector, lector);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "imali@tuneintogiving.com",
                        UserName = "HamzahNeville"
                    };
                    var lector = new Lector
                    {

                        InstitutionOfEducationId = institutionOfEducations.FirstOrDefault(x => x.Name == "Національний університет водного господарства та природокористування").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.Lector, lector);
                }
                #endregion

                #region КПІ
                {
                    var dbUser = new DbUser
                    {
                        Email = "xsdhafer1@usayummy.com",
                        UserName = "StephanPlummer"
                    };
                    var lector = new Lector
                    {
                        DepartmentId = departments.FirstOrDefault(x => x.Name == "Зв’язок з громадкістю").Id,
                        InstitutionOfEducationId = institutionOfEducations.FirstOrDefault(x => x.Name == "Київський політехнічний інститут імені Ігоря Сікорського").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.Lector, lector);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "vweslaine.pg@ericreyess.com",
                        UserName = "AbrahamStafford"
                    };
                    var lector = new Lector
                    {

                        InstitutionOfEducationId = institutionOfEducations.FirstOrDefault(x => x.Name == "Київський політехнічний інститут імені Ігоря Сікорського").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.Lector, lector);
                }
                #endregion

                #region АВВУ
                {
                    var dbUser = new DbUser
                    {
                        Email = "7wave@rose2.ga",
                        UserName = "BradleighHagan"
                    };
                    var lector = new Lector
                    {
                        DepartmentId = departments.FirstOrDefault(x => x.Name == "Редакційно-видавничий").Id,
                        InstitutionOfEducationId = institutionOfEducations.FirstOrDefault(x => x.Name == "Академія внутрішніх військ МВС України").Id,
                        User = dbUser,
                        SpecialtyId = (context.Specialties.FirstOrDefault(x => x.Name == "Інженерія програмного забезпечення")).Id
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.Lector, lector);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "fmcaagent908@rose2.ga",
                        UserName = "GarinBurrows"
                    };
                    var lector = new Lector
                    {
                        DepartmentId = departments.FirstOrDefault(x => x.Name == "Експлуатація комп'ютерних систем").Id,
                        InstitutionOfEducationId = institutionOfEducations.FirstOrDefault(x => x.Name == "Академія внутрішніх військ МВС України").Id,
                        User = dbUser,
                        SpecialtyId = (context.Specialties.FirstOrDefault(x => x.Name == "Комп'ютерні науки")).Id
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.Lector, lector);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "gjuninho.silva.3s@jbnasfjhas96637.ml",
                        UserName = "KaciePalmer"
                    };
                    var lector = new Lector
                    {

                        InstitutionOfEducationId = institutionOfEducations.FirstOrDefault(x => x.Name == "Академія внутрішніх військ МВС України").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.Lector, lector);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "6amine.kikira2@nx-mail.com",
                        UserName = "KadeWood"
                    };
                    var lector = new Lector
                    {

                        InstitutionOfEducationId = institutionOfEducations.FirstOrDefault(x => x.Name == "Академія внутрішніх військ МВС України").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.Lector, lector);
                }
                #endregion
            }
        }

        public async static Task SeedInstitutionOfEducationModerators(EFDbContext context, UserManager<DbUser> userManager)
        {
            if (context.InstitutionOfEducationModerators.Count() == 0)
            {
                var institutionOfEducations = context.InstitutionOfEducations.ToList();
                var admins = context.InstitutionOfEducationAdmins.ToList();

                #region НУВГП
                {

                    var dbUser = new DbUser
                    {
                        Email = "nuweeModerator@gmail.com",
                        UserName = "NuweeModerator"
                    };
                    var institutionOfEducationModerator = new InstitutionOfEducationModerator
                    {
                        AdminId = admins.FirstOrDefault(x => x.InstitutionOfEducation.Name == "Національний університет водного господарства та природокористування").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.InstitutionOfEducationAdmin, institutionOfEducationModerator);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "cfarid.nadji2r@devist.com",
                        UserName = "SafwanWickens"
                    };
                    var institutionOfEducationModerator = new InstitutionOfEducationModerator
                    {
                        AdminId = admins.FirstOrDefault(x => x.InstitutionOfEducation.Name == "Національний університет водного господарства та природокористування").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.InstitutionOfEducationModerator, institutionOfEducationModerator);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "hselma_kra@jomcs.com",
                        UserName = "DanieleHicks"
                    };
                    var institutionOfEducationModerator = new InstitutionOfEducationModerator
                    {
                        AdminId = admins.FirstOrDefault(x => x.InstitutionOfEducation.Name == "Національний університет водного господарства та природокористування").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.InstitutionOfEducationModerator, institutionOfEducationModerator);
                }
                #endregion

                #region КПІ
                {
                    var dbUser = new DbUser
                    {
                        Email = "dill.pazee@azel.xyz",
                        UserName = "GiaVang"
                    };
                    var institutionOfEducationModerator = new InstitutionOfEducationModerator
                    {
                        AdminId = admins.FirstOrDefault(x => x.InstitutionOfEducation.Name == "Київський політехнічний інститут імені Ігоря Сікорського").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.InstitutionOfEducationAdmin, institutionOfEducationModerator);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "2soso@hustletussle.com",
                        UserName = "SuhailMcloughlin"
                    };
                    var institutionOfEducationModerator = new InstitutionOfEducationModerator
                    {
                        AdminId = admins.FirstOrDefault(x => x.InstitutionOfEducation.Name == "Київський політехнічний інститут імені Ігоря Сікорського").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.InstitutionOfEducationModerator, institutionOfEducationModerator);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "vhamimi.salah@nanbianshan.com",
                        UserName = "MirzaReed"
                    };
                    var institutionOfEducationModerator = new InstitutionOfEducationModerator
                    {
                        AdminId = admins.FirstOrDefault(x => x.InstitutionOfEducation.Name == "Київський політехнічний інститут імені Ігоря Сікорського").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.InstitutionOfEducationModerator, institutionOfEducationModerator);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "7chhavi.s@cagi.ru",
                        UserName = "AntonRoberts"
                    };
                    var institutionOfEducationModerator = new InstitutionOfEducationModerator
                    {
                        AdminId = admins.FirstOrDefault(x => x.InstitutionOfEducation.Name == "Київський політехнічний інститут імені Ігоря Сікорського").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.InstitutionOfEducationModerator, institutionOfEducationModerator);
                }
                #endregion

                #region АВВУ
                {
                    var dbUser = new DbUser
                    {
                        Email = "fmourad_v7w@hotmail-s.com",
                        UserName = "AlexisHolding"
                    };
                    var institutionOfEducationModerator = new InstitutionOfEducationModerator
                    {
                        AdminId = admins.FirstOrDefault(x => x.InstitutionOfEducation.Name == "Академія внутрішніх військ МВС України").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.InstitutionOfEducationAdmin, institutionOfEducationModerator);
                }

                {
                    var dbUser = new DbUser
                    {
                        Email = "jleart.blakaj2n@twseel.com",
                        UserName = "ConorBloggs"
                    };
                    var institutionOfEducationModerator = new InstitutionOfEducationModerator
                    {
                        AdminId = admins.FirstOrDefault(x => x.InstitutionOfEducation.Name == "Академія внутрішніх військ МВС України").Id,
                        User = dbUser
                    };
                    await CreateUser(context, userManager, dbUser, ProjectRoles.InstitutionOfEducationModerator, institutionOfEducationModerator);
                }
                #endregion
            }
        }

        public async static Task SeedDepartments(EFDbContext context)
        {
            if (context.Departments.Count() == 0)
            {
                await context.Departments.AddRangeAsync(new List<Department>
                {
                    new Department
                    {
                        Name = "Професійний та кар'єрний розвиток",
                        Description = "Ваше майбутнє у великій мірі залежить від Вас особисто – від вашої наполегливості, вашого бажання зробити кар’єру."
                    },
                    new Department
                    {
                        Name = "Якість освіти",
                        Description = "Сприяє визнанню кваліфікацій, які здобувають студенти, та досвіду вищої освіти, який вони отримують, як головних пріоритетів університету"
                    },
                    new Department
                    {
                        Name = "Зв’язок з громадкістю",
                        Description = "Систематичне та своєчасне, якісне і повне інформування про важливі події в університеті до міських, обласних, загальнодержавних засобів масової інформації"
                    },
                    new Department
                    {
                        Name = "Редакційно-видавничий",
                        Description = "Друк навчальних та навчально-методичних посібників, наукових видань та збірників наукових праць, методичних вказівок, довідкових та інформаційних матеріалів, дипломів та додатків до дипломів, авторефератів, бланкової продукції, журналів обліку та індивідуальних планів, студентських газет, запрошень, оголошень, грамот, вітальних адрес"
                    },
                    new Department
                    {
                        Name = "Експлуатація комп'ютерних систем",
                        Description = "Відділ експлуатації комп'ютерних систем відповідає за комп'ютерне забезпечення навчального процесу, впровадження нової обчислювальної техніки, установка, тестування і оновлення операційних систем та прикладних програм, проведення інвентаризації комп'ютерної техніки, експлуатація та розвиток комп'ютерної мережі університету."
                    }
                });
                await context.SaveChangesAsync();
            }
        }

        public async static Task SeedDisciplines(EFDbContext context)
        {
            if (context.Disciplines.Count() == 0)
            {
                var Lectors = context.Lectors.ToList();
                var Specialities = context.Specialties.ToList();
                var Departments = context.Departments.ToList();
                string[] DepartmentsId = { 

                    Departments.FirstOrDefault(x => x.Name == "Експлуатація комп'ютерних систем").Id,

                    Departments.FirstOrDefault(x => x.Name == "Якість освіти").Id,

                    Departments.FirstOrDefault(x => x.Name == "Зв’язок з громадкістю").Id

                };

                await context.Disciplines.AddRangeAsync(new List<Discipline>
                {
                    new Discipline
                    {
                        Name = "Архітектура комп'ютера",
                        Description = "Дисципліна АРХІТЕКТУРА КОМП'ЮТЕРА передбачає вивчення принципів організації і функціонування сучасних комп’ютерних систем, призначення і роботи на логічному рівні базових логічних елементів і функціональних вузлів комп’ютера, організації і функціонування комп'ютерної пам'яті, призначення і функціонування основних периферійних пристроїв.",
                        LectorId= Lectors.FirstOrDefault(x => x.DepartmentId == DepartmentsId[0]).Id,
                        SpecialityId = Specialities.FirstOrDefault(x => x.Name == "Інженерія програмного забезпечення").Id    
                    },

                 new Discipline
                   {
                        Name = "Об'єктно-орієнтоване програмування",
                        Description = "Мета вивчення курсу – ознайомити студентів з основами об‘єктно-орієнтованого аналізу та проектування. Практичну реалізацію об‘єктно-орієнтованого програмування показати на прикладі мови С++ .",
                        LectorId= Lectors.FirstOrDefault(x => x.DepartmentId == DepartmentsId[1]).Id,
                        SpecialityId = Specialities.FirstOrDefault(x => x.Name == "Комп'ютерні науки").Id
                    },

                    new Discipline
                    {
                        Name = "Алгоритми і структури даних",
                        Description = "Мета вивчення курсу – ознайомити студентів з різними моделями структур даних, які зустрічаються на логічному та фізичному рівнях їх організації, а також вивчити основні алгоритми опрацювання цих структур",
                        LectorId= Lectors.FirstOrDefault(x => x.DepartmentId == DepartmentsId[2]).Id,
                        SpecialityId = Specialities.FirstOrDefault(x => x.Name == "Кібербезпека").Id
                    }
                });
                await context.SaveChangesAsync();
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
                //context.Database.Migrate();
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
                // InstitutionOfEducations: 3
                // InstitutionOfEducationAdmins: 3
                // InstitutionOfEducationModerators: 9
                // Lectures: 9
                // Departments: 5
                // Disciplines 3

                await SeederDB.SeedRoles(managerRole);
                await SeederDB.SeedSuperAdmin(context, manager);

                #region School
                SeederDB.SeedSchools(context);
                SeederDB.SeedSchoolAdmins(context);
                await SeederDB.SeedSchoolModerators(context, manager);
                await SeederDB.SeedGraduates(context, manager);
                #endregion

                #region InstitutionOfEducation
                await SeederDB.SeedDirections(context);
                await SeederDB.SeedSpecialities(context);
                SeederDB.SeedInstitutionOfEducations(context);
                SeederDB.SeedDirectionsAndSpecialitiesToInstitutionOfEducation(context);
                SeederDB.SeedSpecialtyToInstitutionOfEducationToGraduate(context);
                SeederDB.SeedSpecialtyToGraduate(context);
                await SeederDB.SeedInstitutionOfEducationAdmins(context, manager);
                await SeederDB.SeedInstitutionOfEducationModerators(context, manager);
                await SeederDB.SeedDepartments(context);
                await SeederDB.SeedLectures(context, manager);
                await SeederDB.SeedDisciplines(context);
                SeederDB.SeedExams(context);
                SeederDB.SeedSpecialtyToIoEDescription(context);
                #endregion

                Console.WriteLine("Database seeded.");
            }
        }
    }
}