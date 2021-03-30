using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace TokyoChokoku.Bppg.V1
{
    public class BPPGFileV1: BPPGFile
    {
        public BPPGFormatVersion FormatVersion => BPPGFile.FormatVersion1_0;

        public IList<BPPGFieldData> FieldList { get; }

        /// <summary>
        /// 読み取ったフィールドリストを指定して初期化します。
        /// </summary>
        /// <param name="fieldList">読み取ったフィールドリスト</param>
        public BPPGFileV1(IList<BPPGFieldDataV1> fieldList)
        {
            fieldList = fieldList ?? throw new ArgumentNullException();
            FieldList = ImmutableList.CreateRange<BPPGFieldData>(fieldList);
        }
    }
}
