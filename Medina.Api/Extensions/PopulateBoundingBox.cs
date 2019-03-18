using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.Geometry;

namespace Medina.Api
{
    public static partial class Extensions
    {
        public static List<Point3d> PopulateBoundingBox(this BoundingBox box, int number)
        {
            var r = new Random();
            var pts = new List<Point3d>();

            if (number < 1)
            {
                number = 1;
            }

            var dx = box.Max.X - box.Min.X;
            var dy = box.Max.Y - box.Min.Y;
            var dz = box.Max.Z - box.Min.Z;

            var xMin = box.Min.X;
            var yMin = box.Min.Y;
            var zMin = box.Min.Z;

            for (int i = 0; i < number; i++)
            {
                pts.Add(new Point3d(
                    xMin + (r.NextDouble() * 100),
                    yMin + (r.NextDouble() * 100),
                    zMin + (r.NextDouble() * dz)
                    ));
            }

            return pts;
        }
    }
}