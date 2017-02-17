using System.Text.RegularExpressions;

namespace Garage2._5.Helper
{
    public class RegistrationNormalizer
    {
        public static string NormalizeForStorage(string registration) => Regex.Replace(registration, "\\s", "").ToUpper();

        public static string NormalizeForDisplay(string registration) => NormalizeForStorage(registration).Insert(3, " ");
    }
}