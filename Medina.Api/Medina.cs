using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.FileIO;

namespace Medina.Api
{
    public static class Medina
    {
        public static File3dm LoadSite()
        {
            var path = @"G:\My Drive\medina\protocol_site_v0.5.0.3dm";

            var file = File3dm.Read(path);

            foreach (var obj in file.Objects)
            {
                if (obj.Name != null)
                {
                    Console.WriteLine($@"{file.AllLayers.FindIndex(obj.Attributes.LayerIndex).Name} object @ {obj.Name}");
                }        
            }

            return file;
        }
    }
}