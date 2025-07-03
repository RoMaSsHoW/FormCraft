﻿using FormCraft.Application.Commands;
using FormCraft.Application.Common.Persistance;
using FormCraft.Application.Services;
using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using FormCraft.Infrastructure;
using FormCraft.Infrastructure.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FormCraft.API.Extentions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            ConfigureServices(services);

            ConfigureSettigsForDapper(services);

            ConfigureDbContext(services, configuration);

            ConfigureMediatR(services);

            return services;
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddSingleton<ITopicExistenceChecker, TopicExistenceChecker>();
            services.AddSingleton<IUserRoleChecker, UserRoleChecker>();

            services.AddScoped<IFormRepository, FormRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IAnswerRepository, AnswerRepository>();
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
        }

        private static void ConfigureDbContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FormCraftDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("PostgresqlDbConnection"));
            });
        }

        private static void ConfigureSettigsForDapper(IServiceCollection services)
        {
            services.AddSingleton<ISqlConnectionFacroty, SqlConnectionFacroty>();
        }

        private static void ConfigureMediatR(IServiceCollection services)
        {
            services.AddMediatR(mc =>
            {
                mc.RegisterServicesFromAssemblies(
                    typeof(CreateNewFormCommand).Assembly);

                //mc.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));
            });
        }
    }
}
