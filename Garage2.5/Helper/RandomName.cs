using System;
using System.Linq;
using System.Text;

namespace Garage2._5.Helper
{
    public class RandomName
    {
        private readonly string[] femaleNames =
        {
            #region Female Names
            "Maria",
            "Anna",
            "Margareta",
            "Elisabeth",
            "Eva",
            "Kristina",
            "Birgitta",
            "Karin",
            "Elisabet",
            "Marie",
            "Ingrid",
            "Christina",
            "Linnéa",
            "Sofia",
            "Kerstin",
            "Marianne",
            "Lena",
            "Helena",
            "Emma",
            "Inger",
            "Johanna",
            "Linnea",
            "Sara",
            "Cecilia",
            "Elin"
            #endregion
        };

        private readonly string[] lastNames =
        {
            #region Last Names
            "Johansson",
            "Andersson",
            "Karlsson",
            "Nilsson",
            "Eriksson",
            "Larsson",
            "Olsson",
            "Persson",
            "Svensson",
            "Gustafsson",
            "Pettersson",
            "Jonsson",
            "Jansson",
            "Hansson",
            "Bengtsson",
            "Jönsson",
            "Carlsson",
            "Petersson",
            "Lindberg",
            "Magnusson",
            "Lindström",
            "Gustavsson",
            "Olofsson",
            "Lindgren",
            "Axelsson"
            #endregion
        };

        private readonly string[] maleNames =
        {
            #region Male Names
            "Erik",
            "Lars",
            "Karl",
            "Anders",
            "Johan",
            "Per",
            "Nils",
            "Carl",
            "Mikael",
            "Jan",
            "Lennart",
            "Hans",
            "Olof",
            "Peter",
            "Gunnar",
            "Sven",
            "Fredrik",
            "Bengt",
            "Bo",
            "Daniel",
            "Åke",
            "Gustav",
            "Göran",
            "Alexander",
            "Magnus"
            #endregion
        };

        private readonly Random random = new Random();

        public string Next()
        {
            var sb = new StringBuilder();
            sb.Append(random.Next() % 2 == 0
                ? femaleNames[random.Next(femaleNames.Length)]
                : maleNames[random.Next(maleNames.Length)]);
            sb.Append(" ");
            sb.Append(lastNames[random.Next(lastNames.Length)]);
            return sb.ToString();
        }
    }
}