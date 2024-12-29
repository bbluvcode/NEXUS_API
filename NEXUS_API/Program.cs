
using Microsoft.EntityFrameworkCore;
using NEXUS_API.Controllers;
using NEXUS_API.Data;
using NEXUS_API.Repository;
using NEXUS_API.Service;

namespace NEXUS_API
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
            builder.Services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectDB"));
            });
            builder.Services.AddScoped<IEmployeeRepository, EmployeeService>();
            builder.Services.AddScoped<IRegionRepository, RegionService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();
            //var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<DatabaseContext>();
            //SeedData.SeedingData(context);
            app.Run();
        }
    }
}
