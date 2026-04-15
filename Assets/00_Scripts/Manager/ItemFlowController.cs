using System.Collections.Generic;
using UnityEngine;

public class ItemFlowController
{
    public static List<Item_Scriptable> DROPITEMLIST(List<ITEMLIST> m_ItemList)
    {
        List<Item_Scriptable> Get_Item_List = new List<Item_Scriptable>();

        for (int i = 0; i <m_ItemList.Count; i++)
        {
            float RandomValue = Random.Range(0.0f, 100.0f);
            if (RandomValue <= m_ItemList[i].value)
            {
                Get_Item_List.Add(m_ItemList[i].Item_Data);
            }
        }

        return Get_Item_List;
    }
}
