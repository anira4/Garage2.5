using System;
using System.Text;

namespace Garage2._5.Helper
{
    public class RandomPhone
    {
        private readonly Random random = new Random();

        public string Next()
        {
            var sb = new StringBuilder();
            sb.Append("08-");
            for (int i = 0; i < 3; i++)
                sb.Append(random.Next(9).ToString());
            sb.Append(" ");
            for (int i = 0; i < 3; i++)
                sb.Append(random.Next(9).ToString());
            sb.Append(" ");
            for (int i = 0; i < 2; i++)
                sb.Append(random.Next(9).ToString());
            return sb.ToString();
        }
    }
}