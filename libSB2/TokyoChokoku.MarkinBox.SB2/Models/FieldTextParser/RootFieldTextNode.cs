using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class RootFieldTextNode
    {
        public IEnumerable<IFieldTextNode> ElementEnumerable { get; }
        public bool HasCalendar { get; } = false;
        public bool HasSerial   { get; } = false;
        public bool HasLogo     { get; } = false;
        public IList<SerialNode> SerialNodeList { get; }

        public RootFieldTextNode (IEnumerable<IFieldTextNode> next)
        {
            this.ElementEnumerable = next;

            var serialNodeList = new List<SerialNode>();

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
                    serialNodeList.Add((SerialNode)node);
                    break;
                default:
                    break;
                }
            }

            SerialNodeList = serialNodeList.ToImmutableList() ?? throw new InvalidOperationException("Assertion Error");
        }

        public int ElementCount() {
            int count = 0;
            foreach (var node in ElementEnumerable) {
                int currentCount;
                switch (node.FieldTextType) {
                case FieldTextType.Text:
                    {
                        var textNode = ((TextNode)node);
                        currentCount = textNode.CharCount();
                        break;
                    }
                case FieldTextType.Calendar:
                    {
                        var calendarNode = ((CalendarNode)node);
                        currentCount = calendarNode.CharCount ();
                        break;
                    }
                case FieldTextType.Logo:
                    {
                        // var logoNode = ((LogoNode)node);
                        currentCount = 1;
                        break;
                    }
                case FieldTextType.Serial:
                    {
                        var serialNode = ((SerialNode)node);
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

