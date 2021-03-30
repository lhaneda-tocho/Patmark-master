using TokyoChokoku.MarkinBox.Sketchbook.Properties.Stores;
namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public class ShortUpDownTextBoxCellSource : SemiConcretedUpDownTextBoxCellSource <short>
    {
        public ShortUpDownTextBoxCellSource (
            Store<short> store,
            string unit,
            short increment,
            IEditBoxCommonDelegate commonDelegate)
        : base (store, unit, increment, commonDelegate)
        {
            
        }


        public override bool TryParse (string raw, out short result)
        {
            return short.TryParse (raw, out result);
        }

        public override string GetDefaultValue ()
        {
            return "0";
        }

        public override string GetAsString ()
        {
            return store.Content.ToString ();
        }

        public override short Add (short left, short right)
        {
            return (short) (left + right);
        }

        public override short Sub (short left, short right)
        {
            return (short)(left - right);
        }
    }
}

