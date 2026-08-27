using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using Process = System.Diagnostics.Process;

namespace EPLAN_ADDIN_TEMPLATE.Wizard
{
    public sealed class TemplateWizard : IWizard
    {
        private const string MsBuildNamespace = "http://schemas.microsoft.com/developer/msbuild/2003";
        private const string EplanExecutablePathEnvironmentVariable = "EPLAN_EXECUTABLE_PATH";
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

            var eplanExecutableResolution = ResolveEplanExecutable();

            UpdateProjectFile(projectFilePath, assemblyName);
            WriteUserProjectFile(projectFilePath, eplanExecutableResolution);
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
                ? "Company.EplAddIn"
                : string.Concat("Company.EplAddIn.", safeProjectName);
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

        private static void WriteUserProjectFile(string projectFilePath, EplanExecutableResolution eplanExecutableResolution)
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
                        new XElement(xmlNamespace + "StartProgram", eplanExecutableResolution.ExecutablePath),
                        new XElement(xmlNamespace + "StartArguments", EplanArguments),
                        new XElement(xmlNamespace + "EplanStartProgramSource", eplanExecutableResolution.Source))));

            document.Save(userProjectPath);
        }

        private static EplanExecutableResolution ResolveEplanExecutable()
        {
            var overridePath = Environment.GetEnvironmentVariable(EplanExecutablePathEnvironmentVariable);
            if (!string.IsNullOrWhiteSpace(overridePath) && File.Exists(overridePath))
            {
                return new EplanExecutableResolution(overridePath, $"env:{EplanExecutablePathEnvironmentVariable}");
            }

            try
            {
                return DetectEplanExecutableViaScript();
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException(
                    "Unable to locate EPLAN.exe. " +
                    "Set the EPLAN_EXECUTABLE_PATH environment variable to the full path of EPLAN.exe, " +
                    "or install EPLAN so its Bin folder (containing Eplan.EplApi.Base.dll) is reachable " +
                    "through the registry or the standard Program Files location. " +
                    "Underlying error: " + exception.Message,
                    exception);
            }
        }

        private static EplanExecutableResolution DetectEplanExecutableViaScript()
        {
            var scriptPath = LocateDetectEplanScript();
            var workingDirectory = Path.GetDirectoryName(scriptPath);
            if (string.IsNullOrEmpty(workingDirectory))
            {
                throw new FileNotFoundException("Could not determine directory of detect-eplan.ps1.");
            }

            var arguments = string.Concat(
                "-NoProfile -NonInteractive -ExecutionPolicy Bypass -File \"", scriptPath, "\" -AsJson");

            var startInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = arguments,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = workingDirectory,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };

            using (var process = Process.Start(startInfo))
            {
                if (process == null)
                {
                    throw new InvalidOperationException("Failed to start powershell.exe to detect EPLAN.");
                }

                // Read both streams concurrently to avoid a deadlock when one of them fills its
                // pipe buffer while we are blocked reading the other.
                var standardOutputTask = process.StandardOutput.ReadToEndAsync();
                var standardErrorTask = process.StandardError.ReadToEndAsync();
                if (!process.WaitForExit((int)TimeSpan.FromSeconds(30).TotalMilliseconds))
                {
                    try { process.Kill(); } catch { /* best effort */ }
                    throw new TimeoutException("detect-eplan.ps1 did not finish within 30 seconds.");
                }

                var standardOutput = standardOutputTask.GetAwaiter().GetResult();
                var standardError = standardErrorTask.GetAwaiter().GetResult();

                if (process.ExitCode != 0)
                {
                    throw new InvalidOperationException(
                        "detect-eplan.ps1 exited with code " + process.ExitCode + ". " + standardError);
                }

                var installations = ParseInstallations(standardOutput);
                if (installations.Length == 0)
                {
                    throw new FileNotFoundException(
                        "detect-eplan.ps1 reported no EPLAN installations. " +
                        "Install EPLAN or set EPLAN_EXECUTABLE_PATH.");
                }

                var best = installations[0];
                var executablePath = ResolveEplanExecutableFromAssemblyDirectory(best.AssemblyDirectory);
                if (executablePath == null)
                {
                    throw new FileNotFoundException(
                        "detect-eplan.ps1 located Eplan.EplApi.Base.dll in " + best.AssemblyDirectory +
                        " but EPLAN.exe could not be found nearby. " +
                        "Set EPLAN_EXECUTABLE_PATH to the EPLAN.exe full path.");
                }

                return new EplanExecutableResolution(executablePath, "detect-eplan:" + best.DiscoverySource);
            }
        }

        private static string LocateDetectEplanScript()
        {
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var assemblyDirectory = string.IsNullOrEmpty(assemblyLocation)
                ? AppDomain.CurrentDomain.BaseDirectory
                : Path.GetDirectoryName(assemblyLocation);

            if (string.IsNullOrEmpty(assemblyDirectory))
            {
                throw new FileNotFoundException("Could not determine wizard assembly directory.");
            }

            var scriptPath = Path.Combine(assemblyDirectory, "detect-eplan.ps1");
            if (!File.Exists(scriptPath))
            {
                throw new FileNotFoundException(
                    "detect-eplan.ps1 not found next to EPLAN_ADDIN_TEMPLATE.Wizard.dll at " + scriptPath + ". " +
                    "Rebuild the wizard so the script is copied to the output directory.");
            }

            return scriptPath;
        }

        private static InstallationCandidate[] ParseInstallations(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return new InstallationCandidate[0];
            }

            var serializer = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
            var raw = serializer.Deserialize<List<Dictionary<string, object>>>(json);
            if (raw == null)
            {
                return new InstallationCandidate[0];
            }

            var candidates = new List<InstallationCandidate>(raw.Count);
            foreach (var entry in raw)
            {
                if (entry == null) { continue; }

                object assemblyDirectoryValue;
                if (!entry.TryGetValue("AssemblyDirectory", out assemblyDirectoryValue)) { continue; }
                var assemblyDirectory = assemblyDirectoryValue as string;
                if (string.IsNullOrEmpty(assemblyDirectory)) { continue; }

                var discoverySource = string.Empty;
                object discoverySourceValue;
                if (entry.TryGetValue("DiscoverySource", out discoverySourceValue))
                {
                    discoverySource = discoverySourceValue as string ?? string.Empty;
                }

                candidates.Add(new InstallationCandidate(assemblyDirectory, discoverySource));
            }

            return candidates.ToArray();
        }

        private static string ResolveEplanExecutableFromAssemblyDirectory(string assemblyDirectory)
        {
            if (string.IsNullOrEmpty(assemblyDirectory) || !Directory.Exists(assemblyDirectory))
            {
                return null;
            }

            const string executableName = "EPLAN.exe";

            // The script returns the directory that contains Eplan.EplApi.Base[U].dll.
            // In every supported EPLAN layout that is the Bin folder where EPLAN.exe lives.
            var direct = Path.Combine(assemblyDirectory, executableName);
            if (File.Exists(direct)) { return direct; }

            // Defensive fallback: search upward a few levels in case the script ever starts
            // returning a deeper directory.
            var current = new DirectoryInfo(assemblyDirectory);
            for (var i = 0; i < 4 && current != null; i++)
            {
                var candidate = Path.Combine(current.FullName, executableName);
                if (File.Exists(candidate)) { return candidate; }

                var binCandidate = Path.Combine(current.FullName, "Bin", executableName);
                if (File.Exists(binCandidate)) { return binCandidate; }

                current = current.Parent;
            }

            return null;
        }

        private readonly struct EplanExecutableResolution
        {
            public EplanExecutableResolution(string executablePath, string source)
            {
                ExecutablePath = executablePath;
                Source = source;
            }

            public string ExecutablePath { get; }
            public string Source { get; }
        }

        private readonly struct InstallationCandidate
        {
            public InstallationCandidate(string assemblyDirectory, string discoverySource)
            {
                AssemblyDirectory = assemblyDirectory;
                DiscoverySource = discoverySource;
            }

            public string AssemblyDirectory { get; }
            public string DiscoverySource { get; }
        }
    }
}
