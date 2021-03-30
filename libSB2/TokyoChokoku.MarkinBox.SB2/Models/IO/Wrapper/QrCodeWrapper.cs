using System;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    using System;


    public class QrCodeWrapper : BarcodeWrapper
    {
        public QrCodeWrapper (MBDataStructure data) : base (data) {
        }

        /// <summary>
        /// 打刻力
        /// </summary>
        /// <value>The power.</value>
        public short Power {
            get { return data.Power;  }
            set { data.Power = value; }
        }

        /// <summary>
        /// 打刻ヘッドの移動速度
        /// </summary>
        /// <value>The speed.</value>
        public short Speed {
            get { return data.Speed;  }
            set { data.Speed = value; }
        }

        /// <summary>
        /// 基準点
        /// </summary>
        /// <value>The base point.</value>
        public byte BasePoint {
            get { return data.BasePoint;  }
            set { data.BasePoint = value; }
        }

        /// <summary>
        /// X座標
        /// </summary>
        /// <value>The x.</value>
        public float X {
            get { return data.X;  }
            set { data.X = value; }
        }

        /// <summary>
        /// Y座標
        /// </summary>
        /// <value>The y.</value>
        public float Y {
            get { return data.Y;  }
            set { data.Y = value; }
        }

        /// <summary>
        /// テキスト
        /// </summary>
        /// <value>The text.</value>
        public string Text {
            get { return data.Text;  }
            set { data.Text = value; }
        }

        /// <summary>
        /// 時計回り方向を正にした傾き
        /// </summary>
        /// <value>The angle.</value>
        public float Angle {
            get { return data.Angle;  }
            set { data.Angle = value; }
        }

        /// <summary>
        /// 幅
        /// </summary>
        /// <value>The width.</value>
        public float Width {
            get { return data.Pitch;  }
            set { data.Aspect = value; }
        }

        

    }
}