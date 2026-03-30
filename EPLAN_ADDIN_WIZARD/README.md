# EPLAN_ADDIN_WIZARD

local template path
```
C:\Users\cby\Documents\Visual Studio 18\My Exported Templates
```

## How to make

1. Project->Export template...

If dlls are lost after dotnet restore, follow the steps below:

2. After exported, unzip the template, and add dlls explicitly. Edit .vstemplate, and verify this: 
``` xml
<ProjectItem ReplaceParameters="false">DLLs\MyLib.dll</ProjectItem>
```
3. Verify .csproj, making it is relative

``` xml
<Reference Include="MyLib">
  <HintPath>lib\MyLib.dll</HintPath>
</Reference>
```
4. Rezip

## How to use

```
dotnet restore
```