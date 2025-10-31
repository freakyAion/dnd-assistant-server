using dnd_assistant.DB;
using dnd_assistant.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
// using dnd_assistant.Data;

namespace dnd_assistant
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());

            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddDbContext<MyDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseCors("AllowFrontend");
            app.UseStaticFiles();


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
