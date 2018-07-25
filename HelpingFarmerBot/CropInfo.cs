using System;

namespace HelpingFarmerBot
{
    public class CropInfo
    {
        public string name;
        public float lowPrice;
        public float avgPrice;
        public float highPrice;
        public DateTime date;
        public string commodityUnit;
        public string currencySymbol;

        public string toString() 
        {
            return $"{this.name} price today - low: {this.currencySymbol}{this.lowPrice} per {this.commodityUnit} average: {this.currencySymbol}{this.avgPrice} per {this.commodityUnit} high: {this.currencySymbol}{this.highPrice} per {this.commodityUnit}";
        }
    }

    public enum Crop
    {
        Corn, Cocoa, Wheat, Oats, RoughRice, Soybean, SoybeanMeal, SoybeanOil, Canola
    }

    public static class CropExtensions
    {
        public static string PrintAllCrops()
        {
            string ret = "";
            var values = Enum.GetValues(typeof(Crop));
            foreach (Crop crop in values)
            {
                if (crop.Equals(values.GetValue(values.Length-1)))
                {
                    ret += $"or {CropExtensions.toString(crop)}";
                }
                else
                {
                    ret += $"{CropExtensions.toString(crop)}, ";
                }
            }
            return ret;
        }
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