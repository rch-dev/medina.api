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
    public class MedinaMotifInput
    {
        public string Name { get; set; }
        public IGH_Param Param { get; set; }

        public MedinaMotifInput(string name, IGH_Param param)
        {
            Name = name;
            Param = param;
        }
    }
}