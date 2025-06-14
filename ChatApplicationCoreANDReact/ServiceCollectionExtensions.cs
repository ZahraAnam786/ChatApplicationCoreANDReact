using Application.Interfaces;
using Application.Mappings;
using Application.Services;
using Domain.Interfaces;
using Domain.UnitOfWork;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApplicationCoreANDReact
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection service)
        {
            service.AddScoped<IUnitOfWorkAsync, UnitOfWork>();

            service.AddScoped(typeof(IRepositoryAsync<>), typeof(Repository<>));

            service.AddScoped(typeof(IService<>), typeof(Service<>));

            service.AddScoped<IProductService, ProductService>();

            service.AddScoped<IProductRepository, ProductRepository>();

            service.AddScoped<IUserService, UserService>();

            service.AddScoped<IUserMessagesService, UserMessagesService>();

            service.AddAutoMapper(typeof(MappingProfile).Assembly);

            return service;
        }
    }
}
