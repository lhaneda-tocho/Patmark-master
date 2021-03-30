using Foundation;
using TokyoChokoku.Patmark.StorageUtil;
namespace TokyoChokoku.Patmark.iOS
{
    public class FieldStorageForiOS : FieldStorage
    {
        public override string Dir => NSSearchPath.GetDirectories(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User, true)[0];
    }
}
