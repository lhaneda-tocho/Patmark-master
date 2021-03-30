using System;
using Newtonsoft.Json;
using TokyoChokoku.CalendarModule.Ast;
using TokyoChokoku.SerialModule.Ast;
using TokyoChokoku.CalendarModule.Setting;
using TokyoChokoku.SerialModule.Setting;
using TokyoChokoku.FieldModel;

namespace TokyoChokoku.Patmark.EmbossmentKit
{
    /// <summary>
    /// 打刻の付加情報
    /// </summary>
    public abstract class BaseEmbossmentAttrib
    {
        public SerialSetting SerialSetting   { get; }
        public CRSetting     CalendarSetting { get; }

        public SerialNodeProcessor   SerialNodeProcessor   => SerialSetting.CreateNodeProcessor();
        public CalendarNodeProcessor CalendarNodeProcessor => CalendarSetting.CreateNodeProcessorConstructor().CreateWithCurrentTime();

        /// <summary>
        /// Provide the newest field text factory.
        /// </summary>
        /// <value>The field text factory.</value>
        public abstract FieldTextFactory FieldTextFactory { get; }

        public BaseEmbossmentAttrib(SerialSetting serialSetting, CRSetting calendarSetting)
        {
            SerialSetting = serialSetting;
            CalendarSetting = calendarSetting;
        }

        public FieldText FieldTextFrom(string text)
        {
            return FieldTextFactory.Parse(text);
        }

        /// <summary>
        /// Generate the field text factory.
        /// </summary>
        /// <returns>The field text factory.</returns>
        protected FieldTextFactory GenFieldTextFactory() {
            return FieldTextFactory.Create(CalendarNodeProcessor, SerialNodeProcessor);
        }

        //public class JsonFormat
        //{
        //    /// <summary>
        //    /// Gets or sets the SerialSetting.
        //    /// </summary>
        //    /// <value>The serial.</value>
        //    [JsonProperty("serial")]
        //    public SerialSetting SerialSetting { get; set; }

        //    /// <summary>
        //    /// Gets or sets the calendar setting.
        //    /// </summary>
        //    /// <value>The calendar setting.</value>
        //    [JsonProperty("calendar")]
        //    public CRSetting CalendarSetting { get; set; }
        //}
    }

    /// <summary>
    /// Embossment attrib. (Immutable)
    /// </summary>
    public class EmbossmentAttrib: BaseEmbossmentAttrib
    {
        public sealed override FieldTextFactory FieldTextFactory { get; }


        public static EmbossmentAttrib CreateFromGlobal() {
            return new EmbossmentAttrib(SerialGlobalSetting.ImmutableCopy(), CRGlobalSetting.ImmutableCopy());
        }

        public EmbossmentAttrib(SerialSetting serialSetting, CRSetting calendarSetting) : base(
            serialSetting.ImmutableCopy(),
            calendarSetting.ImmutableCopy())
        {
            FieldTextFactory = GenFieldTextFactory();
        }

        public EmbossmentAttrib():base(
            new SerialSetting.Immutable(),
            new CRSetting.Immutable()
        )
        {
            FieldTextFactory = GenFieldTextFactory();
        }
    }

    /// <summary>
    /// Mutable embossment attrib. (Mutable)
    /// </summary>
    public class MutableEmbossmentAttrib: BaseEmbossmentAttrib
    {
        public sealed override FieldTextFactory FieldTextFactory => GenFieldTextFactory();

        public static MutableEmbossmentAttrib CreateFromGlobal()
        {
            return new MutableEmbossmentAttrib(SerialGlobalSetting.ImmutableCopy(), CRGlobalSetting.ImmutableCopy());
        }

        public MutableEmbossmentAttrib(SerialSetting serialSetting, CRSetting calendarSetting) : base(
            serialSetting.MutableCopy(),
            calendarSetting.MutableCopy())
        { }

        public MutableEmbossmentAttrib() : base(
            new SerialSetting.Mutable(),
            new CRSetting.Mutable()
        )
        { }
    }
}
