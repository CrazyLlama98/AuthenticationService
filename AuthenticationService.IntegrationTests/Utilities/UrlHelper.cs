namespace AuthenticationService.IntegrationTests.Utilities
{
    public static class UrlHelper
    {
        private const string rootPath = "api";
        private const string currentVersion = "v1";

        public static class Post
        {
            public static string CreateAccount = $"{rootPath}/{currentVersion}/accounts";
        }
    }
}
