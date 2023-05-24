using System.Text;

namespace Host.Api.Extensions
{
    public static class StringExtensions
    {
        public static string ToErrorMessage(this IEnumerable<string> errors)
        {
            var sb = new StringBuilder();

            sb.Append("Erros: ");
            foreach (var error in errors)
            {
                sb.Append($"'{error}' ");
            }

            return sb.ToString();
        }
    }
}
