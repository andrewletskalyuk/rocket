using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Armstrong.Core.Services.CurveDrawing.Helpers;

namespace Armstrong.Core.Services.CurveDrawing.Models
{
    public class AxisData
    {
        public float MaxX { get; set; }
        public float MaxY { get; set; }
        public float MinX { get; set; }
        public float MinY { get; set; }

        public AxisScalerHelper.Axis XAxis { get; set; }
        public AxisScalerHelper.Axis YAxis { get; set; }
    }
}
