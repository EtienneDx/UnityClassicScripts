using System.Collections.Generic;
using UnityEditor;

namespace UtilsNS
{
    public abstract class TweakableEditor : Editor
    {
       public virtual List<string> ExcludedProperties { get { return new List<string>(); } }

        public override void OnInspectorGUI()
        {
            OnBeforeDefault();

            DrawPropertiesExcluding(serializedObject, ExcludedProperties.ToArray());

            OnAfterDefault();

            serializedObject.ApplyModifiedProperties();
        }

        internal virtual void OnBeforeDefault() { }

        internal virtual void OnAfterDefault() { }
    }
}
