
# FieldCanvasModule

これは，SB2での Canvasの実装の共通化可能な実装を移動させたものです.

## 方針
- 共通モジュールからFieldCanvasの機能の一部にアクセスできるようにする.



## SB2との互換性
- 旧来の CanvasInfo との互換性はない(機能は全く同じです)


## 今後の内容
1. 「iOS Android 共通描画モジュール RenderKit」が低レイヤ側へ移動した際に, インポート先の変更が必要となります．
      TokyoChokoku.Patmark.RenderKit -> TokyoChokoku.RenderKit
   
  
2. (1)に合わせて sb2 との完全な互換性の獲得

3. 最終的には，sb2から参照できるように低レイヤ側へ移動する