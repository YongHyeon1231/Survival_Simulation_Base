using UnityEngine;

[CreateAssetMenu(fileName = "Object_Scriptable", menuName = "Scriptable Objects/Object_Scriptable")]
public class Object_Scriptable : ScriptableObject
{
    public Object_Type m_Type;
    public string Name;
    public int HP;
}
