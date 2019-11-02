namespace AuthenticationService.IntegrationTests.Utilities
{
    public static class UrlHelper
    {
        private const string rootPath = "api";

        public static class Post
        {
            public static string CreateAccount = $"{rootPath}/accounts";
        }
    }
}
