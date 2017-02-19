using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Garage2._5.Models;


namespace Garage2._5.ViewModels
{
    public class ReceiptViewModel
    {
        [Display(Name = "Vehicle type")]
        public string Type { get; set; }

        public string Registration { get; set; }

        [Display(Name = "Parking space")]
        public string ParkingSpot { get; set; }

        [Display(Name = "Checkin time")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime CheckinTime { get; set; }

        [Display(Name = "Checkout time")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime CheckoutTime { get; set; }

        public TimeSpan TotalTime { get; set; }

        [Display(Name = "Time period")]
        public string TotalTimeString { get; set; }

        [Display(Name = "Price per minute")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal Price { get; set; }

        [Display(Name = "Total price")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal TotalPrice { get; set; }

        public void Update(Vehicle vehicle, DateTime checkoutTime, decimal pricePerMinute)
        {
            Type = vehicle.Type.Type;
            Registration = vehicle.Registration;
            ParkingSpot = vehicle.ParkingSpot;
            CheckinTime = vehicle.CheckinTime;
            CheckoutTime = checkoutTime;
            TotalTime = CheckoutTime - CheckinTime;

            var sb = new System.Text.StringBuilder();
            if (TotalTime.TotalDays >= 1)
                sb.AppendFormat("{0:F0} d ", TotalTime.TotalDays);
            if (TotalTime.Hours > 0)
                sb.AppendFormat("{0} hrs ", TotalTime.Hours);
            if (TotalTime.Minutes > 0)
                sb.AppendFormat("{0} min", TotalTime.Minutes);
            TotalTimeString = sb.ToString();

            Price = pricePerMinute;
            TotalPrice = pricePerMinute * (decimal)Math.Round(TotalTime.TotalMinutes);

        }
    }
}
