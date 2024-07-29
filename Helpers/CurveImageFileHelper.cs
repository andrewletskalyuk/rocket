using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Armstrong.Core.Services.CurveDrawing.Constants;
using Armstrong.Core.Services.CurveDrawing.Constatns;
using Armstrong.Core.Services.CurveDrawing.Models;
using Armstrong.Core.Services.CurveDrawing.Models.CurveSeries;

namespace Armstrong.Core.Services.CurveDrawing.Helpers
{
    public class CurveImageFileHelper : ICurveImageFileHelper
    {
        private int _smallFontSize;
        private string _fontFamily;
        private SolidBrush _horzBrush;
        private SolidBrush _vertBrush;
        private float _pageMaxX;
        private float _pageMaxY;
        private float _pageMinX;
        private float _pageMinY;
        private float _pageRangeX;
        private float _pageRangeY;
        private float _parallelTitleY;
        private float _titleY;
        private float _subTitleY;
        private float _certificateAxis;
        private int _totalHeight;
        private int _totalWidth;
        private int _borderSize;
        private int _fluidLabelX;
        private int _fluidLabelY;
        private int _verticalAxisLabelX;
        private int _verticalAxisLabelY;
        private int _horizontalAxisLabelX;
        private int _horizontalAxisLabelY;
        private Font _horzFont;
        private Font _vertFont;
        private Font _curveTagFont;
        private Font _axisTitleFont;
        private Font _titleFont;
        private Font _subTitleFont;
        private Font _horzFluidFont;
        private Font _curveLabelFont;
        private Font _curveCertificateFont;

        public bool OverrideEmfContent { get; set; }  // true for real jpeg

        // .emf format
        [DllImport("gdi32.dll")]
        private static extern IntPtr CopyEnhMetaFile( // Copy EMF to file
            IntPtr hemfSrc, // Handle to EMF
            string lpszFile // File
        );

