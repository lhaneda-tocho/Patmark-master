using System;

namespace TokyoChokoku
{
    /// <summary>
    /// 諸々の定数を定義しています。
    /// </summary>
    [Obsolete("適宜Enum等に置き換えてください。", false)]
    public static class MBConstants
	{
		//public const int PIXEL_FORMAT = PixelFormat.RGBX_8888;

		/// <summary>
		/// 1ミリが画面上の何ピクセルに対応するかを表します。
		/// </summary>
		public const float PixelPerMilli = 10f;

		// 文字コードを指定します。
		public const String CHARSET_ANDROID = "UTF-8";
		public const String CHARSET_PC = "Windows-31j";
		public const String LocalMbFileEncoding = "shift-jis"; // セーブデータのエンコーディング

		// ファイル・フォルダ名関係
		public const String FILE_EX_PPG = ".ppg";

		// バイト数の対応
		public const int SIZE_WORD = 2;		//
		public const int SIZE_LONG = 4;		//
		public const int SIZE_FLOAT = 4;		//
		public const int SIZE_BYTE = 1;		//

		// 最大値
		public const int MAX_OF_BYTE = 256;

		// 通信時スリープ時間
		public const int SLEEP_SHORT = 30;	//
		public const int SLEEP_MEDIUM = 50;	//
		public const int SLEEP_LONG = 300;	//
		public const int SLEEP_MAX = 1000;	//

		// 通信返答待ちタイムアウト時間
		public const int TIMEOUT_MEDIUM = 500;				//
		public const int TIMEOUT_MAX = 10000;					//
		public const int TIMEOUT_MUST_COMPLETE = 15000;		// 時間がかかっても必ず実行させたい
		public const int TIMEOUT_CONNECTION_CHECK = 5000;	//

		// 　
		public const int NUMBER_OF_SIGNIFICANT = 1;		// 浮動小数点数の有効桁数

		//	float[0〜7] まででRectの四隅座標を保持する際の、各座標のインデックス
		//	********************************************************************
		public const int LTX = 0;
		public const int LTY = 1;
		public const int RTX = 2;
		public const int RTY = 3;
		public const int RBX = 4;
		public const int RBY = 5;
		public const int LBX = 6;
		public const int LBY = 7;


		//	カスタムカラー
		//	********************************************************************
//		// MarkinBlue 空色 #52BCDE
//		public const int MARKIN_BLUE = Color.argb(255, 82, 188, 222);
//		// 図形描画用青
//		public const int MEDIUM_BLUE = Color.argb(255, 30, 144, 255);
//		// MarkinBrown ブラウン #392D2A
//		public const int MARKIN_BROWN = Color.argb(255, 57, 45, 42);
//		// MarkinBrown　を少し薄くしたもの
//		public const int MARKIN_BROWN_RIGHT = Color.argb(64, 57, 45, 42);

		public const int MB_OFFLINE = 0;
		public const int MB_ONLINE = 1;

		//	機種に対応する番号 MBのメモリ上に格納されている
		//	0：3315、1,2：1010　　3:8020
		//	********************************************************************
		public const int MB_MACHINE_MODEL_NO_3315 = 0;
		public const int MB_MACHINE_MODEL_NO_1010_Scratch = 1;
		public const int MB_MACHINE_MODEL_NO_1010 = 2;
		public const int MB_MACHINE_MODEL_NO_8020 = 3;
		public const int MB_MACHINE_MODEL_NO_DEFAULT = MB_MACHINE_MODEL_NO_3315;

		//	MB関連
		//	********************************************************************

		// MBファイルのファイル名の余り部分
		public const int MB_FILE_NAME_PREFIX_SIZE = 5;
		public const char MB_FILE_NAME_SUFFIX = 'ÿ';
		public const char MB_FILE_NAME_NONE = '\0';

		// R0[30]に14を書き込んでヘッド移動する際の移動モード
		public const short MB_MOVING_MODE_ABSOLUTE = 1;	// 1：絶対値指定

		// フィールドを１つ打刻した後の待ち時間
		public const short MB_MARKING_INTERVAL_NONE = 0;	// 0:打刻しない

		// 基本動作クロック
		public const int MB2_STD_CLOCK = 750 * 1000; // 750KHzで動作

