<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# GameFrameX Web

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.web)](https://github.com/GameFrameX/com.gameframex.unity.web/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.web)](https://github.com/GameFrameX/com.gameframex.unity.web/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

インディゲーム開発者向けオールインワンソリューション · インディ開発者の夢を支援

<br />

[ドキュメント](https://gameframex.doc.alianblank.com) · [クイックスタート](#クイックスタート) · QQグループ: 467608841 / 233840761

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | **日本語** | [한국어](README.ko.md)

</div>

## プロジェクト概要

GameFrameX Web コンポーネントは、高性能な Unity HTTP ネットワークライブラリで、様々なネットワークリクエストシナリオを処理するためのシンプルで使いやすい API を提供します。GET、POST リクエストに対応し、文字列、JSON、バイナリデータなど複数の形式を処理できます。

## 特徴

- **高性能非同期処理** - C# Task 非同期パターンに基づき、async/await をサポート
- **複数データ形式対応** - 文字列、JSON、バイナリデータ、Protocol Buffers
- **クロスプラットフォーム** - WebGL、PC、モバイルプラットフォームに対応
- **コネクションプール管理** - スマートな接続再利用、最大同時接続数制御
- **安全で信頼性** - 包括的なエラー処理とタイムアウトメカニズム
- **拡張が容易** - モジュラー設計、カスタムデータシリアライズ対応

## クイックスタート

### インストール

以下のいずれかの方法を選択してください：

1. Unity プロジェクトの `Packages/manifest.json` を編集し、`scopedRegistries` セクションを追加してください：
   ```json
   {
     "scopedRegistries": [
       {
         "name": "GameFrameX",
         "url": "https://gameframex.upm.alianblank.uk",
         "scopes": [
           "com.gameframex"
         ]
       }
     ],
     "dependencies": {
       "com.gameframex.unity.web": "1.3.5"
     }
   }
   ```

   `scopes` は、どのパッケージをこのレジストリから解決するかを制御します。`com.gameframex` で始まるパッケージのみがこのレジストリから取得されます。

2. `manifest.json` の `dependencies` に直接追加：
   ```json
   {
      "com.gameframex.unity.web": "https://github.com/gameframex/com.gameframex.unity.web.git"
   }
   ```
3. Unity の **Package Manager** で **Git URL** を使用して追加：`https://github.com/gameframex/com.gameframex.unity.web.git`
4. リポジトリを Unity プロジェクトの `Packages` ディレクトリにクローンしてください。自動的に読み込まれます。
## 使用例

### バイナリデータのアップロード

```csharp
public async Task UploadBinaryDataAsync(byte[] fileData, string fileName)
{
    var webManager = GameFrameworkEntry.GetModule<IWebManager>();

    var queryParams = new Dictionary<string, string>
    {
        { "fileName", fileName }
    };

    var headers = new Dictionary<string, string>
    {
        { "Content-Type", "application/octet-stream" },
        { "Authorization", "Bearer your-token" }
    };

    WebBufferResult result = await webManager.PostToBytes(
        "https://api.example.com/upload",
        fileData,
        queryParams,
        headers
    );

    if (result.IsSuccess)
    {
        Debug.Log("Upload successful!");
        byte[] responseData = result.Data;
    }
}
```

### Protocol Buffers の使用

```csharp
[ProtoContract]
public class UserRequest
{
    [ProtoMember(1)]
    public string UserId { get; set; }
}

[ProtoContract]
public class UserResponse
{
    [ProtoMember(1)]
    public string UserName { get; set; }

    [ProtoMember(2)]
    public string Email { get; set; }
}

public async Task<UserResponse> GetUserProtoBufAsync(string userId)
{
    var request = new UserRequest { UserId = userId };
    return await webManager.PostProtoBuf<UserResponse>(
        "https://api.example.com/user/protobuf",
        request
    );
}
```

### エラー処理

```csharp
public async Task<string> SafeWebRequestAsync(string url)
{
    try
    {
        return await webManager.GetToString(url);
    }
    catch (WebException ex) when (ex.Status == WebExceptionStatus.Timeout)
    {
        Debug.LogError("リクエストタイムアウト: " + ex.Message);
        return null;
    }
    catch (IOException ex)
    {
        Debug.LogError("ネットワークIOエラー: " + ex.Message);
        return null;
    }
    catch (Exception ex)
    {
        Debug.LogError("リクエスト失敗: " + ex.Message);
        return null;
    }
}
```

## API リファレンス

### コアインターフェース：IWebManager

#### GET リクエスト

```csharp
Task<string> GetToString(string url);
Task<string> GetToString(string url, Dictionary<string, string> queryString);
Task<string> GetToString(string url, Dictionary<string, string> queryString, Dictionary<string, string> header);

Task<byte[]> GetToBytes(string url);
Task<byte[]> GetToBytes(string url, Dictionary<string, string> queryString);
Task<byte[]> GetToBytes(string url, Dictionary<string, string> queryString, Dictionary<string, string> header);
```

#### POST リクエスト

```csharp
Task<string> PostToString(string url, Dictionary<string, string> formData = null);
Task<string> PostToString(string url, Dictionary<string, string> formData, Dictionary<string, string> queryString);
Task<string> PostToString(string url, Dictionary<string, string> formData, Dictionary<string, string> queryString, Dictionary<string, string> header);

Task<byte[]> PostToBytes(string url, Dictionary<string, string> formData);
Task<byte[]> PostToBytes(string url, Dictionary<string, string> formData, Dictionary<string, string> queryString);
Task<byte[]> PostToBytes(string url, Dictionary<string, string> formData, Dictionary<string, string> queryString, Dictionary<string, string> header);

Task<WebBufferResult> PostToBytes(string url, byte[] binaryData, Dictionary<string, string> queryString, Dictionary<string, string> header, object userData = null);
```

#### 高度な機能

```csharp
// Protocol Buffers サポート
Task<T> GetProtoBuf<T>(string url) where T : class, IExtensible;
Task<T> PostProtoBuf<T>(string url, IExtensible requestData) where T : class, IExtensible;

// JSON サポート（拡張メソッド経由）
Task<T> GetJson<T>(string url);
Task<T> PostJson<T>(string url, object data);
```

### 設定オプション

```csharp
// リクエストタイムアウト（デフォルト：30秒）
TimeSpan RequestTimeout { get; set; }

// 最大同時接続数（デフォルト：8）
int MaxConnectionPerServer { get; set; }

// 詳細ログの有効/無効
bool EnableWebLog { get; set; }
```

## プラットフォーム対応

| プラットフォーム | 対応状況 | 備考 |
|-----------------|----------|------|
| Windows | 対応 | 完全対応 |
| macOS | 対応 | 完全対応 |
| Linux | 対応 | 完全対応 |
| Android | 対応 | 完全対応 |
| iOS | 対応 | 完全対応 |
| WebGL | 対応 | マルチスレッド非対応、すべてのリクエストはメインスレッドで処理 |

### 設定

```csharp
private void ConfigureWebManager()
{
    var webManager = GameFrameworkEntry.GetModule<IWebManager>();

    // リクエストタイムアウトを60秒に設定
    webManager.RequestTimeout = TimeSpan.FromSeconds(60);

    // 最大同時接続数を16に設定
    webManager.MaxConnectionPerServer = 16;

    // 詳細ログを有効化
    webManager.EnableWebLog = true;
}
```

### トラブルシューティング

1. **WebGL プラットフォームの制限**
   - WebGL はマルチスレッドをサポートしていません。すべてのリクエストはメインスレッドで処理されます
   - ブロッキング呼び出しではなく `await` を使用することをお勧めします

2. **CORS の問題**
   - サーバーに正しい CORS ヘッダーが設定されていることを確認してください
   - WebGL ビルドの場合、サーバーは OPTIONS プリフライトリクエストをサポートする必要があります

3. **HTTPS 証明書の問題**
   - モバイルデバイスでは証明書検証の処理が必要な場合があります
   - カスタム証明書検証コールバックを使用できます

## ドキュメントとリソース

- [GameFrameX メインプロジェクト](https://github.com/gameframex/com.gameframex.unity)
- [公式ドキュメント](https://gameframex.doc.alianblank.com)
- [サンプルプロジェクト](https://github.com/gameframex/com.gameframex.unity.examples)
- [イシュー報告](https://github.com/gameframex/com.gameframex.unity.web/issues)

## コミュニティとサポート

ご質問やサポートが必要な場合は、以下の方法でお問い合わせください：

- メール: alianblank@outlook.com
- [イシューを報告](https://github.com/gameframex/com.gameframex.unity.web/issues)
- [ドキュメントを参照](https://gameframex.doc.alianblank.com)

## コントリビュート

Issue や Pull Request をお気軽に提出ください！

1. このプロジェクトをフォーク
2. フィーチャーブランチを作成 (`git checkout -b feature/amazing-feature`)
3. 変更をコミット (`git commit -m 'Add some amazing feature'`)
4. ブランチにプッシュ (`git push origin feature/amazing-feature`)
5. Pull Request を作成

## 変更履歴

詳細なバージョン更新情報は [CHANGELOG.md](CHANGELOG.md) をご覧ください。


## 依存関係

| パッケージ | 説明 |
|----------|------|
| `com.gameframex.unity` | 1.1.1 |
| `com.gameframex.unity.network` | 2.5.1 |
## ライセンス

詳しくは [LICENSE.md](LICENSE.md) をご参照ください。
