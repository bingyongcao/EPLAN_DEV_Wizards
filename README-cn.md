# EPLAN_DEV_Wizards

如果把`EPLAN`二次开发类比成做饭的话，那么真正先把人劝退的，通常不是炒菜和放佐料，而是前期较为漫长的清洗和改刀过程。[EPLAN_DEV_Wizards](https://github.com/bingyongcao/EPLAN_DEV_Wizards) 这个项目旨在尽可能简化这一前期过程，并提供一些代码案例辅助初学者入门。

> 读者群体：这篇文章面向已经会一点 .NET、但还没把 EPLAN API 开发流程彻底跑顺的工程师。

## 这个仓库到底做了什么

只要接触过`EPLAN`二次开发的人，大概率都会撞上这些问题：项目结构怎么起？程序集名字怎么定义？怎么调试？怎么签名？怎么扩展UI？怎么唤起WPF窗体？

这个仓库的做法是把最容易卡住人的几段流程分别做成模板、向导、教程和脚本示例。这样读者既能照着跑，也能拆开看每一步为什么这么配。

从 `EPLAN_DEV.slnx` 看，解决方案里有五个项目，但真正形成主线的是三段连续动作：

1. 用 `EPLAN_ADDIN_TEMPLATE` 生成一个新 Add-in 项目骨架。
2. 用 `EPLAN_ADDIN_TEMPLATE.Wizard` 在创建项目时自动补上程序集名和调试配置。
3. 用 `EPLAN_ADDIN_TUTORIAL` 和 `EPLAN_SCRIPT_TUTORIAL` 展示 Add-in 与 Script 两类扩展怎么接进 EPLAN。

它还顺手把离线 API 帮助包 `Eplan_API_2026.zip` 放在仓库根目录，并在 README 里提醒如何安装。这件事看起来很小，但很说明设计取向：这里不是只关心“代码能不能编译”，而是关心“开发者能不能持续查文档、持续调试、持续开发”。

## 整体流程一眼看懂

我先用一个 ASCII 图把这个仓库最重要的路径压缩一下：

```text
离线 API 帮助 + Visual Studio 模板安装
                |
                v
在 Visual Studio 新建 EPLAN Add-in 项目
                |
                v
TemplateWizard 写入默认 AssemblyName
并生成 .csproj.user 调试配置
                |
                v
AddInRegister 注册 Ribbon Tab 和命令
                |
                v
Action 执行示例逻辑并打开 WPF 窗口
                |
                v
开发者在 EPLAN 里调试、观察、继续扩展
```

我最在意的是中间这两步。很多教程会从 API 调用开始讲，但这个仓库先处理的是“怎么让项目一创建出来就足够像一个能工作的 EPLAN Add-in”。

## 关键部分是怎么配合的

### 1. 模板项目负责给出一个能落地的起点

`EPLAN_ADDIN_TEMPLATE` 本身就是一个可导出的 Visual Studio 模板。它包含 `AddInRegister.cs`、默认动作 `DefaultAction.cs`、WPF 视图和 ViewModel，还把目标框架明确固定在 `.NET Framework 4.8.1`，依赖里能看到 `HandyControl` 和 `CommunityToolkit.Mvvm`。

### 2. 向导项目负责把容易忘的机械步骤自动化

```csharp
_safeProjectName = GetReplacementValue(replacementsDictionary, "$safeprojectname$");
replacementsDictionary["$eplanassemblyname$"] = BuildAssemblyName(_safeProjectName);
```

项目生成结束后，向导还会继续改 `.csproj` 和 `.csproj.user`。前者确保 `AssemblyName` 变成 `SAC.EplAddIn.<ProjectName>` 这种统一格式；后者写入 `StartProgram` 和 `StartArguments`，把调试启动目标指向本机的 `EPLAN.exe`，参数则是 `/Variant:"Electric P8"`。

这背后的价值很直接：我不用每次新建项目都手工点一遍调试配置，也不用等到第一次 F5 失败才意识到启动程序根本没设。仓库根目录的 `install-template.ps1` 又把模板 zip 和向导 DLL 的安装步骤自动化了一遍，于是“模板可用”这件事从文档步骤变成了脚本步骤。

### 3. 教程项目负责告诉我按钮按下去之后会发生什么

`EPLAN_ADDIN_TUTORIAL` 把 Add-in 注册和几个典型动作拆得很直白。`AddInRegister.cs` 在 `OnRegister` 里新建 Ribbon Tab、Command Group，并挂上 `ProjAction`、`StructAction`、`PageAction`、`MasterDataAction` 四个命令。

以 `ProjAction.cs` 为例，它先从 `ProjectManager` 和 `SelectionSet` 里取项目上下文，再用 `Decider` 弹窗展示当前结果，最后打开 `ProjectPropertiesWindow`。这里的代码不追求复杂，而是刻意把三件最基本的事连在一起：获取上下文、反馈结果、弹出界面。

### 4. 共享辅助项目负责把“属性读取”这种脏活收口

`EPLAN_HELPERS` 是一个 Shared Project，当前暴露的重点是 `PropertyUtility.cs`。里面既有按整数属性 ID 查找 `PropertyValue` 的逻辑，也有把不同 `PropertyDefinition.PropertyType` 转成字符串的逻辑，包括坐标、多语言字符串和带单位的值。

## 还不完美的地方

这个仓库最明确的取舍是：它优先服务真实开发环境，而不是优先做跨机器、跨版本的通用配置。

这有明显好处。比如 `TemplateWizard.cs` 里把 `EplanExecutablePath` 固定为 `D:\Eplan\Platform\2026.0.3\Bin\EPLAN.exe`，能让我的本机上的模板开箱即用；`install-template.ps1` 也把 Visual Studio 18 的路径写死了，说明仓库是围绕一个已知工作站配置来优化的。

代价也同样明显：只要换一台电脑，或者 EPLAN 安装目录不同，这些默认值就要改。教程 README 其实已经提醒它基于 EPLAN Electric P8 2026。

## 写在最后

这个仓库把`EPLAN`二次开发过程中的脏活累活尽量模板化、脚本化了。对我来说，这正是它最有价值的地方。