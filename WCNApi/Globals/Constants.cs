
using Serilog;

namespace WCNApi.Globals
{
    public static class Constants
    {
    }

    public static class SeriLog
    {
        public static void Information(string step, string action, string message)
        {
            Log.Information($"STEP:[{step}] | {action} | {message}");
        }
        public static void Warning(string step, string action, string message)
        {
            Log.Warning($"STEP:[{step}] | {action} | {message}");
        }
        public static void Error(string step, Exception ex, int loggingEvents, string action, string message)
        {
            Log.Error(ex, $"STEP:[{step}] | [{loggingEvents}]{action} | {message}");
        }
    }

   
}
