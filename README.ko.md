<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# GameFrameX Web

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.web)](https://github.com/GameFrameX/com.gameframex.unity.web/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.web)](https://github.com/GameFrameX/com.gameframex.unity.web/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

인디 게임 개발자를 위한 올인원 솔루션 · 인디 개발자의 꿈을 실현

<br />

[문서](https://gameframex.doc.alianblank.com) · [빠른 시작](#빠른-시작) · QQ 그룹: 467608841 / 233840761

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | **한국어**

</div>

## 프로젝트 개요

GameFrameX Web 컴포넌트는 고성능 Unity HTTP 네트워킹 라이브러리로, 다양한 네트워크 요청 시나리오를 처리하기 위한 간결하고 사용하기 쉬운 API를 제공합니다. GET, POST 요청을 지원하며, 문자열, JSON, 바이너리 데이터 등 여러 형식을 처리할 수 있습니다.

## 특징

- **고성능 비동기 처리** - C# Task 비동기 패턴 기반, async/await 지원
- **다중 데이터 형식** - 문자열, JSON, 바이너리 데이터, Protocol Buffers
- **크로스 플랫폼** - WebGL, PC, 모바일 플랫폼 지원
- **연결 풀 관리** - 스마트 연결 재사용, 최대 동시 연결 수 제어
- **안전하고 신뢰성** - 포괄적인 오류 처리 및 타임아웃 메커니즘
- **쉬운 확장** - 모듈식 설계, 사용자 정의 데이터 직렬화 지원

## 설치

### Git URL을 통한 설치 (권장)

1. Unity 에디터에서 Package Manager 열기
2. "+" 버튼을 클릭하고 "Add package from git URL" 선택
3. 다음 URL 입력:
   ```
   https://github.com/gameframex/com.gameframex.unity.web.git
   ```

### manifest.json을 통한 설치

프로젝트의 `Packages/manifest.json`에 다음을 추가:

```json
{
  "dependencies": {
    "com.gameframex.unity.web": "https://github.com/gameframex/com.gameframex.unity.web.git",
    "com.gameframex.unity": "https://github.com/gameframex/com.gameframex.unity.git"
  }
}
```

### 수동 설치

1. 최신 릴리스 패키지 다운로드
2. 프로젝트의 `Packages` 디렉토리에 압축 해제
3. Unity가 자동으로 패키지를 인식하고 로드합니다

## 빠른 시작

### 기본 사용법

```csharp
using GameFrameX.Web.Runtime;
using System.Threading.Tasks;
using System.Collections.Generic;

public class WebExample : MonoBehaviour
{
    private IWebManager webManager;

    private async void Start()
    {
        // WebManager 인스턴스 가져오기
        webManager = GameFrameworkEntry.GetModule<IWebManager>();

        // GET 요청으로 문자열 가져오기
        string result = await webManager.GetToString("https://api.example.com/data");
        Debug.Log("GET Response: " + result);

        // POST 요청으로 폼 데이터 보내기
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

### WebComponent 사용 (권장)

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

## 사용 예시

### 바이너리 데이터 업로드

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

### Protocol Buffers 사용

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

### 오류 처리

```csharp
public async Task<string> SafeWebRequestAsync(string url)
{
    try
    {
        return await webManager.GetToString(url);
    }
    catch (WebException ex) when (ex.Status == WebExceptionStatus.Timeout)
    {
        Debug.LogError("요청 시간 초과: " + ex.Message);
        return null;
    }
    catch (IOException ex)
    {
        Debug.LogError("네트워크 IO 오류: " + ex.Message);
        return null;
    }
    catch (Exception ex)
    {
        Debug.LogError("요청 실패: " + ex.Message);
        return null;
    }
}
```

## API 참조

### 핵심 인터페이스: IWebManager

#### GET 요청

```csharp
Task<string> GetToString(string url);
Task<string> GetToString(string url, Dictionary<string, string> queryString);
Task<string> GetToString(string url, Dictionary<string, string> queryString, Dictionary<string, string> header);

Task<byte[]> GetToBytes(string url);
Task<byte[]> GetToBytes(string url, Dictionary<string, string> queryString);
Task<byte[]> GetToBytes(string url, Dictionary<string, string> queryString, Dictionary<string, string> header);
```

#### POST 요청

```csharp
Task<string> PostToString(string url, Dictionary<string, string> formData = null);
Task<string> PostToString(string url, Dictionary<string, string> formData, Dictionary<string, string> queryString);
Task<string> PostToString(string url, Dictionary<string, string> formData, Dictionary<string, string> queryString, Dictionary<string, string> header);

Task<byte[]> PostToBytes(string url, Dictionary<string, string> formData);
Task<byte[]> PostToBytes(string url, Dictionary<string, string> formData, Dictionary<string, string> queryString);
Task<byte[]> PostToBytes(string url, Dictionary<string, string> formData, Dictionary<string, string> queryString, Dictionary<string, string> header);

Task<WebBufferResult> PostToBytes(string url, byte[] binaryData, Dictionary<string, string> queryString, Dictionary<string, string> header, object userData = null);
```

#### 고급 기능

```csharp
// Protocol Buffers 지원
Task<T> GetProtoBuf<T>(string url) where T : class, IExtensible;
Task<T> PostProtoBuf<T>(string url, IExtensible requestData) where T : class, IExtensible;

// JSON 지원 (확장 메서드를 통해)
Task<T> GetJson<T>(string url);
Task<T> PostJson<T>(string url, object data);
```

### 설정 옵션

```csharp
// 요청 타임아웃 (기본값: 30초)
TimeSpan RequestTimeout { get; set; }

// 최대 동시 연결 수 (기본값: 8)
int MaxConnectionPerServer { get; set; }

// 상세 로그 활성화/비활성화
bool EnableWebLog { get; set; }
```

## 플랫폼 지원

| 플랫폼 | 지원 여부 | 비고 |
|--------|-----------|------|
| Windows | 지원 | 완전 지원 |
| macOS | 지원 | 완전 지원 |
| Linux | 지원 | 완전 지원 |
| Android | 지원 | 완전 지원 |
| iOS | 지원 | 완전 지원 |
| WebGL | 지원 | 멀티스레드 미지원, 모든 요청은 메인 스레드에서 처리 |

### 설정

```csharp
private void ConfigureWebManager()
{
    var webManager = GameFrameworkEntry.GetModule<IWebManager>();

    // 요청 타임아웃을 60초로 설정
    webManager.RequestTimeout = TimeSpan.FromSeconds(60);

    // 최대 동시 연결 수를 16으로 설정
    webManager.MaxConnectionPerServer = 16;

    // 상세 로그 활성화
    webManager.EnableWebLog = true;
}
```

### 문제 해결

1. **WebGL 플랫폼 제한**
   - WebGL은 멀티스레딩을 지원하지 않습니다. 모든 요청은 메인 스레드에서 처리됩니다
   - 블로킹 호출 대신 `await`를 사용하는 것을 권장합니다

2. **CORS 문제**
   - 서버에 올바른 CORS 헤더가 설정되어 있는지 확인하세요
   - WebGL 빌드의 경우 서버가 OPTIONS 사전 요청을 지원해야 합니다

3. **HTTPS 인증서 문제**
   - 모바일 기기에서 인증서 검증 처리가 필요할 수 있습니다
   - 사용자 정의 인증서 검증 콜백을 사용할 수 있습니다

## 문서 및 자료

- [GameFrameX 메인 프로젝트](https://github.com/gameframex/com.gameframex.unity)
- [공식 문서](https://gameframex.doc.alianblank.com)
- [예제 프로젝트](https://github.com/gameframex/com.gameframex.unity.examples)
- [이슈 보고](https://github.com/gameframex/com.gameframex.unity.web/issues)

## 커뮤니티 및 지원

질문이나 도움이 필요한 경우 다음 방법으로 문의해 주세요:

- 이메일: alianblank@outlook.com
- [이슈 등록](https://github.com/gameframex/com.gameframex.unity.web/issues)
- [문서 참조](https://gameframex.doc.alianblank.com)

## 기여

Issue 및 Pull Request를 자유롭게 제출해 주세요!

1. 이 프로젝트를 포크
2. 기능 브랜치 생성 (`git checkout -b feature/amazing-feature`)
3. 변경 사항 커밋 (`git commit -m 'Add some amazing feature'`)
4. 브랜치에 푸시 (`git push origin feature/amazing-feature`)
5. Pull Request 생성

## 변경 로그

자세한 버전 업데이트 정보는 [CHANGELOG.md](CHANGELOG.md)를 참조하세요.

## 라이선스

이 프로젝트는 MIT 라이선스에 따라 배포됩니다 - 자세한 내용은 [LICENSE.md](LICENSE.md)를 참조하세요.
