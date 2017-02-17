using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Garage2._5.Helper
{
    public class RegistrationVerifier
    {
        private readonly IReadOnlyList<string> bannedWords = new List<string>
        {
            #region BannedWords
            "APA",
            "ARG",
            "ASS",
            "BAJ",
            "BSS",
            "CUC",
            "CUK",
            "DUM",
            "DYR",
            "ETA",
            "ETT",
            "FAG",
            "FAN",
            "FEG",
            "FEL",
            "FEM",
            "FES",
            "FET",
            "FNL",
            "FUC",
            "FUK",
            "FUL",
            "GAM",
            "GAY",
            "GEJ",
            "GEY",
            "GHB",
            "GUD",
            "GYN",
            "GAS",
            "GET",
            "GLO",
            "GOM",
            "GUB",
            "GUC",
            "GUK",
            "HAT",
            "HBT",
            "HKH",
            "HOR",
            "HOT",
            "HAL",
            "HAN",
            "HAO",
            "HAR",
            "HAS",
            "HER",
            "HES",
            "HET",
            "HJO",
            "HMO",
            "HOM",
            "HON",
            "HRA",
            "HUD",
            "HUK",
            "HUS",
            "HUT",
            "JUG",
            "JUK",
            "JUO",
            "JUR",
            "KGB",
            "KKK",
            "KUC",
            "KUF",
            "KUG",
            "KUK",
            "KYK",
            "KDS",
            "LAM",
            "LAT",
            "LEM",
            "LOJ",
            "LSD",
            "LUS",
            "LUZ",
            "MAD",
            "MAO",
            "MEN",
            "MES",
            "MLB",
            "MUS",
            "MAS",
            "MUT",
            "NAZ",
            "NRP",
            "NSF",
            "NYP",
            "NEJ",
            "NJA",
            "NOS",
            "NUP",
            "NYS",
            "OND",
            "OOO",
            "ORM",
            "ORA",
            "OST",
            "OXE",
            "PAJ",
            "PKK",
            "PLO",
            "PMS",
            "PUB",
            "PAP",
            "PES",
            "PNS",
            "PRO",
            "PUC",
            "PUK",
            "PYS",
            "RAP",
            "RAS",
            "ROM",
            "RPS",
            "RUS",
            "REA",
            "RUG",
            "RUK",
            "SEG",
            "SEX",
            "SJU",
            "SOS",
            "SPY",
            "SUG",
            "SUP",
            "SUR",
            "SAB",
            "SAC",
            "SAF",
            "SAP",
            "SAT",
            "SEK",
            "SOP",
            "SSU",
            "SWE",
            "SYF",
            "TBC",
            "TOA",
            "TOK",
            "TRE",
            "TYP",
            "TAJ",
            "TOT",
            "UFO",
            "USA",
            "UCK",
            "UFF",
            "UPA",
            "USH",
            "WAM",
            "WAR",
            "WWW",
            "WTC",
            "XTC",
            "XTZ",
            "XXL",
            "XXX",
            "XUK",
            "ZEX",
            "ZOG",
            "ZPY",
            "ZUG",
            "ZUP",
            "ZOO"
            #endregion
        };

        private readonly char[] letters =
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'R', 'S', 'T', 'U', 'W', 'X', 'Y', 'Z'
        };

        public string LastErrorMessage { get; private set; }

        public bool Verify(string input)
        {
            LastErrorMessage = null;
            var result = Regex.IsMatch(Regex.Replace(input, "\\s", ""),
                "^[" + string.Join("|", letters) + "]{3}[0-9]{3}$");
            if (result)
            {
                var str = input.Substring(startIndex: 0, length: 3);
                result = !bannedWords.Contains(str);
                if (!result)
                    LastErrorMessage = $"'{str}' is not an allowed letter combination";
            }
            if (LastErrorMessage == null)
                LastErrorMessage = "Invalid format, expected 3 letters followed by 3 digits restricted to the following letters:\n" +
                    string.Join("", letters);
            return result;
        }
    }
}
