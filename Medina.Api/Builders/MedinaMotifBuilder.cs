using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Grasshopper;
using Grasshopper.Kernel;
using GH_IO.Serialization;

namespace Medina.Api
{
    public class MedinaMotifBuilder
    {
        public string Name { get; set; }
        public GH_Archive GhArchive { get; set; }
        public GH_Document GhDoc { get; set; }
        public List<IGH_Param> Inputs { get; set; } = new List<IGH_Param>();
        public IGH_Param Output { get; set; }

        public MedinaMotifBuilder()
        {
            
        }

        public MedinaMotifBuilder CreateMotif(string name)
        {
            Name = name;

            return this;
        }

        public MedinaMotifBuilder FromGhx(string path)
        {
            var ghxText = System.IO.File.ReadAllText(path);

            GhArchive = new GH_Archive();
            GhArchive.Deserialize_Xml(ghxText);
            GhDoc = new GH_Document();
            GhArchive.ExtractObject(GhDoc, "Definition");

            return this;
        }

        public MedinaMotifBuilder FindInputs()
        {
            foreach (var obj in GhDoc.Objects)
            {
                var param = obj as IGH_Param;
                if (param == null) continue;

                if (param.NickName == "output_main")
                {
                    Output = param;
                }

                if (param.Sources.Count == 0 && param.Recipients.Count != 0 && param.NickName.Length > 1)
                {
                    //Console.WriteLine($"Primary input named {param.NickName} discovered!");
                    Inputs.Add(param);
                }
                else if (param.NickName != null || param.NickName.Length > 1)
                {                  
                    //Console.WriteLine($"Param {param.NickName} skipped. ({param.SourceCount.ToString()} sources / {param.Recipients.Count} recipients)");
                }
            }

            return this;
        }

        public MedinaMotifBuilder FindOutputs()
        {

            return this;
        }

        public static implicit operator MedinaMotif(MedinaMotifBuilder builder)
        {
            return new MedinaMotif(builder);
        }
    }
}