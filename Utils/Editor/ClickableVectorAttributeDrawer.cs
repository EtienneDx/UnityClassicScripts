using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ClickableVectorAttribute))]
public class ClickableVectorAttributeDrawer : PropertyDrawer
{
    private SerializedProperty pr;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.Vector3)
            Debug.LogError("ClickableVectorAttribute only supports Vector3 vectors");
        Rect buttonPos = new Rect(position);
        buttonPos.width *= 0.25f;
        position.width *= 0.75f;
        buttonPos.x += position.width;
        if (GUI.Button(buttonPos, "Choose On Scene"))
        {
            Debug.Log("Click on the scene to choose the Vector3 value");
            pr = property;
            SceneClickUtils.ClearNextClick();
            SceneClickUtils.nextClick += updateVector3;
        }
        EditorGUI.PropertyField(position, property, label);
    }

    public void updateVector3(Vector3 v)
    {
        Debug.Log("changed value");
        pr.vector3Value = v;
        pr.serializedObject.ApplyModifiedProperties();
        
    }
}
