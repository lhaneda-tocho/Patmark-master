using System;
using TokyoChokoku.Patmark.MachineModel;
using Android.Content;
using Android.Content.Res;

namespace TokyoChokoku.Patmark.Droid
{
    public static class PatmarkMachineModelExt
    {
        /// <summary>
        /// ローカライズされた名前を返します。ローカライズ設定されていない場合は、例外がスローされます。
        /// 
        /// この仕様は、Android の Resources.GetString(int) の動作に基づきます。
        /// </summary>
        /// <returns>ローカライズされた名前. null になることはありません。</returns>
        /// <exception cref="Resources.NotFoundException">リソースが存在しない場合</exception>
        /// <seealso cref="https://developer.android.com/reference/android/content/res/Resources#getString(int)"/>
        public static string LocalizedName(this PatmarkMachineModel self, Context context)
        {
            var name = self.LocalizationTag;
            var fieldName = name.ToLower().Replace(":", "_");

            // リフレクションで ローカライズIDを取得する。
            var field = typeof(Resource.String).GetField(fieldName)
                ?? throw new Resources.NotFoundException($"Resource.String.{fieldName} not found.");

            var id = field.GetRawConstantValue() as int?
                ?? throw new Resources.NotFoundException($"Error on accsss: to Resource.String.{fieldName}");

            // IDを文字列に変換
            return context.GetString(id);
        }
    }
}
