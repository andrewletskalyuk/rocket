using Newtonsoft.Json;

namespace Armstrong.Core.Services.CurveDrawing.Models.CurveSeries
{
    [JsonObject(Title = "Lines")]
    public class Line
    {
        public int LineWidth { get; set; }
        public bool Fill { get; set; }
        public string FillColor { get; set; }
    }
}