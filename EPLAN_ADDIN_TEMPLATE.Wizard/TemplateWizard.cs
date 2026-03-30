using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;

namespace EPLAN_ADDIN_TEMPLATE.Wizard
{
    public sealed class TemplateWizard : IWizard
    {
        private const string MsBuildNamespace = "http://schemas.microsoft.com/developer/msbuild/2003";
        private const string EplanExecutablePath = @"D:\Eplan\Platform\2026.0.3\Bin\EPLAN.exe";
        private const string EplanArguments = "/Variant:\"Electric P8\"";

        private string _safeProjectName = string.Empty;

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            if (replacementsDictionary == null)
            {
                throw new ArgumentNullException("replacementsDictionary");
            }

            _safeProjectName = GetReplacementValue(replacementsDictionary, "$safeprojectname$");
            replacementsDictionary["$eplanassemblyname$"] = BuildAssemblyName(_safeProjectName);
        }

        public void ProjectFinishedGenerating(Project project)
        {
            if (project == null)
            {
                return;
            }

            var projectFilePath = project.FullName;
            if (string.IsNullOrWhiteSpace(projectFilePath) || !File.Exists(projectFilePath))
            {
                return;
            }

            var assemblyName = BuildAssemblyName(ResolveSafeProjectName(project, _safeProjectName));

            TrySetProjectProperty(project, "AssemblyName", assemblyName);
            project.Save();

            UpdateProjectFile(projectFilePath, assemblyName);
            WriteUserProjectFile(projectFilePath);
        }

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        public void RunFinished()
        {
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        private static string GetReplacementValue(IReadOnlyDictionary<string, string> replacementsDictionary, string key)
        {
            string value;
            return replacementsDictionary.TryGetValue(key, out value)
                ? value
                : string.Empty;
        }

        private static string ResolveSafeProjectName(Project project, string safeProjectName)
        {
            if (!string.IsNullOrWhiteSpace(safeProjectName))
            {
                return safeProjectName;
            }

            return !string.IsNullOrWhiteSpace(project.Name)
                ? project.Name
                : Path.GetFileNameWithoutExtension(project.FullName);
        }

        private static string BuildAssemblyName(string safeProjectName)
        {
            return string.IsNullOrWhiteSpace(safeProjectName)
                ? "SAC.EplAddIn"
                : string.Concat("SAC.EplAddIn.", safeProjectName);
        }

        private static void TrySetProjectProperty(Project project, string propertyName, object value)
        {
            try
            {
                var properties = project.Properties;
                if (properties == null)
                {
                    return;
                }

                var property = properties.Item(propertyName);
                if (property != null)
                {
                    property.Value = value;
                }
            }
            catch
            {
            }
        }

        private static void UpdateProjectFile(string projectFilePath, string assemblyName)
        {
            var document = XDocument.Load(projectFilePath, LoadOptions.PreserveWhitespace);
            var projectElement = document.Root;
            if (projectElement == null)
            {
                return;
            }

            var xmlNamespace = XNamespace.Get(MsBuildNamespace);
            var assemblyNameElement = document.Descendants(xmlNamespace + "AssemblyName").FirstOrDefault();

            if (assemblyNameElement == null)
            {
                var propertyGroup = projectElement.Elements(xmlNamespace + "PropertyGroup")
                    .FirstOrDefault(element => element.Attribute("Condition") == null);

                if (propertyGroup == null)
                {
                    propertyGroup = new XElement(xmlNamespace + "PropertyGroup");
                    projectElement.AddFirst(propertyGroup);
                }

                propertyGroup.Add(new XElement(xmlNamespace + "AssemblyName", assemblyName));
            }
            else
            {
                assemblyNameElement.Value = assemblyName;
            }

            document.Save(projectFilePath);
        }

        private static void WriteUserProjectFile(string projectFilePath)
        {
            var xmlNamespace = XNamespace.Get(MsBuildNamespace);
            var userProjectPath = projectFilePath + ".user";
            var document = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement(
                    xmlNamespace + "Project",
                    new XAttribute("ToolsVersion", "15.0"),
                    new XElement(
                        xmlNamespace + "PropertyGroup",
                        new XAttribute("Condition", "'$(Configuration)|$(Platform)' == 'Debug|AnyCPU'"),
                        new XElement(xmlNamespace + "StartAction", "Program"),
                        new XElement(xmlNamespace + "StartProgram", EplanExecutablePath),
                        new XElement(xmlNamespace + "StartArguments", EplanArguments))));

            document.Save(userProjectPath);
        }
    }
}
