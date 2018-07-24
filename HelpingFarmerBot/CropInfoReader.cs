using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace SimpleEchoBot.FarmingInfo
{
    public class CropInfoReader
    {
       
        private static string CropPricesJson = Path.Combine(Environment.CurrentDirectory, @"Data\", "output.json");

        private Dictionary<string, CropInfo> crops = null;
        public CropInfoReader()
        {
            populateCropObjects(CropPricesJson);
        }

        public CropInfo GetCropInfo(Crop crop)
        {
            return crops[crop.toString()];
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
                                crops.Add(crop.name, crop);
                            }
                            
                        }
                    }
                    
                }
            }
        }

    }

    
}