using Domain.Models.Settings;
using Infrastructure.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using UseFul.IoC;

namespace AuthenticationApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            DependencyResolve.Instance(builder.Services);

            var authSettingsSection = builder.Configuration.GetSection("AuthSettings");
            builder.Services.Configure<AuthSettings>(authSettingsSection);

            var authSettings = authSettingsSection.Get<AuthSettings>();
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.SecretKey));


            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddPolicyScheme("userapp", "Authorization Bearer or AccessToken", options =>
            {
                options.ForwardDefaultSelector = context =>
                {
                    if (context.Request.Headers["Access-Token"].Any())
                    {
                        return "Access-Token";
                    }

                    return JwtBearerDefaults.AuthenticationScheme;
                };
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = "userapp",

                    ValidateAudience = false,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,

                    // Verify if token is valid
                    ValidateLifetime = true,
                    RequireExpirationTime = true,

                };

            });


            builder.Services.AddDbContext<SintecProjectContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("UserAuthenticationApi")));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CustomerApi", Version = "v1" });

                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
                {
                    Description = "Beared token",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                c.OperationFilter<SecurityRequirementsOperationFilter>();
            });

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

            app.Run();
        }
    }
}