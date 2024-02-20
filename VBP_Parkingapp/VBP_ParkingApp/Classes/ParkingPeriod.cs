using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace VBP_ParkingApp.Classes
{
    class ParkingPeriod
    {
        int id;
        DateTime startTime;
        DateTime endTime;
        int price;
        string numberPlate;

        public ParkingPeriod(int id, DateTime startTime, DateTime endTime, int price, string numberPlate) // finished parking reading from database - PLATENUMBER???
        {
            this.id = id;
            this.startTime = startTime;
            this.endTime = endTime;
            this.price = price;
            // this.numberPlate = plateNumber;
        }

        public ParkingPeriod(DateTime startTime, string numberPlate) // "local" parking object - currentParking - timer
        {
            this.startTime = startTime;
            this.numberPlate = numberPlate;
        }

        public int Id { get => id; set => id = value; }
        public DateTime StartTime { get => startTime; set => startTime = value; }
        public DateTime EndTime { get => endTime; set => endTime = value; }


        public int Price 
        { 
            get => price;
            set 
            {
                price = (EndTime.Subtract(StartTime).Minutes) * 8;
            }
            
        }
        public string NumberPlate 
        { 
            get => numberPlate;
            set 
            {
                string plateNumberPatternOld = @"^[A-Za-z]{3}\d{3}$";
                string plateNumberPatternNew = @"^[A-Za-z]{4}\d{3}$";
                Regex regex1 = new Regex(plateNumberPatternOld);
                Regex regex2 = new Regex(plateNumberPatternNew);

                if (regex1.IsMatch(value) || regex2.IsMatch(value))
                {
                    numberPlate = value;
                }
                else
                {
                    throw new ArgumentException("The platenumber format doesn't match with the hungarian platenumber format (AAA-000 or AAAA-000).");
                }                
            }
        }

        public override string ToString()
        {
            return $"{NumberPlate} - {StartTime} - {EndTime}";
        }
    }
}
