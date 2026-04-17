using System.Collections.Generic;
using UnityEngine;

// 아이템 획득이나 드랍과 관련된 로직을 처리하는 클래스
public class ItemFlowController
{
    public static Dictionary<int, ITEM> Item_Pairs = new Dictionary<int, ITEM>();

    public static List<ITEM> DROPITEMLIST(List<ITEMLIST> m_ItemList)
    {
        List<ITEM> Get_Item_List = new List<ITEM>();

        for (int i = 0; i <m_ItemList.Count; i++)
        {
            float RandomValue = Random.Range(0.0f, 100.0f);
            if (RandomValue <= m_ItemList[i].value)
            {
                int value = Random.Range(1, m_ItemList[i].Maximum);

                Get_Item_List.Add(new ITEM { Data = m_ItemList[i].Item_Data, Count = value });
            }
        }

        return Get_Item_List;
    }

    public static void GETITEM(Item_Scriptable scriptableData, int value)
    {
        ITEM item = new ITEM { Data = scriptableData, Count = value };

        int ID = item.Data.ItemID;

        if(HaveItem(ID))
        {
            Item_Pairs[ID].Count += value;
        }
        else
        {
            Item_Pairs.Add(ID, item);
        }
    }

    public static bool HaveItem(int value)
    {
        if (Item_Pairs.ContainsKey(value))
        {
            return true;
        }
        return false;
    }
}
