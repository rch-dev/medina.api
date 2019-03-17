using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.Geometry;
using Rhino.FileIO;

namespace Medina.Api
{
    public class MedinaSiteSector
    {
        public MedinaSiteSectorId Id { get; set; }
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


        public MedinaSiteSector(string id)
        {
            Id = new MedinaSiteSectorId(id);
        }

        public MedinaSiteSector(string id, List<File3dmObject> objects)
        {
            Id = new MedinaSiteSectorId(id);
            Objects = objects;
        }

        /// <summary>
        /// Constructor for implicit conversion from builder.
        /// </summary>
        /// <param name="builder"></param>
        public MedinaSiteSector(MedinaSiteSectorBuilder builder)
        {
            Id = new MedinaSiteSectorId(builder.Id);
            File = builder.File;
            Objects = builder.Objects;

            Base = builder.Base;
            Footprints = builder.Footprints;
            Courtyards = builder.Courtyards;
            Plazas = builder.Plazas;
            Floors = builder.Floors;
            Roofs = builder.Roofs;
            Balconies = builder.Balconies;
            Massing = builder.Massing;
            Doors = builder.Doors;
            Windows = builder.Windows;
            RuinPoints = builder.RuinPoints;
        }

        public void Audit()
        {
            Console.WriteLine(this.ToString());

            Console.WriteLine($"   Sector base is {(Base == null ? "unset" : "set")}.");
            Console.WriteLine($"   {Footprints.Count.ToString()} footprints");
            Console.WriteLine($"   {Courtyards.Count.ToString()} courtyards");
            Console.WriteLine($"   {Plazas.Count.ToString()} plazas");
            Console.WriteLine($"   {Floors.Count.ToString()} floors");
            Console.WriteLine($"   {Roofs.Count.ToString()} roofs");
            Console.WriteLine($"   {Balconies.Count.ToString()} balconies");
            Console.WriteLine($"   {Massing.Count.ToString()} building masses");
            Console.WriteLine($"   {Doors.Count.ToString()} doors");
            Console.WriteLine($"   {Windows.Count.ToString()} windows");
            Console.WriteLine($"   ...and {RuinPoints.Count.ToString()} ruin points");
        }

        public override string ToString()
        {
            return $"Sector {Id} ({Objects.Count.ToString()} objects)";
        }
    }




}