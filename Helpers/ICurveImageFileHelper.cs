using Armstrong.Core.Services.CurveDrawing.Models.CurveSeries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Armstrong.Core.Services.CurveDrawing.Helpers
{
    interface ICurveImageFileHelper
    {
        string CreateImage(CurveSeriesRoot curveData, string imagePath);
    }
}
