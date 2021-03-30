using System;
using TokyoChokoku.Structure;
using TokyoChokoku.MarkinBox.Sketchbook;

using TokyoChokoku.FieldTextStreamer;
using TokyoChokoku.SerialModule.Setting;

namespace TokyoChokoku.Patmark
{
    public class FieldReader
    {
        /// <summary>
        /// グローバル変数から設定を読み取る FieldReader を作成します。
        /// </summary>
        /// <param name="field">読み取りたいフィールドバイナリ</param>
        /// <returns>FieldReader インスタンス</returns>
        public static FieldReader CreateWithGlobalSource(MBData field)
        {
            return new FieldReader(field,
                serialSettingSource: () =>
                {
                    return SerialGlobalSetting.ImmutableCopy();
                }
            );
        }

        /// <summary>
        /// 不変シリアル値を指定して FieldReader を作成します。
        /// </summary>
        /// <param name="field">読み取りたいフィールドバイナリ</param>
        /// <returns>FieldReader インスタンス</returns>
        public static FieldReader Create(MBData field, SerialSetting.Immutable serialSetting)
        {
            return new FieldReader(field,
                serialSettingSource: () =>
                {
                    return serialSetting;
                }
            );
        }

        // ================

        MBData                          Field               { get; }
        Func<SerialSetting.Immutable>   SerialSettingSource { get; }

        /// <summary>
        /// 読み取るフィールドバイナリと、シリアル設定源を指定して FieldReader を生成します。
        /// </summary>
        /// <param name="field">読み取りたいフィールドバイナリ</param>
        /// <param name="settingSource">シリアル設定源</param>
        protected FieldReader(MBData field, Func<SerialSetting.Immutable> serialSettingSource)
        {
            if (ReferenceEquals(field, null))
                throw new ArgumentNullException(nameof(field));
            if (ReferenceEquals(serialSettingSource, null))
                throw new ArgumentNullException(nameof(serialSettingSource));

            Field               = field;
            SerialSettingSource = serialSettingSource;
        }

        /// <summary>
        /// "FieldTypeName_{FieldType}"
        /// </summary>
        /// <value>The type name code.</value>
        public Structure.FieldType FieldType
        {
            get
            {
                try{
                    return (Structure.FieldType)Field.Type;
                }catch(InvalidCastException e){
                    Log.Error("E", e.ToString());
                    return (Structure.FieldType.Unknown);
                }
            }
        }

        /// <summary>
        /// テキストを返します。
        /// </summary>
        public string Text
        {
            get
            {
                var g = SerialSettingSource();
                var updater = new SerialValueUpdater(g.SettingList, g.CountStateList);
                return updater.Replace(Field.Text);
            }
        }
    }
}
