# EPLAN_SCRIPT_TUTORIAL

This tutorial provides practical examples demonstrating how to extend functionality using EPLAN scripting. Each script showcases different aspects of the EPLAN API and scripting capabilities.

## Script Examples

### 1. AddCustomUI.cs
Demonstrates how to add custom UI elements to EPLAN's ribbon interface:
- Declares actions using `[DeclareAction]` attribute
- Creates custom ribbon buttons
- Shows how to register actions that can be called from EPLAN UI

### 2. AddContextMenu.cs
Shows how to dynamically add context menu items to EPLAN:
- Adds custom menu items to the EPLAN context menu
- Demonstrates UI customization at runtime

**Note:** Context menu items are removed when EPLAN restarts. You need to execute the action again after restarting EPLAN.

### 3. ShowContextMenuInfo.cs
Provides an example of retrieving and displaying context menu information:
- Shows how to query available context menus
- Uses `FrmSelect` dialog to display information
- Demonstrates UI interaction with EPLAN's menu system

### 4. EventHandler.cs
Illustrates EPLAN event handling:
- Uses `[DeclareEventHandler]` attribute to register event callbacks
- Example: Shows a message box when a project is opened (`OnPostOpenProject`)
- Demonstrates automatic event handler cleanup (no manual unregistration needed)

**Note:** EPLAN automatically removes all event handlers added by a script when the script is reloaded or removed.

### 5. CallScriptByCLI.cs
Demonstrates calling EPLAN scripts from the command line with parameters:
- Shows how to use the `[Start]` attribute as script entry point
- Accepts parameters from command line

**Usage Example:**
```bash
W3u.exe ExecuteScript /ScriptFile:"~\SimpleScriptWithParameters.cs" /Param1:"Hello" /Param2:"EPLAN"
```

### 6. Settings.cs
Shows how to interact with EPLAN settings programmatically:
- Access and modify EPLAN settings via `Eplan.EplApi.Base.Settings`
- Demonstrates settings management automation

## Getting Started

### Prerequisites
- EPLAN Electric P8 (version 2026 or later)
- .NET Framework 4.8.1
- Basic knowledge of C# programming

## Common Patterns

### Namespace Conflict Prevention
All scripts include a region to avoid namespace conflicts:
```csharp
#region should be included to avoid namespace conflict
using System;
using System.Windows.Forms;
// ... other system namespaces
#endregion

using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.Scripting;
// ... EPLAN namespaces
```

### RibbonBar Best Practice
Use `RibbonBar(true)` to avoid UI flickering:
```csharp
RibbonBar myRibbonBar = new RibbonBar(true); // refreshAfterChanges = true
```

## References
- [EPLAN Scripting - Johann Weiher - Suplanus](https://eplan-scripting.suplanus.de/v4/en/)
- [EPLAN API Documentation](https://www.eplan.help/en-us/Infoportal/Content/api/2026/index.html)

## Tips

- Event handlers are automatically cleaned up
- Use the `[DeclareAction]` attribute to make functions accessible as EPLAN actions
- Use the `[Start]` attribute for command-line script entry points