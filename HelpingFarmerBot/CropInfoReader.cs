using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using HtmlAgilityPack;
using System.Net;

namespace HelpingFarmerBot
{
    public class CropInfoReader
    {
        private static string CropPricesJson = Path.Combine(Environment.CurrentDirectory, @"Data\", "output.json");
        private static string CountryCurrencyIdJson = Path.Combine(Environment.CurrentDirectory, @"Data\", "countries.json");

        private Dictionary<string, CropInfo> crops = null;
        private Dictionary<string, string> currencyIds = null;
        public CropInfoReader()
        {
            populateCropObjects(CropPricesJson);
            saveCurrencyConversion();
        }

        public CropInfo GetCropInfo(string crop, string countryId)
        {
            float conversion = 1f;

            if (!string.IsNullOrEmpty(countryId))
            {
                var currencyId = currencyIds[countryId];
                var key = $"USD_{currencyId}";
                string conversionUrl = $"http://free.currencyconverterapi.com/api/v5/convert?q={key}&compact=y";

                using (WebClient wc = new WebClient())
                {
                    var json = wc.DownloadString(conversionUrl);
                    var jobj = JObject.Parse(json);
                    conversion = float.Parse(jobj[key]["val"].ToString());
                }
            }
            
            var convertedCrop = crops[crop.ToLower()];
            convertedCrop.avgPrice = convertedCrop.avgPrice * conversion;
            convertedCrop.lowPrice = convertedCrop.lowPrice * conversion;
            convertedCrop.highPrice = convertedCrop.highPrice * conversion;
            return convertedCrop;  
        }

        private void saveCurrencyConversion()
        {
            currencyIds = new Dictionary<string, string>();
            using (StreamReader reader = File.OpenText(CountryCurrencyIdJson))
            {
                JObject jobj = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
                var results = jobj["results"];
                foreach (var countries in jobj)
                {
                    foreach (var country in countries.Value)
                    {
                        var info = country.First;
                        currencyIds.Add(info["id"].ToString(), info["currencyId"].ToString());
                    }
                }
            }
        }

        private void populateCropObjects(string filePath)
        {
            crops = new Dictionary<string, CropInfo>();
            using (StreamReader reader = File.OpenText(filePath))
            {
                JObject jobj = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
                foreach (var clist in jobj)
                {
                    var info = clist.Value["fieldDataCollection"];
                    if (info != null)
                    {
                        foreach (var item in info)
                        {
                            CropInfo crop;
                            crop.name = item["shortName"].ToString();
                            if (item["price"] != null && item["highPrice"] != null && item["lowPrice"] != null && item["priceDate"] != null)
                            {
                                crop.avgPrice = float.Parse(item["price"].ToString());
                                crop.lowPrice = float.Parse(item["lowPrice"].ToString());
                                crop.highPrice = float.Parse(item["highPrice"].ToString());
                                crop.date = DateTime.Parse(item["priceDate"].ToString());
                                crops.Add(crop.name.ToLower(), crop);
                            }
                            
                        }
                    }
                    
                }
            }
        }

    }

    
}