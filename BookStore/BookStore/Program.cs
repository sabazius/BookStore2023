using System.Text;
using AspNetCore.Identity.Mongo.Model;
using AspNetCore.Identity.MongoDbCore.Models;
using BookStore.BL.Interfaces;
using BookStore.BL.Services;
using BookStore.DL.Interfaces;
using BookStore.DL.Repositories.Mongo;
using BookStore.Healthchecks;
using BookStore.Models.Configurations;
using BookStore.Models.Configurations.Identity;
using BookStore.Models.Models.Users;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

namespace BookStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            Log.Information("Starting web application");

            var jwtSettings = new JwtSettings();
            builder.Configuration
                .Bind(nameof(jwtSettings), jwtSettings);
            builder.Services.AddSingleton(jwtSettings);

            builder.Services.AddAuthentication(op =>
                {
                    op.DefaultAuthenticateScheme =
                        JwtBearerDefaults.AuthenticationScheme;
                    op.DefaultScheme =
                        JwtBearerDefaults.AuthenticationScheme;
                    op.DefaultChallengeScheme =
                        JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(op =>
                {
                    op.SaveToken = true;
                    op.TokenValidationParameters = new()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                        ValidateIssuer = false,
                        ValidAudience = null,
                        ValidateLifetime = true
                    };
                });

            builder.Services.Configure<MongoConfiguration>(
                builder.Configuration.GetSection(
                    nameof(MongoConfiguration)));

            // Add services to the container.
            builder.Services
                .AddSingleton<IAuthorRepository, AuthorMongoRepository>();
            builder.Services
                .AddSingleton<IAuthorService, AuthorService>();
            builder.Services
                .AddSingleton<IBookRepository, BookMongoRepository>();
            builder.Services
                .AddSingleton<IBookService, BookService>();
            builder.Services.AddSingleton<ILibraryService, LibraryService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(x =>
            {
                var security = new Dictionary<string, IEnumerable<string>>()
                {
                    {"Bearer", Array.Empty<string>()}
                };
                OpenApiSecurityScheme securityDefinition = new()
                {
                    Name = "Bearer",
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    Description = "Specify the authorization token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http
                };
                x.AddSecurityDefinition("jwt_auth", securityDefinition);
                x.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {securityDefinition, new string[] {}}
                });
            });

            builder.Services
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
            builder.Services
                .AddValidatorsFromAssemblyContaining(typeof(Program));

            builder.Services.AddHealthChecks()
                 .AddCheck<CustomHealthCheck>(nameof(CustomHealthCheck))
                 .AddUrlGroup(new Uri("https://google.bg"), name: "My Service");

           var mongoCfg = builder
                .Configuration
                .GetSection(nameof(MongoConfiguration))
                .Get<MongoConfiguration>();

           builder.Services.AddIdentity<User, MongoIdentityRole>()
               .AddMongoDbStores<User, MongoIdentityRole, Guid>
                   (mongoCfg.ConnectionString, mongoCfg.DatabaseName)
               .AddSignInManager()
               .AddDefaultTokenProviders();

           builder.Host.UseSerilog();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapHealthChecks("/healthz");

            app.Run();
        }
    }
}