using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace EtienneDx.Conditions
{
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif
    public class ConditionList
    {
        private static Dictionary<string, Func<bool>> conditions = new Dictionary<string, Func<bool>>();

        public static string[] PossibleConditions => conditions.Keys.ToArray();

#if UNITY_EDITOR
        static ConditionList()
        {
            Init();
        }
#endif

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            conditions.Clear();
            foreach (MethodInfo mi in AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).SelectMany(t => t.GetMethods(BindingFlags.Static))
                .Where(mi => mi.GetCustomAttribute<ConditionAttribute>(true) != null && 
                    !mi.IsAbstract && mi.ReturnType == typeof(bool) && !mi.GetParameters().Any()))
            {
                string conditionName = mi.GetCustomAttribute<ConditionAttribute>().conditionName;
                if(conditions.ContainsKey(conditionName))
                {
                    Debug.LogError("Detected duplicate condition : '" + conditionName + "' in class " + mi.DeclaringType.FullName);
                }
                else
                {
                    conditions.Add(conditionName, (Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), mi));
                }
            }
        }

        public static bool IsValid(string condition)
        {
            return conditions.ContainsKey(condition) && conditions[condition]();
        }
    }
}