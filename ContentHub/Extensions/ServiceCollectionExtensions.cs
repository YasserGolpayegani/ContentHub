using ContentHub.Api.Authorization;
using ContentHub.Api.Authorization.Handlers;
using ContentHub.Api.Services;
using ContentHub.Application.Abstractions.Authentication;
using ContentHub.Application.Abstractions.Persistence;
using ContentHub.Application.Behaviors;
using ContentHub.Application.Features.Posts.Commands.CreatePost;
using ContentHub.Application.Interfaces.Persistence;
using ContentHub.Domain.Identity;
using ContentHub.Infrastructure.Options;
using ContentHub.Infrastructure.Services;
using ContentHub.Persistence.Contexts;
using ContentHub.Persistence.Repositories;
using ContentHub.Persistence.UnitOfWork;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

//using Microsoft.OpenApi;
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

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IAuditRepository, AuditRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static IServiceCollection AddSwaggerService(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "ContentHub API",
                Version = "v1"
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter JWT token only."
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {

        //services.AddValidatorsFromAssembly(typeof(LoginRequestValidator).Assembly);

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<CreatePostCommand>();

            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(TransactionBehavior<,>));
            config.AddOpenBehavior(typeof(AuditBehavior<,>));
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

        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

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


        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

        //services.AddAuthorization(options =>
        //{
        //    options.AddPolicy(Policies.AdminOnly, policy => policy.RequireRole(Roles.Admin));

        //    options.AddPolicy(
        //        Policies.AuthorOrAdmin,
        //        policy => policy.RequireRole(Roles.Admin, Roles.Author));



        //    options.AddPolicy(
        //         Policies.ViewPosts,
        //         policy => policy.RequireClaim("permission", PermissionNames.Posts.Read));

        //    options.AddPolicy(
        //        Policies.CreatePosts,
        //        policy => policy.RequireClaim("permission", PermissionNames.Posts.Create));

        //    options.AddPolicy(
        //        Policies.UpdatePosts,
        //        policy => policy.RequireClaim("permission", PermissionNames.Posts.Update));

        //    options.AddPolicy(
        //        Policies.DeletePosts,
        //        policy => policy.RequireClaim("permission", PermissionNames.Posts.Delete));

        //    options.AddPolicy(
        //        Policies.PublishPosts,
        //        policy => policy.RequireClaim("permission", PermissionNames.Posts.Publish));
        //});

        services.AddScoped<IJwtService, JwtService>();

        services.AddScoped<IRefreshTokenService, RefreshTokenService>();


        return services;
    }
}

