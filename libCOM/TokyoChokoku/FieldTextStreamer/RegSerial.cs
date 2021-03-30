using System;
using System.Text;
using System.Text.RegularExpressions;

namespace TokyoChokoku.FieldTextStreamer
{
    /// <summary>
    /// シリアルの正規表現です．
    /// </summary>
    public static class RegSerial
    {
        public abstract class Result
        {
            public Match      Match            { get; }
            public string     HeadText         { get; }
            public string     TailText         { get; }
            public SerialPart SerialPart       { get; }


            public bool Success      => Match.Success;
            public int  Index        => Match.Index;
            public int  Length       => Match.Length;
            public int  EndExclusive => Index + Length;


            public GroupCollection Groups => Match.Groups;

            internal Result(string source, Match match)
            {
                if(source == null)
                    throw new ArgumentNullException(nameof(source));
                Match = match ?? throw new ArgumentNullException(nameof(match));
                if(!match.Success)
                {
                    SerialPart = null;
                    HeadText = "source";
                    TailText = "";
                    return;
                }
                var g = match.Groups;
                var value = int.Parse(g[1].Value);
                var id    = int.Parse(g[2].Value);
                SerialPart = new SerialPart(id, value);

                HeadText = source.Substring(0, Index);
                TailText = source.Substring(EndExclusive);
            }
        }

        class ConcreteResult : Result
        {
            public ConcreteResult(string source, Match match) : base(source, match)
            {
            }
        }

        /// <summary>
        /// 正規表現パターンです．
        /// </summary>
        public static Regex Reg = new Regex(@"@S\[(\d{1,4})-(\d{1})\]");

        /// <summary>
        /// フィールドテキストとマッチします．
        /// </summary>
        /// <returns>一致オブジェクト</returns>
        /// <param name="source">フィールドテキスト</param>
        public static Result Match(string source)
        {
            var m = Reg.Match(source);
            return new ConcreteResult(source, m);
        }
    }
}
