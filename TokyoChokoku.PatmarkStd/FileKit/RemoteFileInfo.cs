using System;
using System.Text;
using System.Text.RegularExpressions;
namespace TokyoChokoku.Patmark
{
    public class RemoteFileInfo
    {
        //private Regex FileNameHead = ;

        // ===== ===== ===== =====

        public string Name { get; set; } = "";
        public int NumOfField { get; set; } = 0;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">コントローラから読み出したファイル名。</param>
        /// <param name="numOfField">フィールド名</param>
        public RemoteFileInfo(string name, int numOfField)
        {
            Name = name;
            NumOfField = numOfField;
        }


        /// <summary>
        /// ファイル番号 (1から始まる番号) を指定して UI に表示するファイル名を返します。
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public string GetDisplayNameWithFileNo(int number)
        {
            return this.GetDisplayNameWithIndex(number - 1);
        }


        /// <summary>
        /// ファイルインデックス (0から始まる番号) を指定して UI に表示するファイル名を返します。
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public string GetDisplayNameWithIndex(int index)
        {
            string name = "";
            if (NumOfField > 0)
                name = Name;
            return name;
        }
    }
}
