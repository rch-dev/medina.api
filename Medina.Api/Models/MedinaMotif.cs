using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Medina.Api
{
    public class MedinaMotif
    {
        public string Name { get; set; }

        public MedinaMotif(string name)
        {
            Name = name;
        }
    }
}