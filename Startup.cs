using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;

[assembly: FunctionsStartup(typeof(functionToAppInsights.Startup))]
namespace functionToAppInsights
{
    public class Startup:FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {                        
            builder.Services.AddSingleton<ILoggerProvider>((sp) => 
            {
                
                Log.Logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()                    
                    .WriteTo.ApplicationInsights(sp.GetRequiredService<TelemetryClient>(), TelemetryConverter.Traces)
                    .CreateLogger();
                return new SerilogLoggerProvider(Log.Logger, true);
            });
            //builder.Services.AddLogging(config => config.AddSerilog());
        }
    }
}