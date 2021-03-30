using System.IO;
using System.Collections.Generic;
using TokyoChokoku.MarkinBox.Sketchbook;

namespace TokyoChokoku.Patmark
{
    /// <summary>
    /// PPGファイルドライバです。
    /// TODO: iOS からアプリのフォルダを編集できるようになっている。ディレクトリあった場合の処理を考える。
    /// </summary>
    public class PPGFileIODriver : AbstractFileIODriver
    {
        protected override void WriteEmpty(LocalFilePath path)
        {
            var p = path.ToStringNormalized();

            using (TextWriter writer = File.CreateText(p))
            {
                MBDataTextizer.Save(writer, new List<MBData> { });
            }
        }

        protected override void Write(FileOwner owner, LocalFilePath path)
        {
            var p = path.ToStringNormalized();

            using (TextWriter writer = File.CreateText(p))
            {
                MBDataTextizer.Save(writer, owner.Serializable);
            }
        }

        protected override FileOwner Read(LocalFilePath path)
        {
            var text = File.ReadAllText(path.ToStringNormalized());
            return new FileOwner(
                MBDataTextizer
                    .Of(new StringReader(text))
                    .ToMBData()
            );
        }

        protected override string ReadToString(LocalFilePath path)
        {
            return File.ReadAllText(path.ToStringNormalized());
        }
    }

}

