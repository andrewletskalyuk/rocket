using System.Collections.Generic;

namespace Armstrong.Services.CurveDrawing.Models
{
    public class GraphLine : GraphBase
    {
        public IEnumerable<GraphPoint> Points { get; set; }
    }
}
