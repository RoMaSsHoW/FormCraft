﻿using FormCraft.Application.Commands.FormCommands;
using FormCraft.Application.Commands.Template;
using FormCraft.Application.Common.Persistance;
using FormCraft.Application.Intefaces;
using FormCraft.Application.Models.DTO;
using FormCraft.Application.Queries;
using FormCraft.Application.Services;
using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;
using FormCraft.Infrastructure;
using FormCraft.Infrastructure.Persistance.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.Data;
using System.Text;

namespace FormCraft.API.Extentions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JWTSettings>(configuration.GetSection(nameof(JWTSettings)));

            ConfigureJwtAuthenticationAndAuthorization(services, configuration);

            services.AddHttpContextAccessor();

            ConfigureServices(services);

            ConfigureSettigsForDapper(services, configuration);

            ConfigureDbContext(services, configuration);

            ConfigureMediatR(services);

            return services;
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ITokenService, TokenService>(); 

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<ITopicExistenceChecker, TopicExistenceChecker>();

            services.AddScoped<IFormRepository, FormRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IAnswerRepository, AnswerRepository>();
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFormTagRepository, FormTagRepository>();
        }

        private static void ConfigureJwtAuthenticationAndAuthorization(IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection(nameof(JWTSettings)).Get<JWTSettings>();

            if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.Key))
            {
                throw new InvalidOperationException("JWT settings are not configured properly.");
            }

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingKey
                    };
                });

            services.AddAuthorization();
        }

        private static void ConfigureDbContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FormCraftDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("PostgresqlDbConnection"));
            });
        }

        private static void ConfigureSettigsForDapper(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDbConnection>(provider =>
            {
                return new NpgsqlConnection(configuration.GetConnectionString("PostgresqlDbConnection"));
            });
        }

        private static void ConfigureMediatR(IServiceCollection services)
        {
            services.AddMediatR(mc =>
            {
                mc.RegisterServicesFromAssemblies(
                    typeof(CreateNewFormWithQuestionCommand).Assembly,
                    typeof(UpdateFormWithQuestionCommand).Assembly,
                    typeof(DeleteTagsFromFormCommand).Assembly,
                    typeof(DeleteFormsCommand).Assembly,
                    typeof(AddQuestionsToFormCommand).Assembly,
                    typeof(DeleteQuestionsFromFormCommand).Assembly,
                    typeof(SetAnswerToQuestionCommand).Assembly,
                    typeof(GetAllTemplatesQuery).Assembly,
                    typeof(GetTemplateQuery).Assembly);
            });
        }
    }
}
