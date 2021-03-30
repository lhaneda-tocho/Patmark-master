
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
    public class RemoteFileInfoAdapter
    {
        public IList<RemoteFileInfo> FileList { get; private set; } = new List<RemoteFileInfo>();

        List<String> Empty { get; }

        public ArrayAdapter<String> Adapter { get; }

        public RemoteFileInfoAdapter(Context context)
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

        private IEnumerable<string> FormatForCell(IEnumerable<RemoteFileInfo> lis)
        {
            var indices = Enumerable.Range(0, FileList.Count);
            var text    = from e in FileList
                          select e.NumOfField > 0 ? e.Name : "";
            return indices.Zip(text, (left, right) => (left+1) + ": " + right);
        }

        public void UpdateWithList(List<RemoteFileInfo> list)
        {
            FileList = list?.ToImmutableList() ?? throw new NullReferenceException();
            var strings = FormatForCell(FileList);
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

        public void Update(int index, RemoteFileInfo info)
        {
            if(index < 0 || FileList.Count <= index){
                return;
            }
            // リストの更新
            var tmpFileList = FileList.ToArray();
            tmpFileList[index] = info;
            FileList = tmpFileList.ToList();
            // 表示内容の生成
            var strings = FormatForCell(FileList);
            // アダプタの更新
            Adapter.Clear();
            Adapter.AddAll(strings.ToList());
            Adapter.NotifyDataSetChanged();
        }

        public RemoteFileInfo GetPathNameOrNull(int pos)
        {
            var list = FileList;
            if (pos < 0 || pos >= list.Count)
            {
                return null;
            }
            else
            {
                return list[pos];
            }
        }
    }
}
