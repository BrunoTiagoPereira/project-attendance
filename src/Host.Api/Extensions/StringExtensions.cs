using System.Text;

namespace Host.Api.Extensions
{
    public static class StringExtensions
    {
        public static string ToErrorMessage(this IEnumerable<string> errors)
        {
            var sb = new StringBuilder();

            sb.Append("Erros: ");

            sb.Append(string.Join(", ", errors));

            return sb.ToString();
        }

        public static bool IsNotNullOrWhiteSpace(this string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }
    }
}
