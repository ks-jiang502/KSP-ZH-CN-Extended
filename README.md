# KSP-ZH-CN-Extended

**坎巴拉太空计划 · 中文扩展汉化包**（Kerbal Space Program Simplified Chinese Localization Extension）

仓库：<https://github.com/ks-jiang502/KSP-ZH-CN-Extended> · 下载：<https://github.com/ks-jiang502/KSP-ZH-CN-Extended/releases>

本模组为 KSP 1.12.x 的一批常用插件补充简体中文翻译，分两层：

1. **配置层（v1.2.0）**：Bluedog_DB、FASA、ModularLaunchPads、Tantares 系列、WildBlueIndustries、CoatlAerospace、HabTech2、Cormorant Aeronology、TundraExploration、DMagic、ContractPacks、SCANsat 等 41 个模组目录的部件名称、描述、厂商、实验、资源、机构与变体。
2. **运行时 DLL 层（v1.2.0）**：Harmony 插件 `KSPZHCN.dll` 在渲染前替换各模组程序集里硬编码的界面文字（窗口标题、按钮、提示、状态栏等），词库位于 `DllLocalization/zh-cn.txt`。

- 不修改任何原模组文件，配置层全部通过 `ModuleManager` 补丁与独立 `Localization` 文件实现
- 与 `0000Tinygrox_CNPatches` 等现有汉化包共存，不覆盖、不冲突
- 英文原文完整保留：切换回 English 时显示原文本；DLL 层另有开关可整体关闭
- 开源、可自行增补：所有词条集中在 `GameData/KSP-ZH-CN-Extended/Localization/` 与 `DllLocalization/`，补丁集中在 `Patches/`

## 安装

1. 确认已安装 **ModuleManager 4.x**（本包依赖它来应用补丁）。
2. 将压缩包内的 `GameData` 文件夹合并到 KSP 安装目录（例如 `D:\SteamLibrary\steamapps\common\Kerbal Space Program\`）。
3. 启动游戏，在 **Settings → Language** 中选择 **简体中文 (zh-cn)**。
4. 进入 VAB/SPH 或载入存档，部件名称、描述、实验名称、资源名称等即显示中文。

实机效果（插件加载后的主菜单）：

![主菜单](docs/screenshots/main-menu-zh.png)

安装后的目录结构：

```
GameData/
└── KSP-ZH-CN-Extended/
    ├── KSP-ZH-CN-Extended.version
    ├── Localization/
    │   ├── zh-cn.cfg
    │   └── en-us.cfg
    ├── Patches/
    │   ├── Bluedog_DB.cfg
    │   ├── Tantares.cfg
    │   ├── WildBlueIndustries.cfg
    │   └── ...
    ├── Plugins/
    │   └── KSPZHCN.dll
    └── DllLocalization/
        └── zh-cn.txt
```

## DLL 界面汉化（v1.2.0）

`Plugins/KSPZHCN.dll` 是独立 Harmony 插件，启动时读取 `DllLocalization/zh-cn.txt`，对 77 个方法注册前缀补丁：

- `UnityEngine.GUI` / `GUILayout` 的 `Label`、`Box`、`Button`、`TextField`、`Toggle`、`Window`、`BeginGroup` 等字符串重载
- `GUIContent` 构造函数、`text` / `tooltip` 属性，`UnityEngine.UI.Text` 与 `GUIContent.Temp`
- `ScreenMessages.PostScreenMessage` 的屏幕提示
- KSP 对话框 `DialogGUILabel`、`DialogGUIButton`、`MultiOptionDialog`

词库为 `原文<TAB>译文` 的 UTF-8 文本，共 **2,359 条**，来自对本次更新后模组 DLL 的 IL 分析。启动后可在 `KSP.log` 中确认：

```
[KSPZHCN] dictionary loaded: 2359 entries
[KSPZHCN] harmony patches: ok=77 fail=0
```

- 关闭插件：在 `DllLocalization/` 下新建空文件 `disabled.txt`，重启游戏即可。
- 增补词条：直接在 `zh-cn.txt` 末尾追加 `原文<TAB>译文`（`#` 开头为注释），重启生效。
- 若某些模组界面显示乱码或异常，请先确认 `KSP.log` 中的补丁统计，再提交 Issue。

**v1.1.2 说明**：MechJeb2 自带完整中文界面，其条目已从运行时词库移除（v1.1.1 及更早版本会重复翻译它）。

**v1.1.1 卡顿修复**：v1.1.0 的忽略大小写查找使用 `StringComparer.OrdinalIgnoreCase`，Mono 运行时对部分界面字符串会在 `TextInfo.GetInvariantCaseInsensitiveHashCode` 内抛 `NullReferenceException`，被 MechJeb / RemoteTech 等每个控件每帧触发，造成飞行界面严重卡顿。v1.1.1 改为预构建小写词表 + 序号比较，并移除了可变缓存、给所有前缀加了异常兜底，词库加载后只读、线程安全。

## 覆盖范围（v1.2.0）

配置层共 **5,803 条**待译文本，生成 **5,490 条**中文本地化词条、**5,289 条**英文本地化词条，为 **2,561 个部件**、**449 个部件变体**、**270 个配置节点**写入中文，共 **30 个补丁文件**、覆盖 **41 个模组目录**：

