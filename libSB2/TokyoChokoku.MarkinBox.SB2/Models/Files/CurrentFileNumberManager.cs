using System;
using System.Linq;
using System.CodeDom.Compiler;
using System.Threading.Tasks;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
	//public static class CurrentFileNumberManager
	//{
 //       private static readonly string Key;

 //       private static int _value = 0;
 //       public static int Value
 //       {
 //           get
 //           {
 //               return _value;
 //           }
 //           set
 //           {
 //               _value = value;
 //               PreferenceManager.Instance.Accessor.SetString(Key, Value.ToString());
 //               Log.Debug(String.Format("設定を更新しました ... {0} : {1}", Key, Value));
 //           }
 //       }

	//	static CurrentFileNumberManager(){
	//		Key = DataStoreKey.CurrentFileNumber.ToKey ();
	//		// 初期値を設定します。
	//		int tmpValue;
 //           if (int.TryParse (PreferenceManager.Instance.Accessor.GetString (Key), out tmpValue)) {
	//			Value = tmpValue;
	//		} else {
	//			Value = 0;
	//		}
	//	}

	//	public static bool FileExists(){
	//		return Value > 0;
	//	}

 //       public static bool FileIsPermanent()
 //       {
 //           return Value == (Communication.Sizes.IndexOfRemotePermanentFile + 1);
 //       }
	//}
}

