using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EtienneDx.Utils
{
    public class DefinesManagerWindow : EditorWindow
    {
        private const string defineListExtension = ".defines";
        private Vector2 scrollPos;
        private Dictionary<string, bool> definesVal = new Dictionary<string, bool>();

        [MenuItem("Window/Defines Manager")]
        private static void Init()
        {
            DefinesManagerWindow window = GetWindow<DefinesManagerWindow>("Defines");
            window.Show();
        }

        private void OnGUI()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos, false, false);

            EditorGUILayout.HelpBox("Just hover any define to get more informations", MessageType.Info);

            foreach (var def in FindExistingDefines())
            {
                if (def is DefineSeparation)
                {
                    EditorGUILayout.LabelField(def.Name, EditorStyles.boldLabel);
                }
                else
                {
                    GUIContent c = new GUIContent(GetDisplayName(def.Name), ((Define)def).definition);
                    if (!definesVal.ContainsKey(def.Name))
                        definesVal.Add(def.Name, ((Define)def).enabled);
                    definesVal[def.Name] = EditorGUILayout.Toggle(c, definesVal[def.Name]);
                }
            }

            if (GUILayout.Button("Update all changes"))
            {
                UpdateChanges();
            }

            GUILayout.EndScrollView();
        }

        private string GetDisplayName(string name)
        {
            List<string> s = new List<string>(name.ToLower().Split('_'));
            for (int i = 0; i < s.Count; i++)
            {
                s[i] = s[i][0].ToString().ToUpper() + s[i].Substring(1);
            }
            return string.Join(" ", s.ToArray());
        }

        private void UpdateChanges()
        {
            foreach (var kv in definesVal)
            {
                ChangeDefine(kv.Key, kv.Value);
            }
        }

        private void ChangeDefine(string define, bool b)
        {
            int i = ChangeDefine(define, b, "Assets/");
            AssetDatabase.Refresh();
            if (i > 0)
                Debug.LogWarning("Define <i>" + define.ToUpper().Replace(' ', '_') + "</i> has been <b>" + (b ? "enabled" : "disabled") + "</b> on " +
                    i + (i > 1 ? " different files" : " file"));
            else
                Debug.LogWarning("No reference to define <i>" + define.ToUpper().Replace(' ', '_') + "</i> have been found");
        }

        private int ChangeDefine(string define, bool b, string directory)
        {
            int c = 0;
            foreach (var dir in Directory.GetDirectories(directory))
            {
                c += ChangeDefine(define, b, dir);
            }

            foreach (var fi in Directory.GetFiles(directory))
            {
                if (Path.GetExtension(fi) == ".cs")
                {
                    var lines = File.ReadAllLines(fi);
                    string totalDefine = "#define " + define.ToUpper().Replace(' ', '_');
                    bool anyChange = false;
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].EndsWith(totalDefine))
                        {
                            lines[i] = (b ? "" : "//") + totalDefine;
                            anyChange = true;
                        }
                    }
                    if (anyChange)
                    {
                        File.WriteAllLines(fi, lines);
                        c++;
                    }
                }
                else if (Path.GetExtension(fi) == defineListExtension)
                {
                    var lines = File.ReadAllLines(fi);
                    string totalDefine = define.ToUpper().Replace(' ', '_');
                    bool anyChange = false;
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].StartsWith(totalDefine + "@"))
                        {
                            lines[i] = totalDefine + "@" + (b ? "enable" : "disable");
                            anyChange = true;
                        }
                    }
                    if (anyChange)
                    {
                        File.WriteAllLines(fi, lines);
                        c++;
                    }
                }
            }
            return c;
        }

        private static List<IDefine> FindExistingDefines(string directory = "Assets/")
        {
            List<IDefine> defines = new List<IDefine>();


            foreach (var dir in Directory.GetDirectories(directory))
            {
                foreach (var def in FindExistingDefines(dir))
                    defines.Add(def);
            }

            foreach (var fi in Directory.GetFiles(directory))
            {
                if (Path.GetExtension(fi) == defineListExtension)
                {
                    defines.Add(new DefineSeparation(Path.GetFileNameWithoutExtension(fi)));
                    Define def = new Define("", "", false);
                    foreach (var l in File.ReadAllLines(fi))
                    {
                        if (l == "" || l.StartsWith("//")) continue;
                        bool b = l.Contains("@enable");
                        if (b || l.Contains("@disable"))
                        {
                            if (def.Name != "")
                            {
                                defines.Add(def);
                                def = new Define("", "", false);
                            }
                            def.Name = l.Split('@')[0];
                            def.enabled = b;
                        }
                        else
                        {
                            def.definition += (def.definition == "" ? "" : "\n") + l;
                        }
                    }
                    if (def.Name != "")
                        defines.Add(def);
                }
            }

            return defines;
        }
    }
}