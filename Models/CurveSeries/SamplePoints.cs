using System.Collections.Generic;

namespace Armstrong.Core.Services.CurveDrawing.Models.CurveSeries
{
    public class SamplePoints
    {
        public string FlowUnit { get; set; }
        public string HeadUnit { get; set; }
        public string PowerUnit { get; set; }
        public string NpshrUnit { get; set; }
        public List<SamplePointRow> Rows { get; set; }
    }
}