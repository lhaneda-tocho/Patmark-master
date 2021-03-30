# How to build  

1. ファイル "Global.ttinclude" をソリューションディレクトリに作成します。  

 例）
```
<#@ template language="C#" #>
<#@ assembly name="/Volumes/unicast_disk/sketchbook_touch/src/sketchbook-touch/TokyoChokoku.MarkinBox.Sketchbook.TemplateMeta/bin/Debug/TokyoChokoku.MarkinBox.Sketchbook.TemplateMeta.dll" #>
<#@ assembly name="/Volumes/unicast_disk/sketchbook_touch/src/sketchbook-touch/TokyoChokoku.MarkinBox.Sketchbook.MetaCommunication/bin/Debug/TokyoChokoku.MarkinBox.Sketchbook.MetaCommunication.dll" #>
```  
パスを自身の環境に置き換えてください。  
t4テンプレートで利用するdllを絶対パスで指定します。  

# Trouble shooting  

## LaunchScreenが表示されない  

Launchscreen.storyboard を ディレクトリ移動 → 元に戻す と治ります。  
移動先はどこでも構いません。  
