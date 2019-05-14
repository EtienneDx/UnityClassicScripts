using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EtienneDx.Conditions
{
    [CustomPropertyDrawer(typeof(Condition))]
    public class ConditionDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int arrSize = property.FindPropertyRelative("conditions").arraySize;
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("subConditions")) +
                (arrSize + 2) * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
            //return base.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.LabelField(GetRect(ref position), label, EditorStyles.boldLabel);

            SerializedProperty subCond = property.FindPropertyRelative("subConditions");
            EditorGUI.PropertyField(GetRect(ref position, EditorGUI.GetPropertyHeight(subCond)), subCond);

            SerializedProperty cond = property.FindPropertyRelative("conditions");

            string[] possibleConditions = ConditionList.PossibleConditions;

            for (int i = 0; i < cond.arraySize; i++)
            {
                SerializedProperty prop = cond.GetArrayElementAtIndex(i);

                Rect[] rects = GetControl(GetRect(ref position), 20, 5, 75);

                int optionId = Array.IndexOf(possibleConditions, prop.stringValue);
                optionId = EditorGUI.Popup(rects[2], optionId, possibleConditions);
                prop.stringValue = possibleConditions[optionId];

                if(GUI.Button(rects[0], "X"))
                {
                    // TODO: remove element(copy values and reduce size)
                }
            }
        }

        private Rect GetRect(ref Rect position, float height = -1f)
        {
            if (height < 0)
                height = EditorGUIUtility.singleLineHeight;
            Rect ret = new Rect(position.position, new Vector2(position.width, height));
            position.x += height + EditorGUIUtility.standardVerticalSpacing;
            return ret;
        }

        private Rect[] GetControl(Rect original, params float[] sizes)
        {
            float tot = sizes.Sum();

            Rect[] ret = new Rect[sizes.Length];
            float offset = 0;
            for (int i = 0; i < sizes.Length; i++)
            {
                ret[i] = new Rect(original.x + offset, original.y, sizes[i] * original.width / tot, original.height);
                offset += sizes[i] * original.width / tot;
            }
            return ret;
        }
    }
}
