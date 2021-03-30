using System;
using System.Linq;
using TokyoChokoku.MarkinBox.Sketchbook.Properties.Stores;
using TokyoChokoku.MarkinBox.Sketchbook.Properties;
using TokyoChokoku.MarkinBox.Sketchbook.Validators;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    /// <summary>
    /// ラベルと値をペアで指定し，表示はラベルで，実際の値は対応づいた値で取り扱えるようにします． 
    /// </summary>
    public class LabeledDropdownListCellSource<T> : SemiConcretedDropdownListCellSource<LabeledDropdownListCellSource<T>.LabeledValue>
    {
        public class LabeledValue
        {
            public string Label { get; }
            public T Value { get; }

            public LabeledValue(string label, T value)
            {
                Label = label;
                Value = value;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is LabeledValue))
                    return false;
                var r = (LabeledValue)obj;
                if (Label != r.Label)
                    return false;
                if (!Value.Equals(r))
                    return false;
                return true;
            }

            public override int GetHashCode()
            {
                int hash = 7;
                hash += 13 * Label.GetHashCode();
                hash += 13 * Value.GetHashCode();
                return hash; 
            }
        }

        public static Store<LabeledValue> WrapStore(Store<T> store, Func<T, string> labelGenerator)
        {
            string label = labelGenerator(store.Content);
            return new Store<LabeledValue>(
                (v) => store.Validator(v.Value),
                () => new LabeledValue(label, store.GetContent()),
                (v) => { store.SetContent(v.Value); label = v.Label; }
            );
        }

        public static LabeledDropdownListCellSource<T> Create(
            Store<T> store, T[] array, Func<T, string> labelGenerator, IEditBoxCommonDelegate commonDelegate)
        {
            var contents = array.Select((v)=>new LabeledValue(labelGenerator(v), v)).ToArray();
            return new LabeledDropdownListCellSource<T>(WrapStore(store, labelGenerator), contents, commonDelegate);
        }
        

        public LabeledDropdownListCellSource(Store<LabeledValue> store, LabeledValue[] items, IEditBoxCommonDelegate commonDelegate)
            : base(store, items, commonDelegate)
        {
        }

        public override string AsString(LabeledValue value)
        {
            return value.Label;
        }

        public override nint GetStartRow()
        {
            for (nint i = 0; i < items.Length; i++)
                if (items[i].Value.Equals(store.Content.Value))
                    return i;
            return 0;
        }
    }
}
