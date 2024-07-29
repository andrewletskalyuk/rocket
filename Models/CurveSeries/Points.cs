namespace Armstrong.Core.Services.CurveDrawing.Models.CurveSeries
{
    public class Point
    {
        public bool Show { get; set; }
        public bool Fill { get; set; }
        public string FillColor { get; set; }
        public string Symbol { get; set; }
        public Slope Slope { get; set; }
    }
}