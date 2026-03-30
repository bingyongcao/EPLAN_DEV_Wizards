# EPLAN_ADDIN_TEMPLATE

## Where to find your template
```
C:\Users\<user>\Documents\Visual Studio 18\My Exported Templates
```

## How to make a template

1. Project->Export template...
2. unzip the template, and modify .csproj file to format assembly name and root namespace like below.

``` xml
<PropertyGroup>
  <AssemblyName>SAC.EplAddIn.$safeprojectname$</AssemblyName>
  <RootNamespace>$safeprojectname$</RootNamespace>
</PropertyGroup>
```
3. Rezip, be care of the zip hierarchy.

## How to use

1. restore packages

```
dotnet restore
```
2. set start program to EPLAN.exe
```
D:\Eplan\Platform\2026.0.3\Bin\EPLAN.exe
/Variant:"Electric P8"
```