using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VBP_ParkingApp.Classes;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace VBP_ParkingApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Parking : ContentPage
    {
        int chosenNumberPlateIndex;
        bool parkingOnGoing;
        bool backToParking = false;
        public Parking()
        {
            InitializeComponent();
            StaticData.NumberPlateListRefresh();
         
            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
                if (StaticData.currentParking != null)
                {
                    Console.WriteLine(DateTime.Now);
                    currenParkingOnLbl.Text = ParkingTimeFormat(DateTime.Now);
                    return true;
                }
                else return false;
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            UI_Update();
        }


        private void startBtn_Clicked(object sender, EventArgs e)
        {
            if (!parkingOnGoing && StaticData.currentParking == null) // if there was not ongoing parking then start one
            {
                parkingOnGoing = true;
                chosenNumberPlateIndex = platenumberCV.Position;
                StaticData.chosenNumberPlate = StaticData.numberPlates[chosenNumberPlateIndex];
                StaticData.currentParking = new ParkingPeriod(DateTime.Now, StaticData.chosenNumberPlate);
                UI_Update();
                currenParkingOnLbl.Text = ParkingTimeFormat(DateTime.Now);

            } else if(StaticData.currentParking != null) // if there was ongoing parking then stop one
            {
                Console.WriteLine("Parking stopped");
                StaticData.currentParking.EndTime = DateTime.Now;
                StaticData.currentParking.Price = (StaticData.currentParking.EndTime.Subtract(StaticData.currentParking.StartTime).Minutes) * 8;

                currenParkingOnLbl.Text = "Details of Latest Parking:" + Environment.NewLine + StaticData.currentParking.ToString() + Environment.NewLine + ParkingTimeFormat(StaticData.currentParking.EndTime) + Environment.NewLine + "Price (8HUF/min base) : " + StaticData.currentParking.Price;
                startBtn.IsVisible = false;

                FunctionResult saveToDB = SqlDataHandler.FinishedParkingPeriodSave(StaticData.currentParking);
                if (!saveToDB.Result)
                {
                    currenParkingOnLbl.Text += Environment.NewLine + "There was a problem to save the latest parking period.";
                }

                StaticData.currentParking = null;
                StaticData.chosenNumberPlate = string.Empty;
                StaticData.chosenNumberPlate = string.Empty;
                parkingOnGoing = false;
                backToParking = true;
            }
           
        }

        private string ParkingTimeFormat(DateTime time)
        {
            string text = string.Empty;
            if (parkingOnGoing)
            {
                text = "The current parking is on for ";
            } else
            {
                text = "Lates parking period: ";
            }

            int seconds = time.Subtract(StaticData.currentParking.StartTime).Seconds;
            int minutes = time.Subtract(StaticData.currentParking.StartTime).Minutes;
            int hours = time.Subtract(StaticData.currentParking.StartTime).Hours;
            int days = time.Subtract(StaticData.currentParking.StartTime).Days;
            if (days >= 1)
            {
                text += days + " days and "+ hours +"  hours and ";
            }
            if (hours >= 1)
            {
                text += hours + " hours and ";
            } 
            if (minutes >= 1)
            {
                text += minutes + " minutes ";
            }
            text += seconds + " seconds.";


            return text;
        }

        private void backBtn_Clicked(object sender, EventArgs e)
        {
            if (backToParking && StaticData.currentParking != null)
            {
                UI_Update();
            } else Navigation.PopModalAsync();
        }

        private void settingsBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Settings());
        }

        private void UI_Update()
        {
            if (StaticData.currentParking != null)
            {
                currentParkingOnStackL.IsVisible = true;                
                numberPlateStackL.IsVisible = false;
                startBtn.Text = "Stop";
                ImageForCurrentParking.Source = StaticData.images[chosenNumberPlateIndex];
                currenParkingOnNumberPlateLbl.Text = StaticData.chosenNumberPlate;
                currenParkingOnLbl.Text = ParkingTimeFormat(DateTime.Now);

            } else
            {
                numberPlateStackL.IsVisible = true;
                currentParkingOnStackL.IsVisible = false;

                platenumberCV.ItemsSource = StaticData.numberPlatesList;                
                platenumberCV.IndicatorView = indicatorView;
                platenumberCV.Loop = false;

               
                if (StaticData.numberPlates.Count > 0)
                {
                    startBtn.IsEnabled = true;
                    startBtn.IsVisible = true;
                    startBtn.Text = "Start";
                    settingsBtn.IsEnabled = false;
                    settingsBtn.IsVisible = false;
                    platenumberCV.IsVisible = true;
                    infoLbl.IsVisible = false;

                }
                else// no saved platenumbers
                {
                    startBtn.IsEnabled = false;
                    startBtn.IsVisible = false;
                    startBtn.Text = "Start";
                    settingsBtn.IsEnabled = true;
                    settingsBtn.IsVisible = true;
                    // platenumberCV.IsVisible = false;
                    infoLbl.IsVisible = true;
                }
            }
           
        }
    }
}