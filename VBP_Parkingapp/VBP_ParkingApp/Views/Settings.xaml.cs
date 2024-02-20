using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VBP_ParkingApp.Classes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VBP_ParkingApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        public Settings()
        {
            InitializeComponent();
            StaticData.NumberPlateListRefresh();
            UI_Update();
        }

        private void UI_Update()
        {
            NumberPlateLV.ItemsSource = StaticData.numberPlates;
            if (StaticData.numberPlates.Count >= 1) // there are saved platenumbers
            {
                NumberPlateLV.IsVisible = true;
                noNrPlLbl.IsVisible = false;
                countNrPlLbl.IsVisible = true;
                countNrPlLbl.Text = $"Saved number plates: {StaticData.numberPlates.Count}";
            }
            else
            {
                NumberPlateLV.IsVisible = false;
                noNrPlLbl.IsVisible = true;
                countNrPlLbl.IsVisible = false;
            }
        }

        private void deleteBtn_Clicked(object sender, EventArgs e)
        { // delete chosen numberplate
            string chosenNumberPlate = (sender as Button).BindingContext.ToString();
            if (StaticData.chosenNumberPlate == chosenNumberPlate)
            {
                infoLbl.IsVisible = true;
                infoLbl.Text = "There is an ongoing parking on this number plate, therefore cannot be deleted.";
                return;
            }

            StaticData.numberPlates.Remove(chosenNumberPlate);
            try
            {
                File.WriteAllLines(StaticData.dataPath, SaveNumberPlatesString());
                infoLbl.Text = $"Number plate ({chosenNumberPlate}) was deleted.";
                StaticData.NumberPlateListRefresh();
                UI_Update();
            }
            catch (Exception ex)
            {
                infoLbl.IsVisible = true;
                infoLbl.Text = ex.Message;
            }
        }

        private void backBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new MainPage());
        }

        private void saveBtn_Clicked(object sender, EventArgs e)
        { // save new plate number - max 5 pieces
            string plateNumberPatternOld = @"^[A-Za-z]{3}\d{3}$";
            string plateNumberPatternNew = @"^[A-Za-z]{4}\d{3}$";
            Regex regex1 = new Regex(plateNumberPatternOld);
            Regex regex2 = new Regex(plateNumberPatternNew);

            if (NumberPlateEntry.Text == null)
            {
                infoLbl.IsVisible = true;
                infoLbl.Text = "There is no numberplate given.";
                return;
            }
            string cleanedInput = NumberPlateEntry.Text.Replace(" ", "").Replace("-", "").ToUpper();

            if (StaticData.numberPlates.Count >= 5)
            {
                infoLbl.IsVisible = true;
                infoLbl.Text = "Maximum 5 numberplate can be saved.";
                return;
            }

            if (StaticData.numberPlates.Contains(cleanedInput)) // alredy saved
            {
                infoLbl.IsVisible = true;
                infoLbl.Text = "The numberplate already exists";
                return;
            }

            if (regex1.IsMatch(cleanedInput) || regex2.IsMatch(cleanedInput)) // regex match 
            {
                infoLbl.IsVisible = false;
                StaticData.numberPlates.Add(cleanedInput);

                try
                {
                    File.WriteAllLines(StaticData.dataPath, SaveNumberPlatesString());
                    infoLbl.Text = $"Number plate ({cleanedInput}) is saved.)";
                    NumberPlateEntry.Text = string.Empty;
                    StaticData.NumberPlateListRefresh();
                    UI_Update();
                }
                catch (Exception ex)
                {
                    infoLbl.IsVisible = true;
                    infoLbl.Text = ex.Message;
                }

            }
            else
            {
                infoLbl.IsVisible = true;
                infoLbl.Text = "The number plate format doesn't match with the hungarian number plate format (AAA-000 or AAAA-000).";
            }
        }
    
        private string[] SaveNumberPlatesString()
        {
            string[] savedArray = new string[StaticData.numberPlates.Count];
            for (int i = 0; i < StaticData.numberPlates.Count; i++)
            {
                savedArray[i] = StaticData.numberPlates[i];               
            }
            return savedArray;
        }
    }
}