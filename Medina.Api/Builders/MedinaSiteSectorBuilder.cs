using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rh = Rhino;
using Rhino.Geometry;
using Rhino.FileIO;

namespace Medina.Api
{
    public class MedinaSiteSectorBuilder
    {
        public string Id { get; private set; }
        public File3dm File { get; private set; }
        public List<File3dmObject> Objects { get; private set; } = new List<File3dmObject>();

        //Rhino geometric inputs
        public Brep Base { get; private set; }

        //Ground datums
        public List<Brep> Footprints { get; private set; } = new List<Brep>();
        public List<Brep> Courtyards { get; private set; } = new List<Brep>();
        public List<Brep> Plazas { get; private set; } = new List<Brep>();
        
        //Elevation datums
        public List<Brep> Floors { get; private set; } = new List<Brep>();
        public List<Brep> Roofs { get; private set; } = new List<Brep>();
        public List<Brep> Balconies { get; private set; } = new List<Brep>();

        //Spatial masses
        public List<Brep> Massing { get; private set; } = new List<Brep>();
        public List<Brep> Doors { get; private set; } = new List<Brep>();
        public List<Brep> Windows { get; private set; } = new List<Brep>();

        //Ruins
        public List<Point3d> RuinPoints { get; private set; } = new List<Point3d>();

        public MedinaSiteSectorBuilder CreateSector(File3dm file)
        {
            File = file;
            return this;
        }

        public MedinaSiteSectorBuilder WithId(string id)
        {
            Id = id;
            return this;
        }

        public MedinaSiteSectorBuilder WithObjects(List<File3dmObject> objects)
        {
            Objects.AddRange(objects);
            return this;
        }

        public MedinaSiteSectorBuilder CategorizeObjects()
        {
            foreach (var obj in Objects)
            {
                var layerName = File.AllLayers.FindIndex(obj.Attributes.LayerIndex).Name;

                //Console.WriteLine(layerName);

                var geo = obj.Geometry as Brep;
                var pt = Point3d.Unset;

                if (geo == null)
                {
                    if (layerName != "ruins__points")
                    {
                        if (layerName == "mass__object--building")
                        {
                            //Console.WriteLine(obj.Geometry.ObjectType);
                            var extrusion = obj.Geometry as Extrusion;
                            geo = extrusion.ToBrep();
                        }
                        else
                        {
                            continue;
                        }              
                    }
                    else
                    {
                        var ptRef = obj.Geometry as Point;

                        if (ptRef != null)
                        {
                            RuinPoints.Add(ptRef.Location);
                            continue;
                        }

                        continue;
                    }
                }

                switch (layerName)
                {
                    case "base":
                        Base = geo;                        
                        break;
                    case "datum__ground--footprints":
                        Footprints.Add(geo);
                        break;
                    case "datum__ground--courtyard":
                        Courtyards.Add(geo);
                        break;
                    case "datum_ground--public":
                        Plazas.Add(geo);
                        break;
                    case "datum__floor":
                        Floors.Add(geo);
                        break;
                    case "datum__roof":
                        Roofs.Add(geo);
                        break;
                    case "datum__balcony":
                        Balconies.Add(geo);
                        break;
                    case "mass__object--building":
                        Massing.Add(geo);
                        break;
                    case "mass__space--openings":
                        Windows.Add(geo);
                        break;
                    case "mass__space--doors":
                        Doors.Add(geo);
                        break;
                    default:
                        break;
                }
                
            }

            return this;
        }

        public static implicit operator MedinaSiteSector(MedinaSiteSectorBuilder builder)
        {
            return new MedinaSiteSector(builder);
        }
    }
}