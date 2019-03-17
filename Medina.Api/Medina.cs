using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Special;
using GH_IO.Serialization;
using Rhino.FileIO;

namespace Medina.Api
{
    public static class Medina
    {
        public static string Initialize()
        {
            var site = new MedinaSite();

            site.Sectors = LoadSite(@"G:\My Drive\medina\protocol_site_v0.5.0.3dm");

            site.Motifs.Add(LoadMotif(@"G:\My Drive\medina\motifs\Arc_Construct.ghx", "arch"));
            site.Motifs.Add(LoadMotif(@"G:\My Drive\medina\motifs\Dome_Construct.ghx", "dome"));
            site.Motifs.Add(LoadMotif(@"G:\My Drive\medina\motifs\Fountain_Construct.ghx", "fountain"));

            foreach (var sector in site.Sectors)
            {
                sector.Audit();
            }

            foreach (var motif in site.Motifs)
            {
                motif.Audit();
            }

            return "ok";
        }

        public static List<MedinaSiteSector> LoadSite(string path)
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

                var sector = builder.CreateSector(file)
                    .WithId(key)
                    .WithObjects(objectDictionary[key])
                    .CategorizeObjects();

                sectors.Add(sector);
            }

            return sectors;
        }

        public static MedinaMotif LoadMotif(string path, string name)
        {
            var ghxText = System.IO.File.ReadAllText(path);

            var motif = new MedinaMotif(name, ghxText);

            return motif;
        }

        public static List<MedinaMotif> LoadMotifs(string path)
        {
            var archive = new GH_Archive();

            var ghxText = System.IO.File.ReadAllText(path);

            try
            {
                archive.Deserialize_Xml(ghxText);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            var definition = new GH_Document();

            archive.ExtractObject(definition, "Definition");

            Console.WriteLine($"{definition.ObjectCount.ToString()} objects in loaded definition.");

            foreach (var obj in definition.Objects)
            {
                var param = obj as IGH_Param;
                if (param == null) continue;

                if (param.Sources.Count == 0 && param.Recipients.Count != 0 && param.NickName.Length > 1)
                {
                    Console.WriteLine($"Primary input named {param.NickName} discovered!");
                }
                else if (param.NickName != null || param.NickName.Length > 1)
                {
                    //Console.WriteLine($"Param {param.NickName} skipped. ({param.SourceCount.ToString()} sources / {param.Recipients.Count} recipients)");
                }
            }         

            return null;
        }
    }
}