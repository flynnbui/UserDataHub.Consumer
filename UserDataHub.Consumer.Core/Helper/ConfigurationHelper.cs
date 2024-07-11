namespace UserDataHub.Consumer.Core.Helper
{
    public class ConfigurationHelper
    {
        public static string GetConfiguration(IConfiguration configuration, string key)
        {
            return configuration[key] ?? throw new ArgumentNullException($"{key} is not found");
        }
    }
}