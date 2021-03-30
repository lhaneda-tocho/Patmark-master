using System;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
	/// <summary>
	/// ログ出力インタフェースです。
	/// 出力したい情報のカテゴリ毎にメソッドを実装してください。
	/// </summary>
	public interface ILog
	{
		/// <summary>
		/// 細かな情報を記録します
		/// </summary>
		void Verbose (params String[] messages);

		/// <summary>
		/// 情報を記録します
		/// </summary>
		void Info (params String[] messages);

		/// <summary>
		/// デバッグ情報を記録します
		/// </summary>
		void Debug (params String[] messages);

		/// <summary>
		/// 警告情報を記録します
		/// </summary>
		void Warn (params String[] messages);

		/// <summary>
		/// エラー情報を記録します
		/// </summary>
		void Error (params String[] messages);
	}
}

