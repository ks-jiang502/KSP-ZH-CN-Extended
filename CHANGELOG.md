# 更新日志

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
