<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# GameFrameX Web

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.web)](https://github.com/GameFrameX/com.gameframex.unity.web/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.web)](https://github.com/GameFrameX/com.gameframex.unity.web/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

獨立遊戲前後端一體化解決方案 · 獨立遊戲開發者的圓夢大使

<br />

[文檔](https://gameframex.doc.alianblank.com) · [快速開始](#快速開始) · QQ群: 467608841 / 233840761

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | **繁體中文** | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>

## 項目簡介

GameFrameX Web 元件是一個高效能的 Unity HTTP 網路請求庫，提供簡潔易用的 API 來處理各種網路請求場景。支援 GET、POST 請求，可處理字串、JSON、二進制資料等多種格式。

## 特性

- **高效能非同步處理** - 基於 C# Task 非同步模式，支援 async/await
- **多資料格式支援** - 字串、JSON、二進制資料、Protocol Buffers
- **跨平台相容** - 支援 WebGL、PC、行動平台
- **連線池管理** - 智慧連線複用，支援最大並發連線數控制
- **安全可靠** - 完善的錯誤處理和超時機制
- **易於擴展** - 模組化設計，支援自定義資料序列化

## 安裝

### 透過 Git URL 安裝（推薦）

1. 在 Unity 編輯器中打開 Package Manager
2. 點擊 "+" 按鈕選擇 "Add package from git URL"
3. 輸入以下 URL：
   ```
   https://github.com/gameframex/com.gameframex.unity.web.git
   ```

### 透過 manifest.json 安裝

在專案的 `Packages/manifest.json` 檔案中新增：

```json
{
  "dependencies": {
    "com.gameframex.unity.web": "https://github.com/gameframex/com.gameframex.unity.web.git",
    "com.gameframex.unity": "https://github.com/gameframex/com.gameframex.unity.git"
  }
}
```

### 手動安裝

1. 下載最新版本發布包
2. 解壓縮到專案的 `Packages` 目錄下
3. Unity 會自動識別並載入包

## 快速開始

### 基本用法

```csharp
using GameFrameX.Web.Runtime;
using System.Threading.Tasks;
using System.Collections.Generic;

public class WebExample : MonoBehaviour
{
    private IWebManager webManager;

    private async void Start()
    {
        // 取得 Web 管理器實例
        webManager = GameFrameworkEntry.GetModule<IWebManager>();

        // 發送 GET 請求取得字串
        string result = await webManager.GetToString("https://api.example.com/data");
        Debug.Log("GET Response: " + result);

        // 發送 POST 請求帶表單資料
        var formData = new Dictionary<string, string>
        {
            { "username", "testuser" },
            { "password", "testpass" }
        };

        string postResult = await webManager.PostToString("https://api.example.com/login", formData);
        Debug.Log("POST Response: " + postResult);
    }
}
```

### 使用 WebComponent（推薦）

```csharp
using GameFrameX.Web.Runtime;
using System.Threading.Tasks;
using System.Collections.Generic;

public class MyWebService : MonoBehaviour
{
    private WebComponent webComponent;

    private void Awake()
    {
        webComponent = gameObject.GetOrAddComponent<WebComponent>();
    }

    public async Task<string> GetUserDataAsync(string userId)
    {
        var queryParams = new Dictionary<string, string>
        {
            { "userId", userId }
        };

        var headers = new Dictionary<string, string>
        {
            { "Authorization", "Bearer your-token-here" }
        };

        return await webComponent.GetToString(
            "https://api.example.com/users",
            queryParams,
            headers
        );
    }

    public async Task<byte[]> DownloadFileAsync(string fileUrl)
    {
        return await webComponent.GetToBytes(fileUrl);
    }
}
```

## 使用範例

### 處理二進制資料上傳

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

### 使用 Protocol Buffers

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

### 錯誤處理

```csharp
public async Task<string> SafeWebRequestAsync(string url)
{
    try
    {
        return await webManager.GetToString(url);
    }
    catch (WebException ex) when (ex.Status == WebExceptionStatus.Timeout)
    {
        Debug.LogError("請求逾時: " + ex.Message);
        return null;
    }
    catch (IOException ex)
    {
        Debug.LogError("網路IO錯誤: " + ex.Message);
        return null;
    }
    catch (Exception ex)
    {
        Debug.LogError("請求失敗: " + ex.Message);
        return null;
    }
}
```

## API 參考

### 核心介面：IWebManager

#### GET 請求

```csharp
Task<string> GetToString(string url);
Task<string> GetToString(string url, Dictionary<string, string> queryString);
Task<string> GetToString(string url, Dictionary<string, string> queryString, Dictionary<string, string> header);

Task<byte[]> GetToBytes(string url);
Task<byte[]> GetToBytes(string url, Dictionary<string, string> queryString);
Task<byte[]> GetToBytes(string url, Dictionary<string, string> queryString, Dictionary<string, string> header);
```

#### POST 請求

```csharp
Task<string> PostToString(string url, Dictionary<string, string> formData = null);
Task<string> PostToString(string url, Dictionary<string, string> formData, Dictionary<string, string> queryString);
Task<string> PostToString(string url, Dictionary<string, string> formData, Dictionary<string, string> queryString, Dictionary<string, string> header);

Task<byte[]> PostToBytes(string url, Dictionary<string, string> formData);
Task<byte[]> PostToBytes(string url, Dictionary<string, string> formData, Dictionary<string, string> queryString);
Task<byte[]> PostToBytes(string url, Dictionary<string, string> formData, Dictionary<string, string> queryString, Dictionary<string, string> header);

Task<WebBufferResult> PostToBytes(string url, byte[] binaryData, Dictionary<string, string> queryString, Dictionary<string, string> header, object userData = null);
```

#### 進階功能

```csharp
// Protocol Buffers 支援
Task<T> GetProtoBuf<T>(string url) where T : class, IExtensible;
Task<T> PostProtoBuf<T>(string url, IExtensible requestData) where T : class, IExtensible;

// JSON 支援（透過擴充方法）
Task<T> GetJson<T>(string url);
Task<T> PostJson<T>(string url, object data);
```

### 設定選項

```csharp
// 設定請求逾時時間（預設：30秒）
TimeSpan RequestTimeout { get; set; }

// 設定最大並發連線數（預設：8）
int MaxConnectionPerServer { get; set; }

// 啟用/停用詳細日誌
bool EnableWebLog { get; set; }
```

## 平台支援

| 平台 | 支援情況 | 備註 |
|------|----------|------|
| Windows | 支援 | 完全支援 |
| macOS | 支援 | 完全支援 |
| Linux | 支援 | 完全支援 |
| Android | 支援 | 完全支援 |
| iOS | 支援 | 完全支援 |
| WebGL | 支援 | 不支援多執行緒，所有請求在主執行緒處理 |

### 設定

```csharp
private void ConfigureWebManager()
{
    var webManager = GameFrameworkEntry.GetModule<IWebManager>();

    // 設定請求逾時為 60 秒
    webManager.RequestTimeout = TimeSpan.FromSeconds(60);

    // 設定最大並發連線數為 16
    webManager.MaxConnectionPerServer = 16;

    // 啟用詳細日誌
    webManager.EnableWebLog = true;
}
```

### 常見問題

1. **WebGL 平台限制**
   - WebGL 不支援多執行緒，所有請求都在主執行緒處理
   - 建議使用 `await` 非同步等待而不是阻塞呼叫

2. **跨域問題 (CORS)**
   - 確保伺服器設定了正確的 CORS 標頭資訊
   - 對於 WebGL 建置，伺服器必須支援 OPTIONS 預檢請求

3. **HTTPS 憑證問題**
   - 在行動裝置上可能需要處理憑證驗證
   - 可以使用自訂憑證驗證回呼

## 文檔與資源

- [GameFrameX 主專案](https://github.com/gameframex/com.gameframex.unity)
- [官方文檔](https://gameframex.doc.alianblank.com)
- [範例專案](https://github.com/gameframex/com.gameframex.unity.examples)
- [問題回報](https://github.com/gameframex/com.gameframex.unity.web/issues)

## 社區與支援

如果您有任何問題或需要協助，可以透過以下方式聯絡我們：

- 電子郵件: alianblank@outlook.com
- [提交 Issue](https://github.com/gameframex/com.gameframex.unity.web/issues)
- [查看文檔](https://gameframex.doc.alianblank.com)

## 貢獻

歡迎提交 Issue 和 Pull Request！

1. Fork 本專案
2. 建立功能分支 (`git checkout -b feature/amazing-feature`)
3. 提交變更 (`git commit -m 'Add some amazing feature'`)
4. 推送到分支 (`git push origin feature/amazing-feature`)
5. 建立 Pull Request

## 更新日誌

查看 [CHANGELOG.md](CHANGELOG.md) 取得詳細的版本更新資訊。

## 開源協議

本專案採用 MIT 許可證 - 查看 [LICENSE.md](LICENSE.md) 檔案了解詳情。
