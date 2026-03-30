# EPLAN_ADDIN_TEMPLATE.Wizard

This wizard customizes the generated project after the template is created.

## What it does

- sets `AssemblyName` to `SAC.EplAddIn.<ProjectName>`
- writes a `.csproj.user` file with the Debug start action for `EPLAN.exe`

## Build

Build `EPLAN_ADDIN_TEMPLATE.Wizard.csproj` in Visual Studio or with MSBuild.

## Wire it into the exported template

1. Export `EPLAN_ADDIN_TEMPLATE` as a project template.
2. Unzip the exported template.
3. Edit the `.vstemplate` file and add:

```xml
<WizardExtension>
  <Assembly>EPLAN_ADDIN_TEMPLATE.Wizard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</Assembly>
  <FullClassName>EPLAN_ADDIN_TEMPLATE.Wizard.TemplateWizard</FullClassName>
</WizardExtension>
```

4. Install `EPLAN_ADDIN_TEMPLATE.Wizard.dll` where Visual Studio can load template wizard assemblies, for example `C:\Program Files\Microsoft Visual Studio\18\Community\Common7\IDE\PublicAssemblies`.
5. Rezip the template with the original folder structure.

If your EPLAN path changes, update the constants in `TemplateWizard.cs` before rebuilding.
