using RannaTask.DAL;
using RannaTask.DAL.Contexts;
using RannaTask.Business.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using RannaTask.Business.Helpers;
using RannaTask.API.Hubs;

namespace RannaTask.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtTokenOptions"));

            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy("AllowAllOrigins", policy =>
                {
                    policy.AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials()
                          .SetIsOriginAllowed(_ => true); // For SignalR
                });
            });

            // Add services to the container.
            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                var jwtOptions=builder.Configuration.GetSection("JwtTokenOptions").Get<JwtOptions>();
                opt.RequireHttpsMetadata=false;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtTokenOptions:Key"])),
                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddAuthorization();
            builder.Services.AddControllers();
            builder.Services.AddHttpContextAccessor();

            // SignalR
            builder.Services.AddSignalR();

            // Swagger with JWT Authentication
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "RannaTask API", 
                    Version = "v1",
                    Description = "RannaTask Web API with JWT Authentication"
                });

                // JWT Authorization için Swagger'a güvenlik tanımı ekle
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n" +
                                  "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                                  "Example: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...\""
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[] {}
                    }
                });
            });

            builder.Services.AddDALRegistiration(builder.Configuration);
            builder.Services.AddBusinessServices();

            var app = builder.Build();

            // 🔥 AUTO MIGRATION: Production'da otomatik migration çalıştır
            if (app.Environment.IsProduction() || app.Environment.IsDevelopment())
            {
                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var context = services.GetRequiredService<AppDbContext>();
                        var logger = services.GetRequiredService<ILogger<Program>>();

                        logger.LogInformation("Applying database migrations...");
                        context.Database.Migrate();
                        logger.LogInformation("Database migrations applied successfully!");

                        // 🌱 SEED DATA: Admin kullanıcısı oluştur
                        logger.LogInformation("Checking seed data...");
                        RannaTask.API.Data.DbInitializer.Initialize(context, logger);
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred while migrating the database.");
                        throw; // Hata durumunda container'ı durdur
                    }
                }
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RannaTaskAPI v1"));
            }

            // Production'da da Swagger aktif (Docker test için)
            if (app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RannaTaskAPI v1"));
            }

            app.UseCors("AllowAllOrigins");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapHub<NotificationHub>("/notificationHub");

            app.Run();
        }
    }
}
