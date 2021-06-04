using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;
using YIF.Core.Data;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Data.Seaders;
using YIF.Core.Domain.DtoModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.DtoModels.IdentityDTO;
using YIF.Core.Domain.Repositories;
using YIF.Core.Domain.ServiceInterfaces;
using YIF.Core.Domain.Validators;
using YIF.Core.Service.Concrete.Services;
using YIF_Backend.Infrastructure;
using YIF_Backend.Infrastructure.Middleware;

namespace YIF_Backend
{
    public class Startup
    {
        private readonly IWebHostEnvironment _currentEnvironment;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment currentEnvironment)
        {
            _currentEnvironment = currentEnvironment;
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region CORS
            services.AddCors();
            #endregion

            services.Configure<FormOptions>(options =>
            {
                // Set the limit to 100 MB
                options.ValueCountLimit = 1024;
                options.KeyLengthLimit = 1024 * 2;
                options.ValueLengthLimit = 1024 * 1024 * 100;
            });

            #region Interfaces
            services.AddTransient<IApplicationDbContext, EFDbContext>();
            services.AddTransient<IUserRepository<DbUser, UserDTO>, UserRepository>();
            services.AddTransient<IUserProfileRepository<UserProfile, UserProfileDTO>, UserProfileRepository>();
            services.AddTransient<ISchoolGraduateRepository<SchoolDTO>, SchoolGraduateRepository>();
            services.AddTransient<IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO>, InstitutionOfEducationRepository>();
            services.AddTransient<ISpecialtyRepository<Specialty, SpecialtyDTO>, SpecialtyRepository>();
            services.AddTransient<IDirectionRepository<Direction, DirectionDTO>, DirectionRepository>();
            services.AddTransient<IRepository<DirectionToInstitutionOfEducation, DirectionToInstitutionOfEducationDTO>, DirectionToInstitutionOfEducationRepository>();
            services.AddTransient<ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO>, SpecialtyToInstitutionOfEducationRepository>();
            services.AddTransient<IExamRequirementRepository<ExamRequirement, ExamRequirementDTO>, ExamRequirementRepository>();
            services.AddTransient<ITokenRepository<TokenDTO>, TokenRepository>();
            services.AddTransient<IUserService<DbUser>, UserService>();
            services.AddTransient<ISpecialtyService, SpecialtyService>();
            services.AddTransient<IRecaptchaService, RecaptchaService>();
            services.AddTransient<IEmailService, SendGridService>();
            services.AddTransient<ISuperAdminService, SuperAdminService>();
            services.AddTransient<IInstitutionOfEducationModeratorRepository<InstitutionOfEducationModerator, InstitutionOfEducationModeratorDTO>, InstitutionOfEducationModeratorRepository>();
            services.AddTransient<IInstitutionOfEducationAdminRepository<InstitutionOfEducationAdmin, InstitutionOfEducationAdminDTO>, InstitutionOfEducationAdminRepository>();
            services.AddTransient<ISchoolRepository<SchoolDTO>, SchoolRepository>();
            services.AddTransient<ISchoolModeratorRepository<SchoolModeratorDTO>, SchoolModeratorRepository>();
            services.AddTransient<ISchoolAdminRepository<SchoolAdminDTO>, SchoolAdminRepository>();
            services.AddTransient<IInstitutionOfEducationService<InstitutionOfEducation>, InstitutionOfEducationService>();
            services.AddTransient<IDirectionService, DirectionService>();
            services.AddTransient<ISchoolService, SchoolService>();
            services.AddTransient<IPaginationService, PaginationService>();
            services.AddTransient<IGraduateRepository<Graduate, GraduateDTO>, GraduateRepository>();
            services.AddTransient<IExamRepository<Exam, ExamDTO>, ExamRepository>();
            services.AddTransient<ISpecialtyToIoEDescriptionRepository<SpecialtyToIoEDescription, SpecialtyToIoEDescriptionDTO>, SpecialtyToIoEDescriptionRepository>();
            services.AddTransient<IIoEAdminService, IoEAdminService>();
            services.AddTransient<IIoEModeratorService, IoEModeratorService>();
            services.AddTransient<ISpecialtyToGraduateRepository<SpecialtyToGraduate, SpecialtyToGraduateDTO>, SpecialtyToGraduateRepository>();
            services.AddTransient<ISpecialtyToIoEToGraduateRepository<SpecialtyToInstitutionOfEducationToGraduate, SpecialtyToInstitutionOfEducationToGraduateDTO>, SpecialtyToIoEToGraduateRepository>();
            services.AddTransient<ILectorService, LectorService>();
            services.AddTransient<ILectorRepository<Lector, LectorDTO>, LectorRepository>();

            #endregion

            #region FluentValidation
            services.AddMvc()
                .AddFluentValidation(s =>
                {
                    s.RegisterValidatorsFromAssemblyContaining<SpecialtyDescriptionUpdateApiModelValidator>();
                });
            #endregion

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "YITF API",
                    Description = "A project ASP.NET Core Web API",
                    Contact = new OpenApiContact
                    {
                        Name = "Team YITF",
                        Email = "yifteam2020@gmail.com",
                        Url = new Uri("https://github.com/ita-social-projects/YIF_Backend/blob/dev/README.md")
                    }
                });

                c.OperationFilter<AddAuthorizationHeaderOperationHeader>();
                c.AddSecurityDefinition("Bearer",
                     new OpenApiSecurityScheme
                     {
                         Description = "JWT Authorization header. Use bearer token to authorize.",
                         Type = SecuritySchemeType.Http,
                         Scheme = "bearer",
                         BearerFormat = "JWT"
                     });

                foreach (string xmlFile in Directory.EnumerateFiles(AppContext.BaseDirectory, "*.xml"))
                {
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    if (File.Exists(xmlPath))
                    {
                        c.IncludeXmlComments(xmlPath);
                    }
                }
            });
            #endregion

            #region EntityFramework
            AddDb(ref services);
            services.AddIdentity<DbUser, IdentityRole>(options => options.Stores.MaxLengthForKeys = 128)
                .AddEntityFrameworkStores<EFDbContext>()
                .AddDefaultTokenProviders();
            #endregion

            #region JwtService
            services.AddTransient<IJwtService, JwtService>();
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("SecretPhrase")));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = signingKey,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
            #endregion

            #region IdentitySettings
            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
            });
            #endregion

            #region AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            #endregion

            #region Strings
            services.AddSingleton<ResourceManager>(new ResourceManager("YIF_Backend.Resources.Strings", Assembly.GetExecutingAssembly()));
            #endregion

            services.AddControllers().AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            #region  InitStaticFiles Images
            string pathRoot = InitStaticFilesService.CreateFolderServer(env, this.Configuration,
                    new string[] { "ImagesPath" });

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(pathRoot),
                RequestPath = new PathString('/' + Configuration.GetValue<string>("UrlImages"))
            });
            #endregion

            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

            // app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            #region CORS
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            #endregion

            #region Seeder
            SeederDB.SeedData(app.ApplicationServices);
            if (_currentEnvironment.IsEnvironment("Testing"))
            {
               // SeederDB.SeedData(app.ApplicationServices);
            }
            #endregion

            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.IndexStream = () => GetType().Assembly.GetManifestResourceStream("YIF_Backend.Swagger.index.html");

                c.SwaggerEndpoint("/swagger/v1/swagger.json", "YIF API V1");
            });
            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void AddDb(ref IServiceCollection services)
        {
            if (_currentEnvironment.IsEnvironment("Testing"))
            {
                services.AddDbContextPool<EFDbContext>(options =>
                    options.UseInMemoryDatabase("TestingDB"));
            }
            else
            {
                services.AddDbContext<EFDbContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            }
        }
    }
}
