using TokyoChokoku.MarkinBox.Sketchbook.Properties.Stores;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public class DecimalUpDownTextBoxCellSource : SemiConcretedUpDownTextBoxCellSource <decimal>
    {
        public DecimalUpDownTextBoxCellSource (
            Store<decimal> store,
            string unit,
            decimal increment, IEditBoxCommonDelegate commonDelegate)
            : base (store, unit, increment, commonDelegate)
        {

        }

        /// <summary>
        /// ユーザが入力したテキストを引数にもらい，内容の解釈を行います
        /// </summary>
        /// <returns>The parse.</returns>
        /// <param name="raw">Raw.</param>
        /// <param name="result">Result.</param>
        public override bool TryParse (string raw, out decimal result)
        {
            return decimal.TryParse (raw, out result);
        }

        public override string GetDefaultValue ()
        {
            return "0";
        }

        public override string GetAsString ()
        {
            var simplified = decimal.Truncate (store.Content * 10m) / 10m;
            return simplified.ToString ();
        }

        public override decimal Add (decimal left, decimal right)
        {
            return left + right;
        }

        public override decimal Sub (decimal left, decimal right)
        {
            return left - right;
        }
    }
}

