using System.Text;
using CMD.Appointment.Data;
using CMD.Appointment.Domain;
using CMD.Appointment.Domain.IRepositories;
using CMD.Appointment.Domain.Manager;
using CMD.Appointment.Domain.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CMD.Appointment.ApiService
{
    /// <summary>
    /// The entry point for the application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main method for running the application.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure the database context with the connection string from configuration
            var connectionString = builder.Configuration.GetConnectionString("default");
            builder.Services.AddDbContext<AppointmentDbContext>(option => option.UseSqlServer(connectionString));

            // Add services to the container.
            builder.Services.AddControllers();

            // Configure Swagger/OpenAPI for API documentation
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            /// <summary>
            /// Adds JWT Bearer Authentication service.
            /// </summary>
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                    ClockSkew = TimeSpan.Zero
                };
            });

            // Register services for dependency injection
            builder.Services.AddTransient<AppointmentDbContext>();
            builder.Services.AddTransient<IAppointmentRepo, AppointmentRepo>();
            builder.Services.AddTransient<IAppointmentManager, AppointmentManager>();
            builder.Services.AddTransient<IMessageService, MessageService>();

            // Configure AutoMapper with the mapping profile
            builder.Services.AddAutoMapper(typeof(AppointmentMappingProfile));

            var app = builder.Build();

            //Configure Migration for Database programatically
            //try
            //{
            //    using (var scope = app.Services.CreateScope())
            //    {
            //        var db = scope.ServiceProvider.GetRequiredService<AppointmentDbContext>();
            //        db.ClearDatabase();
            //        db.Database.Migrate();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    // Log or handle migration errors
            //    throw new Exception($"An error occurred while migrating the database: {ex.Message}", ex);
            //}

            // Configure the HTTP request pipeline
            app.UseSwagger();
            app.UseSwaggerUI();

            // Enable CORS (Cross-Origin Resource Sharing)
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            });

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}
