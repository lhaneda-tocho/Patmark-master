using System;
using System.Collections.Generic;
using TokyoChokoku.Patmark.MachineModel;
using UIKit;


namespace TokyoChokoku.Patmark.iOS.Presenter.Settings
{
    public class ModelNoSheetBuilder
    {
        public string Title { get; set; } = "Machine model no.".Localize();
        public List<PatmarkMachineModel> MachineModelList { get; }
        public event Action<UIAlertAction, PatmarkMachineModel> Listener = (_0,_1)=>{};

        public ModelNoSheetBuilder(List<PatmarkMachineModel> specList)
        {
            if (specList == null)
                throw new NullReferenceException();
            MachineModelList = specList;
        }


        public UIAlertController Build(UIView sourceView)
        {
            var alert = UIAlertController.Create(
                Title,
                 
                "Choose machine model no.".Localize(),
                UIAlertControllerStyle.ActionSheet);

            foreach(var spec in MachineModelList) {
                string label = spec.LocalizationTag.Localize();
                AddButton(alert, label, spec, Listener);
            }
                
            
            AddCancelButton(alert);

            // Required for iPad - You must specify a source for the Action Sheet since it is
            // displayed as a popover
            UIPopoverPresentationController presentationPopover = alert.PopoverPresentationController;
            if (presentationPopover != null)
            {
                presentationPopover.SourceView = sourceView;
                presentationPopover.SourceRect = sourceView.Bounds;
                presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Up | UIPopoverArrowDirection.Down;
            }

            return alert;
        }




        static void AddButton(
            UIAlertController alert, string label, PatmarkMachineModel spec, Action<UIAlertAction, PatmarkMachineModel> action)
        {
            alert.AddAction(
                UIAlertAction.Create(
                    label.Localize(),
                    UIAlertActionStyle.Default,
                    (it)=>action(it, spec)
                )
            );
        }


        static void AddCancelButton(UIAlertController alert)
        {
            alert.AddAction(UIAlertAction.Create(
                "Cancel".Localize(), UIAlertActionStyle.Cancel, null));
        }

    }
}
