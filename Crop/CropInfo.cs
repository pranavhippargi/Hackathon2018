using System;

namespace HelpFarmerViaSMS
{
    public struct CropInfo
    {
        public string name;
        public float lowPrice;
        public float avgPrice;
        public float highPrice;
        public DateTime date;
    }

    public enum Crop
    {
        Corn, Cocoa, Wheat, Oats, Rough Rice, Soybean, SoybeanMeal, SoybeanOil, Canola
    }

    public static class CropExtensions
    {
        public static string toString(this Crop crop)
        {
            switch (crop)
            {
                case Corn:
                    return "corn";
                case Cocoa:
                    return "cocoa";
                case Wheat:
                    return "wheat";
                case Oats:
                    return "oats";
                case Rough:
                    return "rough";
                case Soybean:
                    return "soybean";
                case SoybeanMeal: 
                    return "soybeanMeal";
                case SoybeanOil:
                    return "soybeanOil";
                case Canola:
                    return "canola";
            }
        }
    }
}