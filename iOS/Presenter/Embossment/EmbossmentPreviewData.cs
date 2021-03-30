using System;
using System.Collections.Generic;
using TokyoChokoku.Patmark.Presenter.Preview;
using TokyoChokoku.Patmark.iOS.Presenter.FieldPreview;
using TokyoChokoku.Patmark.TextData;
using TokyoChokoku.Patmark.EmbossmentKit;
using TokyoChokoku.Patmark.MachineModel;

namespace TokyoChokoku.Patmark.iOS.Presenter.Embossment
{
    public class EmbossmentPreviewData: CommonEmbossmentPreviewData, IPreviewData
    {

        public EmbossmentPreviewData(PatmarkMachineModel machineModelSpec, EmbossmentData data): base(machineModelSpec, data)
        {
        }

    }
}
