using Armstrong.Services.CurveDrawing.Enums;
using System.Drawing;

namespace Armstrong.Services.CurveDrawing.Models
{
    public class GraphBase
    {
        public GraphBase()
        {
            Thickness = 1;
            LineStyle = LineStyle.Solid;
        }
        public string Description { get; set; }
        public Color Color { get; set; }
        public string HexColor { get; set; }
        public double Thickness { get; set; }
        public bool ShowLabel { get; set; }
        public string LabelColor { get; set; }
        public LineStyle LineStyle { get; set; }
        public bool LineSmooth { get; set; }
        public LabelPosition LabelPosition { get; set; }
        public AlignmentType LabelVerticalAlignment { get; set; }
        public AlignmentType LabelHorizontalAlignment { get; set; }

    }
}
