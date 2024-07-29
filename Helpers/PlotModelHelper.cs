using Armstrong.Services.CurveDrawing.Enums;
using Armstrong.Services.CurveDrawing.Models;
using org.mariuszgromada.math.mxparser;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using AxisPosition = OxyPlot.Axes.AxisPosition;
using LineStyle = OxyPlot.LineStyle;


namespace Armstrong.Services.CurveDrawing.Helpers
{
    public static class PlotModelHelper
    {
        public static Function GetFunctionExpression(string polynomial)
        {
            var function = Regex.Replace(polynomial, @"(\d+)x", @"$1*x");
            var functionExpression = new Function(string.Format("f(x) = {0}", function));
            return functionExpression;
        }

        public static PlotModel CreatePlotModel(CurveData curveData)
        {
            var borderColor = curveData.ExportType == ExportType.Svg ? "#333333" : "#aeaeae";
            var gridLineColor = curveData.ExportType == ExportType.Svg ? "#9e9e9e" : "#dedede";

            var model = new PlotModel
            {
                Title = curveData.Title,
                Subtitle = curveData.Subtitle,
                IsLegendVisible = curveData.ShowLegend,
                Padding = new OxyThickness(0, 7, 20, 0),
                PlotAreaBorderThickness = new OxyThickness(curveData.BorderThickness, curveData.BorderThickness, curveData.BorderThickness, curveData.BorderThickness),
                PlotAreaBorderColor = GetOxyColor(borderColor),
                DefaultFontSize = curveData.FontSize,
                SubtitleFontSize = 13
            };
            if (curveData.ShowLegend)
            {
                model.LegendPlacement = LegendPlacement.Inside;
                model.LegendPosition = LegendPosition.RightTop;
            }

            SetAxes(ref model, curveData.Axes, curveData.FontSize, gridLineColor);
            return model;
        }

        private static void SetAxes(ref PlotModel plotModel, IEnumerable<Models.Axis> axes, double fontSize, string gridLineColor)
        {
            var color = GetOxyColor(gridLineColor);
            foreach (var axis in axes)
            {
                plotModel.Axes.Add(new LinearAxis
                {
                    Position = axis.Position.ConvertEnum<AxisPosition>(),
                    Title = axis.Title,
                    Minimum = axis.Min,
                    Maximum = axis.Max,
                    TickStyle = TickStyle.None,
                    MajorStep = axis.GridSpacing,
                    MajorGridlineColor = color,
                    MajorGridlineStyle = LineStyle.Solid,
                    MajorGridlineThickness = 0.2,
                    AxisTitleDistance = axis.TitleDistance,
                    FontSize = fontSize,
                    TitleFontSize = axis.TitleFontSize
                });
            }
        }

        public static void AddElement(this PlotModel plotModel, IEnumerable<GraphArea> graphAreas)
        {
            if (graphAreas != null)
            {
                foreach (var area in graphAreas)
                {
                    var areaSeries = new AreaSeries
                    {
                        Fill = GetOxyColor(area.HexColor, null, true),
                        StrokeThickness = area.Thickness,
                        MarkerFill = OxyColors.Transparent,
                    };

                    if (area.PointSetMin != null && area.PointSetMax != null)
                    {
                        area.PointSetMin.ToList().ForEach(point => areaSeries.Points2.Add(new DataPoint(point.PointX, point.PointY)));
                        area.PointSetMax.ToList().ForEach(point => areaSeries.Points.Add(new DataPoint(point.PointX, point.PointY)));
                    }
                    else
                    {
                        foreach (var polynomial in area.Polynomials)
                        {
                            if (!string.IsNullOrEmpty(polynomial.Polynomial))
                            {
                                var functionExpression = GetFunctionExpression(polynomial.Polynomial);
                                Func<double, double> func = x => functionExpression.calculate(x);
                                var series = new FunctionSeries(func, polynomial.MinXValue, polynomial.MaxXValue, polynomial.FloatingPoint);

                                if (polynomial.DrawingAreaType == DrawingAreaType.Min)
                                {
                                    foreach (var point in series.Points)
                                    {
                                        areaSeries.Points2.Add(point);
                                    }
                                }
                                else if (polynomial.DrawingAreaType == DrawingAreaType.Max)
                                {
                                    foreach (var point in series.Points)
                                    {
                                        areaSeries.Points.Add(point);
                                    }
                                }
                            }
                        }
                    }
                    plotModel.Series.Add(areaSeries);
                }
            }
        }

        public static void AddElement(this PlotModel plotModel, IEnumerable<GraphPolynomial> polynomials)
        {
            if (polynomials != null)
            {
                foreach (var polynomial in polynomials)
                {
                    if (!string.IsNullOrEmpty(polynomial.Polynomial))
                    {
                        var functionExpression = GetFunctionExpression(polynomial.Polynomial);
                        Func<double, double> func = x => functionExpression.calculate(x);

                        var color = GetOxyColor(polynomial.HexColor, polynomial.Color);
                        var series = new FunctionSeries(func, polynomial.MinXValue, polynomial.MaxXValue, polynomial.FloatingPoint)
                        {
                            Color = color,
                            StrokeThickness = polynomial.Thickness,
                            Title = polynomial.Description,
                            LineStyle = polynomial.LineStyle.ConvertEnum<LineStyle>(),
                            Smooth = polynomial.LineSmooth
                        };

                        if (polynomial.ShowLabel)
                        {
                            var labelPointX = GetLablePointX(polynomial.LabelPosition, polynomial.MinXValue, polynomial.MaxXValue);
                            var labelPointY = functionExpression.calculate(labelPointX);
                            plotModel.AddLabel(polynomial, labelPointX, labelPointY);
                        }
                        plotModel.Series.Add(series);
                    }
                }
            }
        }

