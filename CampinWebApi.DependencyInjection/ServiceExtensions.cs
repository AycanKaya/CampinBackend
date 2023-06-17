using System.Reflection;
using CampinWebApi.Contracts;
using CampinWebApi.Core.Models;
using CampinWebApi.Domain;
using CampinWebApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CampinWebApi.Domain.Entities;
using CampinWebApi.Services.CrediCardMock;
using Microsoft.AspNetCore.Http;
using MediatR;
using Microsoft.OpenApi.Models;


namespace CampinWebApi.DependencyInjection;
public static class DependencyInjection
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        
        services.AddDbContext<CampinDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(CampinDbContext).Assembly.FullName)));
        services.AddScoped<ICampinDbContext>(provider => provider.GetService<CampinDbContext>());
        
        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<CampinDbContext>()
            .AddDefaultTokenProviders();
        
        services.AddRazorPages();
        services.Configure<IdentityOptions>(options =>
        {
            // Password settings.
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings.
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = false;
        });
        
        services.AddTransient<IJWTService, JWTService>();
        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<ICampsiteService, CampsiteService>();
        services.AddTransient<ICommentService, CommentService>();
        services.AddTransient<ICardService, CardService>();
        services.AddTransient<IFavoriteCampsitesService, FavoriteCampsitesService>();
        services.AddTransient<ICampsiteOwnerService, CampsiteOwnerService>();
        services.AddTransient<IRezervationService, RezervationService>();
        services.AddTransient<ICrediCardService, CrediCardService>();
        services.AddTransient<ICityService, CityService>();
        services.AddTransient<IHolidayDestinationService, HolidayDestinationService>();
        services.AddTransient<IUserInfoService, UserInfoService>();
        services.AddTransient<ICountryService, CountryService>();
        services.AddTransient<IAzureBlobStorageService, AzureBlobStorageService>();
        
        services.Configure<JWTModel>(configuration.GetSection("JWTSettings"));
        
        services.AddAuthentication();
             
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JWTSettings:Issuer"],
                    ValidAudience = configuration["JWTSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                };
                o.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";
                        return c.Response.WriteAsync(c.Exception.ToString());
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        var result = (("You are not Authorized"));
                        return context.Response.WriteAsync(result);
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        var result = (("You are not authorized to access this resource"));
                        return context.Response.WriteAsync(result);
                    },
                };

            });
    }
    public static void AddSwaggerExtension(this IServiceCollection services)
    {
      services.AddSwaggerGen(c =>
        {
            
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "WebApi",
                Description = "This Api will be responsible for overall data distribution and authorization.",
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        },
                        Scheme = "Bearer",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    }, new List<string>()
                },
            });
        }); 
    } 
}
