using System;
using System.Collections.Generic;
using System.Text;

namespace VBP_ParkingApp.Classes
{
    internal class NumberPlate
    {
        string numberPlateNr;
        string imgUrl;

        public NumberPlate(string numberPlateNr, string imgUrl)
        {
            this.numberPlateNr = numberPlateNr;
            this.imgUrl = imgUrl;
        }

        public string NumberPlateNr { get => numberPlateNr; set => numberPlateNr = value; }
        public string ImgUrl { get => imgUrl; set => imgUrl = value; }
    }
}