		// 打刻モード
		public const byte MB_MARKING_MODE_PCMODE = 0x00;	// PCから打刻操作
		public const byte MB_MARKING_MODE_MBMODE = 0x01; // MBの本体ボタンで打刻操作

		// 打刻力の基準値
		public const byte MB_MARKING_POWER_DEFAULT = 0x14;
		//
		public const byte MB_MARKING_POWER_TEST = (byte)10;

		// 打刻アラームコード
		public const int MB_ALARM_NONE = 0;
		public const int MB_ALARM_ORGX = 91;            // X 軸原点エラー
		public const int MB_ALARM_ORGY = 92;           // Y 軸原点エラー
		public const int MB_ALARM_ORGZ = 93;           // Z 軸原点エラー

		public const int MB_ALARM_FLNOERR = 10;       // ファイル　番号未設定
		public const int MB_ALARM_NOFILE = 11;           // ファイル　が無い
		public const int MB_ALARM_SD_ERR = 15;          // SD CARD 初期化　不良
		public const int MB_ALARM_RENTAL_OVR = 19;     // レンタル期間　オーバー　発生
		public const int MB_ALARM_HEAD_MV_TIMEOUT = 80;        // 円筒時間　ヘッド移動に時間がオーバー
		public const int MB_ALARM_SERIAL_OVR = 20;        // シリアルカウンタ値　オーバー　発生
		public const int MB_ALARM_SERIAL_STOP = 21;       // シリアルカウンタ値オーバー　設定　Stop 14/06/02

		public const int MB_ALARM_EXECUTE1 = 51;            // 実行時エラーが発生
		public const int MB_ALARM_EXECUTE2 = 52;            // 実行時エラーが発生
		public const int MB_ALARM_EXECUTE3 = 53;           // 実行時エラーが発生
		public const int MB_ALARM_EXECUTE4 = 54;            // 実行時エラーが発生
		public const int MB_ALARM_EXECUTE5 = 55;            // 実行時エラーが発生
		public const int MB_ALARM_EXECUTE6 = 56;            // 実行時エラーが発生
		public const int MB_ALARM_EXECUTE7 = 57;            // 実行時エラーが発生
		public const int MB_ALARM_NO_FONT = 58;


		//	コントローラファイル関連
		//	********************************************************************
		public const int MAX_NUM_OF_FIELD = 51;								    	// フィールド（打刻する図形）数の最大値
		public const int FIELD_SIZE_W = 88;									    	// １フィールドのサイズ（word）
		public const int FIELD_SIZE = FIELD_SIZE_W * SIZE_WORD;				    	// １フィールドのサイズ（byte）
		public const int MAX_INDEX_OF_MB_FILES = 254;						    	// コントローラに保存されているMarkingFileの最大数
		public const int MAX_NUM_OF_MB_FILES = MAX_INDEX_OF_MB_FILES + 1;	        //
		public const int MB_FILE_SIZE_W = FIELD_SIZE_W * MAX_NUM_OF_FIELD;	        // MarkingFileのファイルサイズ（word）
		public const int MB_FILE_SIZE = MB_FILE_SIZE_W * SIZE_WORD;		            // MarkingFileのファイルサイズ（byte）
		public const int MB_FILE_NAME_SIZE = 16;							            // MarkingFileのファイル名サイズ（byte）
		public const int MB_FILE_MAP_SIZE = 512;								        // 仕様確認中... C0[20000 + map_size]以降に256ファイル分のデータが格納されている
		public const int MB_FILE_SELECTOR_GENERAL_FRAME_UNITS = 10;				    // MarkingFileの大分類選択用コンボボックスにて、何件ずつ分類するか
		public const int MB_MESSAGE_MAX_LENGTH_W = 61;								// MBとの通信で、一度にやり取りできる最大文字数（word）
		public const int MB_MESSAGE_MAX_LENGTH = MB_MESSAGE_MAX_LENGTH_W * SIZE_WORD;// MBとの通信で、一度にやり取りできる最大文字数（byte）


		//	カレンダー関連
		//	********************************************************************
		public const int CALENDAR_SETTING_SHIFT_SIZE_W = 3;									// シフト情報のサイズ(word)
		public const int MAX_NUMOF_CALENDAR_SHIFT = 5;

