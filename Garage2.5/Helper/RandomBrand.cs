using System;

namespace Garage2._5.Helper
{
    public class RandomBrand
    {
        private readonly string[] brands =
        {
            #region Brands
            "Daihatsu",
            "Seat",
            "Saab",
            "Suzuki",
            "Audi",
            "AlfaRomeo",
            "Rolls-Royce",
            "Mitsubishi",
            "Volvo",
            "Maserati",
            "Dodge",
            "Jeep",
            "Renault",
            "Jaguar",
            "Citroën",
            "Mercedes",
            "Westfield",
            "Mini",
            "Ferrari",
            "Volkswagen",
            "Fiat",
            "Lexus",
            "Pagani",
            "Peugeot",
            "BMW"
            #endregion
        };

        private readonly Random random = new Random();

        public string Next()
        {
            return brands[random.Next(brands.Length)];
        }
    }
}