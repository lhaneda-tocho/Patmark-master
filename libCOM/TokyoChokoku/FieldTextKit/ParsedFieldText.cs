using System;
using System.Collections.Generic;

using TokyoChokoku.FieldModel;
using TokyoChokoku.CalendarModule.Ast;
using TokyoChokoku.SerialModule.Ast;

namespace TokyoChokoku.FieldTextKit
{
    public sealed class ParsedFieldText: FieldText
    {
        ParsedFieldText(IEnumerable<FieldTextElement> elements) : base(elements)
        {}

        public static Factory CreateFactory(CalendarNodeProcessor cp, SerialNodeProcessor sp) {
            return new Factory(cp, sp);
        }

        public class Factory: FieldTextFactory {
            CalendarNodeProcessor CP { get; }
            SerialNodeProcessor   SP { get; }

            public Factory(CalendarNodeProcessor cp, SerialNodeProcessor sp)
            {
                CP = cp;
                SP = sp;
            }

            public override ParsedFieldText Parse(string text)
            {
                var cp = CP;
                var sp = SP;

                // 入れ物の準備
                var list = new LinkedList<FieldTextElement>();
                // パース
                var root = FieldTextParser.ParseText(text);

                // 解析
                var trav = new NodeTraverser();
                trav.OnLogo = (FTLogoNode obj) =>
                {
                    var id = obj.LogoIdentifier;
                    list.AddLast(new FieldTextLogo(id));
                };
                trav.OnText = (FTTextNode obj) =>
                {
                    Pour(obj.Text, list);
                };
                trav.OnSerial = (FTSerialNode obj) =>
                {
                    Pour(sp.Convert(obj), list);
                };
                trav.OnCalendar = (FTCalendarNode obj) =>
                {
                    Pour(cp.Convert(obj), list);
                };
                // 解析実行
                trav.Traverse(root);
                //Console.WriteLine("count: " + root.ElementCount());
                // インスタンス化
                return new ParsedFieldText(list);
            }


            public override ParsedFieldText ParseWithoutLogo(string text)
            {

                var cp = CP;
                var sp = SP;

                // 入れ物の準備
                var list = new LinkedList<FieldTextElement>();
                // パース
                var root = FieldTextParser.ParseTextWithoutLogo(text);

                // 解析
                var trav = new NodeTraverser();
                trav.OnLogo = (FTLogoNode obj) =>
                {
                    var id = obj.LogoIdentifier;
                    list.AddLast(new FieldTextLogo(id));
                };
                trav.OnText = (FTTextNode obj) =>
                {
                    Pour(obj.Text, list);
                };
                trav.OnSerial = (FTSerialNode obj) =>
                {
                    Pour(sp.Convert(obj), list);
                };
                trav.OnCalendar = (FTCalendarNode obj) =>
                {
                    Pour(cp.Convert(obj), list);
                };
                // 解析実行
                trav.Traverse(root);
                // インスタンス化
                return new ParsedFieldText(list);
            }



            static void Pour(string text, LinkedList<FieldTextElement> list)
            {
                foreach (var c in text)
                {
                    var ins = new FieldTextChar(c);
                    list.AddLast(ins);
                }
            }
        }
    }
}
