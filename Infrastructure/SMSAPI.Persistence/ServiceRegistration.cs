using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SMSAPI.Application.Repositories;
using SMSAPI.Persistence.Contexts;
using SMSAPI.Persistence.Repositories;

namespace SMSAPI.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<StockDbContext>(options =>
                options.UseSqlServer(Configuration.ConnectionString));

            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<ICarPartRepository, CarPartRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
        }
    }
}
