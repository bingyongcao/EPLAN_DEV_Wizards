# EPLAN_DEV

This is a repo of EPLAN .NET Wizards for Visual Studio, including 

`EPLAN_ADDIN_TEMPLATE`: export as visual studio template, 

`EPLAN_ADDIN_TEMPLATE.Wizard`: custom wizard for the add-in template, running during template creation,

`EPLAN_ADDIN_TUTORIAL`: tutorial for creating EPLAN add-ins,

`EPLAN_SCRIPT_TUTORIAL`: tutorial for creating EPLAN scripts.

## Offline API help

### Installer

- offline API installer is `Eplan_API_2026.zip` located in the root of this repo, 

- follow the official installation guide: [EPLAN API 2026 Help Structure](https://www.eplan.help/en-us/Infoportal/Content/api/2026/Help%20structure.html)

- after installation, one more thing you can do is binding the `F1` with `EPLAN API Help` in Visual Studio, so that you can directly open the API help by pressing `F1` when coding.

## Tech stack we recommend

- Runtime: `.NET framework 4.8.1`
- UI framework: `WPF`
- UI style: `HandyControl`
- MVVM framework：`CommunityToolkit.Mvvm`
- Logging: `Serilog`
