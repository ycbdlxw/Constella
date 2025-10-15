
using Serilog;
using Serilog.Events;

namespace myapp
{
    public static class LogHelper
    {
        public static void SetupSerilog()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();
        }
    }
}
