using System;
using System.Collections.Generic;
using TokyoChokoku.MarkinBox.Sketchbook;
using System.Linq;
using Monad;
using TokyoChokoku.MarkinBox.Sketchbook.Parameters;

namespace TokyoChokoku.Patmark
{
    public class FileContext
    {
        public readonly FileOwner Owner;
        public readonly IFileSource Source;
        public readonly QuickModeEditor QuickEditor;

        public FileContext(FileOwner owner, IFileSource source)
        {
            Owner = owner;
            Source = source;
            QuickEditor = new QuickModeEditor(this);
        }

        public Boolean isRemoteFile
        {
            get
            {
                return Source.GetType() == typeof(FileSourceFromRemote) || Source.GetType() == typeof(FileSourceFromLatest) || Owner.IsAdvanceMode;
            }
        }

        public Boolean isLocalFile
        {
            get
            {
                return !isRemoteFile;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return Owner.IsEmpty;
            }
        }

        public bool HasSerial
        {
            get
            {
                return Owner.HasSerial;
            }
        }

        public void AutoSave(){
            Source.Autosave(Owner);
        }

        public FileContext CloneWithOwner(FileOwner owner){
            return new FileContext(
                owner,
                Source.Clone
            );
        }

        public static FileContext Empty(){
            return new FileContext(
                new FileOwner(new List<MBData>()),
                new FileSourceFromOther()
            );
        }

        public class QuickModeEditor{
            FileContext Src;

            public QuickModeEditor(FileContext src){
                Src = src;
            }

            /// <summary>
            /// アドバンスモードの時は例外を発生させます。
            /// </summary>
            /// <returns>The text.</returns>
            /// <param name="text">Text.</param>
            public FileContext ApplyText(string text)
            {
                return Apply((data) =>
                {
                    data.Text = text;
                    return data;
                });
            }

            /// <summary>
            /// アドバンスモードの時は例外を発生させます。
            /// </summary>
            public FileContext Apply(Func<MBDataStructure, MBDataStructure> apply)
            {
                if (Src.isRemoteFile)
                {
                    // アドバンスモードの時は単にクローンします。
                    return Src.CloneWithOwner(new FileOwner(Src.Owner.Serializable));
                }
                else
                {
                    // 先頭の要素に新しいテキストを適用します。
                    var fields = Src.Owner.Serializable.ToList();
                    MBDataStructure first;
                    if (fields.Count == 0)
                    {
                        first = MutableHorizontalTextParameter.Create().ToSerializable().ToMutable();
                    }
                    else
                    {
                        first = fields[0].ToMutable();
                        fields.RemoveAt(0);
                    }

                    first = apply.Invoke(first);

                    fields.Insert(0, new MBData(first));

                    return Src.CloneWithOwner(new FileOwner(fields));
                }
            }
        }
    }
}
