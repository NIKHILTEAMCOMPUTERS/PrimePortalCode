using Microsoft.EntityFrameworkCore;
using RMS.BackgroundServices;
using RMS.Data.Models;
using RMS.Interfaces;
using RMS.Service;
using RMS.Service.Interfaces;
using RMS.Service.Interfaces.Extentions;
using RMS.Service.Repositories.Extentions;
using RMS.Services;
using System.Text.Json.Serialization;

namespace RMS.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddDbContext<RmsDevContext>(option =>
                option.UseNpgsql(config.GetConnectionString("RmsContext"))
            );

            services.AddControllers()
                    .AddJsonOptions(options =>
           options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
            });
            services.AddHttpContextAccessor();
            // services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddCors();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddSingleton<EmployeeDataUpdateService>();
            services.AddScoped<IEmployeeStatusService, EmployeeStatusService>();

            return services;
        }
    }
}
