using System;
namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class MatchingGrammer
    {
        public static int? SearchSerialNoOrNull(string text)
        {
            var node = FieldTextParser.ParseText(text);
            if(node.HasSerial)
            {
                return node.SerialNodeList[0].SerialIndexOrNull;
            } else
            {
                return null;
            }
        }
    }
}