		//	シリアル関連
		//	********************************************************************
		public const int KINDS_OF_SERIAL_SETTING = 4;										// シリアル設定ファイルの種類（１〜４）
		public const int SERIAL_SETTING_SIZE_W = 8;											// シリアルカウンタファイルのサイズ(word)
		public const int SERIAL_SETTING_SIZE = SERIAL_SETTING_SIZE_W * SIZE_WORD;			// シリアルカウンタファイルのサイズ(byte)

		public const int NUM_OF_SERIAL_COUNTERS_MARKING_FILE = MAX_INDEX_OF_MB_FILES;	// MarkingFileのシリアルカウンタファイルの数
		public const int SERIAL_COUNTER_SIZE_W = 8;											// シリアルカウンタファイルのサイズ(word)
		public const int SERIAL_COUNTER_SIZE = SERIAL_COUNTER_SIZE_W * SIZE_WORD;			// シリアルカウンタファイルのサイズ(byte)


		//	Intentリクエストコード
		//	********************************************************************
		public const int REQ_CODE_CHAIN_OF_PROCESSING = 1;	// 複数のActivityにまたがる一連の処理を纏める際に用いるコード
		public const int REQ_CODE_WIRELESS_SETTINGS = 11;	// 「無線とネットワーク」設定画面を開く
		public const int REQ_CODE_ENABLE_BT = 12;			// Bluetoothを有効にする
		public const int REQ_CODE_CALENDAR_SETTINGS = 1000;	// カレンダー設定
		public const int REQ_CODE_SERIAL_SETTINGS = 1001;	// シリアル設定


		//	プレビュー領域の格子の範囲
		//  ********************************************************************
		public const int AREA_X_3315 = 33;
		public const int AREA_Y_3315 = 15;
		public const int AREA_X_1010 = 100;
		public const int AREA_Y_1010 = 100;
		public const int AREA_X_8020 = 80;
		public const int AREA_Y_8020 = 20;

		//	打刻データファイルに関するもの　（※のついているものは、PC版から吐き出されるファイルで存在を確認できなかったもの）
		//  ********************************************************************

		public const int FIELD_TEXT_LENGTH_MAX = 52;
		public const int FIELD_TEXT_OF_2DCODE_LENGTH_MAX = 80;

		//  セクション

		public const String section_base = "BASE";
		public const String section_field = "FIELD";

		//  パラメータのキー　

		//    基本情報
		public const String param_machine_model = "Kishu";

		public const String param_char = "Cd"; //	表示データ mm
		public const String param_char_count = "mCnt"; //	表示データの文字数 mm
		public const String param_mode = "mode";
		public const String param_prm_fl = "PrmFl"; //
		public const String param_id = "ID"; //	 PC 上の　ID　Field No (1-21)
		public const String param_flg = "flg";
		public const String param_type = "Type";

		//    図形情報
		public const String param_x = "X";
		public const String param_y = "Y";
		public const String param_height = "Ht";
		public const String param_pitch = "Pitch";
		public const String param_aspect = "Aspect";
		public const String param_angle = "Angle";
		public const String param_radius = "Radius"; //	円半径 mm
		public const String param_arcRadi = "ArcRadi"; // 円弧半径 mm
		public const String param_arc_center_x = "ArcCenterX"; //	円弧
		public const String param_arc_center_y = "ArcCenterY"; //	円弧

		//    打刻情報
		public const String param_speed = "Speed"; //	打刻速度を 0-9 で設定します
		public const String param_density = "Density"; //	打刻速度の逆数値　9-0 を設定します		...( Speed + Density = 9 ) になります
		public const String param_power = "Power"; //	打刻力を 0-9 で設定します
		public const String param_host_version = "Host_Version"; //	Host Versionが入ります Ver1.01.30 = 0x1130		...とりあえず　0x9999 としておいてください

