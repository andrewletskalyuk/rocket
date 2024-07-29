using System.Collections.Generic;

namespace Armstrong.Core.Services.CurveDrawing.Models.CurveSeries
{
	public class DesignEnvelopePoint
	{
		/// <summary>
		/// Flow in USgpm
		/// </summary>
		public double Flow { get; set; }

		/// <summary>
		/// Head in feet
		/// </summary>
		public double Head { get; set; }
	}
}