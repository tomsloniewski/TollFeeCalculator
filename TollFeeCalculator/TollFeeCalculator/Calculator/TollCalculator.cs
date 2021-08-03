using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using TollFeeCalculator.Calculator.Helpers;
using TollFeeCalculator.Calculator.Vehicles;

namespace TollFeeCalculator
{
    public class TollCalculator
    {
        /**
         * Calculate the total toll fee for one day
         * 
         * @param vehicle - the vehicle
         * @param dates - date and time of all passes on one day
         * @return - the total toll fee for that day
         */

        public int GetTollFee(IVehicle vehicle, DateTime[] dates)
        {
            DateTime intervalStart = dates[0];
            int totalFee = 0;

            foreach(DateTime date in dates)
            {
                int nextFee = GetTollFee(date, vehicle);
                int tempFee = GetTollFee(intervalStart, vehicle);

                double minutes = date.Subtract(intervalStart).TotalMinutes;

                if (minutes <= 60)
                {
                    if (totalFee > 0) totalFee -= tempFee;
                    if (nextFee >= tempFee) tempFee = nextFee;
                    totalFee += tempFee;
                }
                else
                {
                    totalFee += nextFee;
                }
            }

            if (totalFee > 60) totalFee = 60;

            return totalFee;
        }

        /**
         * Calculate the toll fee for one vehicle pass
         * 
         * @param date - date and time of the current pass of the vehicle
         * @param vehicle - the vehicle
         * @return - the toll fee for the particular pass
         */
        public int GetTollFee(DateTime date, IVehicle vehicle)
        {
            if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

            int hour = date.Hour;
            int minute = date.Minute;

            if (hour == 6 && minute >= 0 && minute <= 29) return 8;
            else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
            else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
            else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
            else if ((hour == 8 && minute >= 30 && minute < 59) 
                || (hour >= 9 && hour <= 14 && minute <= 59)) return 8;
            else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
            else if ((hour == 15 && minute >= 30 && minute < 59) 
                || (hour == 16 && minute <= 59)) return 18;
            else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
            else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
            else return 0;
        }

        /**
         * Checks if the vehicle should pay the toll the particular day
         * 
         * @param date - date and time of the current pass of the vehicle
         * @return - boolean value if the vehicle should pay
         */
        public bool IsTollFreeDate(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;

            // Checks if the date is Saturday or Sunday
            DayOfWeek dayOfWeek = new DateTime(year, month, day).DayOfWeek;
            if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday) return true;

            // Checks if the date is listed under freeDates.json
            string path = Directory.GetCurrentDirectory() + "\\Calculator\\Helpers\\freeDates.json";
            JObject jsonFile = JObject.Parse(File.ReadAllText(path));
            JToken dates;

            try
            {
                dates = jsonFile["dates"][year.ToString()][month.ToString()];
            }
            catch (Exception)
            {
                // the year/month entry was not found
                return false;
            }

            if (dates.Values().Any() is false) return true; // the month entry was found, empty array means the whole month is free
            else if (dates.Values().Contains(day.ToString())) return true; // the entry year/month/day was found
            else return false;
        }

        /**
         * Checks if the provided vehicle is free of charge
         * 
         * @param vehicle - the vehicle
         * @return - boolean value if the vehicle is free of charge
         */
        private bool IsTollFreeVehicle(IVehicle vehicle)
        {
            if (vehicle == null) return false;
            return vehicle is IFreeVehicle;
        }

        /**
         *  Toll free vehicles definition
         */
        private enum TollFreeVehicles
        {
            Motorbike,
            Tractor,
            Emergency,
            Diplomat,Foreign,
            Military
        }
    }
}
