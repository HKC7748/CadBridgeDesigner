using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;
using Exception = System.Exception;

namespace CadBridgeDesigner.TestTool
{
    public class TestTool
    {
        private const string PluginDirectory = @"D:\WorkSpace\ProgramProject\C#\CadBridgeDesigner\out";
        private static readonly Dictionary<string, Assembly> ResolvedAssemblies = new(StringComparer.OrdinalIgnoreCase);
        private static readonly Dictionary<string, IExtensionApplication> LoadedApplications = new(StringComparer.OrdinalIgnoreCase);
        private static IExtensionApplication _toolApplication;
        private static bool _initialized;

        [CommandMethod("LOAD_TEST_DLL")]
        public static void LoadTestDll()
        {
            Document document = Application.DocumentManager.MdiActiveDocument;
            try
            {
                if (!_initialized)
                {
                    AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
                    ResolvedAssemblies["CadBridgeDesigner.Core"] = Assembly.Load(ReadAssemblyBytes("CadBridgeDesigner.Core"));
                    _initialized = true;
                }
                DetachPreviousApplication();
                ResolvedAssemblies["CadBridgeDesigner.Tool"] = Assembly.Load(ReadAssemblyBytes("CadBridgeDesigner.Tool"));
                _toolApplication = (IExtensionApplication)ResolvedAssemblies["CadBridgeDesigner.Tool"].CreateInstance("CadBridgeDesigner.Tool.Main");
                LoadedApplications[$"{LoadedApplications.Count}"] = _toolApplication;
                document?.Editor.WriteMessage("视图与工具已重新加载\n");
            }
            catch (Exception exception)
            {
                document?.Editor.WriteMessage((exception.InnerException ?? exception).Message + "\n");
            }
        }
        private static byte[] ReadAssemblyBytes(string assemblyName)
        {
            return File.ReadAllBytes(Path.Combine(PluginDirectory, assemblyName + ".dll"));
        }

        private static Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            string name = new AssemblyName(args.Name).Name;
            if (!name.StartsWith("CadBridgeDesigner.", StringComparison.Ordinal)) return null;
            if (name == "CadBridgeDesigner.Tool" || name == "CadBridgeDesigner.TestTool") return null;
            if (ResolvedAssemblies.TryGetValue(name, out Assembly cachedAssembly)) return cachedAssembly;
            string path = Path.Combine(PluginDirectory, name + ".dll");
            if (!File.Exists(path)) return null;
            Assembly assembly = Assembly.Load(File.ReadAllBytes(path));
            ResolvedAssemblies[name] = assembly;
            return assembly;
        }
        
        private static void DetachPreviousApplication()
        {
            foreach (KeyValuePair<string, IExtensionApplication> loadedApplication in LoadedApplications)
            {
                loadedApplication.Value?.Terminate();
            }
            LoadedApplications.Clear();
        }
    }
}
