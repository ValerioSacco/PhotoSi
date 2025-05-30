using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PhotoSi.Shared.Behaviors;
using PhotoSi.UsersService.Services;
using System.Reflection;

namespace PhotoSi.UsersService.Features
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddFeatureServices(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
            });

            services.AddHttpClient<IAddressChecker, AddressChecker>();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
