using AuthorizationService.ApplicationLogic;
using AuthorizationService.ApplicationLogic.Interfaces;
using AuthorizationService.ApplicationLogic.Services;
using AuthorizationService.DAL.Context;
using AuthorizationService.DAL.Entities;
using AuthorizationService.DAL.Interfaces;
using AuthorizationService.DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AuthorizationService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = Constants.Issuer,
                    ValidateAudience = true,
                    ValidAudience = Constants.Audience,
                    ValidateLifetime = true,
                    IssuerSigningKey = Constants.Key,
                    ValidateIssuerSigningKey = true
                };
            });
           

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AuthServiceDbContext>(optionBuiled => optionBuiled
            .UseSqlServer(connectionString));

            builder.Services.AddScoped(typeof(IUserService), typeof(UserService));
            builder.Services.AddScoped(typeof(IRepository<User>), typeof(BaseRepository<User>));
            builder.Services.AddScoped(typeof(IRepository<RefreshToken>), typeof(BaseRepository<RefreshToken>));

            var app = builder.Build();
            app.UseAuthentication();
            app.UseAuthorization();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.MapControllers();
            app.Run();
        }
    }
}