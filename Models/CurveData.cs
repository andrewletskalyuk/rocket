using Armstrong.Services.CurveDrawing.Enums;
using Armstrong.Services.PumpCurveData.Models;
using System.Collections.Generic;

namespace Armstrong.Services.CurveDrawing.Models
{
    public class CurveData : IOutputData, IInputData
    {
        public CurveData()
        {
            Width = 600;
            Height = 400;
            FontSize = 11;
            BorderThickness = 0.2;
        }

        public string Title { get; set; }
        public string Subtitle { get; set; }
        public IEnumerable<GraphPolynomial> Polynomials { get; set; }
        public IEnumerable<GraphPoint> Points { get; set; }
        public IEnumerable<GraphLine> Lines { get; set; }
        public IEnumerable<GraphAnnotation> Annotations { get; set; }
        public IEnumerable<GraphArea> Areas { get; set; }
        public IEnumerable<Axis> Axes { get; set; }
        public bool ShowLegend { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public double FontSize { get; set; }
        public ExportType ExportType { get; set; }
        public double BorderThickness { get; set; }
    }
}