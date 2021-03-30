using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TokyoChokoku.MarkinBox.Sketchbook.TemplateMeta
{
    public static class MetaStores
    {

        public static ReadOnlyCollection<StoreTypeDefinition> Definitions { get; }


        public static StoreTypeDefinition Text { get; } = new StoreTypeDefinition (
            name: "TextStore",
            contentTypeName: "string");
        /*--------------*/



        public static StoreTypeDefinition Mode { get; } = new StoreTypeDefinition (
            name: "ModeStore",
            contentTypeName: "short");
        /*--------------*/



        public static StoreTypeDefinition PrmFl { get; } = new StoreTypeDefinition (
            name: "PrmFlStore",
            contentTypeName: "short");
        /*--------------*/



        public static StoreTypeDefinition Id { get; } = new StoreTypeDefinition (
            name: "IdStore",
            contentTypeName: "short");
        /*--------------*/



        public static StoreTypeDefinition X { get; } = new StoreTypeDefinition (
                name: "XStore",
                contentTypeName: "decimal");
        /*--------------*/



        public static StoreTypeDefinition Y { get; } = new StoreTypeDefinition (
            name: "YStore",
            contentTypeName: "decimal");
        /*--------------*/



        public static StoreTypeDefinition ZDepth { get; } = new StoreTypeDefinition (
            name: "ZDepthStore",
            contentTypeName: "short");
        /*--------------*/



        public static StoreTypeDefinition Height { get; } = new StoreTypeDefinition (
            name: "HeightStore",
            contentTypeName: "decimal");
        /*--------------*/



        public static StoreTypeDefinition Pitch { get; } = new StoreTypeDefinition (
            name: "PitchStore",
            contentTypeName: "decimal");
        /*--------------*/



        public static StoreTypeDefinition Aspect { get; } = new StoreTypeDefinition (
            name: "AspectStore",
            contentTypeName: "decimal");
        /*--------------*/



        public static StoreTypeDefinition Angle { get; } = new StoreTypeDefinition (
            name: "AngleStore",
            contentTypeName: "decimal");
        /*--------------*/



        public static StoreTypeDefinition ArcRadius { get; } = new StoreTypeDefinition (
            name: "RadiusStore",
            contentTypeName: "decimal");
        /*--------------*/



        public static StoreTypeDefinition Mirrored { get; } = new StoreTypeDefinition (
            name: "MirroredStore",
            contentTypeName: "bool");
        /*--------------*/



        public static StoreTypeDefinition Speed { get; } = new StoreTypeDefinition (
            name: "SpeedStore",
            contentTypeName: "short");
        /*--------------*/



        public static StoreTypeDefinition Density { get; } = new StoreTypeDefinition (
            name: "DensityStore",
            contentTypeName: "short");
        /*--------------*/



        public static StoreTypeDefinition Power { get; } = new StoreTypeDefinition (
            name: "PowerStore",
            contentTypeName: "short");
        /*--------------*/



        public static StoreTypeDefinition HostVersion { get; } = new StoreTypeDefinition (
            name: "HostVersionStore",
            contentTypeName: "ushort");
        /*--------------*/



        public static StoreTypeDefinition DotCount { get; } = new StoreTypeDefinition (
            name: "DotCountStore",
            contentTypeName: "DotCount2D");
        /*--------------*/



        public static StoreTypeDefinition BasePoint { get; } = new StoreTypeDefinition (
            name: "BasePointStore",
            contentTypeName: "byte");
        /*--------------*/



        public static StoreTypeDefinition Time { get; } = new StoreTypeDefinition (
            name: "TimeStore",
            contentTypeName: "decimal");
        /*--------------*/



        public static StoreTypeDefinition IsBezierCurve { get; } = new StoreTypeDefinition (
            name: "IsBezierCurveStore",
            contentTypeName: "bool");
        /*--------------*/



        public static StoreTypeDefinition Jogging { get; } = new StoreTypeDefinition(
            name: "JoggingStore",
            contentTypeName: "bool");
        /*--------------*/



        public static StoreTypeDefinition Pause { get; } = new StoreTypeDefinition (
            name: "PauseStore",
            contentTypeName: "bool");
        /*--------------*/



        public static StoreTypeDefinition Font { get; } = new StoreTypeDefinition (
            name: "FontStore",
            contentTypeName: "FontMode");
        /*--------------*/



        public static StoreTypeDefinition Reverse { get; } = new StoreTypeDefinition (
            name: "ReverseStore",
            contentTypeName: "bool");
        /*--------------*/



        static MetaStores () {
            List<StoreTypeDefinition> list = new List <StoreTypeDefinition> ();

            list.Add(Text);
            list.Add(Font);
            list.Add(Mode);
            list.Add(PrmFl);
            list.Add(Id);
            list.Add(X);
            list.Add(Y);
            list.Add(ZDepth);
            list.Add(Height);
            list.Add(Pitch);
            list.Add(Aspect);
            list.Add(Angle);
            list.Add(ArcRadius);
            list.Add(Mirrored);
            list.Add(Speed);
            list.Add(Jogging);
            list.Add(Pause);
            list.Add(Density);
            list.Add(Power);
            list.Add(HostVersion);
            list.Add(DotCount);
            list.Add(BasePoint);
            list.Add(Time);
            list.Add(IsBezierCurve);
            list.Add(Reverse);


            Definitions = list.AsReadOnly ();
        }
    }
}