		//    Field Link
		public const String param_link_flag = "LnlFlg"; //
		public const String param_field_link = "FldLnk"; //
		public const String param_option_sw = "OptSw"; //
		public const String param_serial_no = "SerStNo"; //
		public const String param_serial_set_no = "SerialSetNo";
		public const String param_now_serial_no = "NowSerialNo";
		public const String param_field_link_name = "FlNm";
		public const String param_field_link_flag = "FldLnkFlg";
		public const String param_field_link_1 = "FldLnk1";
		public const String param_field_link_2 = "FldLnk2";
		public const String param_field_link_3 = "FldLnk3";
		public const String param_field_link_4 = "FldLnk4";
		public const String param_field_link_5 = "FldLnk5";

		public const String param_z_depth = "ZDepth"; //
		public const String param_base_point = "KijyunPoint"; // 基準点（元 "Bp"）
		//	1：左下　2：左中　3：左上
		//	4：中央下　5：中央中　6：中央上
		//	7：右下　8：右中　0：右上

		public const String param_spare = "sp3"; //

		//    解説無し 、かつ分類困難、かつPC版から吐き出されるファイルには存在しているもの
		public const String param_dd = "dd";
		public const String param_ddorg = "ddorg";
		public const String param_pause_exe = "PauseExe";
		public const String param_rect_sw = "RectSw";
		public const String param_stx = "stx";
		public const String param_sty = "sty";
		public const String param_enx = "enx";
		public const String param_eny = "eny";
		public const String param_p_ht = "p_ht";
		public const String param_wd = "wd";
		public const String param_op_ode = "OpMode";
		public const String param_mouse_click_x = "MouseClickX";
		public const String param_mouse_click_y = "MouseClickY";
		public const String param_p_center_x = "p_CenterX";
		public const String param_p_center_y = "p_CenterY";
		public const String param_p_radius = "p_Radius";
		public const String param_p_x3 = "p_X3";
		public const String param_p_y3 = "p_Y3";
		public const String param_p_x4 = "p_X4";
		public const String param_p_y4 = "p_Y4";
		public const String param_p_x13 = "p_X13";
		public const String param_p_y13 = "p_Y13";
		public const String param_p_x14 = "p_X14";
		public const String param_p_y14 = "p_Y14";
		public const String param_p_arc_length = "p_ArcLng";
		public const String param_rel_angle = "rel_Angle";
		public const String param_zoom_fct = "ZoomFct"; //	1mm当たり何px使用するかを定義している。 stx,sty,enx,enyなどはこれを元にセットされている模様
		public const String param_kijun_point = "KijyunPoint";
		public const String param_layer_work_shape = "Layer_WORK_SHAPE";

		// 図形タイプ（種類）
		public const byte FieldTypeText = 0; // 文字列
		public const byte FieldTypeOuterArcText = 1; // 文字列（外弧）
		public const byte FieldTypeInnerArcText = 2; // 内弧
		public const byte FieldTypeXVerticalText = 3; // 文字列（縦書き 文字の底辺が x のプラス方向へ向くのが標準）
		public const byte FieldTypeYVerticalText = 4; // 文字列（縦書き 文字の底辺が y のプラス方向へ向くのが標準）
		public const byte FieldTypeQrCode = 5; // QRコード
		public const byte FieldTypeDataMatrix = 6; // データマトリクス
		public const byte FieldTypeRectangle = 10; // 矩形
		public const byte FieldTypeCircle = 11; // 完全な円
		public const byte FieldTypeLine = 12; // 直線
		public const byte FieldTypeEllipse = 13; // 楕円
		public const byte FieldTypeTriangle = 14; // 三角
		public const byte FieldTypeBypass = 15;// バイパス

		// 図形モード（ビットフラグ）
		public const int FieldModeNone = 0x0000;					// モード指定無し
		public const int FieldModeUnknwon1 = 0x0001;				//
		public const int FieldModeCalendar = 0x0002;				// カレンダー
		public const int FieldMode4 = 0x0004; 					//
		public const int FieldModeSerial = 0x0008; 				// シリアル
		public const int FieldModeUnknwon10 = 0x0010;				//
		public const int FieldModeOuterArc = 0x0020;				// 外弧
		public const int FieldModeInnerArc = 0x0040;				// 内弧
		public const int FieldModeUnknwon80 = 0x0080;				//
		public const int FieldModeUnknwon100 = 0x0100;			//
		public const int FieldModeTriangle = 0x0200;				// 三角
		public const int FieldModeQrCode = 0x0400;				// QRコード
		public const int FieldModeDataMatrix = 0x0800;			//
		public const int FieldModeVerticalY = 0x1000;				// 縦書きテキスト　縦方向　vertical y
		public const int FieldModeVerticalX = 0x2000;				// 縦書きテキスト　横方向　vertical x
		public const int FieldModeRectOrLineOrTriangle = 0x4000;	// Rectangle/Line/Triangle その他のパラメータから判定
		public const int FieldModeCircleOrEllipse = 0x8000;		// Circle/Ellipse その他のパラメータから判定

