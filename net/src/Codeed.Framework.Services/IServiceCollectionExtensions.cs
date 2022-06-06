using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Codeed.Framework.Services
{
    public static class IServiceCollectionExtensions
    {
        public static IEnumerable<Assembly> GetAllProjectsAssemblies(this IServiceCollection services, string assemblyPattern)
        {
            return Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, $"{assemblyPattern}*.dll")
                .Select(file => Assembly.Load(AssemblyName.GetAssemblyName(file)));
        }

        public static IServiceCollection GetAllProjectsAssemblies(this IServiceCollection services, string assemblyPattern, Action<IEnumerable<Assembly>> action)
        {
            var assemblies = services.GetAllProjectsAssemblies(assemblyPattern);
            action(assemblies);

            return services;
        }

        public static IServiceCollection RegisterClassesThatInherit<T>(this IServiceCollection services, ServiceLifetime serviceLifetime)
        {
            return services.RegisterClassesThatInherit<T>(typeof(T).Assembly, serviceLifetime);
        }

        public static IServiceCollection RegisterClassesThatInherit<T>(this IServiceCollection services, Assembly assembly, ServiceLifetime serviceLifetime)
        {
            var types = assembly.GetTypes()
                                .Where(type => typeof(T).IsAssignableFrom(type) && !type.IsAbstract);

            foreach (var type in types)
            {
                switch (serviceLifetime)
                {
                    case ServiceLifetime.Singleton:
                        services.AddSingleton(typeof(T), type);
                        break;
                    case ServiceLifetime.Transient:
                        services.AddTransient(typeof(T), type);
                        break;
                    default:
                        services.AddScoped(typeof(T), type);
                        break;
                }
            }

            return services;
        }
        public static IServiceCollection RegisterServicesFromAssemblies(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                services.RegisterServicesFromAssembly(assembly);
            }

            return services;
        }

        public static IServiceCollection RegisterServicesFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            var servicesCore = assembly.GetTypes().Where(IsService);
            foreach (var service in servicesCore)
            {
                services.AddScoped(service);
            }

            return services;
        }

        private static bool IsService(System.Type type)
        {
            return typeof(IServiceCore).IsAssignableFrom(type) &&
                !type.IsAbstract;
        }
    }
}
