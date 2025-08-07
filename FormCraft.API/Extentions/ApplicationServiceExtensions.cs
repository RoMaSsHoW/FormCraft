using FluentMigrator.Runner;
using FormCraft.Application.Commands.FormWithQuestionCommands;
using FormCraft.Application.Common.Persistance;
using FormCraft.Application.Models.DTO;
using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;
using FormCraft.Infrastructure.Persistance;
using FormCraft.Infrastructure.Persistance.Migrations;
using FormCraft.Infrastructure.Persistance.Repositories;
using FormCraft.Infrastructure.Persistance.Services;
using MassTransit;
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

            ConfigureRabbitMq(services, configuration);

            ConfigureMediatR(services);

            ConfigureFluentMigrator(services, configuration);

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

        private static void ConfigureFluentMigrator(IServiceCollection services, IConfiguration configuration)
        {
            services.AddFluentMigratorCore()
                .ConfigureRunner(runner => runner
                    .AddPostgres()
                    .WithGlobalConnectionString(configuration.GetConnectionString("PostgresqlDbConnection"))
                    .ScanIn(typeof(InitialMigration).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole());
        }

        private static void ConfigureMediatR(IServiceCollection services)
        {
            services.AddMediatR(mc =>
            {
                mc.RegisterServicesFromAssemblies(
                    typeof(CreateNewFormWithQuestionCommand).Assembly);
            });
        }

        private static void ConfigureRabbitMq(IServiceCollection services, IConfiguration configuration)
        {
            var rabbitSettings = configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();

            if (rabbitSettings is null || string.IsNullOrEmpty(rabbitSettings.Host))
                throw new InvalidOperationException("RabbitMQ settings are not configured properly.");

            services.AddMassTransit(x =>
            {
                // Register consumers
                x.AddConsumers(typeof(Program).Assembly); // Or specify a particular assembly

                // Configure RabbitMQ
                x.UsingRabbitMq((context, cfg) =>
                {
                    var uri = new Uri($"rabbitmq://{rabbitSettings.Host}:{rabbitSettings.Port}/{rabbitSettings.VirtualHost}");

                    cfg.Host(uri, h =>
                    {
                        h.Username(rabbitSettings.Username);
                        h.Password(rabbitSettings.Password);
                    });

                    cfg.ConfigureEndpoints(context); // Automatically configure endpoints for consumers
                });
            });
        }
    }

}
