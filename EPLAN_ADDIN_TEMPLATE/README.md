# EPLAN_ADDIN_TEMPLATE

## Where to find your template
```
C:\Users\<user>\Documents\Visual Studio 18\My Exported Templates
C:\Users\<user>\Documents\Visual Studio 18\Templates\ProjectTemplates
```

## Custom wizard

The custom wizard project is in `EPLAN_ADDIN_TEMPLATE.Wizard`.

It applies these template defaults automatically:

- `AssemblyName` = `SAC.EplAddIn.<ProjectName>`
- Debug start action = `D:\Eplan\Platform\2026.0.3\Bin\EPLAN.exe`
- Debug start arguments = `/Variant:"Electric P8"`

## How to make a template

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

## How to use

1. restore packages

```
dotnet restore
```