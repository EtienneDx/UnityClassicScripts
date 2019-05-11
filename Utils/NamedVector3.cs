using UnityEngine;

[System.Serializable]
public struct NamedVector3
{
    public string name;
    [ClickableVector]
    public Vector3 vector;
}