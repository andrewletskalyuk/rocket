using System.Collections.Generic;

namespace Armstrong.Services.CurveDrawing.Models
{
    public class GraphArea : GraphBase
    {
        public IEnumerable<GraphPolynomial> Polynomials { get; set; }
        public IEnumerable<GraphPoint> PointSetMin { get; set; }
        public IEnumerable<GraphPoint> PointSetMax { get; set; }
    }
}
