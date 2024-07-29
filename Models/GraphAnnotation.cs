using Armstrong.Services.CurveDrawing.Enums;

namespace Armstrong.Services.CurveDrawing.Models
{
    public class GraphAnnotation : GraphBase
    {
        public double PointX { get; set; }
        public double PointY { get; set; }
        public AnnotationType AnnotationType { get; set; }
        public string BackgroundColor { get; set; }
    }
}