		// 図形基準点
		public const int FieldBasePointLB = 0; // 基準点（左下） 
		public const int FieldBasePointLM = 1; // 基準点（左中）
		public const int FieldBasePointLT = 2; // 基準点（左上）
		public const int FieldBasePointCB = 3; // 基準点（中央下）
		public const int FieldBasePointCM = 4; // 基準点（中央中）
		public const int FieldBasePointCT = 5; // 基準点（中央上）
		public const int FieldBasePointRB = 6; // 基準点（右下）
		public const int FieldBasePointRM = 7; // 基準点（右中）
		public const int FieldBasePointRT = 8; // 基準点（右上）

		// 図形オプション
		public const int fig_option_mirror = 1;	// オプション：ミラー

		// フィールド内のフォント指定
		public const int field_fontcode_tc_font = 0;		// オプション：2Dバーコード
		public const int field_fontcode_barcode = 50;	// オプション：2Dバーコード
		public const int field_fontcode_5x7_dot = 64;	// オプション：５x７ドット
		public const int field_fontcode_pc_font = 128;	// オプション：2Dバーコード

		// 図形描画に関する設定
		public const int DRAW_BASEPOINT_RADIUS = 3;	// 描画する基準点の半径

		// シリアル設定値
		public const int SERIAL_FORMAT_ZERO = 0;		// 0埋め
		public const int SERIAL_FORMAT_RIGHT = 1;	// 右詰め
		public const int SERIAL_FORMAT_LEFT = 2;		// 左詰め

		// 正規表現
		public const String REGEX_BLOCK_QUOTE_INNER = "\\[(.+)\\]";              // 汎用（鍵括弧の中身を取得）
		public const String REGEX_LOGO = "@L\\[[0-9]+\\]";				        // ロゴの値取得用
		public const String REGEX_LOGO_VALUE = "@L\\[([0-9]+)\\]";		        // ロゴの値取得用
		public const String REGEX_SERIAL = "@S\\[[0-9]+-[0-9]\\]";			        // シリアルの値検出用
		public const String REGEX_SERIAL_VALUE = "@S\\[([^\\]]+)\\]";	        // シリアルの値取得用
		public const String REGEX_SERIAL_SETTINGS = "@S\\[([0-9]+)-([0-9]+)\\]"; // シリアルの設定値取得用
		public const String REGEX_SERIAL_SETTINGS_NO = "@S\\[[0-9]+-([0-9]+)\\]"; // シリアルの設定番号取得用
		public const String REGEX_CALENDAR = "@C\\[[^\\]]+\\]";			        // カレンダーの値検出用
		public const String REGEX_CALENDAR_VALUE = "@C\\[([^\\]]+)\\]";          // カレンダーの値取得用

		public const String REGEX_AVAILABLE_CODES_OF_2D_CODE = "[0-9a-zA-Z !-/:-@≠\\[-`{-~]";       // 2DCodeに使用可能な文字
		public const String REGEX_UNAVAILABLE_CODES_OF_DATAMATRIX = "[^0-9a-zA-Z !-/:-@≠\\[-`{-~]";       // 2DCodeに使用不可能な文字
		public const String REGEX_AVAILABLE_CODES_OF_QR_CODE = "[0-9A-Z \\$\\%\\*\\+\\-\\.\\/\\:]"; // QRCodeに使用可能な文字
		public const String REGEX_UNAVAILABLE_CODES_OF_QR_CODE = "[^0-9A-Z \\$\\%\\*\\+\\-\\.\\/\\:]"; // QRCodeに使用不可能な文字

		// 文字列関係
		public const String SPLITER_SERIAL_TEXT = "-";
	}
}
