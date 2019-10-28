namespace AuthenticationService.Utilities
{
    public interface IEnvironmentProvider
    {
        string GetConfigValue(string key);

        string GetConnectionString(string connectionName);
    }
}
