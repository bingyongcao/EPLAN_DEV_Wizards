# eplan-addin-template

<p align="center">
    <a href="https://github.com/bingyongcao/eplan-addin-template/blob/main/README-cn.md">中文</a>
    |
    <a href="https://github.com/bingyongcao/eplan-addin-template/blob/main/README.md">English</a>
</p>

This is a repo of EPLAN add-in template for Visual Studio, including 

`EPLAN_ADDIN_TEMPLATE`: export as visual studio template, 

`EPLAN_ADDIN_TEMPLATE.Wizard`: custom wizard running during template creation.

## Custom wizard

It applies these template defaults automatically:

- `AssemblyName` = `Company.EplAddIn.<ProjectName>`
- Debug start action = `EPLAN.exe` (path auto-detected by `detect-eplan.ps1`)
- Debug start arguments = `/Variant:"Electric P8"`

## How to install a template

1. Project->Export template->Choose icon in solution directory
2. build `EPLAN_ADDIN_TEMPLATE.Wizard`
3. unzip the template, then edit the `.vstemplate` file and add the wizard extension inside VSTemplate tag

```xml
<WizardExtension>
  <Assembly>EPLAN_ADDIN_TEMPLATE.Wizard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</Assembly>
  <FullClassName>EPLAN_ADDIN_TEMPLATE.Wizard.TemplateWizard</FullClassName>
</WizardExtension>
```
4. rezip the template, be care of the zip hierarchy

5. install `EPLAN_ADDIN_TEMPLATE.Wizard.dll` and `detect-eplan.ps1` where Visual Studio can load template wizard assemblies, for example:

```
C:\Program Files\Microsoft Visual Studio\18\Community\Common7\IDE\PublicAssemblies
```

6. copy the template zip to the template folder:

```
C:\Users\<user>\Documents\Visual Studio 18\Templates\ProjectTemplates
```

## Where to find your template
```
C:\Users\<user>\Documents\Visual Studio 18\My Exported Templates
C:\Users\<user>\Documents\Visual Studio 18\Templates\ProjectTemplates
```

## Something after creation from template

1. restore packages

```
dotnet restore
```

2. dll signing ([EADN signing guide](../Resources/EADN-Signing-Script-V1.8/EADN_signing_guide-Eplan_Cloud.pdf))

	1. create conditional compilation for Debug/Release mode respectively (only sign in Release mode)

	1. bind your public key to the assembly and activate the "Delay sign only" flag

	1. add signing command line into post-build event [PostBuildScript.ps1 provide by offical](../Resources/EADN-Signing-Script-V1.8/PostBuildScript.ps1)

	```
	powershell -ExecutionPolicy Bypass -file "<YourFolderName>\PostBuildScript.ps1" -baseUrl "https://api.eplan.com.cn/eadn-signing/v1.0" -comment "signing dll from $(USERNAME)" -accessToken "<YourPAT>" -assemblies "$(OutDir)$(AssemblyName).dll" -destinationPath "$(OutDir)." -deleteAfterwards
	```

	> where to get PAT
	![PAT](./Resources/Snipaste_1.png)


## Offline API help

### Installer

- [offline API installer for 2026](Resources/Eplan_API_2026.zip)

- follow the official installation guide: [EPLAN API 2026 Help Structure](https://www.eplan.help/en-us/Infoportal/Content/api/2026/Help%20structure.html)

- after installation, one more thing you can do is binding the `F1` with `EPLAN API Help` in Visual Studio, so that you can directly open the API help by pressing `F1` when coding.

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