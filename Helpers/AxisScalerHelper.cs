using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Armstrong.Core.Services.CurveDrawing.Helpers
{
    public static class AxisScalerHelper
    {
        public struct Axis
        {
            public float Min;
            public float Max;
            public float Tick;
        }

        public static Axis GetAxis(double min, double max, int resolution)
        {
            var range = max - min;
            var numTicks = 0.3 * Math.Sqrt(resolution);
            var delta = range / numTicks;
            var decimalBase = Math.Floor(Math.Log(delta) / Math.Log(10));
            var magnification = Math.Pow(10, decimalBase);
            var norm = delta / magnification;
            double size = 10;
            bool found = false;
            var sizes = new Dictionary<double, double>() { { 1.5, 1 }, { 2.25, 2 }, { 3, 2.5 }, { 7.5, 5 }};
            var matched = sizes.Where(e => e.Key > norm && (found = true)).FirstOrDefault();
            if (found)
            {
                size = matched.Value;
            }
            
            size *= magnification;

            var axis = new Axis()
            {
                Min = (float)(Math.Floor(min / size) * size),
                Max = (float)(Math.Ceiling(max / size) * size),
                Tick = (float)size
            };

            return axis;

        }
    }
}