        public static void AddElement(this PlotModel plotModel, IEnumerable<GraphPoint> points)
        {
            if (points != null)
            {
                foreach (var point in points)
                {
                    if (point != null)
                    {
                        var color = GetOxyColor(point.HexColor, point.Color);
                        var lineSeries = new LineSeries
                        {
                            MarkerType = MarkerType.Circle,
                            MarkerSize = point.Thickness,
                            Title = point.Description,
                            MarkerFill = color
                        };

                        if (point.ShowLabel)
                        {
                            lineSeries.LineLegendPosition = LineLegendPosition.End;
                            lineSeries.TextColor = color;
                        }

                        lineSeries.Points.Add(new DataPoint(point.PointX, point.PointY));
                        plotModel.Series.Add(lineSeries);
                    }
                }
            }
        }

        public static void AddElement(this PlotModel plotModel, IEnumerable<GraphLine> lines)
        {
            if (lines != null)
            {
                foreach (var line in lines)
                {
                    var color = GetOxyColor(line.HexColor, line.Color);
                    var lineSeries = new LineSeries()
                    {
                        Title = line.Description,
                        Color = GetOxyColor(line.HexColor, line.Color),
                        StrokeThickness = line.Thickness,
                        LineStyle = line.LineStyle.ConvertEnum<LineStyle>(),
                        Smooth = line.LineSmooth
                    };
                    line.Points
                        .Where(p => p != null)
                        .ToList()
                        .ForEach(p => lineSeries.Points.Add(new DataPoint(p.PointX, p.PointY)));

                    plotModel.Series.Add(lineSeries);

                    if (line.ShowLabel && line.Points != null)
                    {
                        if (line.Points.Count() > 0)
                        {
                            var minX = line.Points.Select(point => point.PointX).Min();
                            var maxX = line.Points.Select(point => point.PointX).Max();
                            var labelPointX = GetLablePointX(line.LabelPosition, minX, maxX);
                            if (line.LabelPosition == LabelPosition.Mid)
                            {
                                labelPointX = line.Points.Select(point => point.PointX).ToList().OrderBy(x => Math.Abs((long)x - labelPointX)).First();
                            }
                            var labelPointY = line.Points.SingleOrDefault(point => point.PointX == labelPointX).PointY;
                            plotModel.AddLabel(line, labelPointX, labelPointY);
                        }
                    }

                }
            }
        }

        private static double GetLablePointX(LabelPosition labelPosition, double minX, double maxX)
        {
            return labelPosition == LabelPosition.Mid
                ? (minX + maxX) / 2
                : labelPosition == LabelPosition.End
                    ? maxX
                    : minX;
        }

        public static void AddElement(this PlotModel plotModel, IEnumerable<GraphAnnotation> annotations)
        {
            if (annotations != null)
            {
                foreach (var annotation in annotations)
                {
                    TextualAnnotation item = null;
                    switch (annotation.AnnotationType)
                    {

                        case AnnotationType.Arrow:
                            item = new ArrowAnnotation
                            {
                                ArrowDirection = new ScreenVector(5, -5),
                                EndPoint = new DataPoint(annotation.PointX, annotation.PointY),
                                HeadLength = 4,
                                HeadWidth = 4,
                                Veeness = 0,
                                Color = GetOxyColor(annotation.HexColor, annotation.Color),
                                Text = annotation.Description
                            };
                            break;

                        case AnnotationType.Text:
                            item = new TextAnnotation
                            {
                                TextPosition = new DataPoint(annotation.PointX, annotation.PointY),
                                Text = annotation.Description,
                                TextColor = GetOxyColor(annotation.HexColor, annotation.Color),
                                TextHorizontalAlignment = (annotation.LabelHorizontalAlignment == AlignmentType.Right) ? HorizontalAlignment.Right : HorizontalAlignment.Left,
                                TextVerticalAlignment = (annotation.LabelVerticalAlignment == AlignmentType.Top) ? VerticalAlignment.Top : VerticalAlignment.Bottom,
                                StrokeThickness = 0,
                                Background = !string.IsNullOrEmpty(annotation.BackgroundColor) ? GetOxyColor(annotation.BackgroundColor) : OxyColors.Transparent
                            };
                            break;
                    }

                    plotModel.Annotations.Add(item);
                }
            }
        }

        private static void AddLabel(this PlotModel plotModel, GraphBase element, double pointX, double pointY)
        {
            if (element.ShowLabel)
            {
                var annotations = new List<GraphAnnotation>()
                {
                    new GraphAnnotation
                    {
                        PointX  = pointX,
                        PointY = pointY,
                        HexColor = element.HexColor,
                        Color = !string.IsNullOrEmpty(element.LabelColor) ? GetColor(element.LabelColor) : element.Color,
                        Description = element.Description,
                        AnnotationType = AnnotationType.Text,
                        LabelVerticalAlignment = element.LabelVerticalAlignment,
                        LabelHorizontalAlignment = element.LabelHorizontalAlignment
                    }
                };
                plotModel.AddElement(annotations);

            }
        }

        private static OxyColor GetOxyColor(string hexColor, Color? color = null, bool? transparent = false)
        {
            var rgb = !string.IsNullOrEmpty(hexColor) ? GetColor(hexColor) : color.Value;
            return transparent.Value
                ? OxyColor.FromArgb(150, rgb.R, rgb.G, rgb.B)
                : OxyColor.FromRgb(rgb.R, rgb.G, rgb.B);
        }

        private static Color GetColor(string hexColor)
        {
            return ColorTranslator.FromHtml(hexColor);
        }


    }
}
