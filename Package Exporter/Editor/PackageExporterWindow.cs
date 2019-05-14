using EtienneDx.Utils;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EtienneDx.PackageExporter
{
    [InitializeOnLoad]
    public class PackageExporterWindow : EditorWindow
    {

        static PackageExporterWindow()
        {
            if (!File.Exists("exporter.dat"))
            {
                File.Create("exporter.dat").Close();
                File.WriteAllText("exporter.dat", "Assets/\n");
            }
            string[] s = File.ReadAllLines("exporter.dat");
            if (s.Length > 0)
            {
                exportMainFolder = s[0];
                try
                {
                    commonFolder = s[1];
                }
                catch { commonFolder = ""; }
            }
            else
            {
                exportMainFolder = "Assets/";
            }
        }

        private static string exportMainFolder = "";
        private static string commonFolder = "";
        private Vector2 scrollPos;
        private Dictionary<string, bool> packageCheck = new Dictionary<string, bool>();
        private Mode mode = Mode.EXPORT;

        [MenuItem("Window/Packages Exporter")]
        private static void Init()
        {
            PackageExporterWindow window = GetWindow<PackageExporterWindow>("Exports");
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);

            if (GUILayout.Button("Export", EditorStyles.toolbarButton))
            {
                mode = Mode.EXPORT;
                packageCheck.Clear();
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Change Options", EditorStyles.toolbarButton))
            {
                mode = Mode.OPTIONS;
            }

            GUILayout.EndHorizontal();

            scrollPos = GUILayout.BeginScrollView(scrollPos, false, false);

            if (mode == Mode.EXPORT)
            {

                bool def = false;

                if (GUILayout.Button("Export"))
                {
                    Export();
                }
                GUILayout.Space(5);
                if (GUILayout.Button("Clear"))
                {
                    packageCheck.Clear();
                }
                if (GUILayout.Button("Select All"))
                {
                    packageCheck.Clear();
                    def = true;
                }

                foreach (var kv in PossiblePackages)
                {
                    if (!packageCheck.ContainsKey(kv.Value))
                        packageCheck.Add(kv.Value, def);
                    packageCheck[kv.Value] = EditorGUILayout.Toggle(kv.Key, packageCheck[kv.Value]);
                }

            }
            else if (mode == Mode.OPTIONS)
            {
                exportMainFolder = EditorGUILayout.TextField("Main Folder", exportMainFolder);
                commonFolder = EditorGUILayout.TextField("Common Folder", commonFolder);

                if (GUILayout.Button("Save Data"))
                {
                    if (!File.Exists("exporter.dat"))
                        File.Create("exporter.dat").Close();
                    File.WriteAllText("exporter.dat", exportMainFolder + "\n" + commonFolder);
                }

                if (GUILayout.Button("Revert"))
                {
                    string[] s = File.ReadAllLines("exporter.dat");
                    if (s.Length > 0)
                    {
                        exportMainFolder = s[0];
                        try
                        {
                            commonFolder = s[1];
                        }
                        catch { commonFolder = ""; }
                    }
                    else
                    {
                        exportMainFolder = "Assets/";
                    }
                }
            }

            GUILayout.EndScrollView();
        }

        private void Export()
        {
            if (!Directory.Exists("Exports"))
                Directory.CreateDirectory("Exports");
            foreach (var kv in packageCheck)
            {
                if (kv.Value)
                {
                    AssetDatabase.ExportPackage(new string[] {
                kv.Key,
                exportMainFolder + commonFolder
                }, "Exports/" + Path.GetFileName(kv.Key) + ".unitypackage", ExportPackageOptions.Recurse);
                }
            }
            ShowRootExplorer(Path.GetFullPath("Exports/"));
        }

        private void ShowRootExplorer(string path)
        {
#if UNITY_EDITOR_WIN
            path = path.Replace(@"/", @"\");   // explorer doesn't like front slashes
            System.Diagnostics.Process.Start("explorer.exe", "/root," + path);
#elif UNITY_EDITOR_OSX
        EditorUtility.RevealInFinder(path)
#endif
        }

        private static Counter refresh = new Counter(10);
        private static Dictionary<string, string> _possiblePackage;

        private static Dictionary<string, string> PossiblePackages
        {
            get
            {
                if (refresh.Ready)
                    _possiblePackage = GetPossiblePackages();
                return _possiblePackage ?? (_possiblePackage = GetPossiblePackages());
            }
        }
        private static Dictionary<string, string> GetPossiblePackages()
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            foreach (var f in Directory.GetDirectories(exportMainFolder))
            {
                if (Path.GetFileName(f) != commonFolder)
                {
                    ret.Add(Path.GetFileName(f), f);
                }
            }

            return ret;
        }
    }

    internal enum Mode
    {
        EXPORT, OPTIONS
    }
}