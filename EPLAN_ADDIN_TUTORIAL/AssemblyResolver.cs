using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace EPLAN_API_TUTORIAL
{
    /// <summary>
    /// Runtime assembly resolver for the EPLAN add-in.
    /// <para>
    /// EPLAN hosts the add-in inside its own process / AppDomain. The CLR's default
    /// probing path does not include the add-in's directory, so dependencies that
    /// are deployed alongside the add-in DLL (e.g. <c>EPLAN_UTILITIES.dll</c>,
    /// third-party libraries under <c>DLLs\</c>) cannot be located by the runtime
    /// and <see cref="AppDomain.AssemblyResolve"/> fires with a
    /// <see cref="FileNotFoundException"/> result.
    /// </para>
    /// <para>
    /// This resolver hooks that event and probes:
    /// </para>
    /// <list type="number">
    ///   <item>The add-in's own directory (where <c>SAC.EplAddIn.Tools.dll</c> lives).</item>
    ///   <item>A <c>DLLs\</c> subfolder next to it (matches this project's layout).</item>
    /// </list>
    /// <para>
    /// Resolved assemblies are cached by simple name to avoid repeated disk hits
    /// and to break accidental recursion when a freshly loaded assembly pulls in
    /// its own dependencies.
    /// </para>
    /// </summary>
    internal static class AssemblyResolver
    {
        private static readonly ConcurrentDictionary<string, Assembly> _cache
            = new ConcurrentDictionary<string, Assembly>(StringComparer.OrdinalIgnoreCase);

        private static readonly string _addInDirectory;
        private static readonly string _dllsDirectory;
        private static readonly string _logFilePath;
        private static readonly object _logLock = new object();
        private static bool _registered;

        static AssemblyResolver()
        {
            try
            {
                var location = Assembly.GetExecutingAssembly().Location;
                _addInDirectory = !string.IsNullOrEmpty(location)
                    ? Path.GetDirectoryName(location)
                    : AppDomain.CurrentDomain.BaseDirectory;

                _dllsDirectory = string.IsNullOrEmpty(_addInDirectory)
                    ? null
                    : Path.Combine(_addInDirectory, "DLLs");

                _logFilePath = string.IsNullOrEmpty(_addInDirectory)
                    ? null
                    : Path.Combine(_addInDirectory, "assembly_resolver.log");
            }
            catch (Exception ex)
            {
                // Path probing is best-effort; do not let init failures escape
                // a static constructor (which would tear down the AppDomain).
                Trace.WriteLine($"[SAC.AssemblyResolver] init error: {ex}");
            }
        }

        /// <summary>
        /// Wires <see cref="AppDomain.AssemblyResolve"/> to <see cref="OnAssemblyResolve"/>.
        /// Safe to call multiple times — only the first call has effect.
        /// </summary>
        public static void Register()
        {
            if (_registered) return;
            _registered = true;

            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
            Trace.WriteLine(
                $"[SAC.AssemblyResolver] registered. AddIn dir = '{_addInDirectory}', DLLs dir = '{_dllsDirectory}'.");
        }

        private static Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Name))
                return null;

            Assembly probe;
            string simpleName;
            try
            {
                // args.Name is a full identity, e.g.
                // "EPLAN_UTILITIES, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
                // We only care about the simple name.
                simpleName = new AssemblyName(args.Name).Name;
            }
            catch
            {
                return null;
            }

            if (string.IsNullOrEmpty(simpleName))
                return null;

            // Satellite resource lookups (".resources") are handled by the runtime
            // through ResourceManager — never try to load them as assemblies here.
            if (simpleName.EndsWith(".resources", StringComparison.OrdinalIgnoreCase))
                return null;

            if (_cache.TryGetValue(simpleName, out var cached))
                return cached;

            probe = TryLoadFrom(simpleName + ".dll") ?? TryLoadFrom(simpleName + ".exe");
            if (probe != null)
            {
                _cache[simpleName] = probe;
                return probe;
            }

            LogMiss(simpleName);
            return null;
        }

        private static Assembly TryLoadFrom(string fileName)
        {
            foreach (var directory in new[] { _addInDirectory, _dllsDirectory })
            {
                if (string.IsNullOrEmpty(directory)) continue;

                var candidate = Path.Combine(directory, fileName);
                if (!File.Exists(candidate)) continue;

                try
                {
                    return Assembly.LoadFrom(candidate);
                }
                catch (Exception ex)
                {
                    LogException(candidate, ex);
                    // Continue to the next directory — a corrupt candidate in
                    // one folder shouldn't block resolution from another.
                }
            }

            return null;
        }

        private static void LogMiss(string simpleName)
        {
            var line =
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] miss: '{simpleName}' " +
                $"(probed: '{_addInDirectory}', '{_dllsDirectory}').{Environment.NewLine}";
            Trace.WriteLine(line);
            TryAppendLog(line);
        }

        private static void LogException(string candidate, Exception ex)
        {
            var line =
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] load error: '{candidate}' — {ex}{Environment.NewLine}";
            Trace.WriteLine(line);
            TryAppendLog(line);
        }

        private static void TryAppendLog(string line)
        {
            if (string.IsNullOrEmpty(_logFilePath)) return;
            try
            {
                lock (_logLock)
                {
                    File.AppendAllText(_logFilePath, line, Encoding.UTF8);
                }
            }
            catch
            {
                // Logging is best-effort; never let it propagate to the resolver
                // callback (which is invoked inside the CLR's load path).
            }
        }
    }
}
