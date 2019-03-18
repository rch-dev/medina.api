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
            foreach (var input in Inputs)
            {
                var name = input.NickName;

                var path = new GH_Path(0);

                switch (name)
                {
                    case "BUILDING_MASS":               
                        for (int i = 0; i < sector.Massing.Count; i++)
                        {
                            var data = new GH_Brep(sector.Massing[i]);
                            input.AddVolatileData(path, i, data);
                        }
                        Console.WriteLine($"Param {name} staged successfully with {input.VolatileDataCount.ToString()} items.");
                        break;
                    case "FOOTPRINTS":
                        for (int i = 0; i < sector.Footprints.Count; i++)
                        {
                            var data = new GH_Brep(sector.Footprints[i]);
                            input.AddVolatileData(path, i, data);
                        }
                        Console.WriteLine($"Param {name} staged successfully with {input.VolatileDataCount.ToString()} items.");
                        break;
                    case "SLAB_DATUMS":
                        for (int i = 0; i < sector.Floors.Count; i++)
                        {
                            var data = new GH_Brep(sector.Floors[i]);
                            input.AddVolatileData(path, i, data);
                        }
                        Console.WriteLine($"Param {name} staged successfully with {input.VolatileDataCount.ToString()} items.");
                        break;
                    case "ROOF_DATUMS":
                        for (int i = 0; i < sector.Roofs.Count; i++)
                        {
                            var data = new GH_Brep(sector.Roofs[i]);
                            input.AddVolatileData(path, i, data);
                        }
                        Console.WriteLine($"Param {name} staged successfully with {input.VolatileDataCount.ToString()} items.");
                        break;
                    case "COURT_DATUMS":
                        for (int i = 0; i < sector.Courtyards.Count; i++)
                        {
                            var data = new GH_Brep(sector.Courtyards[i]);
                            input.AddVolatileData(path, i, data);
                        }
                        Console.WriteLine($"Param {name} staged successfully with {input.VolatileDataCount.ToString()} items.");
                        break;
                    case "PLAZA_DATUMS":
                        for (int i = 0; i < sector.Plazas.Count; i++)
                        {
                            var data = new GH_Brep(sector.Plazas[i]);
                            input.AddVolatileData(path, i, data);
                        }
                        Console.WriteLine($"Param {name} staged successfully with {input.VolatileDataCount.ToString()} items.");
                        break;
                    case "BALCONY_DATUMS":
                        for (int i = 0; i < sector.Balconies.Count; i++)
                        {
                            var data = new GH_Brep(sector.Balconies[i]);
                            input.AddVolatileData(path, i, data);
                        }
                        Console.WriteLine($"Param {name} staged successfully with {input.VolatileDataCount.ToString()} items.");
                        break;
                    case "DOORS":
                        for (int i = 0; i < sector.Doors.Count; i++)
                        {
                            var data = new GH_Brep(sector.Doors[i]);
                            input.AddVolatileData(path, i, data);
                        }
                        Console.WriteLine($"Param {name} staged successfully with {input.VolatileDataCount.ToString()} items.");
                        break;
                    case "WINDOWS":
                        for (int i = 0; i < sector.Windows.Count; i++)
                        {
                            var data = new GH_Brep(sector.Windows[i]);
                            input.AddVolatileData(path, i, data);
                        }
                        Console.WriteLine($"Param {name} staged successfully with {input.VolatileDataCount.ToString()} items.");
                        break;
                    case "RUINS_POINTS":
                        for (int i = 0; i < sector.RuinPoints.Count; i++)
                        {
                            var data = new GH_Point(sector.RuinPoints[i]);
                            input.AddVolatileData(path, i, data);
                        }
                        Console.WriteLine($"Param {name} staged successfully with {input.VolatileDataCount.ToString()} items.");
                        break;
                    default:
                        break;
                }
            }

            Console.WriteLine($"Sector {sector.Id.ToString()} staged for motif ({Name})");
        }

        public List<Brep> SolveAt(Point3d point)
        {
            var inputParam = Inputs.First(x => x.NickName == "INPUT_POINTS");

            var path = new GH_Path(0);
            var data = new GH_Point(point);
            inputParam.AddVolatileData(path, 0, data);

            //Console.WriteLine($"Param {inputParam.NickName} staged successfully with {inputParam.VolatileDataCount.ToString()} items.");

            //Console.WriteLine("Solving...");

            //Console.WriteLine($"Solving for param {Output.NickName}...");

            try
            {
                Output.Phase = GH_SolutionPhase.Blank;
                GhDoc.NewSolution(false);
                Output.CollectData();
                Output.ComputeData();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            //Console.WriteLine($"Solve finished with {Output.VolatileDataCount.ToString()} objects created.");

            var result = new List<Brep>();

            Console.WriteLine(Output.VolatileDataCount.ToString());
            Output.Sources.ToList().ForEach(x => Console.WriteLine(x.Name));

            for (int i = 0; i < Output.VolatileData.PathCount; i++)
            {
                var dat = Output.VolatileData.get_Branch(i);

                var ghVal = dat as GH_Brep;

                foreach (var geo in dat)
                {
                    var brep = geo as GH_Brep;
                    if (geo != null)
                    {
                        result.Add(brep.Value);
                    }
                }
            }

            return result;
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