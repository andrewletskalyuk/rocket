using Armstrong.Services.CurveDrawing.Enums;

namespace Armstrong.Services.CurveDrawing.Models
{
    public class GraphPolynomial : GraphBase
    {
        public GraphPolynomial()
        {
            MinXValue = 0;
            MaxXValue = 1000;
            FloatingPoint = 5.0;
            DrawingAreaType = DrawingAreaType.None;
        }
        public string Polynomial { get; set; }
        public double MinXValue { get; set; }
        public double MaxXValue { get; set; }
        public double FloatingPoint { get; set; }
        public DrawingAreaType DrawingAreaType { get; set; }
    }
}