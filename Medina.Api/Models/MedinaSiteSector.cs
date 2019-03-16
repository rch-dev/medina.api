using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.FileIO;

namespace Medina.Api
{
    public class MedinaSiteSector
    {
        public MedinaSiteSectorId Id { get; set; }
        public List<File3dmObject> Objects { get; set; } = new List<File3dmObject>();

        //Datums

        //Masses

        public MedinaSiteSector(string id)
        {
            Id = new MedinaSiteSectorId(id);
        }

        public MedinaSiteSector(string id, List<File3dmObject> objects)
        {
            Id = new MedinaSiteSectorId(id);
            Objects = objects;
        }

        public override string ToString()
        {
            return $"Sector {Id} ({Objects.Count.ToString()} objects)";
        }
    }




}