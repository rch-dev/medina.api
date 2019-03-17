using Nancy;
using Newtonsoft.Json;

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

                return JsonConvert.SerializeObject(Medina.Initialize());
            };

            Get["/init/motifs"] = parameters =>
            {
                //Please work I beg you

                return JsonConvert.SerializeObject(Medina.LoadMotifs());
            };
        }
    }
}