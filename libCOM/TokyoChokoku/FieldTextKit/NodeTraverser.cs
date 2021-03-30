using System;
namespace TokyoChokoku.FieldTextKit
{
    public class NodeTraverser
    {
        public Action<FTTextNode    > OnText     { get; set; }
        public Action<FTLogoNode    > OnLogo     { get; set; }
        public Action<FTCalendarNode> OnCalendar { get; set; }
        public Action<FTSerialNode  > OnSerial   { get; set; }

        public void Traverse(FTRootFieldTextNode root)
        {
            foreach (var node in root.ElementEnumerable)
            {
                switch (node.FieldTextType)
                {
                    case FieldTextType.Text:
                        {
                            OnText?.Invoke((FTTextNode)node);
                            break;
                        }
                    case FieldTextType.Calendar:
                        {
                            OnCalendar?.Invoke((FTCalendarNode)node);
                            break;
                        }
                    case FieldTextType.Logo:
                        {
                            OnLogo?.Invoke((FTLogoNode)node);
                            break;
                        }
                    case FieldTextType.Serial:
                        {
                            OnSerial?.Invoke((FTSerialNode)node);
                            break;
                        }
                    default:
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                }
            }
        }
    }
}
