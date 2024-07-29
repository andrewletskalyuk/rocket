namespace Armstrong.Core.Services.CurveDrawing.Models.CurveSeries
{
    public class Position
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    public class IsoPosition
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Eff { get; set; }
        public string CurveBase { get; set; }
        public bool IsLeftBound { get; set; }
    }
}