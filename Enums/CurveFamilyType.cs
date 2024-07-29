namespace Armstrong.Core.Services.CurveDrawing.Enums
{
    // COM
    public enum CurveFamilyType
    {
        DesignEnvelopeRegion = 0,
        WorklogCurves = 1,
        MinMaxCurves = 2,
        DynamicCurves = 3,
        MotorPowerLines = 4,
        EfficiencyLines = 5,
        InBetweenVarSpeeds = 6,
        SafetyFactorRegion = 7,
        AllDiscreteBeltDriveRPM = 8,
        AllSizes = 9,
        AllDiscreteRPM = 10, // 0x0000000A
        PumpInBoxCurves = 11, // 0x0000000B
        Slopes = 12, // 0x0000000C
        ViscosityCorrectionRegion = 13, // 0x0000000D
        MultiStage = 14, // 0x0000000E
        FMFireMarkerLines = 16, // 0x00000010
        NPSHRCurves = 17, // 0x00000011
        LPCMarkerLines = 18, // 0x00000012
        DynamicLPCFSysCurves = 19, // 0x00000013
        DynamicLPCFCurves = 20, // 0x00000014
    }
}
