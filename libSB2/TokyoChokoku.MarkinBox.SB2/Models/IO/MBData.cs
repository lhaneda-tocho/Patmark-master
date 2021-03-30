// FIXME : Validatorを導入する．
namespace TokyoChokoku.MarkinBox.Sketchbook
{
    /// <summary>
    /// 文字列や図形の情報を保存するためのデータ構造を表現します．
    /// </summary>
	public sealed class MBData
	{
		readonly MBDataStructure model;

		/// <summary>
		/// テキスト を取得
		/// </summary>
		public string Text { get { return model.Text; } }

		/// <summary>
		/// モード を取得
		/// </summary>
		public ushort Mode { get { return model.Mode; } }

		/// <summary>
		/// PrmFl を取得
		/// </summary>
		public short PrmFl { get { return model.PrmFl; } }

		/// <summary>
		/// Id を取得
		/// </summary>
		public short Id { get { return model.Id; } }

		/// <summary>
		/// X座標 を取得
		/// </summary>
		public float X { get { return model.X; } }

		/// <summary>
		/// Y座標 を取得
		/// </summary>
		public float Y { get { return model.Y; } }

		/// <summary>
		/// 高さ を取得
		/// </summary>
		public float Height { get { return model.Height; } }

		/// <summary>
		/// 文字間隔 を取得
		/// </summary>
		public float Pitch { get { return model.Pitch; } }

		/// <summary>
		/// 文字間隔 を取得
		/// </summary>
		public float Aspect { get { return model.Aspect; } }

		/// <summary>
		/// 角度 を取得
		/// </summary>
		public float Angle { get { return model.Angle; } }

		/// <summary>
		/// 円弧半径 を取得
		/// </summary>
		public float ArcRadius { get { return model.ArcRadius; } }

		/// <summary>
		/// 打刻時ヘッド移動速度 を取得
		/// </summary>
		public short Speed { get { return model.Speed; } }

		/// <summary>
		/// 打刻深度 を取得
		/// </summary>
		public short Density { get { return model.Density; } }

		/// <summary>
		/// 打刻力 を取得
		/// </summary>
		public short Power { get { return model.Power; } }

		/// <summary>
		/// コントローラバージョン を取得
		/// </summary>
		public ushort HostVersion { get { return model.HostVersion; } }

		/// <summary>
		/// 未使用
		/// </summary>
		public byte LinkFlug { get { return model.LinkFlug; } }

		/// <summary>
		/// 未使用
		/// </summary>
		public byte[] Links { get { return model.Links; } }

		/// <summary>
		/// ミラーなどのオプションを取得
		/// </summary>
		public byte OptionSw { get { return model.OptionSw; } }

        /// <summary>
        /// シリアル番号？ を取得．
        /// </summary>
        /// <value>The serial no.</value>
		public byte SerialNo { get { return model.SerialNo; } }

		/// <summary>
		/// 打刻深度？ を取得
		/// </summary>
		public short ZDepth { get { return model.ZDepth; } }

		/// <summary>
		/// 図形タイプを取得．列挙値として定義されています．
		/// 255:どの図形でもない
		/// </summary>
		public byte Type { get { return model.Type; } }

		/// <summary>
		/// 基準ポイントの位置情報を取得．列挙値として定義されています．
		/// </summary>
		public byte BasePoint { get { return model.BasePoint; } }

		/// <summary>
		/// スペア領域を取得．未使用の領域です．
		/// </summary>
		public short[] Spares { get { return model.Spares; } }


        /// <summary>
        /// MBDataをインスタンス化します．
        /// </summary>
		public MBData ()
		{
			model = new MBDataStructure ();
			// validate(model);
		}


        /// <summary>
        /// MBDataStructureの内容でMBDataをインスタンス化します．
        /// </summary>
        /// <param name="model">Model.</param>
		public MBData (MBDataStructure model)
		{
			this.model = model.Clone ();
			this.model.UpdateType ();
		}

        /// <summary>
        /// MBDataStructureに変換します．
        /// </summary>
        /// <returns>The mutable.</returns>
        public MBDataStructure ToMutable () {
            return model.Clone ();
        }

	}
}
