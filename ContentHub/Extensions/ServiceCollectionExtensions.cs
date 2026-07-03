using ContentHub.Application.Interfaces.Categories;
using ContentHub.Application.Interfaces.Comments;
using ContentHub.Application.Interfaces.Posts;
using ContentHub.Application.Interfaces.Tags;
using ContentHub.Application.Services;
using ContentHub.Persistence.Contexts;
using ContentHub.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
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
}

