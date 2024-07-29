using Armstrong.Services.CurveDrawing.Models;
using System.Collections.Generic;
using System.IO;

namespace Armstrong.Services.CurveDrawing.Service
{
    public interface ICurveDrawingService
    {
        MemoryStream GetCurveStream(CurveData curveInput);
        string GetCurveFileName(CurveData curveInput, string fileFolder);
        IEnumerable<GraphPoint> GetPoints(GraphPolynomial polynomial);
    }
}
