#!/usr/bin/env bash
set -euo pipefail

WINDOWS_WIZARD_DLL='D:\repos\EPLAN_API_TUTORIAL\EPLAN_ADDIN_TEMPLATE.Wizard\bin\Release\EPLAN_ADDIN_TEMPLATE.Wizard.dll'
WINDOWS_PUBLIC_ASSEMBLIES='C:\Program Files\Microsoft Visual Studio\18\Community\Common7\IDE\PublicAssemblies'
WINDOWS_TEMPLATE_ZIP_SOURCE='C:\Users\cby\Documents\Visual Studio 18\My Exported Templates\EPLAN_ADDIN_TEMPLATE.zip'
WINDOWS_TEMPLATE_ZIP_DEST='C:\Users\cby\Documents\Visual Studio 18\Templates\ProjectTemplates\EPLAN_ADDIN_TEMPLATE.zip'

to_bash_path() {
    if command -v cygpath >/dev/null 2>&1; then
        cygpath -u "$1"
    else
        printf '%s\n' "$1"
    fi
}

WIZARD_DLL="$(to_bash_path "$WINDOWS_WIZARD_DLL")"
PUBLIC_ASSEMBLIES_DIR="$(to_bash_path "$WINDOWS_PUBLIC_ASSEMBLIES")"
TEMPLATE_ZIP_SOURCE="$(to_bash_path "$WINDOWS_TEMPLATE_ZIP_SOURCE")"
TEMPLATE_ZIP_DEST="$(to_bash_path "$WINDOWS_TEMPLATE_ZIP_DEST")"

if [[ ! -f "$WIZARD_DLL" ]]; then
    echo "Wizard DLL not found: $WINDOWS_WIZARD_DLL" >&2
    echo "Build EPLAN_ADDIN_TEMPLATE.Wizard in Release first." >&2
    exit 1
fi

if [[ ! -d "$PUBLIC_ASSEMBLIES_DIR" ]]; then
    echo "Visual Studio PublicAssemblies folder not found: $WINDOWS_PUBLIC_ASSEMBLIES" >&2
    exit 1
fi

if [[ ! -f "$TEMPLATE_ZIP_SOURCE" ]]; then
    echo "Template zip not found: $WINDOWS_TEMPLATE_ZIP_SOURCE" >&2
    exit 1
fi

mkdir -p "$(dirname "$TEMPLATE_ZIP_DEST")"
cp -f "$WIZARD_DLL" "$PUBLIC_ASSEMBLIES_DIR/"
cp -f "$TEMPLATE_ZIP_SOURCE" "$TEMPLATE_ZIP_DEST"

echo "Copied wizard DLL to: $WINDOWS_PUBLIC_ASSEMBLIES"
echo "Copied template zip to: $WINDOWS_TEMPLATE_ZIP_DEST"
echo "Done. Restart Visual Studio if it was open."
