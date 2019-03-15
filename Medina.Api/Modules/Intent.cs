using Nancy;

namespace Medina.Api.Modules
{
    public class Intent : NancyModule
    {
        public Intent()
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
        }
    }
}