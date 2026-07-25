using ContentHub.Application.Behaviors;
using ContentHub.Application.Features.Posts.Commands.CreatePost;
using ContentHub.Application.Interfaces;
using ContentHub.Application.Interfaces.Categories;
using ContentHub.Application.Interfaces.Comments;
using ContentHub.Application.Interfaces.Posts;
using ContentHub.Application.Interfaces.Tags;
using ContentHub.Application.Services;
using ContentHub.Application.Validators;
using ContentHub.Domain.Identity;
using ContentHub.Infrastructure.Options;
using ContentHub.Infrastructure.Services;
using ContentHub.Persistence.Contexts;
using ContentHub.Persistence.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ContentHub.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"));
        });

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<ICategoryServices, CategoryService>();
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<ICommentService, CommentService>();

        //services.AddValidatorsFromAssembly(typeof(LoginRequestValidator).Assembly);

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<CreatePostCommand>();

            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssemblyContaining<CreatePostCommandValidator>();

        return services;
    }

    public static IServiceCollection AddPersistenceRepositories(this IServiceCollection services)
    {
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();

        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

        services.AddScoped<IJwtService, JwtService>();
        services.AddIdentity<AppUser, IdentityRole<Guid>>(options =>
        {
            options.Password.RequiredLength = 3;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;

            options.User.RequireUniqueEmail = true;
        })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}

