using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Serilog;

namespace myapp.Services
{
    public class PluginEngine
    {
        public List<Assembly> LoadedAssemblies { get; private set; }

        public PluginEngine()
        {
            LoadedAssemblies = new List<Assembly>();
        }

        public void LoadPlugins(string path = "src/Plugins")
        {
            Log.Information("Loading plugins from: {Path}", Path.GetFullPath(path));

            if (!Directory.Exists(path))
            {
                Log.Warning("Plugin directory not found: {Path}", Path.GetFullPath(path));
                return;
            }

            var pluginAssemblies = Directory.GetFiles(path, "*.dll")
                .Select(file =>
                {
                    try
                    {
                        // Use LoadFrom to load the assembly
                        return Assembly.LoadFrom(Path.GetFullPath(file));
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Error loading plugin assembly: {File}", file);
                        return null; // Return null if loading fails
                    }
                })
                .Where(assembly => assembly != null) // Filter out nulls
                .Cast<Assembly>() // Cast to non-nullable Assembly
                .ToList();

            LoadedAssemblies.AddRange(pluginAssemblies);

            Log.Information("Loaded {Count} plugin assemblies.", pluginAssemblies.Count);
        }
    }
}
