using System.Collections.Generic;

namespace Armstrong.Core.Services.CurveDrawing.Models.CurveSeries
{
    public class DeBoosterEnvelope
	{
		public IEnumerable<DesignEnvelopePoint> Envelopes { get; set; }
		public int BoosterEnvelopeId { get; set; }
		public byte RegionId { get; set; }
		public string BoosterSeries { get; set; }
		public decimal OverlapIndex { get; set; }
		public string EnvelopeDataCsv { get; set; }
		public string PumpSeries { get; set; }
		public string PumpSize { get; set; }
		public decimal? MotorSize { get; set; }
		public decimal? MotorSpeed { get; set; }
		public decimal? PumpSpeed { get; set; }
		public decimal? RunQuantity { get; set; }
		public byte? Red { get; set; }
		public byte? Green { get; set; }
		public byte? Blue { get; set; }
		public string ModelNum { get; set; }
		public int? GraphLabelIndex { get; set; }
		public byte? GraphHorzAlign { get; set; }
		public byte? GraphVertAlign { get; set; }
		public decimal? Bep { get; set; }
		public string BepCurveDataCsv { get; set; }
		public byte? FlowUnitIndex { get; set; }
		public byte? HeadUnitIndex { get; set; }
		public bool? StandbyOptionAvailable { get; set; }
		public bool? IsStandbyPump { get; set; }
		public decimal? StandbyQuantity { get; set; }
		public decimal? TotalQuantity { get; set; }
	}
}