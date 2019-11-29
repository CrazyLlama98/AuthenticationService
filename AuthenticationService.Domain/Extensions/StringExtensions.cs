namespace AuthenticationService.Domain.Extensions
{
    public static class StringExtensions
    {
        public static string ToCamelCaseFromPascalCase(this string text)
        {
            return char.ToLowerInvariant(text[0]) + text.Substring(1);
        }
    }
}
