#define DEBUG

using Assets.EtienneDx.ConfigFile.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Assets.EtienneDx.ConfigFile
{
    public static class ConfigFile
    {
        private const string configFolder = "Config/";

        public static string ConfigFolder
        {
            get
            {
                return Path.Combine(Application.persistentDataPath, configFolder);
            }
        }

        private static Dictionary<string, IConfigFile> configFiles;

        [RuntimeInitializeOnLoadMethod]
        static void Init()
        {
            if (!Application.isPlaying)
            {
                return;// must be playing, otherwise it makes no sense
            }
#if DEBUG
            Debug.Log("Loading configs from folder : " + ConfigFolder);
#endif

            ReloadConfig();
        }

        public static void SaveConfig()
        {
            foreach (IConfigFile config in configFiles.Values)
            {
                SaveConfig(config);
            }
        }

        public static void SaveConfig(string config)
        {
            if (configFiles.ContainsKey(config))
            {
                SaveConfig(configFiles[config]);
            }
        }

        public static void SaveConfig(IConfigFile config)
        {
            ConfigNameAttribute attr = (ConfigNameAttribute)config.GetType().GetCustomAttributes(typeof(ConfigNameAttribute), false).FirstOrDefault();
            string cfgName = attr == null ? config.GetType().Name : attr.name;

            File.WriteAllText(GetPath(cfgName), JsonUtility.ToJson(config));
        }

        public static IConfigFile GetConfig(string cfg)
        {
            return configFiles.ContainsKey(cfg) ? configFiles[cfg] : null;
        }

        public static T GetConfig<T>() where T : IConfigFile
        {
            return (T)configFiles.Values.FirstOrDefault(cfg => cfg is T);
        }

        public static void ReloadConfig()
        {
            if (configFiles == null)
            {
                configFiles = new Dictionary<string, IConfigFile>();
            }
            else
            {
                configFiles.Clear();
            }

            List<Type> configs = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(t => typeof(IConfigFile).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract && t.IsSerializable)
                .ToList();

            foreach (Type t in configs)
            {
                LoadConfig(t);
            }
        }

        private static void LoadConfig(Type t)
        {
            ConfigNameAttribute attr = (ConfigNameAttribute)t.GetCustomAttributes(typeof(ConfigNameAttribute), false).FirstOrDefault();
            string cfgName = attr == null ? t.Name : attr.name;

            string path = GetPath(cfgName);
            if (Directory.Exists(ConfigFolder) && File.Exists(path))
            {
                configFiles.Add(cfgName, (IConfigFile)JsonUtility.FromJson(File.ReadAllText(path), t));
            }
            else
            {
                IConfigFile cfg = (IConfigFile)Activator.CreateInstance(t);
                configFiles.Add(cfgName, cfg);

                Directory.CreateDirectory(ConfigFolder);
                File.WriteAllText(path, JsonUtility.ToJson(cfg));// save default
            }
        }

        private static string GetPath(string cfgName)
        {
            return Path.Combine(Application.persistentDataPath, configFolder, cfgName + ".json");
        }
    }
}
