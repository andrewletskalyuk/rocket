using Armstrong.Services.CurveDrawing.Enums;

namespace Armstrong.Services.CurveDrawing.Models
{
    public class Axis
    {
        public Axis()
        {
            TitleFontSize = 13;
            TitleDistance = 13;
        }
        public string Title { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public AxisPosition Position { get; set; }
        public double GridSpacing { get; set; }
        public double TitleFontSize { get; set; }
        public double TitleDistance { get; set; }
    }
}