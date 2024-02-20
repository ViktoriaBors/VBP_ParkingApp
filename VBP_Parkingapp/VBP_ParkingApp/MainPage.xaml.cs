using Android.OS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VBP_ParkingApp.Classes;
using VBP_ParkingApp.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace VBP_ParkingApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            StaticData.NumberPlateListRefresh();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            startBtn.Text = StaticData.currentParking == null ? "Start" : "Resume";
        }      

        private void startBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Parking());
        }

        private void settingsBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Settings());
        }

        async private void getLocationBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                var location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromMinutes(1))); 
                // accuracy., how long we should try to find the location - after we time out - goes toe xception

                /*
                 locatoion - result
                accuracy, altitude, latitude, longitude, coarse - direction, speed, OpenMapAsync - laucn map tog et direction to certain location, calculatedistance (between 2 sets of lang-long)
                 */

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    locaResultLbl.Text = $"Latitude: {((float)location.Latitude)}, Longitude: {((float)location.Longitude)}, Altitude: {((float)location.Altitude)}";
                } else
                {
                    locaResultLbl.Text = "Couldn't determine location.";
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

    }
}
