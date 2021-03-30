using TokyoChokoku.MarkinBox.Sketchbook.Fields;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class UIFieldEditBoxFactory : IMutableFieldVisitor<Nil, IFieldEditBoxBuilder>
    {

        public Nil Visit (HorizontalText.Mutable target, IFieldEditBoxBuilder builder)
        {
            var p = target.Parameter;

            builder.Append (p.PowerStore);
            builder.Append (p.SpeedStore);
            builder.Append (p.ReverseStore);
            builder.Append (p.PauseStore);

            builder.Append (p.TextStore, p.FontStore);

            builder.Append (p.MirroredStore);
            builder.Append (p.JoggingStore);
            builder.Append (p.XStore);
            builder.Append (p.YStore);
            builder.Append (p.AngleStore);

            builder.Append (p.HeightStore);
            builder.Append (p.AspectStore);
            builder.Append (p.PitchStore);

            builder.Append (p.BasePointStore);

            return null;
        }

        public Nil Visit (YVerticalText.Mutable target, IFieldEditBoxBuilder builder)
        {
            var p = target.Parameter;

            builder.Append (p.PowerStore);
            builder.Append (p.SpeedStore);
            builder.Append (p.PauseStore);

            builder.Append (p.TextStore, p.FontStore);

            builder.Append (p.MirroredStore);
            builder.Append (p.JoggingStore);
            builder.Append (p.XStore);
            builder.Append (p.YStore);
            builder.Append (p.AngleStore);

            builder.Append (p.HeightStore);
            builder.Append (p.AspectStore);
            builder.Append (p.PitchStore);

            builder.Append (p.BasePointStore);

            return null;
        }

        public Nil Visit (XVerticalText.Mutable target, IFieldEditBoxBuilder builder)
        {
            var p = target.Parameter;

            builder.Append (p.PowerStore);
            builder.Append (p.SpeedStore);
            builder.Append (p.PauseStore);

            builder.Append (p.TextStore, p.FontStore);

            builder.Append (p.MirroredStore);
            builder.Append (p.JoggingStore);
            builder.Append (p.XStore);
            builder.Append (p.YStore);
            builder.Append (p.AngleStore);

            builder.Append (p.HeightStore);
            builder.Append (p.AspectStore);
            builder.Append (p.PitchStore);

            builder.Append (p.BasePointStore);

            return null;
        }

        public Nil Visit (OuterArcText.Mutable target, IFieldEditBoxBuilder builder)
        {
            var p = target.Parameter;

            builder.Append (p.PowerStore);
            builder.Append (p.SpeedStore);
            builder.Append (p.ReverseStore);
            builder.Append (p.PauseStore);

            builder.Append (p.TextStore, p.FontStore);

            builder.Append (p.MirroredStore);
            builder.Append (p.JoggingStore);
            builder.Append (p.XStore);
            builder.Append (p.YStore);
            builder.Append (p.AngleStore);

            builder.Append (p.HeightStore);
            builder.Append (p.AspectStore);
            builder.Append (p.PitchStore);
            builder.Append (p.RadiusStore);

            builder.Append (p.BasePointStore);

            builder.Append (p); //  パラメータ固有の設定

            return null;
        }

        public Nil Visit (InnerArcText.Mutable target, IFieldEditBoxBuilder builder)
        {
            var p = target.Parameter;

            builder.Append (p.PowerStore);
            builder.Append (p.SpeedStore);
            builder.Append (p.ReverseStore);
            builder.Append (p.PauseStore);

            builder.Append (p.TextStore, p.FontStore);

            builder.Append (p.MirroredStore);
            builder.Append(p.JoggingStore);
            builder.Append (p.XStore);
            builder.Append (p.YStore);
            builder.Append (p.AngleStore);

            builder.Append (p.HeightStore);
            builder.Append (p.AspectStore);
            builder.Append (p.PitchStore);
            builder.Append (p.RadiusStore);

            builder.Append (p.BasePointStore);

            builder.Append (p); //  パラメータ固有の設定

            return null;
        }

        public Nil Visit (DataMatrix.Mutable target, IFieldEditBoxBuilder builder)
        {
            var p = target.Parameter;

            builder.Append (p.PowerStore);
            builder.Append (p.SpeedStore);
            builder.Append (p.PauseStore);

            builder.Append (p.TextStore, FontMode.FontBarcode);
            builder.Append (p.DotCountStore);

            builder.Append (p.MirroredStore);
            builder.Append(p.JoggingStore);
            builder.Append (p.XStore);
            builder.Append (p.YStore);
            builder.Append (p.AngleStore);

            builder.Append (p.HeightStore);

            builder.Append (p.BasePointStore);

            return null;
        }

        public Nil Visit (QrCode.Mutable target, IFieldEditBoxBuilder builder)
        {
            var p = target.Parameter;

            builder.Append (p.PowerStore);
            builder.Append (p.SpeedStore);
            builder.Append (p.PauseStore);

            builder.Append (p.TextStore, FontMode.FontBarcode);

            builder.Append (p.MirroredStore);
            builder.Append(p.JoggingStore);
            builder.Append (p.XStore);
            builder.Append (p.YStore);
            builder.Append (p.AngleStore);
            builder.Append (p.HeightStore);

            builder.Append (p.BasePointStore);

            return null;
        }

        public Nil Visit (Rectangle.Mutable target, IFieldEditBoxBuilder builder)
        {
            var p = target.Parameter;

            builder.Append (p.PowerStore);
            builder.Append (p.SpeedStore);
            builder.Append (p.PauseStore);

            builder.Append(p.JoggingStore);
            builder.Append (p.XStore);
            builder.Append (p.YStore);
            builder.Append (p.AngleStore);

            builder.Append (p.HeightStore);
            builder.Append (p.AspectStore);

            builder.Append (p.BasePointStore);

            return null;
        }

        public Nil Visit (Circle.Mutable target, IFieldEditBoxBuilder builder)
        {
            var p = target.Parameter;

            builder.Append (p.PowerStore);
            builder.Append (p.SpeedStore);
            builder.Append (p.PauseStore);

            builder.Append(p.JoggingStore);
            builder.Append (p.XStore);
            builder.Append (p.YStore);

            builder.Append (p.RadiusStore);

            builder.Append (p.BasePointStore);

            return null;
        }

        public Nil Visit (Triangle.Mutable target, IFieldEditBoxBuilder builder)
        {
            var p = target.Parameter;

            builder.Append (p.PowerStore);
            builder.Append (p.SpeedStore);
            builder.Append (p.PauseStore);

            builder.Append(p.JoggingStore);
            builder.Append (p.XStore);
            builder.Append (p.YStore);
            builder.Append (p.HornXStore, "Top X");
            builder.Append (p.AngleStore);

            builder.Append (p.HeightStore);
            builder.Append (p.AspectStore);

            builder.Append (p.BasePointStore);

            return null;
        }

        public Nil Visit (Line.Mutable target, IFieldEditBoxBuilder builder)
        {
            var p = target.Parameter;

            builder.Append (p.PowerStore);
            builder.Append (p.SpeedStore);
            builder.Append (p.PauseStore);

            builder.Append (p.IsBezierCurveStore);

            builder.Append(p.JoggingStore);
            builder.Append (p.XStore);
            builder.Append (p.YStore);

            builder.Append (p.StartXStore, "Start X");
            builder.Append (p.StartYStore, "Start Y");

            builder.Append (p.CenterXStore, "Center X");
            builder.Append (p.CenterYStore, "Center Y");

            builder.Append (p.EndXStore, "End X");
            builder.Append (p.EndYStore, "End Y");


            return null;
        }

        public Nil Visit (Bypass.Mutable target, IFieldEditBoxBuilder builder)
        {
            var p = target.Parameter;

            builder.Append (p.PowerStore);
            builder.Append (p.SpeedStore);
            builder.Append (p.PauseStore);


            builder.Append(p.JoggingStore);
            builder.Append (p.XStore);
            builder.Append (p.YStore);

            return null;
        }

        public Nil Visit (Ellipse.Mutable target, IFieldEditBoxBuilder builder)
        {
            var p = target.Parameter;


            builder.Append (p.PowerStore);
            builder.Append (p.SpeedStore);
            builder.Append (p.PauseStore);

            builder.Append(p.JoggingStore);
            builder.Append (p.XStore);
            builder.Append (p.YStore);
            builder.Append (p.AngleStore);

            builder.Append (p.HeightStore);
            builder.Append (p.AspectStore);

            builder.Append (p.BasePointStore);

            return null;
        }
    }
}

