using System;
namespace TokyoChokoku.Patmark.Droid.Custom
{
    public class AspectRatio
    {
        public enum SpecMode { Exactly, Elastic, Unspecified }

		public float Value { get; }
        public SpecMode Mode { get; }

        public bool IsExactry => Mode == SpecMode.Exactly;
        public bool IsElastic => Mode == SpecMode.Elastic;

        public bool IsUnspecified => Mode == SpecMode.Unspecified;
        public bool IsSpecified => !IsUnspecified;

        AspectRatio()
        {
            Value = 0f;
            Mode = SpecMode.Unspecified;
        }

        public AspectRatio(SpecMode mode, float value) {
            if (value <= 0f) {
                Value = 0f;
                Mode = SpecMode.Unspecified;
            } else {
                Value = value;
                Mode = mode;
            }
        }

        public static AspectRatio Exact(float value) {
            return new AspectRatio(SpecMode.Exactly, value);
        }
        public static AspectRatio Elastic(float value) {
            return new AspectRatio(SpecMode.Elastic, value);
        }
        public static AspectRatio Unspecified { get; } = new AspectRatio();
    }
}
