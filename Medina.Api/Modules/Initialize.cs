using System;

using Nancy;
using Newtonsoft.Json;

using GH_IO.Serialization;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.PlugIns;
using Grasshopper;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Special;

namespace Medina.Api.Modules
{
    public class Initialize : NancyModule
    {
        public Initialize()
        {
            After.AddItemToEndOfPipeline((ctx) =>
            {
                ctx.Response.WithHeader("Access-Control-Allow-Origin", "*")
                    .WithHeader("Access-Control-Allow-Methods", "POST,GET")
                    .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type");
            });

            Get["/"] = parameters =>
            {
                return "handshake successful";
            };

            Get["/init/site"] = parameters =>
            {
                //Do the logic.

                return JsonConvert.SerializeObject(Medina.LoadSite(@"G:\My Drive\medina\protocol_site_v0.5.0.3dm"));
            };

            Get["/init/motifs"] = parameters => TestMotifs(Context);
            //{
                //Please work I beg you

                //return JsonConvert.SerializeObject(Medina.LoadMotifs(@"G:\My Drive\medina\motifs\pi_test.ghx"));
            //};           
        }

        static Response TestMotifs(NancyContext ctx)
        {
            var archive = new GH_Archive();

            var ghxText = System.IO.File.ReadAllText(@"G:\My Drive\medina\motifs\Dome_Construct.ghx");

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

                if (param.Sources.Count == 0 && param.Recipients.Count != 0)
                {
                    Console.WriteLine($"Primary input named {param.NickName} discovered!");
                }
                else if (param.NickName != null || param.NickName.Length > 1)
                {
                    Console.WriteLine($"Param {param.NickName} skipped. ({param.SourceCount.ToString()} sources / {param.Recipients.Count} recipients)");
                }
            }

            return null;
        }
    }
}