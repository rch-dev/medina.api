using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Rhino.Geometry;

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
        public List<IGH_Param> Inputs { get; set; } = new List<IGH_Param>();
        public IGH_Param Output { get; set; }

        public MedinaMotif(string name, string ghxText)
        {
            Name = name;

            GhArchive = new GH_Archive();
            GhArchive.Deserialize_Xml(ghxText);
            GhDoc = new GH_Document();
            GhArchive.ExtractObject(GhDoc, "Definition");
        }

        public MedinaMotif(MedinaMotifBuilder builder)
        {
            Name = builder.Name;
            GhArchive = builder.GhArchive;
            GhDoc = builder.GhDoc;
            Inputs = builder.Inputs;
            Output = builder.Output;
        }

        public void StageSector(MedinaSiteSector sector)
        {
            throw new NotImplementedException();
        }

        public List<Brep> SolveAt(Point3d point)
        {
            throw new NotImplementedException();
        }
        
        public override string ToString()
        {
            return $"Motif {Name} ({GhDoc.ObjectCount.ToString()} objects)";
        }

        public void Audit()
        {
            Console.WriteLine(this.ToString());

            Console.WriteLine($"   {Inputs.Count.ToString()}/11 inputs loaded");
            Console.WriteLine($"   {(Inputs.Any(x => x.NickName == "INPUT_POINTS") ? "Anchor point input located" : "Anchor point input NOT located")}");
            Console.WriteLine($"   Output {(Output == null ? "unidentified" : "identified")}");
        }
    }
}