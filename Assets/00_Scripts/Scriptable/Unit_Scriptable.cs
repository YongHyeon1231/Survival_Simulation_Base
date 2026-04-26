using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit_Scriptable", menuName = "Scriptable Objects/Unit_Scriptable")]
public class Unit_Scriptable : Scriptable_Base
{
    public float timer;

    public List<ITEM> itemList = new List<ITEM>();

    
}
