# EPLAN_DEV_Wizards

<p align="center">
    <a href="https://github.com/bingyongcao/EPLAN_DEV_Wizards/blob/main/README-cn.md">中文</a>
    |
    <a href="https://github.com/bingyongcao/EPLAN_DEV_Wizards/blob/main/README.md">English</a>
</p>

这是一个面向 Visual Studio 的 EPLAN .NET 向导仓库，包含：

`EPLAN_ADDIN_TEMPLATE`：导出为 Visual Studio 项目模板，

`EPLAN_ADDIN_TEMPLATE.Wizard`：用于 Add-in 模板的自定义向导，在模板创建期间运行，

`EPLAN_ADDIN_TUTORIAL`：用于创建 EPLAN Add-in 的教程，

`EPLAN_SCRIPT_TUTORIAL`：用于创建 EPLAN 脚本的教程，

`EPLAN_UTILITIES`：共享辅助类库。

## 仓库结构

- `EPLAN_ADDIN_TEMPLATE` - EPLAN Add-in 的 Visual Studio 项目模板，预置了 WPF、HandyControl、CommunityToolkit.Mvvm 运行时 API 和 Serilog。
- `EPLAN_ADDIN_TEMPLATE.Wizard` - Visual Studio 模板向导，用于为新建 Add-in 项目设置默认程序集名称和调试配置。
- `EPLAN_ADDIN_TUTORIAL` - Add-in 示例，涵盖项目属性、页面和主数据等内容。
- `EPLAN_SCRIPT_TUTORIAL` - 脚本示例，涵盖 Ribbon UI、上下文菜单、事件处理、设置以及命令行执行脚本。
- `EPLAN_UTILITIES` - 可复用的辅助工具，包含 EPLAN 设置、Windows 主题检测、属性访问等功能。
- `install-template.ps1` - 辅助脚本，用于将导出的模板 zip 和向导程序集复制到 Visual Studio 2026 的模板目录。

每个项目下也都包含各自的 `README.md`，用于提供更具体的说明。

## 离线 API 帮助

### 安装包

- [2026 离线 API 安装包](Resources/Eplan_API_2026.zip)

- 请参考官方安装指南：[EPLAN API 2026 Help Structure](https://www.eplan.help/en-us/Infoportal/Content/api/2026/Help%20structure.html)

- 安装完成后，还可以在 Visual Studio 中将 `F1` 绑定到 `EPLAN API Help`，这样编码时按下 `F1` 就能直接打开 API 帮助。

## 模板工作流

1. 将 `EPLAN_ADDIN_TEMPLATE` 导出为 Visual Studio 项目模板。
2. 构建 `EPLAN_ADDIN_TEMPLATE.Wizard`。
3. 在导出的 `.vstemplate` 文件中添加向导扩展配置。
4. 将生成的模板 zip 复制到 Visual Studio 2026 的模板目录。
5. 将 `EPLAN_ADDIN_TEMPLATE.Wizard.dll` 复制到 Visual Studio 的 `PublicAssemblies` 目录。

在复制步骤中，可以先根据本机环境调整硬编码路径，然后使用 `install-template.ps1` 来完成。

当前自定义向导在创建新的 Add-in 项目时会自动应用以下默认值：

- `AssemblyName` = `SAC.EplAddIn.<ProjectName>`
- 调试启动操作 = `D:\Eplan\Platform\2026.0.3\Bin\EPLAN.exe`
- 调试启动参数 = `/Variant:"Electric P8"`

## 推荐技术栈

- 运行时：`.NET framework 4.8.1`
- UI 框架：`WPF`
- UI 风格：`HandyControl`
- MVVM 框架：`CommunityToolkit.Mvvm`
- 日志：`Serilog`

⚠️**注意**：CommunityToolkit.Mvvm 的源生成器功能要求项目为 SDK-style，因此这里请直接使用工具包的运行时 API。

## SVG 图标

> 可以从 [lucide](https://lucide.dev/icons/) 查找 svg 图标资源。不要忘记修改描边颜色。

EPLAN 配色表：

<div style="display: flex; flex-wrap: wrap; gap: 20px; padding: 16px; border-radius: 12px;">

  <div style="text-align: center;">
    <div style="width: 80px; height: 80px; background-color: #E9EAEA; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1);"></div>
    <code style="margin-top: 8px; display: block;">#E9EAEA</code>
  </div>

  <div style="text-align: center;">
    <div style="width: 80px; height: 80px; background-color: #464646; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1);"></div>
    <code style="margin-top: 8px; display: block;">#464646</code>
  </div>

  <div style="text-align: center;">
    <div style="width: 80px; height: 80px; background-color: #0D9BE2; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1);"></div>
    <code style="margin-top: 8px; display: block;">#0D9BE2</code>
  </div>

  <div style="text-align: center;">
    <div style="width: 80px; height: 80px; background-color: #E2001A; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1);"></div>
    <code style="margin-top: 8px; display: block;">#E2001A</code>
  </div>

  <div style="text-align: center;">
    <div style="width: 80px; height: 80px; background-color: #F7CC1B; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1);"></div>
    <code style="margin-top: 8px; display: block;">#F7CC1B</code>
  </div>

  <div style="text-align: center;">
    <div style="width: 80px; height: 80px; background-color: #F7821B; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1);"></div>
    <code style="margin-top: 8px; display: block;">#F7821B</code>
  </div>

  <div style="text-align: center;">
    <div style="width: 80px; height: 80px; background-color: #62BA46; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1);"></div>
    <code style="margin-top: 8px; display: block;">#62BA46</code>
  </div>

</div>