        [DllImport("gdi32.dll")]
        private static extern int DeleteEnhMetaFile( // Delete EMF
            IntPtr hemf // Handle to EMF
        );
        private void Init()
        {
            var titleFontFamily = "Lucida Sans A";
            _smallFontSize = 8;
            _fontFamily = "Verdana";
            _totalHeight = 450;
            _totalWidth = 582;
            _borderSize = 50;
            _parallelTitleY = 20f;
            _titleY = 40f;
            _subTitleY = 60f;

            _horzBrush = new SolidBrush(Color.Black);
            _vertBrush = new SolidBrush(Color.Black);

            _horzFont = new Font(_fontFamily, _smallFontSize, FontStyle.Bold);
            _vertFont = new Font(_fontFamily, _smallFontSize, FontStyle.Bold);
            _curveTagFont = new Font(_fontFamily, _smallFontSize, FontStyle.Bold);
            _curveCertificateFont = new Font(_fontFamily, _smallFontSize, FontStyle.Regular);

            int TittleFontSize = 9;
            _titleFont = new Font(titleFontFamily, TittleFontSize, FontStyle.Bold);
            _subTitleFont = new Font(titleFontFamily, TittleFontSize, FontStyle.Regular);
            _axisTitleFont = new Font(titleFontFamily, _smallFontSize, FontStyle.Regular);

            _horzFluidFont = new Font(titleFontFamily, _smallFontSize - 1, FontStyle.Regular);
            _curveLabelFont = new Font(titleFontFamily, _smallFontSize, FontStyle.Regular);
            
            _fluidLabelX = 2;
            _fluidLabelY = _totalHeight - 15;
            _verticalAxisLabelX = 15;
            _verticalAxisLabelY = _totalHeight / 2;
            _horizontalAxisLabelX = 100;
            _horizontalAxisLabelY = _totalHeight - 15;

            _pageMaxX = _totalWidth - _borderSize;
            _pageMaxY = _totalHeight - _borderSize;
            _pageMinX = 60;
            _pageMinY = 80;
            _certificateAxis = 10;
        }
        private Metafile CreateMetaImageFile(int imageWidth, int imageHeight)
        {
            Metafile metaFile;
            using (var stream = new MemoryStream())
            {
                using (Bitmap bitmap = new Bitmap(imageWidth, imageHeight))
                {
                    using (Graphics offScreenBufferGraphics = Graphics.FromImage(bitmap))
                    {
                        IntPtr deviceContextHandle = offScreenBufferGraphics.GetHdc();
                        metaFile = new Metafile(
                            stream,
                            deviceContextHandle,
                            new RectangleF(0, 0, imageWidth, imageHeight),
                            MetafileFrameUnit.Pixel,
                            EmfType.EmfPlusOnly);
                        offScreenBufferGraphics.ReleaseHdc();
                        DeleteEnhMetaFile(deviceContextHandle);
                    }
                }
            }
            return metaFile;
        }
        private Graphics CreateGraphicContext(Graphics context, Metafile metaFile, int imageWidth, int imageHeight)
        {
            context.SmoothingMode = SmoothingMode.AntiAlias;
            context.PixelOffsetMode = PixelOffsetMode.HighQuality;
            context.CompositingQuality = CompositingQuality.HighQuality;
            context.InterpolationMode = InterpolationMode.HighQualityBicubic;
            context.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            var metafileHeader = metaFile.GetMetafileHeader();
            context.ScaleTransform(
                metafileHeader.DpiX / context.DpiX,
                metafileHeader.DpiY / context.DpiY);

            context.PageUnit = GraphicsUnit.Pixel;
            var scale = 1.18;
            context.SetClip(
                new RectangleF(
                    0, 0, (int)((float)imageWidth * scale), (int)((float)imageHeight * scale)));

            context.Clear(Color.White);
            return context;
        }
        private string ExpertMetafileToImage(Metafile metaFile, string imagePath)
        {
            var fileName = GetFileName();
            var imageFullPath = string.Format(@"{0}/{1}", imagePath, fileName);

            if (!OverrideEmfContent)
            {
                var iptrMetafileHandle = metaFile.GetHenhmetafile();
                var ptr2 = CopyEnhMetaFile(iptrMetafileHandle, imageFullPath);

                DeleteEnhMetaFile(iptrMetafileHandle);
                DeleteEnhMetaFile(ptr2);
            }
            else
            {
                metaFile.Save(imageFullPath, ImageFormat.Jpeg);
            }

            return fileName;
        }
        private void DrawGraphTitle(Graphics context, CurveSeriesRoot curveData)
        {
            var titles = curveData.Title.Split('-');
            if (!string.IsNullOrWhiteSpace(titles[0]))
            {
                context.DrawString(titles[0], _titleFont, _horzBrush, _pageMinX, _parallelTitleY);
                if (titles.Length > 1)
                    context.DrawString(titles[1], _subTitleFont, _horzBrush, _pageMinX, _titleY);
            }
            else
            {
                try { 
                    context.DrawString(titles[1], _titleFont, _horzBrush, _pageMinX, _titleY); 
                }
                catch (Exception ex) {
                    /* Catching Exception arrising due to curveData.Title blank or does not have data for titles[1] */
                }
            }
            if(!string.IsNullOrWhiteSpace(curveData.Title2))
                context.DrawString(curveData.Title2, _subTitleFont, _horzBrush, _pageMinX, _titleY);
            context.DrawString(curveData.SubTitle, _subTitleFont, _horzBrush, _pageMinX, _subTitleY);
            context.DrawString(curveData.FluidLabel, _horzFluidFont, _horzBrush, _fluidLabelX, _fluidLabelY);
            context.DrawString(curveData.HorizontalAxisLabel, _axisTitleFont, _horzBrush, _horizontalAxisLabelX, _horizontalAxisLabelY);
            context.DrawString(curveData.VerticalAxisLabel, _axisTitleFont, _vertBrush, _verticalAxisLabelX, _verticalAxisLabelY, new StringFormat(StringFormatFlags.DirectionVertical));
        }
        private AxisData GetAxisData(CurveSeriesRoot curveData)
        {
            var axisData = new AxisData();
            var curves = curveData.CurveSeries;
            foreach (var curve in curves)
            {   
                if (curve != null)
                {
                    foreach (var point in curve.Data)
                    {
                        float x = (float)point.X;
                        float y = (float)point.Y;
                        if (x < axisData.MinX && x >= 0.0f)
                        {
                            axisData.MinX = x;
                        }
                        if (x > axisData.MaxX && x >= 0.0f)
                        {
                            axisData.MaxX = x + 0;
                        }
                        if (y < axisData.MinY && y >= 0.0f)
                        {
                            axisData.MinY = y;
                        }
                        if (y > axisData.MaxY && y >= 0.0f)
                        {
                            axisData.MaxY = y;
                        }
                    }
                }
            }

            axisData.XAxis = AxisScalerHelper.GetAxis(axisData.MinX, axisData.MaxX, _totalWidth);
            _pageRangeX = axisData.XAxis.Max;
            axisData.YAxis = AxisScalerHelper.GetAxis(axisData.MinY, axisData.MaxY, _totalHeight);
            _pageRangeY = axisData.YAxis.Max;

            return axisData;
        }
        private float CalcX(float x)
        {
            var result = x / _pageRangeX * (_pageMaxX - _pageMinX) + _pageMinX;
            return result;
        }
        private float CalcY(float y)
        {
            var result = y / _pageRangeY * (_pageMinY - _pageMaxY) + _pageMaxY;
            return result;
        }
        private void DrawAxis(Graphics context, CurveSeriesRoot curveData)
        {
            var axisData = GetAxisData(curveData);
            for (var i = axisData.XAxis.Min; i <= axisData.XAxis.Max; i += axisData.XAxis.Tick)
            {
                var xaxisLabelStartPosition = (int)CalcX(i);
                context.DrawString(i.ToString(CultureInfo.InvariantCulture), _vertFont, _vertBrush, xaxisLabelStartPosition, _pageMaxY + 5);
                context.DrawLine(new Pen(Color.LightGray, 0.5f), new PointF(xaxisLabelStartPosition, _pageMaxY),
                    new PointF(xaxisLabelStartPosition, _pageMinY));
            }

            for (var i = axisData.YAxis.Min; i <= axisData.YAxis.Max; i += axisData.YAxis.Tick)
            {
                var yaxisLabelStartPosition = (int)CalcY(i);
                context.DrawString(i.ToString(CultureInfo.InvariantCulture), _vertFont, _vertBrush, _pageMinX - GetYaxisLabelDelta(i)-10, yaxisLabelStartPosition);
                context.DrawLine(new Pen(Color.LightGray, 0.5f), new PointF(_pageMinX, yaxisLabelStartPosition),
                    new PointF(_pageMaxX, yaxisLabelStartPosition));
            }
        }
        private float GetYaxisLabelDelta(float labelValue)
        {
            float yaxisLabelDelta = 10.0f;
            var length = labelValue.ToString().Length;
            bool found = false;
            var deltas = new Dictionary<int, float>() { 
                { 2, 16.0f }, 
                { 3, 22.0f }, 
                { 4, 28.0f }
            };
            var exceptions = new Tuple<int, float, float>[] { 
                new Tuple<int, float, float>(3, 10.0f, 19.0f),
                new Tuple<int, float, float>(4, 100.0f, 25.0f)
            };
            var matched = deltas.Where(e => e.Key == length && (found = true)).FirstOrDefault();
            if (found)
            {
                var exception  = exceptions.Where(e => e.Item1.Equals(matched.Key) && e.Item2 > labelValue).FirstOrDefault();
                yaxisLabelDelta = exception != null ? exception.Item3 : matched.Value;
            }

            return yaxisLabelDelta;
        }
        private IList<PointF> ConvertPositionToPointF(IList<Position> positions)
        {
            IList<PointF> result = new List<PointF>();
            foreach (var position in positions)
            {
                result.Add(new PointF((float)position.X, (float)position.Y));
            }
            return result;
        }
        private Color ToColor(string color)
        {
            string[] matched = Regex.Split(color, @"\D+");
            return System.Drawing.Color.FromArgb(int.Parse(matched[1]), int.Parse(matched[2]), int.Parse(matched[3]));
        }
        private PointF[] GetArrowHeadPoints(IList<PointF> points)
        {
            var x0 = CalcX((float)points[0].X);
            var y0 = CalcY((float)points[0].Y);

            return new PointF[]
            {
                new PointF(x0, y0),
                new PointF(x0 - 15, y0),
                new PointF(x0, y0 + 10)
            };
        }
        private PointF[] GetBepPoints(IList<PointF> points, Slope slope)
        {
            var point = points[0];
            var baseDetlaX = point.X / 30;
            var baseDetlaY = point.Y / 30;

            var slopeAngel = Math.Atan2(slope.Y, slope.X);
            var deltaX = (float)(baseDetlaX * Math.Cos(slopeAngel));
            var deltaY = (float)(baseDetlaY * Math.Sin(slopeAngel));
            var x0 = (float)point.X;
            var y0 = (float)point.Y;
            var x1 = x0 - deltaX;
            var y1 = y0 - deltaY;
            var x2 = x0 + deltaX;
            var y2 = y0 + deltaY;

            return new PointF[]
            {
                new PointF(CalcX(x1), CalcY(y1)),
                new PointF(CalcX(x0), CalcY(y0)),
                new PointF(CalcX(x2), CalcY(y2))
            };
        }
        private PointF[] GetDotPoints(IList<PointF> points)
        {
            var x0 = CalcX((float)points[0].X);
            var y0 = CalcY((float)points[0].Y);

            return new PointF[]
            {
                new PointF(x0, y0),
                new PointF(x0 + 3, y0 + 3),
                new PointF(x0, y0 + 6),
                new PointF(x0 - 3, y0 + 3)
            };
        }
        private void DrawPoints(Graphics context, CurveSeriesRoot curveData)
        {
            foreach (var curve in curveData.CurveSeries)
            {
                var points = ConvertPositionToPointF(curve.Data);
                var graphPath = new GraphicsPath();
                if (curve.Point != null && points.Any())
                {
                    switch (curve.Point.Symbol)
                    {
                        case CurveSymbol.BEP:
                            {
                                graphPath.AddLines(GetBepPoints(points, curve.Point.Slope).ToArray());
                                var pen = new Pen(ToColor(CurveColors.IMPELLER_CURVE_RGBA), 1);
                                pen.DashStyle = DashStyle.Solid;
                                pen.DashCap = DashCap.Round;
                                context.DrawPath(pen, graphPath);
                                break;
                            }
                        case CurveSymbol.ARROWHEAD:
                            {
                                graphPath.AddLines(GetArrowHeadPoints(points).ToArray()); 
                                var brush = new SolidBrush(ToColor(CurveColors.ARROW_HEAD_RBGBA));
                                context.FillPath(brush, graphPath);
                                break;
                            }
                        case CurveSymbol.DOT:
                            {
                                graphPath.AddLines(GetDotPoints(points).ToArray());
                                var brush = new SolidBrush(ToColor(CurveColors.PROJECTED_POINT_RGBA));
                                context.FillPath(brush, graphPath);
                                if (curve.Label != null)
                                    context.DrawString(curve.Label.Text, _horzFont, brush, points[0].X, points[0].Y);
                                break;
                            }
                    }

                }
            }
        }
        private double GetAbsoluteFromEquation(Position previous, Position point)
        {
            var a = (previous.Y - point.Y) / (previous.X - point.X);
            return point.Y - a * point.X;
        }
        private List<PointF> GetPoints(CurveSeries curve)
        {
            var graphPoints = new List<PointF>();
            Position prevPoint = null;
            foreach (var point in curve.Data)
            {
                if (prevPoint == null)
                {
                    prevPoint = new Position();
                }
                if (point.X < 0)
                {
                    point.Y = GetAbsoluteFromEquation(prevPoint, point);
                    point.X = 0;
                }
                var calcX = CalcX((float)point.X);
                var calcY = CalcY((float)point.Y);
                graphPoints.Add(new PointF(calcX, calcY));
                prevPoint.X = point.X;
                prevPoint.Y = point.Y;
            }
            return graphPoints.Distinct().ToList();
        }
        private PointF GetCurvelabelPosition(CurveSeries curve, int textXOffSet, int textYOffSet)
        {
            var ph = PointF.Empty;
            ph.X = CalcX((float)curve.Label.Position.X) + textXOffSet;
            ph.Y = CalcY((float)curve.Label.Position.Y) + textYOffSet;

            return ph;
        }
        private Pen GetPen(CurveSeries curve)
        {
            var color = curve.Color == null ? Color.Black : ToColor(curve.Color);
            var pen = new Pen(color);
            pen.StartCap = LineCap.Round;
            pen.EndCap = LineCap.Round;
            pen.DashCap = DashCap.Round;
            pen.Width = curve.Lines == null ? 1.0f : (float)curve.Lines.LineWidth;
            pen.DashStyle = curve.Style == null ? 
                DashStyle.Solid :
                (curve.Style.Count() > 0 ? DashStyle.Dash : DashStyle.Solid);
            if (pen.DashStyle == DashStyle.Dash)
            {
                float[] dashValues = { curve.Style[0], curve.Style[1] };
                pen.DashPattern = dashValues;
            }

            return pen;
        }
        private SolidBrush GetBrush(CurveSeries curve)
        {
            var color = curve.Color == null 
                ? Color.Black 
                : (curve.Color == "rgba(255,0,0,1)" ? ToColor(curve.Color) : Color.Black);
            return new SolidBrush(color);
        }
        private void DrawLines(Graphics context, CurveSeriesRoot curveData)
        {
            foreach (var curve in curveData.CurveSeries)
            {
                var graphPoints = GetPoints(curve);
                var pen = GetPen(curve);

                if (graphPoints.Count() > 1)
                {
                    context.DrawLines(pen, graphPoints.ToArray());
                    if (!String.IsNullOrEmpty(curve.Label.Text))
                    {
                        int textXOffset = 0;
                        int textYOffSet = 0;
                        var point = GetCurvelabelPosition(curve, textXOffset, textYOffSet);
                        var brush = GetBrush(curve);

                        context.DrawString(curve.Label.Text, _curveLabelFont, brush, point.X, point.Y);
                    }
                }
            }
        }
        private PointF GetCurveTagPosition(Graphics context, string text, Font font)
        {
            var point = PointF.Empty;
            if (text.Length > 0)
            {
                var sizeF = context.MeasureString(text, font);

                point.X = _pageMaxX - sizeF.Width;
                point.Y = _pageMinY;
            }
            return point;
        }


