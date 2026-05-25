# EPLAN_DEV_Wizards

<p align="center">
    <a href="https://github.com/bingyongcao/EPLAN_DEV_Wizards/blob/main/README-cn.md">中文</a>
    |
    <a href="https://github.com/bingyongcao/EPLAN_DEV_Wizards/blob/main/README.md">English</a>
</p>

This is a repo of EPLAN .NET Wizards for Visual Studio, including 

`EPLAN_ADDIN_TEMPLATE`: export as visual studio template, 

`EPLAN_ADDIN_TEMPLATE.Wizard`: custom wizard for the add-in template, running during template creation,

`EPLAN_ADDIN_TUTORIAL`: tutorial for creating EPLAN add-ins,

`EPLAN_SCRIPT_TUTORIAL`: tutorial for creating EPLAN scripts,

`EPLAN_UTILITIES`: shared helper library.

## Repository structure

- `EPLAN_ADDIN_TEMPLATE` - Visual Studio project template for EPLAN add-ins, with WPF, HandyControl, CommunityToolkit.Mvvm runtime APIs, and Serilog preconfigured.
- `EPLAN_ADDIN_TEMPLATE.Wizard` - Visual Studio template wizard that sets the default assembly name and debug profile for new add-in projects.
- `EPLAN_ADDIN_TUTORIAL` - add-in examples for project properties, pages, and master data.
- `EPLAN_SCRIPT_TUTORIAL` - script examples for ribbon UI, context menus, event handlers, settings, and command-line script execution.
- `EPLAN_UTILITIES` - reusable helpers for EPLAN settings, Windows theme detection, property access and so on.
- `install-template.ps1` - helper script that copies the exported template zip and wizard assembly into the Visual Studio 2026 template locations.

Each project also contains its own `README.md` with more focused instructions.

## Offline API help

### Installer

- [offline API installer for 2026](Resources/Eplan_API_2026.zip)

- follow the official installation guide: [EPLAN API 2026 Help Structure](https://www.eplan.help/en-us/Infoportal/Content/api/2026/Help%20structure.html)

- after installation, one more thing you can do is binding the `F1` with `EPLAN API Help` in Visual Studio, so that you can directly open the API help by pressing `F1` when coding.

## Template workflow

1. Export `EPLAN_ADDIN_TEMPLATE` as a Visual Studio project template.
2. Build `EPLAN_ADDIN_TEMPLATE.Wizard`.
3. Add the wizard extension entry into the exported `.vstemplate` file.
4. Copy the generated template zip into your Visual Studio 2026 template folder.
5. Copy `EPLAN_ADDIN_TEMPLATE.Wizard.dll` into the Visual Studio `PublicAssemblies` folder.

For the copy step, you can use `install-template.ps1` after adjusting the hard-coded paths to your local environment.

The custom wizard currently applies these defaults when a new add-in project is created:

- `AssemblyName` = `SAC.EplAddIn.<ProjectName>`
- Debug start action = `D:\Eplan\Platform\2026.0.3\Bin\EPLAN.exe`
- Debug start arguments = `/Variant:"Electric P8"`

## Tech stack we recommend

- Runtime: `.NET framework 4.8.1`
- UI framework: `WPF`
- UI style: `HandyControl`
- MVVM framework：`CommunityToolkit.Mvvm`
- Logging: `Serilog`

⚠️**Note**: the source generator feature of CommunityToolkit.Mvvm requires SDK-style projects, so just use the toolkit's runtime API directly.

## SVG Icon

> we can find svg icon resources from [lucide](https://lucide.dev/icons/). Don't forget to modify stroke color.

EPLAN color chart:

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