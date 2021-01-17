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
using System.Collections.Generic;
using System.IO;
using System.Text;
using YIF.Core.Data;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.DtoModels.IdentityDTO;
using YIF.Core.Domain.DtoModels.School;
using YIF.Core.Domain.DtoModels.SchoolAdmin;
using YIF.Core.Domain.DtoModels.SchoolModerator;
using YIF.Core.Domain.Repositories;
using YIF.Core.Domain.ServiceInterfaces;
using YIF.Core.Service.Concrete.Services;

namespace YIF_Backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Interfaces
            services.AddTransient<IApplicationDbContext, EFDbContext>();
            services.AddTransient<IUserRepository<DbUser, UserDTO>, UserRepository>();
            services.AddTransient<ISchoolGraduateRepository<SchoolDTO>, SchoolGraduateRepository>();
            services.AddTransient<IRepository<University, UniversityDTO>, UniversityRepository>();
            services.AddTransient<IRepository<Speciality, SpecialityDTO>, SpecialityRepository>();
            services.AddTransient<IRepository<Direction, DirectionDTO>, DirectionRepository>();
            services.AddTransient<IRepository<DirectionToUniversity, DirectionToUniversityDTO>, DirectionToUniversityRepository>();
            services.AddTransient<IRepository<SpecialityToUniversity, SpecialityToUniversityDTO>, SpecialityToUniversityRepository>();
            services.AddTransient<ITokenRepository, TokenRepository>();
            services.AddTransient<IUserService<DbUser>, UserService>();
            services.AddTransient<ISpecialityService, SpecialtyService>();
            services.AddTransient<IRecaptchaService, RecaptchaService>();
            services.AddTransient<IEmailService, SendGridService>();
            services.AddTransient<ISuperAdminService, SuperAdminService>();
            services.AddTransient<IUniversityModeratorRepository<UniversityModeratorDTO>, UniversityModeratorRepository>();
            services.AddTransient<IUniversityAdminRepository<UniversityAdminDTO>, UniversityAdminRepository>();
            services.AddTransient<IUniversityRepository<University, UniversityDTO>, UniversityRepository>();

            services.AddTransient<ISchoolRepository<SchoolDTO>, SchoolRepository>();
            services.AddTransient<ISchoolModeratorRepository<SchoolModeratorDTO>, SchoolModeratorRepository>();
            services.AddTransient<ISchoolAdminRepository<SchoolAdminDTO>, SchoolAdminRepository>();
            services.AddTransient<IUniversityService<University>, UniversityService>();
            services.AddTransient<IDirectionService, DirectionService>();
            #endregion

            #region FluentValidation
            services.AddMvc().AddFluentValidation();
            #endregion

            services.Configure<FormOptions>(options =>
            {
                // Set the limit to 10 MB
                options.MultipartBodyLengthLimit = 10 * 1024 * 1024;
            });

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "YIF API",
                    Description = "A project ASP.NET Core Web API",
                    Contact = new OpenApiContact
                    {
                        Name = "Team YIF",
                        Email = "yifteam2020@gmail.com",
                        Url = new Uri("https://github.com/ita-social-projects/YIF_Backend/blob/dev/README.md")
                    }
                });

                c.AddSecurityDefinition("Bearer",
                     new OpenApiSecurityScheme
                     {
                         Description = "JWT Authorization header using the Bearer scheme.",
                         Type = SecuritySchemeType.Http,
                         Scheme = "bearer"
                     });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
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

            #region CORS
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            #endregion

            #region EntityFramework
            services.AddDbContext<EFDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

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

            services.AddControllers();
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

            // app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            #region CORS
            app.UseCors(builder => builder
                 .AllowAnyHeader()
                 .AllowAnyMethod()
                 .SetIsOriginAllowed((host) => true)
                 .AllowCredentials()
             );
            #endregion

            #region Seeder
            SeederDB.SeedData(app.ApplicationServices);
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
    }
}