| 模组 | 标签 | 部件 | 变体 | 节点 |
| --- | --- | --- | --- | --- |
| Bluedog_DB | 2768 | 1438 | 18 | 58 |
| FASA | 404 | 200 | 0 | 8 |
| ModularLaunchPads | 458 | 233 | 0 | 7 |
| WildBlueIndustries | 318 | 155 | 54 | 35 |
| CoatlAerospace | 306 | 145 | 0 | 15 |
| HabTech2 | 234 | 100 | 2 | 43 |
| Cormorant Aeronology | 123 | 60 | 0 | 1 |
| TundraExploration | 101 | 51 | 4 | 3 |
| DMagicOrbitalScience | 84 | 36 | 0 | 18 |
| Eisenhower-Astronautics | 78 | 38 | 0 | 1 |
| JNH | 71 | 38 | 0 | 0 |
| TantaresSP | 67 | 25 | 72 | 3 |
| KerbalReusabilityExpansion | 57 | 41 | 0 | 0 |
| TantaresRecolors | 48 | 18 | 122 | 0 |
| Tantares | 41 | 36 | 59 | 5 |
| htRobotics | 28 | 16 | 9 | 0 |
| Benjee10_sharedAssets | 26 | 10 | 0 | 0 |
| Corvus | 22 | 0 | 0 | 0 |
| KIU | 30 | 17 | 44 | 0 |
| LonesomeRobots | 483 | 0 | 0 | 0 |
| 其余 21 个模组 | 318 | 30 | 67 | 85 |

本次更新已移除对已卸载的 UmbraSpaceIndustries、Interkosmos 等模组的条目，旧版词条不再保留。

运行时 DLL 层（`DllLocalization/zh-cn.txt`，2,359 条）覆盖的界面字符串主要来自 Chatterer、FreeIva、ResonantOrbitCalculator、WaypointManager、FinalFrontier、KolonyTools、kOS、KIS、ContractConfigurator、RCSBuildAid、RemoteTech、TarsierSpaceTech、Trajectories、USI_WOLF、OrbitPOInts、Konstruction、DPAI、Firespitter、GPWS、B9PartSwitch、ScienceAlert 等程序集。MechJeb2 自带中文界面，其条目已移除。

## 已知限制

- **DLL 层词库基于启发式提取**：只收集 IL 中紧邻 GUI 调用点的字符串常量，运行时拼接、格式化或从配置文件读取的文本可能仍是英文；可在 `zh-cn.txt` 中手动补充。
- **搜索关键词（tags）**：部分标签的中文译文会替换原有英文关键词，中文搜索更友好，但可能影响英文关键词检索。
- 配置层已通过静态校验（括号平衡、标签引用、占位符、空值检查）；DLL 层已验证插件加载、77 个补丁全部注册成功、主菜单无异常，v1.1.1 修复了 v1.1.0 的每帧 `NullReferenceException` 卡顿问题，但未对所有模组的每个窗口逐一点击验证。如发现漏译、错译或卡顿，请先附上 `KSP.log` 提交 Issue。

## 卸载

删除 `GameData/KSP-ZH-CN-Extended/` 文件夹即可，不会影响原模组。

## 构建与贡献

- 词条文件：`GameData/KSP-ZH-CN-Extended/Localization/zh-cn.cfg`（`Localization { zh-cn { #标签 = 文本 } }`）。
- 补丁文件：`GameData/KSP-ZH-CN-Extended/Patches/<模组>.cfg`，均带 `:NEEDS[模组]` 与 `:FINAL`，仅在对应模组存在时生效。
- 字面量字段采用“标签间接”方案：补丁把 `title/description/manufacturer/displayName` 指向 `#LOC_KSPZH_<id>`，英文原文写入 `en-us.cfg`，中文写入 `zh-cn.cfg`，因此切换语言不会串味。
- 新增翻译时请保持 `<<1>>`、`<color=...>`、`\n`、`%s`、`{0}` 等占位符与原文一致。
- DLL 插件源码在 `Source/KSPZHCN/`（`Translator.cs`、`Patches.cs`、`Bootstrap.cs`、`Util.cs`）。使用 .NET Framework 4.x 的 `csc.exe` 编译：

```bat
csc /target:library /langversion:5 /out:GameData\KSP-ZH-CN-Extended\Plugins\KSPZHCN.dll ^
  /r:"<KSP>\KSP_x64_Data\Managed\Assembly-CSharp.dll" ^
  /r:"<KSP>\KSP_x64_Data\Managed\UnityEngine.dll" ^
  /r:"<KSP>\KSP_x64_Data\Managed\UnityEngine.CoreModule.dll" ^
  /r:"<KSP>\KSP_x64_Data\Managed\UnityEngine.IMGUIModule.dll" ^
  /r:"<KSP>\KSP_x64_Data\Managed\UnityEngine.UIModule.dll" ^
  /r:"<KSP>\KSP_x64_Data\Managed\UnityEngine.UI.dll" ^
  /r:"<KSP>\GameData\000_Harmony\0Harmony.dll" ^
  Source\KSPZHCN\*.cs
```

- 新增 DLL 词条：直接编辑 `DllLocalization/zh-cn.txt`，每行 `原文<TAB>译文`；新增补丁点请改 `Source/KSPZHCN/Patches.cs` 后重新编译。

## 许可

MIT License，详见 [LICENSE](LICENSE)。翻译文本基于各原模组的英文原文创作；原模组版权归各自作者所有，本包不包含任何原模组代码或资源。

## 致谢

- [tinygrox/KSP_Mod_Localization_Guide](https://github.com/tinygrox/KSP_Mod_Localization_Guide)：本地化方法与规范
- 所有被翻译模组的作者
- KSP 中文社区
