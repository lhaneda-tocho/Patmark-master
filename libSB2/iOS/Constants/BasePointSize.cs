using System;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public static class BasePointSize
    {
        private const float IOS_DEBUG = 6.0f;

        public static float GetCurrent () {
            return IOS_DEBUG;
        }
    }
}

