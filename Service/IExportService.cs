using Armstrong.Services.CurveDrawing.Enums;
using OxyPlot;
using System.Collections.Generic;
using System.IO;

namespace Armstrong.Services.CurveDrawing.Service
{
    public interface IExportService
    {
        string ExportFile(PlotModel plotModel, ExportType exportType);
        IEnumerable<string> ExportFiles(PlotModel plotModel, IEnumerable<ExportType> exportTypes);
        MemoryStream ExportStream(PlotModel plotModel, ExportType exportType);
    }
}
