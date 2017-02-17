using System.Globalization;
using System.Linq;
using System.Text;

namespace Garage2._5.Helper
{
    public static class StringExtensions
    {
        public static string RemoveDiacritics(this string str)
        {
            if (str == null)
                return null;
            var chars = str.Normalize(NormalizationForm.FormD)
                .ToCharArray()
                .Select(c => new {c, uc = CharUnicodeInfo.GetUnicodeCategory(c)})
                .Where(t => t.uc != UnicodeCategory.NonSpacingMark)
                .Select(t => t.c);
            var cleanStr = new string(chars.ToArray()).Normalize(NormalizationForm.FormC);

            return cleanStr;
        }
    }
}