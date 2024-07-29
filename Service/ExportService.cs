using Armstrong.Services.CurveDrawing.Enums;
using OxyPlot;
using OxyPlot.WindowsForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace Armstrong.Services.CurveDrawing.Service
{
    public class ExportService : IExportService
    {
        private int _curveWidth;
        private int _curveHeight;
        private string _fileFolder;

        public ExportService(int width, int height, string fileFolder)
        {
            _curveWidth = width;
            _curveHeight = height;
            _fileFolder = fileFolder;
        }

        public string ExportFile(PlotModel plotModel, ExportType exportType)
        {
            var fileName = GetFileName(exportType);

            Trace.WriteLine(string.Format("ExportFile-{0}", fileName));
            try
            {
                using (var stream = File.Create(GetFilePath(fileName)))
                {
                    GetExporter(exportType).Export(plotModel, stream);
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(string.Format("Export Error-{0}", e.Message.ToString()));
            }
            return fileName;
        }

        public IEnumerable<string> ExportFiles(PlotModel plotModel, IEnumerable<ExportType> exportTypes)
        {
            var fileNames = new List<string>();

            foreach (var type in exportTypes)
            {
                var fileName = ExportFile(plotModel, type);
                if (string.IsNullOrEmpty(fileName))
                {
                    fileNames.Add(fileName);
                }
            }
            return fileNames;
        }

        public MemoryStream ExportStream(PlotModel plotModel, ExportType exportType)
        {
            var stream = new MemoryStream();
            GetExporter(exportType).Export(plotModel, stream);

            if (exportType == ExportType.Jpeg)
            {
                stream = ConvertPngToJpg(stream);
            }

            return stream;
        }

        public IExporter GetExporter(ExportType exportType)
        {
            IExporter exporter = null;
            switch (exportType)
            {
                case ExportType.Pdf:
                    exporter = new PdfExporter { Width = _curveWidth, Height = _curveHeight };
                    break;
                case ExportType.Svg:
                    exporter = new OxyPlot.SvgExporter { Width = _curveWidth, Height = _curveHeight };
                    break;
                default:
                    exporter = new PngExporter { Width = _curveWidth, Height = _curveHeight, Background = OxyColors.White };
                    break;
            }
            return exporter;
        }

        private MemoryStream ConvertPngToJpg(MemoryStream pngStream)
        {
            Image img = Image.FromStream(pngStream);
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                return stream;
            }
        }

        private string GetFilePath(string fileName)
        {
            return string.Format("{0}{1}", _fileFolder, fileName);
        }

        private static string GetFileName(ExportType exportType)
        {
            var fileName = string.Format(@"{0}.{1}", Guid.NewGuid(), exportType.ToString().ToLower());
            return fileName;
        }

    }
}
