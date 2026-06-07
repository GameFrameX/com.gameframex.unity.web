<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# GameFrameX Web

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.web)](https://github.com/GameFrameX/com.gameframex.unity.web/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.web)](https://github.com/GameFrameX/com.gameframex.unity.web/releases)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

All-in-One Solution for Indie Game Development · Empowering Indie Developers' Dreams

<br />

[Documentation](https://gameframex.doc.alianblank.com) · [Quick Start](#quick-start) · [QQ Group](https://qm.qq.com/q/5U9Fvebw)

<br />

**English** | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>
## Project Overview

GameFrameX Web is a high-performance Unity HTTP networking library that provides a clean and easy-to-use API for handling various network request scenarios. It supports GET and POST requests, and can process strings, JSON, binary data, and other formats.

## Features

- **High-Performance Async** - Based on C# Task async pattern, supports async/await
- **Multiple Data Formats** - String, JSON, binary data, Protocol Buffers
- **Cross-Platform** - Supports WebGL, PC, and mobile platforms
- **Connection Pool Management** - Smart connection reuse with max concurrent connections control
- **Secure & Reliable** - Comprehensive error handling and timeout mechanisms
- **Easy to Extend** - Modular design, supports custom data serialization

## Installation

### Via Git URL (Recommended)

1. Open Package Manager in Unity Editor
2. Click the "+" button and select "Add package from git URL"
3. Enter the following URL:
   ```
   https://github.com/gameframex/com.gameframex.unity.web.git
   ```

### Via manifest.json

Add the following to your project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.gameframex.unity.web": "https://github.com/gameframex/com.gameframex.unity.web.git",
    "com.gameframex.unity": "https://github.com/gameframex/com.gameframex.unity.git"
  }
}
```

### Manual Installation

1. Download the latest release package
2. Extract it to your project's `Packages` directory
3. Unity will automatically recognize and load the package

## Quick Start

### Basic Usage

```csharp
using GameFrameX.Web.Runtime;
using System.Threading.Tasks;
using System.Collections.Generic;

public class WebExample : MonoBehaviour
{
    private IWebManager webManager;

    private async void Start()
    {
        // Get WebManager instance
        webManager = GameFrameworkEntry.GetModule<IWebManager>();

        // Send GET request for string response
        string result = await webManager.GetToString("https://api.example.com/data");
        Debug.Log("GET Response: " + result);

        // Send POST request with form data
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

### Using WebComponent (Recommended)

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

## Usage Examples

### Binary Data Upload

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

### Using Protocol Buffers

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

### Error Handling

```csharp
public async Task<string> SafeWebRequestAsync(string url)
{
    try
    {
        return await webManager.GetToString(url);
    }
    catch (WebException ex) when (ex.Status == WebExceptionStatus.Timeout)
    {
        Debug.LogError("Request timeout: " + ex.Message);
        return null;
    }
    catch (IOException ex)
    {
        Debug.LogError("Network IO error: " + ex.Message);
        return null;
    }
    catch (Exception ex)
    {
        Debug.LogError("Request failed: " + ex.Message);
        return null;
    }
}
```

## API Reference

### Core Interface: IWebManager

#### GET Requests

```csharp
Task<string> GetToString(string url);
Task<string> GetToString(string url, Dictionary<string, string> queryString);
Task<string> GetToString(string url, Dictionary<string, string> queryString, Dictionary<string, string> header);

Task<byte[]> GetToBytes(string url);
Task<byte[]> GetToBytes(string url, Dictionary<string, string> queryString);
Task<byte[]> GetToBytes(string url, Dictionary<string, string> queryString, Dictionary<string, string> header);
```

#### POST Requests

```csharp
Task<string> PostToString(string url, Dictionary<string, string> formData = null);
Task<string> PostToString(string url, Dictionary<string, string> formData, Dictionary<string, string> queryString);
Task<string> PostToString(string url, Dictionary<string, string> formData, Dictionary<string, string> queryString, Dictionary<string, string> header);

Task<byte[]> PostToBytes(string url, Dictionary<string, string> formData);
Task<byte[]> PostToBytes(string url, Dictionary<string, string> formData, Dictionary<string, string> queryString);
Task<byte[]> PostToBytes(string url, Dictionary<string, string> formData, Dictionary<string, string> queryString, Dictionary<string, string> header);

Task<WebBufferResult> PostToBytes(string url, byte[] binaryData, Dictionary<string, string> queryString, Dictionary<string, string> header, object userData = null);
```

#### Advanced Features

```csharp
// Protocol Buffers support
Task<T> GetProtoBuf<T>(string url) where T : class, IExtensible;
Task<T> PostProtoBuf<T>(string url, IExtensible requestData) where T : class, IExtensible;

// JSON support (via extension methods)
Task<T> GetJson<T>(string url);
Task<T> PostJson<T>(string url, object data);
```

### Configuration Options

```csharp
// Request timeout (default: 30 seconds)
TimeSpan RequestTimeout { get; set; }

// Max concurrent connections (default: 8)
int MaxConnectionPerServer { get; set; }

// Enable/disable verbose logging
bool EnableWebLog { get; set; }
```

## Platform Support

| Platform | Supported | Notes |
|----------|-----------|-------|
| Windows | Yes | Full support |
| macOS | Yes | Full support |
| Linux | Yes | Full support |
| Android | Yes | Full support |
| iOS | Yes | Full support |
| WebGL | Yes | Single-threaded, all requests processed on main thread |

### Configuration

```csharp
private void ConfigureWebManager()
{
    var webManager = GameFrameworkEntry.GetModule<IWebManager>();

    // Set request timeout to 60 seconds
    webManager.RequestTimeout = TimeSpan.FromSeconds(60);

    // Set max concurrent connections to 16
    webManager.MaxConnectionPerServer = 16;

    // Enable verbose logging
    webManager.EnableWebLog = true;
}
```

### Troubleshooting

1. **WebGL Platform Limitations**
   - WebGL does not support multi-threading; all requests are processed on the main thread
   - Use `await` for async operations instead of blocking calls

2. **CORS Issues**
   - Ensure the server has correct CORS headers configured
   - For WebGL builds, the server must support OPTIONS preflight requests

3. **HTTPS Certificate Issues**
   - Mobile devices may require custom certificate validation
   - Use a custom certificate validation callback

## Documentation & Resources

- [GameFrameX Main Project](https://github.com/gameframex/com.gameframex.unity)
- [Official Documentation](https://gameframex.doc.alianblank.com)
- [Example Project](https://github.com/gameframex/com.gameframex.unity.examples)
- [Report Issues](https://github.com/gameframex/com.gameframex.unity.web/issues)

## Community & Support

If you have any questions or need help, reach out through:

- Email: alianblank@outlook.com
- [Submit an Issue](https://github.com/gameframex/com.gameframex.unity.web/issues)
- [Read the Docs](https://gameframex.doc.alianblank.com)

## Contributing

Contributions are welcome! Please feel free to submit Issues and Pull Requests.

1. Fork this project
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Create a Pull Request

## Changelog

See [CHANGELOG.md](CHANGELOG.md) for detailed version history.

## License

This project is licensed under the MIT License - see [LICENSE.md](LICENSE.md) for details.
