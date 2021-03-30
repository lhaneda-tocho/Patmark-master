using System;
using Android;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace TokyoChokoku.Patmark.Droid
{
    public class CommonStringsImpl : ICommonStrings
    {
        Context Ctx;

        public CommonStringsImpl(Context ctx){
            Ctx = ctx;
        }

        public string FileSourceLabelLocal => Ctx.Resources.GetString(Resource.String.file_source_label_local);
       
        public string FileSourceLabelRemote => Ctx.Resources.GetString(Resource.String.file_source_label_remote);

        public string FileSourceLabelLatest => Ctx.Resources.GetString(Resource.String.file_source_label_latest);

    }
}
