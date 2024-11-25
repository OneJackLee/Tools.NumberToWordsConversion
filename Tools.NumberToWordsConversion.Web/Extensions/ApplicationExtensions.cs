using System.Reflection;
using Tools.NumberToWordsConversion.Application.Kernel;

namespace Tools.NumberToWordsConversion.Web.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var types = typeof(ISingletonService).Assembly.GetTypes()
            .Where(x => x.IsClass && !x.IsAbstract)
            .ToList();

        return AddDependencies(services, types);
    }

    private static IServiceCollection AddDependencies(IServiceCollection services, List<Type> types)
    {
        foreach (var type in types.Where(type => type.IsAssignableTo(typeof(ISingletonService))))
        {
            RegisterSingleton(services, type);
            
            // May extend to support Transient and Scoped service if needed.
        }

        return services;
    }

    private static void RegisterSingleton(IServiceCollection services, Type type)
    {
        services.AddSingleton(type);

        var interfaces = type.GetInterfaces();
        foreach (var interfaceType in interfaces)
        {
            var interfaceName = interfaceType.Name[1..];
            if (type.Name.EndsWith(interfaceName, StringComparison.InvariantCultureIgnoreCase))
                services.AddSingleton(interfaceType, type);
        }
    }
}