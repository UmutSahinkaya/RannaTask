using RannaTask.DAL;
using RannaTask.Business.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using RannaTask.Business.Helpers;

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
                    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyHeader();
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

            builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "RannaTaskAPI", Version = "v1" }));

            builder.Services.AddDALRegistiration(builder.Configuration);

            builder.Services.AddBusinessServices();

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RannaTaskAPI v1"));
            }
            // Configure the HTTP request pipeline.

            app.UseCors("AllowAllOrigins");
            app.UseAuthentication();
            app.UseAuthorization();
            

            app.MapControllers();

            app.Run();
        }
    }
}
