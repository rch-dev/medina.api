using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.FileIO;

namespace Medina.Api
{
    public static class Medina
    {
        public static MedinaSite Initialize()
        {
            var site = new MedinaSite();

            site.Sectors = LoadSite(@"G:\My Drive\medina\protocol_site_v0.5.0.3dm");

            foreach (var sector in site.Sectors)
            {
                Console.WriteLine($"Loaded {sector.ToString()}");
            }

            return site;
        }

        private static List<MedinaSiteSector> LoadSite(string path)
        {
            var file = File3dm.Read(path);

            Console.WriteLine($"Reading {path.Split('\\').Last().Replace(".3dm", "")} with {file.Objects.Count.ToString()} objects...");

            var sectors = new List<MedinaSiteSector>();

            //Read all objects and group by sector.
            var objectDictionary = new Dictionary<string, List<File3dmObject>>();

            foreach (var obj in file.Objects)
            {
                if (obj.Name != null)
                {
                    objectDictionary.TryGetValue(obj.Name, out var res);

                    if (res != null)
                    {
                        //Add object.
                        objectDictionary[obj.Name].Add(obj);
                    }
                    else
                    {
                        //Initialize object list.
                        objectDictionary[obj.Name] = new List<File3dmObject>();
                        objectDictionary[obj.Name].Add(obj);
                    }

                    //Console.WriteLine($@"{file.AllLayers.FindIndex(obj.Attributes.LayerIndex).Name} object @ {obj.Name}");
                }        
            }

            foreach (var key in objectDictionary.Keys.Select(x => x.ToString()))
            {
                var builder = new MedinaSiteSectorBuilder();

                var sector = builder.CreateSector(key)
                    .WithObjects(objectDictionary[key]);

                sectors.Add(sector);
            }

            return sectors;
        }
    }
}