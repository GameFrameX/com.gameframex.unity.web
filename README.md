# GameFrameX Web 组件

[![Unity Version](https://img.shields.io/badge/Unity-2019.4%2B-blue.svg)](https://unity3d.com)
[![Version](https://img.shields.io/badge/version-1.1.8-green.svg)](package.json)
[![License](https://img.shields.io/badge/license-MIT-orange.svg)](LICENSE.md)

GameFrameX Web 组件是一个高性能的 Unity HTTP 网络请求库，提供简洁易用的 API 来处理各种网络请求场景。支持 GET、POST 请求，可处理字符串、JSON、二进制数据等多种格式。

## ✨ 特性

- 🚀 **高性能异步处理** - 基于 C# Task 异步模式，支持 async/await
- 📦 **多数据格式支持** - 字符串、JSON、二进制数据、Protocol Buffers
- 🌐 **跨平台兼容** - 支持 WebGL、PC、移动平台
- ⚡ **连接池管理** - 智能连接复用，支持最大并发连接数控制
- 🔒 **安全可靠** - 完善的错误处理和超时机制
- 🛠️ **易于扩展** - 模块化设计，支持自定义数据序列化

## 📦 安装

### 通过 Git URL 安装（推荐）

1. 在 Unity 编辑器中打开 Package Manager
2. 点击 "+" 按钮选择 "Add package from git URL"
3. 输入以下 URL：
   ```
   https://github.com/gameframex/com.gameframex.unity.web.git
   ```

### 通过 manifest.json 安装

在项目的 `Packages/manifest.json` 文件中添加：

```json
{
  "dependencies": {
    "com.gameframex.unity.web": "https://github.com/gameframex/com.gameframex.unity.web.git",
    "com.gameframex.unity": "https://github.com/gameframex/com.gameframex.unity.git"
  }
}
```

### 手动安装

1. 下载最新版本发布包
2. 解压到项目的 `Packages` 目录下
3. Unity 会自动识别并加载包

## 🚀 快速开始

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
        // 获取 Web 管理器实例
        webManager = GameFrameworkEntry.GetModule<IWebManager>();
        
        // 发送 GET 请求获取字符串
        string result = await webManager.GetToString("https://api.example.com/data");
        Debug.Log("GET Response: " + result);
        
        // 发送 POST 请求带表单数据
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

### 使用 WebComponent（推荐）

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

## 📚 API 参考

### 核心接口：IWebManager

#### GET 请求

```csharp
// 获取字符串响应
Task<string> GetToString(string url);
Task<string> GetToString(string url, Dictionary<string, string> queryString);
Task<string> GetToString(string url, Dictionary<string, string> queryString, Dictionary<string, string> header);

// 获取字节数组响应
Task<byte[]> GetToBytes(string url);
Task<byte[]> GetToBytes(string url, Dictionary<string, string> queryString);
Task<byte[]> GetToBytes(string url, Dictionary<string, string> queryString, Dictionary<string, string> header);
```

#### POST 请求

```csharp
// 表单 POST 请求
Task<string> PostToString(string url, Dictionary<string, string> formData = null);
Task<string> PostToString(string url, Dictionary<string, string> formData, Dictionary<string, string> queryString);
Task<string> PostToString(string url, Dictionary<string, string> formData, Dictionary<string, string> queryString, Dictionary<string, string> header);

Task<byte[]> PostToBytes(string url, Dictionary<string, string> formData);
Task<byte[]> PostToBytes(string url, Dictionary<string, string> formData, Dictionary<string, string> queryString);
Task<byte[]> PostToBytes(string url, Dictionary<string, string> formData, Dictionary<string, string> queryString, Dictionary<string, string> header);

// 二进制数据 POST 请求
Task<WebBufferResult> PostToBytes(string url, byte[] binaryData, Dictionary<string, string> queryString, Dictionary<string, string> header, object userData = null);
```

#### 高级功能

```csharp
// Protocol Buffers 支持
Task<T> GetProtoBuf<T>(string url) where T : class, IExtensible;
Task<T> PostProtoBuf<T>(string url, IExtensible requestData) where T : class, IExtensible;

// JSON 支持（通过扩展方法）
Task<T> GetJson<T>(string url);
Task<T> PostJson<T>(string url, object data);
```

### 配置选项

```csharp
// 设置请求超时时间（默认：30秒）
TimeSpan RequestTimeout { get; set; }

// 设置最大并发连接数（默认：8）
int MaxConnectionPerServer { get; set; }

// 启用/禁用详细日志
bool EnableWebLog { get; set; }
```

## 🔧 高级用法

### 处理二进制数据上传

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
        // 处理响应数据
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
    
    // 使用扩展方法发送 Protocol Buffers 请求
    return await webManager.PostProtoBuf<UserResponse>(
        "https://api.example.com/user/protobuf", 
        request
    );
}
```

### 错误处理

```csharp
public async Task<string> SafeWebRequestAsync(string url)
{
    try
    {
        return await webManager.GetToString(url);
    }
    catch (WebException ex) when (ex.Status == WebExceptionStatus.Timeout)
    {
        Debug.LogError("请求超时: " + ex.Message);
        return null;
    }
    catch (IOException ex)
    {
        Debug.LogError("网络IO错误: " + ex.Message);
        return null;
    }
    catch (Exception ex)
    {
        Debug.LogError("请求失败: " + ex.Message);
        return null;
    }
}
```

## ⚙️ 配置

### 在编辑器中配置

1. 在 Unity 编辑器中打开 "GameFrameX/Web" 菜单
2. 选择 "Web Settings"
3. 配置以下选项：
   - **Request Timeout**: 请求超时时间（秒）
   - **Max Connections**: 最大并发连接数
   - **Enable Logging**: 启用详细日志输出

### 代码配置

```csharp
private void ConfigureWebManager()
{
    var webManager = GameFrameworkEntry.GetModule<IWebManager>();
    
    // 设置请求超时为 60 秒
    webManager.RequestTimeout = TimeSpan.FromSeconds(60);
    
    // 设置最大并发连接数为 16
    webManager.MaxConnectionPerServer = 16;
    
    // 启用详细日志
    webManager.EnableWebLog = true;
}
```

## 🐛 故障排除

### 常见问题

1. **WebGL 平台限制**
   - WebGL 不支持多线程，所有请求都在主线程处理
   - 建议使用 `await` 异步等待而不是阻塞调用

2. **跨域问题 (CORS)**
   - 确保服务器配置了正确的 CORS 头信息
   - 对于 WebGL 构建，服务器必须支持 OPTIONS 预检请求

3. **HTTPS 证书问题**
   - 在移动设备上可能需要处理证书验证
   - 可以使用自定义证书验证回调

### 调试技巧

```csharp
// 启用详细调试日志
webManager.EnableWebLog = true;

// 在 Player Settings 中启用 Development Build 和 Script Debugging
// 这样可以获得更详细的错误信息
```

## 📋 版本历史

查看 [CHANGELOG.md](CHANGELOG.md) 获取详细的版本更新信息。

## 🤝 贡献

欢迎提交 Issue 和 Pull Request！

1. Fork 本项目
2. 创建特性分支 (`git checkout -b feature/amazing-feature`)
3. 提交更改 (`git commit -m 'Add some amazing feature'`)
4. 推送到分支 (`git push origin feature/amazing-feature`)
5. 创建 Pull Request

## 📄 许可证

本项目采用 MIT 许可证 - 查看 [LICENSE.md](LICENSE.md) 文件了解详情。

## 🔗 相关链接

- [GameFrameX 主项目](https://github.com/gameframex/com.gameframex.unity)
- [官方文档](https://gameframex.doc.alianblank.com)
- [示例项目](https://github.com/gameframex/com.gameframex.unity.examples)
- [问题反馈](https://github.com/gameframex/com.gameframex.unity.web/issues)

## 💬 支持

如果你有任何问题或需要帮助，可以通过以下方式联系我们：

- 📧 邮箱: alianblank@outlook.com
- 🐛 [提交 Issue](https://github.com/gameframex/com.gameframex.unity.web/issues)
- 📖 [查看文档](https://gameframex.doc.alianblank.com)