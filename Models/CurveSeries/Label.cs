using Armstrong.Core.Services.CurveDrawing.Helpers;
using Newtonsoft.Json;

namespace Armstrong.Core.Services.CurveDrawing.Models.CurveSeries
{
    public class Label
    {
        public string Text { get; set; }
        [JsonConverter(typeof(PositionConverter))]
        public Position Position { get; set; }
        public string TextBaseline { get; set; }
        public string TextAlign { get; set; }
    }
}