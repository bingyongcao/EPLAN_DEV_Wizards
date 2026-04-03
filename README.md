# EPLAN_DEV

This is a repo of EPLAN .NET Wizards for Visual Studio, including 

`EPLAN_ADDIN_TEMPLATE`: export as visual studio template, 

`EPLAN_ADDIN_TEMPLATE.Wizard`: custom wizard for the add-in template, running during template creation,

`EPLAN_ADDIN_TUTORIAL`: tutorial for creating EPLAN add-ins,

`EPLAN_SCRIPT_TUTORIAL`: tutorial for creating EPLAN scripts.

## Tech stack we recommend

- Runtime: `.NET framework 4.8.1`
- UI framework: `WPF`
- UI style: `HandyControl`
- MVVM framework：`CommunityToolkit.Mvvm`
- Logging: `Serilog`

## EPLAN_ADDIN_TEMPLATE

### Where to find your template
```
C:\Users\<user>\Documents\Visual Studio 18\My Exported Templates
C:\Users\<user>\Documents\Visual Studio 18\Templates\ProjectTemplates
```

### Custom wizard

The custom wizard project is in `EPLAN_ADDIN_TEMPLATE.Wizard`.

It applies these template defaults automatically:

- `AssemblyName` = `SAC.EplAddIn.<ProjectName>`
- Debug start action = `D:\Eplan\Platform\2026.0.3\Bin\EPLAN.exe`
- Debug start arguments = `/Variant:"Electric P8"`

### How to make & install a template

1. Project->Export template...
2. build `EPLAN_ADDIN_TEMPLATE.Wizard`
3. unzip the template, then edit the `.vstemplate` file and add the wizard extension inside VSTemplate tag

```xml
<WizardExtension>
  <Assembly>EPLAN_ADDIN_TEMPLATE.Wizard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</Assembly>
  <FullClassName>EPLAN_ADDIN_TEMPLATE.Wizard.TemplateWizard</FullClassName>
</WizardExtension>
```
4. rezip the template, be care of the zip hierarchy

5. install `EPLAN_ADDIN_TEMPLATE.Wizard.dll` where Visual Studio can load template wizard assemblies, for example:

```
C:\Program Files\Microsoft Visual Studio\18\Community\Common7\IDE\PublicAssemblies
```

6. copy the template zip to the template folder:

```
C:\Users\<user>\Documents\Visual Studio 18\Templates\ProjectTemplates
```

Note: we can run install-template.ps1 to do steps 5-6.

### How to use

1. restore packages

```
dotnet restore
```

## EPLAN_ADDIN_TUTORIAL
