using dnd_assistant.DB;
using dnd_assistant.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

namespace dnd_assistant
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddSingleton<JWTService>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                return new JWTService(
                    key: configuration["Jwt:Key"]!,
                    issuer: configuration["Jwt:Issuer"]!,
                    audience: configuration["Jwt:Audience"]!
                );
            });

            builder.Services.AddDbContext<MyDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")),
                ServiceLifetime.Scoped);

            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            builder.Services.AddAuthorizationBuilder()
                .AddPolicy("Admin", policy => policy.RequireRole("Admin"))
                .AddPolicy("AdminOrModerator", policy => policy.RequireRole("Admin", "Moderator"))

                .AddPolicy("Self", policy => policy.RequireAssertion(context =>
                {
                    if (context.Resource is not HttpContext httpContext) return false;

                    if (!httpContext.Request.RouteValues.TryGetValue("id", out var routeIdObj)) return false;
                    string? routeId = routeIdObj?.ToString();

                    var userId = context.User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                                 ?? context.User.FindFirstValue(ClaimTypes.NameIdentifier);

                    return !string.IsNullOrEmpty(routeId) && string.Equals(userId, routeId, StringComparison.OrdinalIgnoreCase);
                }))

                .AddPolicy("AdminOrSelf", policy => policy.RequireAssertion(context =>
                {
                    var role = context.User.FindFirstValue(ClaimTypes.Role);
                    if (role == "Admin") return true;

                    if (context.Resource is not HttpContext httpContext) return false;

                    if (!httpContext.Request.RouteValues.TryGetValue("id", out var routeIdObj)) return false;
                    string? routeId = routeIdObj?.ToString();
                    if (string.IsNullOrEmpty(routeId)) return false;

                    var userId = context.User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                                 ?? context.User.FindFirstValue(ClaimTypes.NameIdentifier);

                    return string.Equals(userId, routeId, StringComparison.OrdinalIgnoreCase);
                }));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                };
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<MyDbContext>();
                db.Database.Migrate();
                dnd_assistant.Data.DbSeeder.Seed(db);
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowFrontend");
            app.UseStaticFiles();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}