using System.Linq;
using System.Collections.Generic;

using TokyoChokoku.MarkinBox.Sketchbook.Fields;
using TokyoChokoku.MarkinBox.Sketchbook.Properties.Stores;
using TokyoChokoku.MarkinBox.Sketchbook.Parameters;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    /// <summary>
    /// 編集用のGUIを生成するクラスです．
    /// </summary>
    public class iOSEditBoxBuilder : IFieldEditBoxBuilder
    {
        readonly List<IEditBoxCell> containerList = new List<IEditBoxCell> ();


        IEditBoxCommonDelegate CommonDelegate { get; }
        iOSFieldManager FieldManager { get; }
        iOSOwner EditTarget { get; }


        public iOSEditBoxBuilder (iOSFieldManager fm, IEditBoxCommonDelegate subDelegate)
        {
            if (!fm.IsEditingAny)
                throw new System.ArgumentException ();
            FieldManager = fm;
            CommonDelegate = subDelegate;
            EditTarget = FieldManager.EditTarget;
        }


        public PropertyEditBoxSource Build ()
        {
            var source = new PropertyEditBoxSource (containerList);

            return source;
        }

        public void Append (CopyRemoveCellDelegate cddelegate, string title)
        {
            title = title.Localize ();
            containerList.Add (
                PreviewContextCell.Create (
                    title,
                    cddelegate,
                    CommonDelegate
                )
            );
        }

        public void Append (TextStore textStore, FontStore fontStore, string title)
        {
            var cell = TextBoxCell.Create (
                title.Localize (),
                TextBoxCellSource.Create (textStore, fontStore, CommonDelegate)
            );

            containerList.Add (cell);

            if (TextFieldCollector.IsSerialContainer (EditTarget)) {
                containerList.Add (
                    TextBoxOptional.Create (
                        FieldManager, cell
                    )
                );
            }
        }

        public void Append (TextStore textStore, FontMode font, string title)
        {
            title = title.Localize ();

            var cell = TextBoxCell.Create (
                    title,
                    TextBoxCellSource.Create (textStore, font, CommonDelegate)
            );

            containerList.Add (cell);

            if (TextFieldCollector.IsSerialContainer (EditTarget)) {
                containerList.Add (
                    TextBoxOptional.Create (
                        FieldManager, cell
                    )
                );
            }
        }

        public void Append (XStore store, string title)
        {
            title = title.Localize ();

            var unit = "mm";
            var increment = 0.20m;

            containerList.Add (
                UpDownTextBoxCell.Create (
                    title,
                    new DecimalUpDownTextBoxCellSource (store, unit, increment, CommonDelegate)
                )
            );
        }

        public void Append (YStore store, string title)
        {
            title = title.Localize ();

            var unit = "mm";
            var increment = 0.20m;

            containerList.Add (
                UpDownTextBoxCell.Create (
                    title,
                    new DecimalUpDownTextBoxCellSource (store, unit, increment, CommonDelegate)
                )
            );
        }

        public void Append (AngleStore store, string title)
        {
            title = title.Localize ();

            var unit = "mm";
            var increment = 1m;

            containerList.Add (
                UpDownTextBoxCell.Create (
                    title,
                    new DecimalUpDownTextBoxCellSource (store, unit, increment, CommonDelegate)
                )
            );
        }

        public void Append (PitchStore store, string title)
        {
            title = title.Localize ();

            var unit = "mm";
            var increment = 0.20m;

            containerList.Add (
                UpDownTextBoxCell.Create (
                    title,
                    new DecimalUpDownTextBoxCellSource (store, unit, increment, CommonDelegate)
                )
            );
        }

        public void Append (HeightStore store, string title)
        {
            title = title.Localize ();

            var unit = "mm";
            var increment = 0.20m;

            containerList.Add (
                UpDownTextBoxCell.Create (
                    title,
                    new DecimalUpDownTextBoxCellSource (store, unit, increment, CommonDelegate)
                )
            );
        }

        public void Append (AspectStore store, string title)
        {
            title = title.Localize ();

            var unit = "％";
            var increment = 1m;

            containerList.Add (
                UpDownTextBoxCell.Create (
                    title,
                    new DecimalUpDownTextBoxCellSource (store, unit, increment, CommonDelegate)
                )
            );
        }

        public void Append (BasePointStore store, string title)
        {
            title = title.Localize ();
        }


        public void Append (RadiusStore store, string title)
        {
            title = title.Localize ();

            var unit = "mm";
            var increment = 1m;

            containerList.Add (
                UpDownTextBoxCell.Create (
                    title,
                    new DecimalUpDownTextBoxCellSource (store, unit, increment, CommonDelegate)
                )
            );
        }


        public void Append (TimeStore store, string title)
        {
            title = title.Localize ();

            var unit = "s";
            var increment = 1m;

            containerList.Add (
                UpDownTextBoxCell.Create (
                    title,
                    new DecimalUpDownTextBoxCellSource (store, unit, increment, CommonDelegate)
                )
            );
        }


        public void Append (PowerStore store, string title)
        {
            title = title.Localize ();

            var source = LabeledDropdownListCellSource<short>.Create(
                store, PowerRange.ToArray(), (v) => (v+1).ToString(), CommonDelegate);

            containerList.Add(
                DropdownListCell.Create(
                    title,
                    source
                )
            );
        }


        public void Append (SpeedStore store, string title)
        {
            title = title.Localize ();

            var source = LabeledDropdownListCellSource<short>.Create(
                store, SpeedRange.ToArray(), (v)=>(v+1).ToString(), CommonDelegate);

            containerList.Add(
                DropdownListCell.Create(
                    title,
                    source
                )
            );
        }


        public void Append(JoggingStore store, string title)
        {
            title = title.Localize();

            containerList.Add(
                ToggleSwitchCell.Create(
                    title,
                    new BoolToggleSwitchCellSource(store, CommonDelegate)
                )
            );
        }


        public void Append (PauseStore store, string title)
        {
            title = title.Localize ();

            containerList.Add (
                ToggleSwitchCell.Create (
                    title,
                    new BoolToggleSwitchCellSource (store, CommonDelegate)
                )
            );
        }

        public void Append (ReverseStore store, string title)
        {
            title = title.Localize ();

            containerList.Add (
                ToggleSwitchCell.Create (
                    title,
                    new BoolToggleSwitchCellSource (store, CommonDelegate)
                )
            );
        }



        public void Append (MutableInnerArcTextParameter p)
        {
            var title = "Pivot".Localize ();

            containerList.Add (
                PivotCell.Create (
                    title,
                    PivotCellSource.Of (p, CommonDelegate)
                )
            );
        }

        public void Append (MutableOuterArcTextParameter p)
        {
            var title = "Pivot".Localize ();

            containerList.Add (
                PivotCell.Create (
                    title,
                    PivotCellSource.Of (p, CommonDelegate)
                )
            );
        }

        public void Append (MirroredStore store, string title)
        {
            title = title.Localize ();

            containerList.Add (
                ToggleSwitchCell.Create (
                    title,
                    new BoolToggleSwitchCellSource (store, CommonDelegate)
                )
            );
        }

        public void Append (DotCountStore store, string title)
        {
            title = title.Localize ();

            containerList.Add (
                DropdownListCell.Create (
                    title,
                    new DotCountDropdownListCellSource (store, DataMatrixConstant.DotCountArray (), CommonDelegate)
                )
            );
        }

        public void Append (IsBezierCurveStore store, string title)
        {
            title = title.Localize ();

            containerList.Add (
                ToggleSwitchCell.Create (
                    title,
                    new BoolToggleSwitchCellSource (store, CommonDelegate)
                )
            );
        }

        public void Append (Editor editor, string title)
        {
            title = title.Localize ();

            containerList.Add (
                DropdownListCell.Create (
                    title,
                    new TypeDropdownListCellSource (editor, CommonDelegate)
                )
            );
        }
    }
}

