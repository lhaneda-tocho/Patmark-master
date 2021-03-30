using System;
using System.Collections.Generic;

using TokyoChokoku.CalendarModule;

namespace TokyoChokoku.FieldTextKit
{
    public class FTRootFieldTextNode
    {
        public IEnumerable<IFieldTextNode> ElementEnumerable { get; }
        public bool HasCalendar { get; } = false;
        public bool HasSerial   { get; } = false;
        public bool HasLogo     { get; } = false;

        public FTRootFieldTextNode (IEnumerable<IFieldTextNode> next)
        {
            this.ElementEnumerable = next;

            foreach (var node in ElementEnumerable) {
                switch (node.FieldTextType) {
                case FieldTextType.Calendar:
                    HasCalendar = true;
                    break;
                case FieldTextType.Logo:
                    HasLogo = true;
                    break;
                case FieldTextType.Serial:
                    HasSerial = true;
                    break;
                default:
                    break;
                }
            }
        }

        public int ElementCount() {
            int count = 0;
            foreach (var node in ElementEnumerable) {
                int currentCount;
                switch (node.FieldTextType) {
                case FieldTextType.Text:
                    {
                        var textNode = ((FTTextNode)node);
                        currentCount = textNode.CharCount;
                        break;
                    }
                case FieldTextType.Calendar:
                    {
                        var calendarNode = ((FTCalendarNode)node);
                        currentCount = calendarNode.CharCount;
                        break;
                    }
                case FieldTextType.Logo:
                    {
                        // var logoNode = ((FTLogoNode)node);
                        currentCount = 1;
                        break;
                    }
                case FieldTextType.Serial:
                    {
                        var serialNode = ((FTSerialNode)node);
                        currentCount = serialNode.CharCount;
                        break;
                    }
                default:
                    {
                        throw new ArgumentOutOfRangeException ();
                    }
                }
                count += currentCount;
            }
            return count;
        }
    }
}

