# 更新日志

## v1.3.0 — 2026-11-26

适配再次更换后的模组库（CKAN 72 个模块 / 58 个含配置的模组目录 / 5,955 个 cfg / 98 个 DLL）。

- 重新审计并翻译 **284 条**配置文本，生成 **313 条**中文词条、**229 条**英文词条，覆盖 **93 个部件**、**70 个部件变体**、**5 个配置节点**，共 **13 个补丁文件**。
- 运行时词库按当前 DLL 重建为 **1,331 条**（保留仍存在的 1,079 条 + 新译 252 条），已卸载模组的旧词条自动清除。
- 性能：`Translator` 改用零分配的 ASCII 忽略大小写比较器（不再每帧 `ToLowerInvariant` 分配），并移除冗余的 `GUIContent.Temp` 补丁（77 → 76 个补丁点）。
- 新增 `docs/汉化模组清单.md` 与 `docs/汉化模组清单.csv`：列出本次汉化的模组、CKAN ID 与版本号。
- 实机验证：`ModuleManager: 5939 patches applied`（0 错误）、`[KSPZHCN] dictionary loaded: 1331 entries`、`harmony patches: ok=76 fail=0`。

## v1.2.0 — 2026-11-26

适配模组大更新后的全量重新汉化。

- 游戏模组从 5,740 个 cfg / 70 个 DLL 增加到 12,566 个 cfg / 141 个 DLL、41 个模组目录；新增 Bluedog_DB、FASA、ModularLaunchPads、Tantares 系列、WildBlueIndustries、CoatlAerospace、HabTech2、Cormorant Aeronology、TundraExploration、DMagic、ContractPacks、SCANsat 等，UmbraSpaceIndustries 与 Interkosmos 已被移除。
- 重新审计并翻译 **5,803 条**配置文本，生成 **5,490 条**中文词条、**5,289 条**英文词条，覆盖 **2,561 个部件**、**449 个部件变体**、**270 个配置节点**，共 **30 个补丁文件**。
- 运行时词库重建为 **2,359 条**（保留仍存在的旧词条 793 条，新增 1,566 条）。
- 修复变体补丁在 ModuleManager 补丁文件（`@PART[name] { … }`）中 PART 名为空、生成 `@PART[]` 的问题；修复 `:NEEDS[Cormorant Aeronology]` 含空格导致的 61 条 ModuleManager 语法错误（改用 `?` 通配）。
- 版本号升至 1.2.0。

## v1.1.2 — 2026-11-26

- MechJeb2 自带中文界面，移除其 96 条运行时词条，避免重复翻译与潜在冲突；词库由 1394 条减为 1298 条。
- 仓库发布到 GitHub：ks-jiang502/KSP-ZH-CN-Extended，并创建 v1.1.2 Release 与安装包。

## v1.1.1 — 2026-11-26

修复飞行界面卡顿。

- 根因：`Translator` 的忽略大小写查找使用 `StringComparer.OrdinalIgnoreCase`，Mono 运行时在处理部分界面字符串时会在 `TextInfo.GetInvariantCaseInsensitiveHashCode` 内抛出 `NullReferenceException`；该前缀异常被 MechJeb、RemoteTech 等每个 GUI 控件每帧触发一次，日志暴涨并造成严重卡顿。
- 修复：改用预构建的小写词表 + `StringComparer.Ordinal` 查找，不再使用 `OrdinalIgnoreCase`；移除可变的共享缓存，词库加载后只读，天然线程安全；`T()` 与所有 Harmony 前缀都加了异常兜底，任何意外都不会再向游戏抛异常。
- 验证：`KSP.log` 中 `[KSPZHCN] dictionary loaded: 1394 entries`、`harmony patches: ok=77 fail=0`，且无任何 `KSPZHCN` 相关异常。
- 仍保留 `disabled.txt` 开关与英文回退。

## v1.1.0 — 2026-11-26

运行时 DLL 界面汉化（深度汉化）。

- 新增 Harmony 插件 `Plugins/KSPZHCN.dll`：在渲染前替换 `UnityEngine.GUI`、`GUILayout`、`GUIContent`、`UI.Text`、`ScreenMessages` 以及 KSP 对话框（`DialogGUILabel` / `DialogGUIButton` / `MultiOptionDialog`）中的界面字符串。
- 新增运行时词库 `DllLocalization/zh-cn.txt`：**1394 条**中文映射，来自对 **56 个模组 DLL** 的 IL 分析（提取 1879 处界面字符串，去重 1664 条），覆盖 Chatterer、MechJeb2、FreeIva、WaypointManager、FinalFrontier、kOS、KIS、ContractConfigurator、RCSBuildAid、RemoteTech、Trajectories、ScienceAlert 等。
- 共注册 **77 个 Harmony 补丁点**，启动日志输出 `[KSPZHCN] dictionary loaded` 与 `harmony patches: ok=77 fail=0` 便于排查。
- 保留英文原文用于回退：切换语言或删除词库后界面恢复英文；在 `DllLocalization/` 放置 `disabled.txt` 可完全关闭插件。
- 与 `0000Tinygrox_CNPatches` 的 `KSP_Chinese_Patches.dll` 共存，不修改其文件。

## v1.0.0 — 2026-11-26

首个版本。

- 新增 780 条简体中文词条，覆盖 20 个模组目录。
- 为 320 个部件、46 个部件变体、14 个配置节点写入中文（共 584 处字段）。
- 重点覆盖：Umbra Space Industries（USI）、Interkosmos、VABOrganizer、KIS、KAS、James Webb、kOS、SquadExpansion 等。
- 采用标签间接方案：英文原文保留在 `en-us.cfg`，切换语言不会串味。
- 与 `0000Tinygrox_CNPatches` 等既有汉化包共存。
- 静态校验通过：括号平衡、标签引用完整、占位符一致、无空值。
