# EPLAN_ADDIN_TEMPLATE.Wizard

This wizard customizes the generated project after the template is created.

## What it does

- sets `AssemblyName` to `SAC.EplAddIn.<ProjectName>`
- writes a `.csproj.user` file with the Debug start action for `EPLAN.exe`

## How the EPLAN path is resolved

The wizard no longer hard-codes the EPLAN path. It runs the bundled
`detect-eplan.ps1` (a copy of
`eplan-api-skills/.../scripts/detect-eplan.ps1`) and reads the highest-version
installation it finds. The script walks the EPLAN registry keys, the standard
`Program Files\EPLAN` location, and any explicit `-SearchRoot` you pass.

Resolution order:

1. **`EPLAN_EXECUTABLE_PATH`** environment variable — if set and the file
   exists, the wizard uses it as-is and stamps the `.csproj.user` with
   `EplanStartProgramSource=env:EPLAN_EXECUTABLE_PATH`.
2. **`detect-eplan.ps1 -AsJson`** — runs in a child `powershell.exe` with
   `-NoProfile -NonInteractive -ExecutionPolicy Bypass`. The first
   `AssemblyDirectory` from the JSON output is used to locate `EPLAN.exe`
   (it is the `Bin` folder in every supported EPLAN layout; if not, the wizard
   walks up to four parent levels looking for `EPLAN.exe` or `Bin\EPLAN.exe`).
   The `.csproj.user` records `EplanStartProgramSource=detect-eplan:<source>`
   (e.g. `detect-eplan:Registry:EPLAN Platform 2026.0.3`).
3. If both fail, the wizard throws an `InvalidOperationException` that names
   the env var and the install locations the script checks. The template is
   not created.

Keep `detect-eplan.ps1` next to the wizard DLL (`CopyToOutputDirectory`
handles this in the `.csproj`). If you update the script in
`eplan-api-skills`, copy the new version over the one in this folder and
rebuild.

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

4. Install `EPLAN_ADDIN_TEMPLATE.Wizard.dll` and the bundled
   `detect-eplan.ps1` (both ship in `bin\Release\`) where Visual Studio can
   load template wizard assemblies, for example
   `C:\Program Files\Microsoft Visual Studio\18\Community\Common7\IDE\PublicAssemblies`.
5. Rezip the template with the original folder structure.

If you need to point the wizard at a different EPLAN installation without
rebuilding, set the `EPLAN_EXECUTABLE_PATH` environment variable to the full
path of `EPLAN.exe` before launching Visual Studio.
