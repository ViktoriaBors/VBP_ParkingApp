using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VBP_ParkingApp.Classes
{
    public class StaticData
    {
        internal static ParkingPeriod currentParking = null;
        public static string chosenNumberPlate = string.Empty;
        public static List<string> numberPlates = new List<string>();
        public static List<string> images = new List<string>() {"cars.jpg", "cars1.jpg", "cars2.jpg", "cars3.jpg", "cars4.jpg" };

        internal static List<NumberPlate> numberPlatesList = new List<NumberPlate>();

        public static string dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "data.txt");

        internal static List<string> SavedNumberPlates()
        {
            return File.ReadAllLines(StaticData.dataPath).ToList();
        }

        internal static void NumberPlateListRefresh()
        {
            if (File.Exists(StaticData.dataPath))
            {
                numberPlates.Clear();
                numberPlates = SavedNumberPlates();
               
                numberPlatesList.Clear();
                for (int i = 0; i < numberPlates.Count; i++)
                {
                    numberPlatesList.Add(new NumberPlate(numberPlates[i], images[i]));
                }
            }
               
        }



    }
}
