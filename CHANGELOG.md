## [2.0.1](https://github.com/gameframex/com.gameframex.unity.web/compare/2.0.0...2.0.1) (2026-10-15)


### Bug Fixes

* **extensions:** 处理服务器错误响应解析 ([593b6a0](https://github.com/gameframex/com.gameframex.unity.web/commit/593b6a026cb6c06061360741ff86afe9b9e1009f))

# [2.0.0](https://github.com/gameframex/com.gameframex.unity.web/compare/1.4.1...2.0.0) (2026-10-15)


* feat(module)!: Web 模块薄入口化并新增 WebOptions ([97c362b](https://github.com/gameframex/com.gameframex.unity.web/commit/97c362babc38cd6aeab2957c485a904432c99611))


### BREAKING CHANGES

* WebComponent 序列化字段移除，编辑期配置入口收敛到代码 WebOptions。

## [Unreleased]

### Bug Fixes

* **extensions:** `ToHttpJsonResultData<T>` 处理服务器/网关错误响应：缺少约定 `code` 字段的错误体（如 ASP.NET Core ProblemDetails `status`、旧版 `statusCode`、Spring Boot `status`+`error`）不再被误判为成功；空响应、非 JSON 错误页统一返回 `Code=500`；`HttpJsonResultData<T>` 新增 `Message` 携带服务端错误消息

### ⚠ BREAKING CHANGES

* **module:** 新增 `WebOptions` 启动配置（`GameFrameXOptions.Configure`，首场景加载前注入；字段默认值同原 Inspector 序列化默认）：`Timeout`（默认 5 秒）
* **module:** 组件值参数序列化字段移除（`m_Timeout`），`WebComponent` 保留为薄入口（纯转发），模块经 `GetModule<T>()` 直达；编辑期配置入口收敛到代码 Options

**迁移对照：**

| 旧访问方式 | 新访问方式 | 说明 |
|---|---|---|
| `m_Timeout` 序列化字段（默认 5f，单位秒） | `WebOptions.Timeout` | 实时型：Play 期可经活视图 / 模块 API（`IWebManager.Timeout`）调整 |

# [2.0.0](https://github.com/gameframex/com.gameframex.unity.web/compare/1.4.1...2.0.0) (2026-10-08)


### ⚠ BREAKING CHANGES

* **module:** 模块契约全接口化：删除 `GameFrameworkModule` 抽象基类，改为 `IGameFrameworkModule` 标记接口 + 可选能力接口

**新契约形态与迁移对照：**

| 旧基类成员 | 新契约 | 迁移方式 |
|---|---|---|
| `: GameFrameworkModule` | `IGameFrameworkModule` | 声明处直接替换（标记接口，可注册进 `GameFrameworkEntry`） |
| `override void Update(float, float)` | `IUpdatableModule` | 真实实现改 `public`；空实现删除（未实现者不再被轮询） |
| `override void Shutdown()` | `IShutdownableModule` | 真实实现改 `public`；空实现删除（未实现者不再被关闭） |
| `override int Priority` | `IPrioritizedModule` | 改 `public int Priority { get; }`；未实现视为 0 |

本包变更：`WebManager` 实现 `IUpdatableModule`、`IShutdownableModule`。

`GameFrameworkEntry.Update` 仅分派 `IUpdatableModule`；`Shutdown` 按优先级反序仅分派 `IShutdownableModule`；tick 与关闭顺序与旧实现完全一致。能力方法仅由宿主入口调用，业务侧继续通过 `GetModule<IXxxManager>()` 访问模块。

## [1.4.1](https://github.com/gameframex/com.gameframex.unity.web/compare/1.4.0...1.4.1) (2026-07-29)


### Bug Fixes

* **dependencies:** 更新依赖版本以支持新功能和修复已知问题 ([509ac07](https://github.com/gameframex/com.gameframex.unity.web/commit/509ac07b30f06988fa9a647d5f5fafd3f626839d))

# [1.4.0](https://github.com/gameframex/com.gameframex.unity.web/compare/1.3.6...1.4.0) (2026-06-16)


### Features

* **json:** 切换 JSON 序列化库为 GameFrameX.LitJSON ([872da8e](https://github.com/gameframex/com.gameframex.unity.web/commit/872da8e4dd7bd3cf61926b83c98e8c78c30f2707))

## [1.3.6](https://github.com/gameframex/com.gameframex.unity.web/compare/1.3.5...1.3.6) (2026-06-07)


### Bug Fixes

* 补全包规范文件（LICENSE/CHANGELOG/URL 字段/unity 字段） ([c183226](https://github.com/gameframex/com.gameframex.unity.web/commit/c1832262c27473d33d7e9895b5840e5f52a99cb0))

## [1.3.5](https://github.com/gameframex/com.gameframex.unity.web/compare/1.3.4...1.3.5) (2026-05-29)


### Bug Fixes

* **web:** 修复并发请求时 MemoryStream 数据覆盖问题 ([84a3356](https://github.com/gameframex/com.gameframex.unity.web/commit/84a33565b707ff0daca6c57d3a370402fcd31e93))


### Performance Improvements

* **web:** 字典初始化使用源字典容量避免扩容 ([e38487e](https://github.com/gameframex/com.gameframex.unity.web/commit/e38487e77001ebfb2292cca96416f8882c014ca2))


### Reverts

* **web:** 恢复 WebData 为 public ([d5107b1](https://github.com/gameframex/com.gameframex.unity.web/commit/d5107b12fb28a30151420280107acf507a7123e0))

## [1.3.4](https://github.com/gameframex/com.gameframex.unity.web/compare/1.3.3...1.3.4) (2026-05-28)


### Bug Fixes

* **ci:** 统一 .github 工作流配置 ([cd31519](https://github.com/gameframex/com.gameframex.unity.web/commit/cd315198d38e344c50b3f67eb42adaf32c5b20fe))
* **deps:** 补充 package.json 中缺失的包依赖 ([8968daf](https://github.com/gameframex/com.gameframex.unity.web/commit/8968daffe269e4c9b460e96f3ee9c4c79d11f439))

## [1.3.3](https://github.com/gameframex/com.gameframex.unity.web/compare/1.3.2...1.3.3) (2026-03-13)


### Bug Fixes

* **Web:** 修复无内容长度时内存流未重置的问题 ([9e61fcd](https://github.com/gameframex/com.gameframex.unity.web/commit/9e61fcde70753cda751b2b14efdb469cba2818b7))

## [1.3.2](https://github.com/gameframex/com.gameframex.unity.web/compare/1.3.1...1.3.2) (2026-03-13)


### Bug Fixes

* **WebManager:** 修复Transfer-Encoding存在时设置内存流长度的问题 ([53a5a5b](https://github.com/gameframex/com.gameframex.unity.web/commit/53a5a5ba38649cdf19541eeca2f4e3404e458d4e))

## [1.3.1](https://github.com/gameframex/com.gameframex.unity.web/compare/1.3.0...1.3.1) (2026-03-02)


### Bug Fixes

* **Web:** 为所有POST请求添加默认证书验证绕过 ([ad703b5](https://github.com/gameframex/com.gameframex.unity.web/commit/ad703b5fd78fcb97f96a63bfa2a6b0269a090754))

# [1.3.0](https://github.com/gameframex/com.gameframex.unity.web/compare/1.2.0...1.3.0) (2026-01-19)


### Features

* **WebComponent:** 添加基础表单、请求头和查询参数管理方法 ([3924616](https://github.com/gameframex/com.gameframex.unity.web/commit/39246160d934f6c3e74cef7a8bdb8a3d96546c9b))
* **Web:** 添加基础请求数据管理和合并功能 ([06b701c](https://github.com/gameframex/com.gameframex.unity.web/commit/06b701cd2b33e189425cb35f409cde15f4ace132))

# [1.2.0](https://github.com/gameframex/com.gameframex.unity.web/compare/1.1.8...1.2.0) (2025-12-23)


### Features

* **WebManager:** 添加二进制数据POST请求支持 ([dc499bd](https://github.com/gameframex/com.gameframex.unity.web/commit/dc499bdbbcd4ce98038a918d472e459299896d45))
* **Web:** 为二进制请求添加内容类型头 ([216ee49](https://github.com/gameframex/com.gameframex.unity.web/commit/216ee4931e8843db0fdc075d863eddb9b65e0c54))
* **Web组件:** 添加支持表单数据的Post请求方法 ([8ea40d5](https://github.com/gameframex/com.gameframex.unity.web/commit/8ea40d5b12ec491aefe773cfb2b01447e9608698))

# Changelog

## [1.1.8](https://github.com/GameFrameX/com.gameframex.unity.web/tree/1.1.8) (2025-05-31)

[Full Changelog](https://github.com/GameFrameX/com.gameframex.unity.web/compare/1.1.7...1.1.8)

## [1.1.7](https://github.com/GameFrameX/com.gameframex.unity.web/tree/1.1.7) (2025-05-30)

[Full Changelog](https://github.com/GameFrameX/com.gameframex.unity.web/compare/1.1.6...1.1.7)

## [1.1.6](https://github.com/GameFrameX/com.gameframex.unity.web/tree/1.1.6) (2025-04-07)

[Full Changelog](https://github.com/GameFrameX/com.gameframex.unity.web/compare/1.1.5...1.1.6)

## [1.1.5](https://github.com/GameFrameX/com.gameframex.unity.web/tree/1.1.5) (2025-01-20)

[Full Changelog](https://github.com/GameFrameX/com.gameframex.unity.web/compare/1.1.3...1.1.5)

## [1.1.3](https://github.com/GameFrameX/com.gameframex.unity.web/tree/1.1.3) (2024-12-30)

[Full Changelog](https://github.com/GameFrameX/com.gameframex.unity.web/compare/1.1.2...1.1.3)

## [1.1.2](https://github.com/GameFrameX/com.gameframex.unity.web/tree/1.1.2) (2024-12-27)

[Full Changelog](https://github.com/GameFrameX/com.gameframex.unity.web/compare/1.1.1...1.1.2)

## [1.1.1](https://github.com/GameFrameX/com.gameframex.unity.web/tree/1.1.1) (2024-10-11)

[Full Changelog](https://github.com/GameFrameX/com.gameframex.unity.web/compare/1.1.0...1.1.1)

## [1.1.0](https://github.com/GameFrameX/com.gameframex.unity.web/tree/1.1.0) (2024-09-10)

[Full Changelog](https://github.com/GameFrameX/com.gameframex.unity.web/compare/1.0.6...1.1.0)

## [1.0.6](https://github.com/GameFrameX/com.gameframex.unity.web/tree/1.0.6) (2024-09-10)

[Full Changelog](https://github.com/GameFrameX/com.gameframex.unity.web/compare/1.0.4...1.0.6)

## [1.0.4](https://github.com/GameFrameX/com.gameframex.unity.web/tree/1.0.4) (2024-09-10)

[Full Changelog](https://github.com/GameFrameX/com.gameframex.unity.web/compare/1.0.5...1.0.4)

## [1.0.5](https://github.com/GameFrameX/com.gameframex.unity.web/tree/1.0.5) (2024-09-10)

[Full Changelog](https://github.com/GameFrameX/com.gameframex.unity.web/compare/1.0.3...1.0.5)

## [1.0.3](https://github.com/GameFrameX/com.gameframex.unity.web/tree/1.0.3) (2024-09-10)

[Full Changelog](https://github.com/GameFrameX/com.gameframex.unity.web/compare/1.0.2...1.0.3)

## [1.0.2](https://github.com/GameFrameX/com.gameframex.unity.web/tree/1.0.2) (2024-09-10)

[Full Changelog](https://github.com/GameFrameX/com.gameframex.unity.web/compare/1.0.1...1.0.2)

## [1.0.1](https://github.com/GameFrameX/com.gameframex.unity.web/tree/1.0.1) (2024-05-20)

[Full Changelog](https://github.com/GameFrameX/com.gameframex.unity.web/compare/163a297eefa9ade04e6bdd245eb15ac27c7e3c27...1.0.1)



\* *This Changelog was automatically generated by [github_changelog_generator](https://github.com/github-changelog-generator/github-changelog-generator)*
