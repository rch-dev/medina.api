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

            Get["/init"] = parameters =>
            {
                return JsonConvert.SerializeObject(Medina.Initialize());
            };

            Get["/init/site"] = parameters =>
            {
                //Do the logic.

                return JsonConvert.SerializeObject(Medina.LoadSite(@"G:\My Drive\medina\protocol_site_v0.5.0.3dm"));
            };

            Get["/init/motifs"] = parameters =>
            {
                //Please work I beg you

                return JsonConvert.SerializeObject(Medina.LoadMotifs(@"G:\My Drive\medina\motifs\Arc_Construct.ghx"));
            };           
        }
    }
}