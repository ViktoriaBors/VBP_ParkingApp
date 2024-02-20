using Android.Content;
using Android.Content.Res;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

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

        public static string settingPath = "settings1.ini"; // "settings.ini";

        public static string connectionString = string.Empty;
        public static Context AppContext { get; set; }

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

        public static string Connection_Settings(Context context, string SettingIniPath)
        {
            List<string> lines = new List<string>();
            try
            {
                // Get a reference to the AssetManager
                AssetManager assets = context.Assets;

                // Open the file from the Assets folder
                using (StreamReader sr = new StreamReader(assets.Open(SettingIniPath)))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }

                Dictionary<string, string> connectionStringParams = new Dictionary<string, string>();

                foreach (string line in lines)
                {
                    string[] parts = line.Split('=');

                    if (parts.Length == 2)
                    {
                        connectionStringParams.Add(parts[0].Trim(), parts[1].Trim());
                    }
                }

                if (connectionStringParams.ContainsKey("server") &&
                    connectionStringParams.ContainsKey("uid") &&
                    connectionStringParams.ContainsKey("pwd") &&
                    connectionStringParams.ContainsKey("database"))
                {
                    return $"server={connectionStringParams["server"]};uid={connectionStringParams["uid"]};pwd={connectionStringParams["pwd"]};database={connectionStringParams["database"]}";
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (IOException ex)
            {
                // Handle IOException (e.g., file not found)
                // Log the error or handle it appropriately
                // For example:
                Console.WriteLine("Error reading file from Assets: " + ex.Message);
                return string.Empty;
            } 
        }



    }
}