        private void DrawCurveTag(Graphics context, CurveSeriesRoot curveData)
        {
            PointF position = GetCurveTagPosition(context, curveData.CurveTag, _horzFluidFont);
            if (!position.IsEmpty)
            {
                context.DrawString(curveData.CurveTag, _curveTagFont, _horzBrush, position.X, position.Y);
            }
        }

        private void DrawCertificationData(Graphics context, CurveSeriesRoot curveData)
        {
            PointF position = GetCurveTagPosition(context, curveData.CertificationData, _horzFluidFont);
            if (!position.IsEmpty)
            {
                context.DrawString(curveData.CertificationData, _curveCertificateFont, _horzBrush, position.X + _certificateAxis, position.Y + _certificateAxis);
            }
        }

        private void DrawGraph(Graphics context, CurveSeriesRoot curveData)
        {
            DrawGraphTitle(context, curveData);
            DrawAxis(context, curveData);
            DrawLines(context, curveData);
            DrawPoints(context, curveData);
            DrawCurveTag(context, curveData);
            DrawCertificationData(context, curveData);
        }
        private static string GetFileName()
        {
            return string.Format(@"{0}.jpg", Guid.NewGuid());
        }

        public string CreateImage(CurveSeriesRoot curveData, string imagePath)
        {
            Init();

            var metaFile = CreateMetaImageFile(_totalWidth, _totalHeight);
            Graphics context = null;
            using (context = Graphics.FromImage(metaFile))
            {
                CreateGraphicContext(context, metaFile, _totalWidth, _totalHeight);
                DrawGraph(context, curveData);
                context.Dispose();
            }
            var fileName = ExpertMetafileToImage(metaFile, imagePath);
            metaFile.Dispose();
            return fileName;
        }
    }
}
