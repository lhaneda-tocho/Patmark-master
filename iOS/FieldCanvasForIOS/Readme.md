
# FieldCanvasForIOS

これは，SB2での Canvasの実装を新実装に合わせて修正したモジュールです. 

## 方針
- iOSのCoreGraphicsに直接アクセスして描画可能
- Patmark以降の「共通描画モジュール RenderKit」上で実装
- PatmarkのCanvasから生成可能
    PatmarkCanvas ---- create ----> FieldCanvas



## SB2との互換性
- 旧来の FieldCanvas との互換性はない
    - テキスト描画に修正がありますが，それ以外は同じです.

- 旧来の Drawableとの互換性はない
  reason  A: 「RenderKit」の登場で 変更が必要になったため
  measure A: 「RenderKit」への対応
  
  reason  B: sb2とpatmarkの共通化プロジェクトの登場で 変更が必要になったため
             - TokyoChokoku 直下のモジュール
             - TokyoChokoku.Communication モジュール
  measure B: 共通化プロジェクトを取り込んだものに対応


## 今後の内容
1. 「iOS Android 共通描画モジュール RenderKit」が低レイヤ側へ移動した際に, インポート先の変更が必要となります．
      TokyoChokoku.Patmark.RenderKit -> TokyoChokoku.RenderKit
   
  
2. (1)に合わせて sb2 との完全な互換性の獲得

3. 最終的には，sb2から参照できるようプロジェクトの分離を行う
      TokyoChokoku.Patmark.iOS.FieldCanvasForIOS -> TokyoChokoku.iOS.FieldCanvasForIOS
      (名前空間がかち合うので TokyoChokoku.iOS.FieldCanvas という名前は X )
