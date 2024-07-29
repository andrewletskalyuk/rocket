using System;
namespace Armstrong.Services.CurveDrawing.Helpers
{
    public static class ConvertHelper
    {
        public static TEnum ConvertEnum<TEnum>(this Enum source)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), source.ToString(), true);
        }
    }
}