using Armstrong.Services.CurveDrawing.Helpers;
using Armstrong.Services.CurveDrawing.Models;
using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.IO;

namespace Armstrong.Services.CurveDrawing.Service
{
    public class CurveDrawingService : ICurveDrawingService
    {
        public MemoryStream GetCurveStream(CurveData curveData)
        {
            var plotModel = CreateCurve(curveData);
            var stream = new ExportService(curveData.Width, curveData.Height, string.Empty).ExportStream(plotModel, curveData.ExportType);
            return stream;
        }

        public string GetCurveFileName(CurveData curveData, string fileFolder)
        {
            var plotModel = CreateCurve(curveData);
            var fileName = new ExportService(curveData.Width, curveData.Height, fileFolder).ExportFile(plotModel, curveData.ExportType);
            return fileName;
        }

        public IEnumerable<GraphPoint> GetPoints(GraphPolynomial polynomial)
        {
            var functionExpression = PlotModelHelper.GetFunctionExpression(polynomial.Polynomial);
            Func<double, double> func = x => functionExpression.calculate(x);
            var series = new FunctionSeries(func, polynomial.MinXValue, polynomial.MaxXValue, polynomial.FloatingPoint);
            var points = series.Points;

            var results = new List<GraphPoint>();
            points.ForEach(point => results.Add(new GraphPoint { PointX = point.X, PointY = point.Y }));
            return results;
        }

        private static PlotModel CreateCurve(CurveData curveData)
        {
            var plotModel = PlotModelHelper.CreatePlotModel(curveData);

            plotModel.AddElement(curveData.Areas);
            plotModel.AddElement(curveData.Polynomials);
            plotModel.AddElement(curveData.Lines);
            plotModel.AddElement(curveData.Points);
            plotModel.AddElement(curveData.Annotations);

            return plotModel;
        }

    }
}
