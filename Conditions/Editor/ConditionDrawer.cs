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
                (arrSize + 4) * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
            //return base.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.LabelField(GetRect(ref position), label, EditorStyles.boldLabel);

            EditorGUI.PropertyField(GetRect(ref position), property.FindPropertyRelative("mode"), new GUIContent("Mode"));

            SerializedProperty subCond = property.FindPropertyRelative("subConditions");
            EditorGUI.PropertyField(GetRect(ref position, EditorGUI.GetPropertyHeight(subCond)), subCond, true);

            EditorGUI.LabelField(GetRect(ref position), "Conditions");

            position = EditorGUI.IndentedRect(position);

            SerializedProperty cond = property.FindPropertyRelative("conditions");

            string[] possibleConditions = ConditionList.PossibleConditions;

            for (int i = 0; i < cond.arraySize; i++)
            {
                SerializedProperty prop = cond.GetArrayElementAtIndex(i);

                Rect[] rects = GetControl(GetRect(ref position), 20, 5, 75);

                if (possibleConditions.Any())
                {

                    int optionId = Array.IndexOf(possibleConditions, prop.stringValue);
                    if (optionId < 0)
                        optionId = 0;
                    optionId = EditorGUI.Popup(rects[2], optionId, possibleConditions);
                    prop.stringValue = possibleConditions[optionId];
                }
                else
                {
                    EditorGUI.LabelField(rects[2], "No existing condition");
                }

                if(GUI.Button(rects[0], "X"))
                {
                    cond.DeleteArrayElementAtIndex(i--);
                }
            }

            if(GUI.Button(GetRect(ref position), "Add condition"))
            {
                cond.arraySize++;
            }
        }

        private Rect GetRect(ref Rect position, float height = -1f)
        {
            if (height < 0)
                height = EditorGUIUtility.singleLineHeight;
            Rect ret = new Rect(position.position, new Vector2(position.width, height));
            position.y += height + EditorGUIUtility.standardVerticalSpacing;
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
