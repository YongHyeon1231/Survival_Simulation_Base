using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building_Scriptable", menuName = "Scriptable Objects/Building_Scriptable")]
public class Building_Scriptable : ScriptableObject
{
    public string Name;
    public float timer;
    public List<ITEM> m_Items = new List<ITEM>();

    public Building_OBJ obj;
}
