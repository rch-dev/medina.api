using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Special;
using GH_IO.Serialization;

namespace Medina.Api
{
    public class MedinaMotif
    {
        public string Name { get; set; }
        public GH_Archive GhArchive { get; set; }
        public GH_Document GhDoc { get; set; }
        public List<MedinaMotifInput> Inputs { get; set; } = new List<MedinaMotifInput>();
        public List<MedinaMotifOutput> Outputs { get; set; } = new List<MedinaMotifOutput>();

        public MedinaMotif(string name, string ghxText)
        {
            Name = name;

            GhArchive = new GH_Archive();
            GhArchive.Deserialize_Xml(ghxText);
            GhDoc = new GH_Document();
            GhArchive.ExtractObject(GhDoc, "Definition");
        }

        public override string ToString()
        {
            return $"Motif {Name} ({GhDoc.ObjectCount.ToString()} objects)";
        }

        public void Audit()
        {
            Console.WriteLine(this.ToString());

            Console.WriteLine($"   {Inputs.Count.ToString()} inputs");
            Console.WriteLine($"   {Outputs.Count.ToString()} outputs");
        }
    }
}