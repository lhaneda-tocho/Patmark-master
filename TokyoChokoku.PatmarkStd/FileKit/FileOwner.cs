using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using TokyoChokoku.MarkinBox.Sketchbook;
using System.Linq;

namespace TokyoChokoku.Patmark
{
    /// <summary>
    /// フィールドリストの格納場所として利用します。
    /// </summary>
    public class FileOwner
    {
        public ImmutableList<MBData> Fields{
            get; private set;
        }

        public bool IsEmpty
        {
            get
            {
                var fields = Fields;
                if (ReferenceEquals(fields, null))
                    return true;
                if (fields.Count == 0)
                    return true;
                if (IsQuickMode)
                {
                    return string.IsNullOrWhiteSpace(Fields[0].Text);
                } else
                {
                    fields = EmptyFieldRemover.FilterNotEmpty(fields).ToImmutableList();
                    if (fields.IsEmpty)
                        return true;
                }
                return false;               
            }
        }

        public bool HasSerial
        {
            get
            {
                var fields = Fields;
                if (ReferenceEquals(fields, null))
                    return false;
                if (fields.Count == 0)
                    return false;
                foreach(var field in fields)
                {
                    if (field.HasSerial())
                        return true;
                }
                return false;
            }
        }

        public bool IsQuickMode
        {
            get {
                return Fields.Count() == 0 || (
                    Fields.Count() == 1 && 
                    Fields[0].Type == MBConstants.FieldTypeText
                );
            }
        }

        public bool IsAdvanceMode
        {
            get { return !IsQuickMode; }
        }

        public FileOwner(IEnumerable<MBData> fields)
        {
            Fields = fields.ToImmutableList();
        }

        public List<FieldReader> Readers
        {
            get
            {
                return (
                    from field in Fields
                    select FieldReader.CreateWithGlobalSource(field)
                ).ToList();
            }
        }

        /// <summary>
        /// コピーが発生するため、パフォーマンスの低下に注意してください。
        /// </summary>
        /// <value>The serializable.</value>
        public IEnumerable<MBData> Serializable
        {
            get
            {
                return from f in Fields
                    select new MBData(f.ToMutable());
            }
        }
    }
}
