using System.Collections.Generic;
using Armstrong.Core.Services.CurveDrawing.Enums;
using Armstrong.Core.Services.CurveDrawing.Helpers;
using Newtonsoft.Json;

namespace Armstrong.Core.Services.CurveDrawing.Models.CurveSeries
{
    public class CurveSeries
    {
        public CurveSeries() 
        {
            // Defauilt for NewEngine
            Group = CurveFamilyType.DynamicCurves;
        }

        public string Color { get; set; }
        [JsonConverter(typeof(PositionConverter))]
        public IList<Position> Data { get; set; }
        [JsonProperty("Points")]
        public Point Point { get; set; }
        public Label Label { get; set; }
        public List<int> Style { get; set; }
        public Line Lines { get; set; }
        public bool IsHighlightable { get; set; }
        public CurveFamilyType Group { get; set; }
        public bool IsLegend { get; set; }
        public bool ShowLegend { get; set; }
        public bool IsDesignPoint { get; set; }
        public string CurveName { get; set; }
    }
}