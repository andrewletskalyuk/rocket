using System.Collections.Generic;

namespace Armstrong.Core.Services.CurveDrawing.Models.CurveSeries
{
    public class CurveSeriesRoot
    {
        public List<CurveSeries> CurveSeries { get; set; }
        public string Description { get; set; }
        public string SubTitle { get; set; }
        public string Title { get; set; }
        public string Title2 { get; set; }
        public string VerticalAxisLabel { get; set; }
        public string HorizontalAxisLabel { get; set; }
        public string FluidLabel { get; set; }
        public List<string> Messages { get; set; }
        public bool ShowLegend { get; set; }
        public string CurveTag { get; set; }
        public string CertificationData { get; set; }
        public SamplePoints SamplePoints { get; set; }
        public string BuildVersion { get; set; }
    }
}