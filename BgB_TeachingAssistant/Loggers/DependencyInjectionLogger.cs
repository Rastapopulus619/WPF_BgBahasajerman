using Microsoft.Extensions.DependencyInjection;
using static Bgb_DataAccessLibrary.Helpers.ExtensionMethods.StringExtensionMethods;
using System;

namespace BgB_TeachingAssistant
{
    public static class DependencyInjectionLogger
    {
        public static bool IsLoggingEnabled { get; set; } = true;

        public static void Log(string message)
        {
            if (IsLoggingEnabled)
            {
                Console.WriteLine(message);
            }
        }

        public static void LogStartup(IServiceCollection services)
        {
            if (!IsLoggingEnabled) return;

            // Define the list of service names to exclude
            var excludedServices = new HashSet<string>
            {
                "IHostingEnvironment",
                "IHostEnvironment",
                "HostBuilderContext",
                "IApplicationLifetime",
                "IHostApplicationLifetime",
                "IHostLifetime",
                "IHost",
                "IOptions`1",
                "IOptionsSnapshot`1",
                "IOptionsMonitor`1",
                "IOptionsFactory`1",
                "IOptionsMonitorCache`1",
                "IConfigureOptions`1",
                "ILoggerFactory",
                "ILogger`1",
                "IMeterFactory",
                "MetricsSubscriptionManager",
                "IStartupValidator",
                "IMetricListenerConfigurationFactory",
                "ILoggerProviderConfigurationFactory",
                "ILoggerProviderConfiguration`1",
                "IOptionsChangeTokenSource`1",
                "LoggingConfiguration",
                "ConsoleFormatter",
                "ILoggerProvider",
                "LoggingEventSource",
                "MetricsConfiguration"
            };

            Console.WriteLine("=== DI Service Registration ===");
            foreach (var service in services)
            {
                // Exclude services by their ServiceType.Name
                if (excludedServices.Contains(service.ServiceType.Name))
                {
                    continue;
                }
                ConsoleColor lifeTimeColor = service.Lifetime.ToString() == "Singleton" ? ConsoleColor.DarkRed:
                                             service.Lifetime.ToString() == "Scoped" ? ConsoleColor.DarkYellow :
                                             service.Lifetime.ToString() == "Transient" ? ConsoleColor.Yellow :
                                             ConsoleColor.White;

                ConsoleColor implementationColor =
                    (service.ImplementationType?.Name ?? "Factory/Instance") == "Factory/Instance"
                        ? ConsoleColor.DarkGray
                        : ConsoleColor.Cyan;

                // Determine HashCode if the instance is already created (e.g., singleton)
                string hashCodeInfo = service.Lifetime == ServiceLifetime.Singleton && service.ImplementationInstance != null
                    ? service.ImplementationInstance.GetHashCode().ToString()
                    : "N/A";

                ConsoleColor hashCodeColor = hashCodeInfo == "N/A"
                        ? ConsoleColor.DarkGray
                        : ConsoleColor.DarkYellow;

                ColorizeLine(
                    ("Reg. ", ConsoleColor.Magenta),
                    ($"{service.ServiceType.Name} -> ", ConsoleColor.DarkGray),
                    ($"{service.ImplementationType?.Name ?? "Factory/Instance"}", implementationColor),
                    ($"<{service.Lifetime}>", lifeTimeColor),
                    (", Hash: ", ConsoleColor.DarkGray),
                    ($"{hashCodeInfo}", hashCodeColor)
                );
            }
            Console.WriteLine("===============================");
        }

        public static void LogResolution(Type serviceType, object resolvedInstance)
        {
            if (!IsLoggingEnabled) return;

            if (resolvedInstance != null)
            {
                ColorizeLine(($"[DI] Resolved:", ConsoleColor.Magenta),($"{serviceType.Name} -> ", ConsoleColor.DarkGray),($"{resolvedInstance.GetType().Name}", ConsoleColor.White), ("| HashCode: ", ConsoleColor.DarkGray),($"{resolvedInstance.GetHashCode()}", ConsoleColor.DarkYellow));
                //Console.WriteLine($"[DI] Resolved: {serviceType.Name} -> Instance: {resolvedInstance.GetType().Name} | HashCode: {resolvedInstance.GetHashCode()}");
            }
            else
            {
                ColorizeLine(($"[DI] Failed to resolve: {serviceType.Name}", ConsoleColor.Magenta));
            }
        }
    }
}
