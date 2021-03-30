using System;
using System.ComponentModel;

using Foundation;
using UIKit;
using TokyoChokoku.Patmark.MachineModel;

namespace TokyoChokoku.Patmark.iOS.Presenter.Settings
{

    [Register("MachineModelNoCell"), DesignTimeVisible(true)]
    public partial class MachineModelNoCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("MachineModelNoCell");
        public static readonly UINib Nib;


        [Outlet]
        UILabel ModelName { get; set; }

        [Outlet]
        UILabel TitleName { get; set; }

        public string Title { get { return TitleName.Text; }}
        public string Model { get { return ModelName.Text; }}

        public PatmarkMachineModel MachineModel {
            get {
                return machineModel;
            }
            set {
                if (value == null)
                    throw new NullReferenceException();
                machineModel = value;
                UpdateLabel();
            }
        }

        PatmarkMachineModel machineModel = PatmarkMachineModel.Patmark1515;

        static MachineModelNoCell()
        {
            Nib = UINib.FromName("MachineModelNoView", NSBundle.MainBundle);
        }



        // Storyboard/xib から初期化はここから
        public MachineModelNoCell(NSCoder coder) : base(coder)
        {
            //CommonInit();
        }

        public MachineModelNoCell(UITableViewCellStyle style, string reuseIdentifier) : base(style, reuseIdentifier)
        {
            CommonInit();
        }

        public MachineModelNoCell()
        {
            CommonInit();
        }

        public MachineModelNoCell(UITableViewCellStyle style, NSString reuseIdentifier) : base(style, reuseIdentifier)
        {
            CommonInit();
        }


        protected MachineModelNoCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            CommonInit();
        }

        void CommonInit()
        {
            // 参考: http://himaratsu.hatenablog.com/entry/ios/customview
            var view = Nib.Instantiate(this, null)[0] as UIView;
            AddSubview(view);


            view.TranslatesAutoresizingMaskIntoConstraints = false;

            view.LeftAnchor  .ConstraintEqualTo(LayoutMarginsGuide.LeftAnchor  ).Active = true;
            view.RightAnchor .ConstraintEqualTo(LayoutMarginsGuide.RightAnchor ).Active = true;
            view.TopAnchor   .ConstraintEqualTo(LayoutMarginsGuide.TopAnchor   ).Active = true;
            view.BottomAnchor.ConstraintEqualTo(LayoutMarginsGuide.BottomAnchor).Active = true;

            UpdateLabel();
        }



        public void UpdateLabel()
        {
            ModelName.Text = MachineModel.LocalizationTag.Localize();;
            ModelName.SetNeedsDisplay();
        }



        //public void LocalizeTitle()
        //{
        //    //TitleLabel.Text = ???
        //}
    }
}
