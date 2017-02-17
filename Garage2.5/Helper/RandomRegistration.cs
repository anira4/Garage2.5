using System;
using System.Text;

namespace Garage2._5.Helper
{
    public class RandomRegistration
    {
        private readonly char[] letters =
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'R', 'S', 'T', 'U', 'W', 'X', 'Y', 'Z'
        };

        private readonly Random random = new Random();
        private readonly RegistrationVerifier verifier = new RegistrationVerifier();

        public string Next()
        {
            while (true)
            {
                var sb = new StringBuilder();
                for (var i = 0; i < 3; i++)
                {
                    sb.Append(letters[random.Next(letters.Length)]);
                }
                for (var i = 0; i < 3; i++)
                {
                    sb.Append(random.Next(maxValue: 9));
                }
                var ret = sb.ToString();
                if (!verifier.Verify(ret))
                    continue;
                return ret;
            }
        }
    }
}
