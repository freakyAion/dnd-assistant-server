using dnd_assistant.DB;
using dnd_assistant.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
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

            builder.Services.AddSingleton<JWTService>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();

                return new JWTService(
                    key: configuration["Jwt:Key"]!,
                    issuer: configuration["Jwt:Issuer"]!,
                    audience: configuration["Jwt:Audience"]!
                );
            });

            builder.Services.AddDbContext<MyDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);
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
                    var httpContext = (context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext)?.HttpContext ?? context.Resource as HttpContext;

                    if (httpContext == null) return false;

                    if (!httpContext.Request.RouteValues.TryGetValue("id", out var routeIdObj)) return false;

                    string? routeId = routeIdObj?.ToString();

                    var userId = context.User.FindFirstValue("sub") ?? context.User.FindFirstValue(ClaimTypes.NameIdentifier);

                    return routeId != null && userId == routeId;
                }))

                .AddPolicy("AdminOrSelf", policy =>
                policy.RequireAssertion(context =>
                {
                    var role = context.User.FindFirstValue(ClaimTypes.Role);
                    if (role == "Admin" || role == "Moderator") return true;

                    var mvcContext = context.Resource as AuthorizationFilterContext;
                    var httpContext = mvcContext?.HttpContext;

                    if (httpContext == null) return false;

                    if (!httpContext.Request.RouteValues.TryGetValue("id", out var routeIdObj)) return false;

                    string? routeId = routeIdObj?.ToString();
                    if (string.IsNullOrEmpty(routeId)) return false;

                    var userId = context.User.FindFirstValue(JwtRegisteredClaimNames.Sub ?? context.User.FindFirstValue(ClaimTypes.NameIdentifier));

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
                //ClassSeedData.Seed(db);
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
