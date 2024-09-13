
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
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("default");
            builder.Services.AddDbContext<AppointmentDbContext>(option => option.UseSqlServer(connectionString));

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Adding Jwt Bearer for Verifying Token
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

            // Add CORS services
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    policyBuilder =>
                    {
                        policyBuilder.AllowAnyOrigin()
                                     .AllowAnyMethod()
                                     .AllowAnyHeader();
                    });
            });

            //add depencies to inject
            builder.Services.AddTransient<AppointmentDbContext>();
            builder.Services.AddTransient<IAppointmentRepo,AppointmentRepo>();
            builder.Services.AddTransient<IAppointmentManager,AppointmentManager>();
            builder.Services.AddTransient<IMessageService, MessageService>();

            builder.Services.AddAutoMapper(typeof(AppointmentMappingProfile));

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseCors("AllowSpecificOrigin");
            

            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}
