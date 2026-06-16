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
