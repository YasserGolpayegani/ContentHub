using ContentHub.Application.Authorization;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

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
        //this section should be remove later because we user MediatR and no need services
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();

        return services;
    }


    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

        var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();


        services.AddIdentityCore<AppUser>(options =>
               {
                   options.Password.RequiredLength = 3;
                   options.Password.RequireDigit = false;
                   options.Password.RequireLowercase = false;
                   options.Password.RequireUppercase = false;
                   options.Password.RequireNonAlphanumeric = false;

                   options.User.RequireUniqueEmail = true;
               })
                   .AddRoles<IdentityRole<Guid>>()
                   .AddEntityFrameworkStores<AppDbContext>()
                   .AddDefaultTokenProviders();


        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtOptions.Issuer,

                ValidateAudience = true,
                ValidAudience = jwtOptions.Audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),

                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };
        });


        services.AddAuthorization(options =>
        {
            options.AddPolicy(
                Policies.AdminOnly,
                policy => policy.RequireRole(Roles.Admin));
            
            options.AddPolicy(
                Policies.AuthorOrAdmin, 
                policy => policy.RequireRole(Roles.Admin, Roles.Author));



            options.AddPolicy(
                 Policies.ViewPosts,
                 policy => policy.RequireClaim("permission", Permissions.Posts.View));

            options.AddPolicy(
                Policies.CreatePosts, 
                policy => policy.RequireClaim("permission", Permissions.Posts.Create));

            options.AddPolicy(
                Policies.UpdatePosts,
                policy => policy.RequireClaim("permission", Permissions.Posts.Update));

            options.AddPolicy(
                Policies.DeletePosts,
                policy => policy.RequireClaim("permission", Permissions.Posts.Delete));

            options.AddPolicy(
                Policies.PublishPosts,
                policy => policy.RequireClaim("permission", Permissions.Posts.Publish));
        });

        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}

