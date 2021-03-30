using System;
using System.Linq;
using System.CodeDom.Compiler;
using System.Threading.Tasks;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
	public static class OperationModeManager
	{
		private static readonly string Key;
		private static  OperationMode Value;

		public static OperationMode Get(){
			return Value;
		}

		public static void Set(OperationMode mode){
            // アプリの設定を更新します。
            Value = mode;
            PreferenceManager.Instance.Accessor.SetString(mode.ToString(), Key);
			Log.Debug("設定を更新しました ... " +  Key + " : " + mode.ToString());
		}

		static OperationModeManager(){
			Key = DataStoreKey.OperationMode.ToKey ();
			// 初期値を設定します。
			OperationMode tmpValue;
            if (Enum.TryParse (PreferenceManager.Instance.Accessor.GetString (Key), out tmpValue)) {
				Value = tmpValue;
			} else {
				Value = OperationMode.Administrator;
			}
		}

		public static bool IsAdministrator(){
			return Value == OperationMode.Administrator;
		}

		public static bool IsOperator(){
			return Value == OperationMode.Operator;
		}
	}
}

