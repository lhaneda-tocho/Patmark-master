
using TokyoChokoku.MarkinBox.Sketchbook.Fields;
using TokyoChokoku.MarkinBox.Sketchbook.Properties.Stores;
using TokyoChokoku.MarkinBox.Sketchbook.Parameters;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
	public interface IFieldEditBoxBuilder
	{
        void Append(TextStore      textstore,  FontStore    fontStore, string title);
        void Append(TextStore      textstore,  FontMode  constantFont, string title);
        void Append(XStore             store,  string title);
        void Append(YStore             store,  string title);
        void Append(AngleStore         store,  string title);
        void Append(PitchStore         store,  string title);
        void Append(HeightStore        store,  string title);
        void Append(AspectStore        store,  string title);
        void Append(BasePointStore     store,  string title);
        void Append(RadiusStore        store,  string title);
        void Append(TimeStore          store,  string title);
        void Append(JoggingStore       store,  string title);
        void Append(PauseStore         store,  string title);
        void Append(SpeedStore         store,  string title);
        void Append(PowerStore         store,  string title);
        void Append(ReverseStore       store,  string title);
        void Append(MirroredStore      store,  string title);
        void Append(DotCountStore      store,  string title);
        void Append(IsBezierCurveStore store,  string title);
        void Append(Editor             editor, string title);


		void Append(MutableInnerArcTextParameter p);
		void Append(MutableOuterArcTextParameter p);
	}

    public static class MBParameterUICreationBuilderExt {
        public static void Append (this IFieldEditBoxBuilder builder, TextStore textStore, FontStore fontStore)
        {
            builder.Append (textStore, fontStore, "Text");
        }

        public static void Append (this IFieldEditBoxBuilder builder, TextStore textStore, FontMode constantFont)
        {
            builder.Append (textStore, constantFont, "Text");
        }

        public static void Append(this IFieldEditBoxBuilder builder, XStore         store){
            builder.Append (store, "X");
        }

        public static void Append(this IFieldEditBoxBuilder builder, YStore         store){
            builder.Append (store, "Y");
        }

        public static void Append(this IFieldEditBoxBuilder builder, AngleStore     store){
            builder.Append (store, "Angle");
        }

        public static void Append(this IFieldEditBoxBuilder builder, PitchStore     store){
            builder.Append (store, "Pitch");
        }

        public static void Append(this IFieldEditBoxBuilder builder, HeightStore    store){
            builder.Append (store, "Height");
        }
        public static void Append(this IFieldEditBoxBuilder builder, AspectStore    store){
            builder.Append (store, "Aspect");
        }

        public static void Append(this IFieldEditBoxBuilder builder, BasePointStore store){
            builder.Append (store, "Base Point");
        }

        public static void Append(this IFieldEditBoxBuilder builder, RadiusStore    store){
            builder.Append (store, "Radius");
        }

        public static void Append(this IFieldEditBoxBuilder builder, TimeStore      store){
            builder.Append (store, "Time");
        }

        public static void Append(this IFieldEditBoxBuilder builder, JoggingStore store)
        {
            builder.Append(store, "Jogging");
        }

        public static void Append (this IFieldEditBoxBuilder builder, PauseStore store)
        {
            builder.Append (store, "Pause");
        }
        public static void Append (this IFieldEditBoxBuilder builder, SpeedStore store)
        {
            builder.Append (store, "Speed");
        }
        public static void Append(this IFieldEditBoxBuilder builder, PowerStore     store){
            builder.Append (store, "Power");
        }
        public static void Append(this IFieldEditBoxBuilder builder, ReverseStore store){
            builder.Append (store, "Reverse");
        }
        public static void Append(this IFieldEditBoxBuilder builder, MirroredStore  store){
            builder.Append (store, "Mirror");
        }
        public static void Append(this IFieldEditBoxBuilder builder, DotCountStore store){
            builder.Append (store, "DotCount");
        }
        public static void Append(this IFieldEditBoxBuilder builder, IsBezierCurveStore store){
            builder.Append (store, "Bezier");
        }

        public static void Append(this IFieldEditBoxBuilder builder, Editor editor) {
            builder.Append (editor, "Type");
        }
    }
}

