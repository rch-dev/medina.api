using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.FileIO;

namespace Medina.Api
{
    public class MedinaSiteSectorBuilder
    {
        public string Id { get; private set; }
        public List<File3dmObject> Objects { get; private set; } = new List<File3dmObject>();

        public MedinaSiteSectorBuilder CreateSector(string id)
        {
            Id = id;
            return this;
        }

        public MedinaSiteSectorBuilder WithObjects(List<File3dmObject> objects)
        {
            Objects.AddRange(objects);
            return this;
        }

        public static implicit operator MedinaSiteSector(MedinaSiteSectorBuilder builder)
        {
            return new MedinaSiteSector(builder.Id) { Objects = builder.Objects };
        }
    }
}