using System;
namespace TokyoChokoku.Communication
{
    /// <summary>
    /// MBAlert.
    /// </summary>
    public enum MBAlert : ushort
    {
        None = 0, // 異常無し
        Who = 52, // 刻印実行時異常
        Encoding = 58 // 打刻フィールドの中に、コントローラで解釈できない文字コードが含まれていた
    }
    public static class MBAlertExt
    {
        public static MBAlert ToMBAlert(this ushort value)
        {
            return (MBAlert)value;
        }
        public static MBAlert ToMBAlert(this short value)
        {
            return (MBAlert)value;
        }
    }

    /// <summary>
    /// MBMarking mode.
    /// </summary>
    public enum MBMarkingMode : byte
    {
        Pc = 0,
        Permanent = 1
    }
    public static class MBMarkingModeExt
    {
        public static MBMarkingMode ToMBMarkingMode(this byte value)
        {
            return (MBMarkingMode)value;
        }
        public static MBMarkingMode ToMBMarkingMode(this sbyte value)
        {
            return (MBMarkingMode)value;
        }
    }

    /// <summary>
    /// MBMarking status.
    /// </summary>
    public enum MBMarkingStatus : ushort
    {
        Stop = 0,
        Start = 1
    }
    public static class MBMarkingStatusExt
    {
        public static MBMarkingStatus ToMBMarkingStatus(this ushort value)
        {
            return (MBMarkingStatus)value;
        }
        public static MBMarkingStatus ToMBMarkingStatus(this short value)
        {
            return (MBMarkingStatus)value;
        }
    }

    /// <summary>
    /// MBMarking result.
    /// </summary>
    public enum MBMarkingResult : ushort
    {
        Success = 0,
        Failure = 1
    }
    public static class MBMarkingResultExt
    {
        public static MBMarkingResult ToMBMarkingResult(this ushort value)
        {
            return (MBMarkingResult)value;
        }
        public static MBMarkingResult ToMBMarkingResult(this short value)
        {
            return (MBMarkingResult)value;
        }
    }

    /// <summary>
    /// MBMarking pausing status.
    /// </summary>
    public enum MBMarkingPausingStatus : byte
    {
        None = 0,
        Pause = 1,
        Resume = 2,
        Stop = 3
    }
    public static class MBMarkingPausingStatusExt
    {
        public static MBMarkingPausingStatus ToMBMarkingPausingStatus(this byte value)
        {
            return (MBMarkingPausingStatus)value;
        }
        public static MBMarkingPausingStatus ToMBMarkingPausingStatus(this sbyte value)
        {
            return (MBMarkingPausingStatus)value;
        }
    }

    /// <summary>
    /// MBMachine model.
    /// </summary>
    public enum MBMachineModel : ushort
    {
        No3315 = 0,
        No1010 = 2,
        No8020 = 3,
        No2015 = 4,
    }

    public enum MBHeadButtonMarkingAbility : ushort
    {
        Enabled = 0,
        Disabled = 1
    }
    public static class MBHeadButtonMarkingAbilityExt
    {
        public static MBHeadButtonMarkingAbility ToMBHeadButtonMarkingAbility(this ushort value)
        {
            return (MBHeadButtonMarkingAbility)value;
        }
        public static MBHeadButtonMarkingAbility ToMBHeadButtonMarkingAbility(this short value)
        {
            return (MBHeadButtonMarkingAbility)value;
        }
    }

    public static class MBRemoteMarkingFileNo
    {
        public const short PC     = 999;
        public const short MBMode = 256;
    }

    public static class MBMachineModelExt {

        public static MBMachineModel ToMBMachineMode(this ushort value)
        {
            return (MBMachineModel)value;
        }
        public static MBMachineModel ToMBMachineMode(this short value)
        {
            return (MBMachineModel)value;
        }

        public class IntSize2D
        {
            public int X { get; private set; }
            public int Y { get; private set; }

            public IntSize2D(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        public static IntSize2D LatticeSize(this MBMachineModel model)
        {
            switch (model)
            {
                case MBMachineModel.No3315:
                    return new IntSize2D(33, 15);
                case MBMachineModel.No1010:
                    return new IntSize2D(100, 100);
                case MBMachineModel.No8020:
                    return new IntSize2D(80, 20);
                case MBMachineModel.No2015:
                    return new IntSize2D(200, 150);
                default:
                    throw new ArgumentOutOfRangeException("MachineModelExt.LatticeSize - ケース設定に漏れがあります。");
            }
        }

        // ヘッド移動オプション

        public class HeadMovingSpan
        {
            public float X { get; private set; }
            public float Y { get; private set; }

            public HeadMovingSpan(float x, float y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        public class HeadMovingPulse
        {
            public float X { get; private set; }
            public float Y { get; private set; }

            public HeadMovingPulse(float x, float y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        private static HeadMovingSpan Span3315 = new HeadMovingSpan(
            33.0f,
            15.0f
        );
        private static HeadMovingPulse Pulse3315 = new HeadMovingPulse(
            7600.0f,
            3620.0f
        );


        private static HeadMovingSpan Span1010 = new HeadMovingSpan(
            100.0f,
            100.0f
        );
        private static HeadMovingPulse Pulse1010 = new HeadMovingPulse(
            7640.0f,
            7640.0f
        );


        private static HeadMovingSpan Span8020 = new HeadMovingSpan(
            80.0f,
            20.0f
        );
        private static HeadMovingPulse Pulse8020 = new HeadMovingPulse(
            14230.0f,
            3720.0f
        );


        private static HeadMovingSpan Span2015 = new HeadMovingSpan(
            200.0f,
            150.0f
        );
        private static HeadMovingPulse Pulse2015 = new HeadMovingPulse(
            15280.0f,
            11460.0f
        );


        public static HeadMovingSpan StandardHeadMovingSpan(this MBMachineModel model)
        {
            switch (model)
            {
                case MBMachineModel.No3315:
                    return Span3315;
                case MBMachineModel.No1010:
                    return Span1010;
                case MBMachineModel.No8020:
                    return Span8020;
                case MBMachineModel.No2015:
                    return Span2015;
                default:
                    throw new ArgumentOutOfRangeException("MachineModelExt.StandardHeadMovingSpan - ケース設定に漏れがあります。");
            }
        }

        public static HeadMovingPulse StandardHeadMovingPulse(this MBMachineModel model)
        {
            switch (model)
            {
                case MBMachineModel.No3315:
                    return Pulse3315;
                case MBMachineModel.No1010:
                    return Pulse1010;
                case MBMachineModel.No8020:
                    return Pulse8020;
                case MBMachineModel.No2015:
                    return Pulse2015;
                default:
                    throw new ArgumentOutOfRangeException("MachineModelExt.StandardHeadMovingPulse - ケース設定に漏れがあります。");
            }
        }
    }
}
