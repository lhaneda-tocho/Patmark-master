using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    using Fields;
    using Parameters;

    public static partial class FieldFactory
    {
        public static
        List<IField<IConstantParameter>> ParsePpg (string ini)
        {
            var textizer = MBDataTextizer.Of (new StringReader (ini));
            return Create (textizer.ToMBData ());
        }

        public static
        List<IMutableField<IMutableParameter>> ParsePpgAsMutable (string ini)
        {
            var textizer = MBDataTextizer.Of (new StringReader (ini));
            return CreateMutable (textizer.ToMBData ());
        }



        /// <summary>
        /// MBDataのリストから IFieldを 作成します．
        /// MBDataの値が不正であり，読み取ることができなかった場合は リストから除外されます
        /// リスト中の要素に null が入ると <code>NullReferenceException</code>
        /// が発生します．
        /// </summary>
        /// <param name="raws">Raws.</param>
        public static
        List<IField<IConstantParameter>> Create (ICollection<MBData> raws)
        {
            return raws
                .Select ((raw) => Create (raw))
                .Where  ((raw) => raw != null)
                .ToList ();
        }


        /// <summary>
        /// MBDataのリストから IMutableFieldを 作成します．
        /// MBDataの値が不正であり，読み取ることができなかった場合は リストから除外されます
        /// リスト中の要素に null が入ると <code>NullReferenceException</code>
        /// が発生します．
        /// </summary>
        /// <param name="raws">Raws.</param>
        public static
        List<IMutableField<IMutableParameter>> CreateMutable (List<MBData> raws)
        {
            return raws
                .Select ((raw) => CreateMutable (raw))
                .Where ((raw) => raw != null)
                .ToList ();
        }


        /// <summary>
        /// Create (MBData)中で実装し忘れがあると発生する例外です．
        /// この例外をキャッチするべきではありません．
        /// </summary>
        sealed class ForgottenToImplementFactoryException : Exception
        {}
    }
}

