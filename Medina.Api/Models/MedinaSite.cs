using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino;

namespace Medina.Api
{
    public class MedinaSite
    {
        public List<MedinaSiteSector> Sectors { get; set; } = new List<MedinaSiteSector>();
        public List<MedinaMotif> Motifs { get; set; } = new List<MedinaMotif>();

        public MedinaSite()
        {

        }
    }
}