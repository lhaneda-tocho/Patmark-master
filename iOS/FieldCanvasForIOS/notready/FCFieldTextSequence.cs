using System;
using System.Linq;
using System.Collections.Generic;
using TokyoChokoku.FieldTextKit;
using TokyoChokoku.CalendarModule.Ast;
using TokyoChokoku.SerialModule.Ast;

using TokyoChokoku.Patmark.RenderKit.Value;
using TokyoChokoku.Patmark.RenderKit.Transform;
using TokyoChokoku.Patmark.iOS.RenderKitForIOS;
using System.Collections;

namespace TokyoChokoku.Patmark.iOS.FieldCanvasForIOS
{
    /// <summary>
    /// FCField text sequence.
    /// </summary>
    public class FCFieldTextSequence: IEnumerable<FCRenderable>
    {
        readonly LinkedList<FCRenderable> renderableList = new LinkedList<FCRenderable>();

        public static FCFieldTextSequence Parse(string text, CalendarNodeProcessor cp, SerialNodeProcessor sp) {
            var root = FieldTextParser.ParseText(text);
            return new FCFieldTextSequence(root, cp, sp);
        }

        public static FCFieldTextSequence ParseWithoutLogo(string text, CalendarNodeProcessor cp, SerialNodeProcessor sp) {
            var root = FieldTextParser.ParseTextWithoutLogo(text);
            return new FCFieldTextSequence(root, cp, sp);
        }

        public FCFieldTextSequence(FTRootFieldTextNode root, CalendarNodeProcessor cp, SerialNodeProcessor sp)
        {
            var trav = new NodeTraverser();
            trav.OnText = (FTTextNode node) => {
                foreach(var c in node.Text) {
                    var r = new FCChar();
                    r.Char = c;
                    renderableList.AddLast(r);
                }
            };
            trav.OnLogo = (FTLogoNode node) => {
                var r = new FCLogoFrame();
                renderableList.AddLast(r);
            };
            trav.OnCalendar = (FTCalendarNode node) => {
                foreach (var c in cp.Convert(node))
                {
                    var r = new FCChar();
                    r.Char = c;
                    renderableList.AddLast(r);
                }
            };
            trav.OnSerial = (FTSerialNode node) => {
                foreach (var c in sp.Convert(node))
                {
                    var r = new FCChar();
                    r.Char = c;
                    renderableList.AddLast(r);
                }
            };
            // 解析実行
            trav.Traverse(root);
        }

        public IEnumerator<FCRenderable> GetEnumerator()
        {
            return ((IEnumerable<FCRenderable>)renderableList).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<FCRenderable>)renderableList).GetEnumerator();
        }
    }
}
