using System;
using TokyoChokoku.Patmark.EmbossmentKit;

namespace TokyoChokoku.Patmark.iOS.Presenter.Embossment
{
    public class EmbossmentUIItems : MutableEmbossmentMode
    {
        #region Label
        public string TextSizeLabel
        {
            get
            {
                return TextSize.GetName();
            }
        }

        public string ForceLabel
        {
            get
            {
                return Force.GetName();
            }
        }

        public string QualityLabel
        {
            get
            {
                return Quality.GetName();
            }
        }
        #endregion

        #region Wiring
        public void SetTextSizeWithSegment(nint segment)
        {
            var next = TextSize;
            switch (segment)
            {
                case 0: next = TextSizeLevel.Small; break;
                case 1: next = TextSizeLevel.Medium; break;
                case 2: next = TextSizeLevel.Large; break;
                default: break;
            }
            TextSize = next;
        }

        public void SetForceWithSegment(nint segment)
        {
            var next = Force;
            switch (segment)
            {
                case 0: next = ForceLevel.Weak; break;
                case 1: next = ForceLevel.Medium; break;
                case 2: next = ForceLevel.Strong; break;
                default: break;
            }
            Force = next;
        }

        public void SetQualityWithSegment(nint segment)
        {
            var next = Quality;
            switch (segment)
            {
                case 0: next = QualityLevel.Dot; break;
                case 1: next = QualityLevel.Medium; break;
                case 2: next = QualityLevel.Line; break;
                default: break;
            }
            Quality = next;
        }
        #endregion
    }
}
