# Unicast.ProgressKit

# 概要

UIに進捗情報を伝える必要のある非同期処理を管理・起動するライブラリです。

# プロジェクト設定の説明

## BUG: TOCHO\_MARKING\_APP-90 の対策

このバグの対策のため、 スレッド安全性・例外安全性に関わる 一部の機能を無効化しております。

無効化された機能を有効化するには ENABLE_THREAD_CHECK を有効にします。
これは、以下の手順で設定できます、

1. 「プロジェクト▶️右クリック▶️オブション」 をクリック
2. 「ビルド▶️ビルド設定▶️コンパイラ」 をクリック
3. 設定を変更する「ビルドターゲット」を選択する (「Debug」か「Release」)
4. 「シンボル」のテキストボックスの先頭に `ENABLE_THREAD_CHECK;` を追記する (セミコロンまで入れること)
5. OK を押して設定を保存する。


# 既存のバグについて

## #5E504D87

| Title | Value |
|:--|:--|
| 発生日   | 2020/05/01 |
| ID      | TOCHO_MARKING_APP-90 |
| Backlog | https://unicastinc.backlog.com/view/TOCHO_MARKING_APP-90 |


PM/Android にて、ProgressTaskWorker.CurrentThreadUsing() の2回目の呼び出し時に Xamarin がクラッシュする問題が発生しております。


```
[] * Assertion at /Users/builder/jenkins/workspace/archive-mono/2019-10/android/release/mono/mini/mini-trampolines.c:543, condition `generic_virtual->is_inflated' not met
[libc] Fatal signal 6 (SIGABRT), code -6 in tid 14564 (chokoku.patmark)
```


# 使い方
## ProgressAPI の初期化

UI スレッド上で、下記のように初期化します。

```csharp
// UI スレッドの SynchronizationContext を使用して
// ProgressTaskWorker を初期化
ProgressTaskWorker mainWorker =
        ProgressTaskWorkerOnSyncContext.CreateCurrentContext();

// ProgressAPI に mainWorker を
// コンストラクタ・インジェクションして初期化
ProgressAPI api =
        new ProgressAPI(mainWorker);
```

上記の `new ProgressAPI(mainWorker)` で ProgressAPI の初期化を行い、プログレス監視サービスの起動が行われます。


----


## 非同期処理の起動

ProgressAPI インスタンスのメソッドを使用して起動します。
第一引数には使用するワーカ、第二引数には実行するタスクを指定します。

```csharp
// メンバ変数に入れておく
ProgressAPI API = new ProgressAPI(mainWorker);

// 起動したいタスク. ProgressTaskHandler を引数に取る必要がある。
async Task HeavyTask(ProgressTaskHandler taskHandler) { ... }

async Task Start()
{
    // メインスレッド上でタスクを起動する方法
    await API.LaunchAsync(null, HeavyTask);

    // スレッドプール上でタスクを起動する方法
    await API.LaunchAsync(ProgressTaskWorkerOnThreadPool.Instance, HeavyTask);
}

```