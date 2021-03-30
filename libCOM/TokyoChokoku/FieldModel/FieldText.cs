using System;
using System.Linq;
using System.Collections.Generic;

using System.Text;
using TokyoChokoku.FieldTextKit;
using TokyoChokoku.CalendarModule.Ast;
using TokyoChokoku.SerialModule.Ast;
namespace TokyoChokoku.FieldModel
{
    /// <summary>
    /// Field text factory.
    /// </summary>
    public abstract class FieldTextFactory
    {
        public abstract ParsedFieldText Parse(string text);
        public abstract ParsedFieldText ParseWithoutLogo(string text);

        public static FieldTextFactory Create(CalendarNodeProcessor cp, SerialNodeProcessor sp) {
            return ParsedFieldText.CreateFactory(cp, sp);
        }
    }

    /// <summary>
    /// Field text.
    /// </summary>
    public class FieldText
    {
        IList<FieldTextElement> List;
        public int Count => List.Count;

        public FieldTextElement this[int index] {
            get {
                return List[index];
            }
        }

        public FieldText(IEnumerable<FieldTextElement> elements)
        {
            List = elements.ToList();
        }

        public static FieldText Parseless(string text) {
            var list = from c in text
                       select new FieldTextChar(c);
            return new FieldText(list);
        }

        public static FieldText Parse(string text, CalendarNodeProcessor cp, SerialNodeProcessor sp) {
            return FieldTextFactory.Create(cp, sp).Parse(text);
        }

        public static FieldText ParseWithoutLogo(string text, CalendarNodeProcessor cp, SerialNodeProcessor sp)
        {
            return FieldTextFactory.Create(cp, sp).ParseWithoutLogo(text);
        }

        public IList<FieldText> SplitPer(int count) {
            if (count <= 0)
                throw new InvalidOperationException("count must not be zero or negative.");
            
            var newlist = new List<FieldText>(List.Count / count);

            for (int i = 0; i < Count; i+=count) {
                var sublist = new List<FieldTextElement>(count);
                for (int j = 0; j < count; j++) {
                    var index = i + j;
                    if (i + j >= Count)
                        break;
                    sublist.Add(List[index]);
                }
                newlist.Add(new FieldText(sublist));
            }

            Console.WriteLine("newlist: " + newlist.Count);

            return newlist;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            var res = from e in List
                      select e.ToString();
            foreach(var chip in res) {
                sb.Append(chip);
            }
            return sb.ToString();
        }
    }


    public enum FieldTextElementType
    {
        Char, Logo,
    }

    public interface FieldTextElement
    {
        FieldTextElementType Type { get; }
    }

    public class FieldTextChar: FieldTextElement
    {
        public FieldTextElementType Type { get; } = FieldTextElementType.Char;

        public char Char { get; }
        public FieldTextChar(char c)
        {
            Char = c;
        }

        public override string ToString()
        {
            return Char.ToString();
        }
    }

    public class FieldTextLogo: FieldTextElement
    {
        public FieldTextElementType Type { get; } = FieldTextElementType.Logo;

        public int id { get; }
        public FieldTextLogo(int id)
        {
            this.id = id;
        }

        public override string ToString()
        {
            return String.Format("@L[{0:D2}]", id);
        }
    }

    public static class FieldTextElementExt
    {
        public static string GetName(this FieldTextElementType self) {
            return Enum.GetName(typeof(FieldTextElementType), self);
        }

        public static R Match<R>(this FieldTextElementType type,
                                 Func<FieldTextElementType, R> isChar,
                                 Func<FieldTextElementType, R> isLogo) {
            switch(type) {
                case FieldTextElementType.Char:
                    return isChar(type);
                case FieldTextElementType.Logo:
                    return isLogo(type);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public static void Do(this FieldTextElementType type,
                              Action<FieldTextElementType> isChar,
                              Action<FieldTextElementType> isLogo)
        {
            switch (type)
            {
                case FieldTextElementType.Char:
                    isChar(type); break;
                case FieldTextElementType.Logo:
                    isLogo(type); break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static R Match<R>(this FieldTextElement element,
                                Func<FieldTextChar, R> isChar,
                                Func<FieldTextLogo, R> isLogo) {
            return element.Type.Match(
                isChar: (type) => isChar((FieldTextChar) element),
                isLogo: (type) => isLogo((FieldTextLogo) element)
            );
        }

        public static void Do(this FieldTextElement element,
                              Action<FieldTextChar> isChar,
                              Action<FieldTextLogo> isLogo) {
            element.Type.Do(
                isChar: (type) => isChar((FieldTextChar)element),
                isLogo: (type) => isLogo((FieldTextLogo)element)
            );
        }
    }
}
