using System;

namespace HelpingFarmerBot
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
        Corn, Cocoa, Wheat, Oats, RoughRice, Soybean, SoybeanMeal, SoybeanOil, Canola
    }

    public static class CropExtensions
    {
        public static string toString(this Crop crop)
        {
            switch (crop)
            {
                case Crop.Corn:
                    return "Corn";
                case Crop.Cocoa:
                    return "Cocoa";
                case Crop.Wheat:
                    return "Wheat";
                case Crop.Oats:
                    return "Oats";
                case Crop.RoughRice:
                    return "Rough Rice";
                case Crop.Soybean:
                    return "Soybean";
                case Crop.SoybeanMeal:
                    return "Soybean Meal";
                case Crop.SoybeanOil:
                    return "Soybean Oil";
                case Crop.Canola:
                    return "Canola";
                default:
                    return null;
            }
        }
    }
}