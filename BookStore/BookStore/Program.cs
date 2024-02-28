using BookStore.BL.Interfaces;
using BookStore.BL.Services;
using BookStore.DL.Interfaces;
using BookStore.DL.Repositories;
using BookStore.DL.Repositories.Mongo;
using BookStore.Healthchecks;
using BookStore.Models.Configurations;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace BookStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<MongoConfiguration>(
                builder.Configuration.GetSection(
                    nameof(MongoConfiguration)));

            // Add services to the container.
            builder.Services
                .AddSingleton<IAuthorRepository, AuthorRepository>();
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
            builder.Services.AddSwaggerGen();

            builder.Services
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
            builder.Services
                .AddValidatorsFromAssemblyContaining(typeof(Program));

           builder.Services.AddHealthChecks()
                .AddCheck<CustomHealthCheck>(nameof(CustomHealthCheck))
                .AddUrlGroup(new Uri("https://google.bg"), name: "My Service");

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.MapHealthChecks("/healthz");

            app.Run();
        }
    }
}