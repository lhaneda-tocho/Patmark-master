using System;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
    using TokyoChokoku.MarkinBox.Sketchbook;

	public static class MBMemories{

		public enum Alert : short
		{
			None = 0, // 異常無し
			Who = 52, // 刻印実行時異常
			Encoding = 58 // 打刻フィールドの中に、コントローラで解釈できない文字コードが含まれていた
		}

		public enum MarkingMode : byte
		{
			Pc = 0,
			Permanent = 1
		}

		public enum MarkingStatus : short
		{
			Stop = 0,
			Start = 1
		}

		public enum MarkingResult : short
		{
			Success = 0,
			Failure = 1
		}

		public enum MarkingPausingStatus : byte
		{
			None = 0,
			Pause = 1,
			Resume = 2,
			Stop = 3
		}

		public enum MachineModel : short
		{
			No3315 = 0,
			No1010 = 2,
			No8020 = 3,
			No2015 = 4,
		}

		public enum HeadButtonMarkingAbility : short
		{
			Enabled = 0,
			Disabled = 1
		}

		public const short RemoteMarkingFileNoPC = 999;
	}

    public static class MachineModelExt{

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

        public static IntSize2D LatticeSize(this MBMemories.MachineModel model)
        {
            switch (model)
            {
                case MBMemories.MachineModel.No3315:
                    return new IntSize2D(33, 15);
                case MBMemories.MachineModel.No1010:
                    return new IntSize2D(100, 100);
				case MBMemories.MachineModel.No8020:
					return new IntSize2D(80, 20);
                case MBMemories.MachineModel.No2015:
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


        public static HeadMovingSpan StandardHeadMovingSpan(this MBMemories.MachineModel model)
        {
            switch (model)
            {
                case MBMemories.MachineModel.No3315:
                    return Span3315;
                case MBMemories.MachineModel.No1010:
                    return Span1010;
				case MBMemories.MachineModel.No8020:
					return Span8020;
				case MBMemories.MachineModel.No2015:
					return Span2015;
                default:
                    throw new ArgumentOutOfRangeException("MachineModelExt.StandardHeadMovingSpan - ケース設定に漏れがあります。");
            }
        }

        public static HeadMovingPulse StandardHeadMovingPulse(this MBMemories.MachineModel model)
        {
            switch (model)
            {
                case MBMemories.MachineModel.No3315:
                    return Pulse3315;
                case MBMemories.MachineModel.No1010:
                    return Pulse1010;
				case MBMemories.MachineModel.No8020:
					return Pulse8020;
				case MBMemories.MachineModel.No2015:
					return Pulse2015;
                default:
                    throw new ArgumentOutOfRangeException("MachineModelExt.StandardHeadMovingPulse - ケース設定に漏れがあります。");
            }
        }
    }
}

