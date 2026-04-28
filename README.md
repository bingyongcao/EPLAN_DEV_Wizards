# EPLAN_DEV

This is a repo of EPLAN .NET Wizards for Visual Studio, including 

`EPLAN_ADDIN_TEMPLATE`: export as visual studio template, 

`EPLAN_ADDIN_TEMPLATE.Wizard`: custom wizard for the add-in template, running during template creation,

`EPLAN_ADDIN_TUTORIAL`: tutorial for creating EPLAN add-ins,

`EPLAN_SCRIPT_TUTORIAL`: tutorial for creating EPLAN scripts.

## Offline API help

### Installer

- [offline API installer for 2026](../Resources/Eplan_API_2026.zip)

- follow the official installation guide: [EPLAN API 2026 Help Structure](https://www.eplan.help/en-us/Infoportal/Content/api/2026/Help%20structure.html)

- after installation, one more thing you can do is binding the `F1` with `EPLAN API Help` in Visual Studio, so that you can directly open the API help by pressing `F1` when coding.

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

## License
This tutorial is provided as-is for educational purposes.