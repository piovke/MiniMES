using Microsoft.EntityFrameworkCore;
using MiniMES.Models;

namespace MiniMesTrainApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            string? connString = builder.Configuration.GetConnectionString("MiniProduction");
            if (connString == null)
            {
                Console.WriteLine("There is no ConnectionString called MiniProduction in appsettings.json");
            }
            else
            {
                builder.Services.AddDbContext<MiniProductionDbContext>(options =>
                    options.UseMySql(
                        builder.Configuration.GetConnectionString("MiniProduction"),
                        new MySqlServerVersion(new Version(10, 4, 32))
                    ));
            }

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}