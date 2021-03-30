
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using TokyoChokoku.Patmark.Presenter.FileMenu;
using TokyoChokoku.Patmark.Droid.Presenter.Alert;

namespace TokyoChokoku.Patmark.Droid.Presenter.FileMenu
{
    public class PathNameAdapterManager
    {
        public IList<PathName> FileList { get; private set; } = new List<PathName>();

        List<String> Empty { get; }

        public ArrayAdapter<String> Adapter { get; }

        public PathNameAdapterManager(Context context)
        {
            Empty = new string[] {
                //context.GetString(Resource.String.none)
            }.ToList();

            Adapter = new ArrayAdapter<string>(
                context,
                Android.Resource.Layout.SimpleListItem1,
                Empty
            );
        }

        public void UpdateWithList(List<PathName> list)
        {
            FileList = list?.ToImmutableList() ?? throw new NullReferenceException();
            var strings = from e in FileList
                          select e.Simple;
            Adapter.Clear();
            if (list.Count == 0)
            {
                Adapter.AddAll(Empty);
            }
            else
            {
                Adapter.AddAll(strings.ToList());
            }
            Adapter.NotifyDataSetChanged();
        }

        public PathName? GetPathNameOrNull(int pos) {
            var list = FileList;
            if(pos < 0 || pos >= list.Count) {
                return null;
            } else {
                return list[pos];
            }
        }
    }
}
