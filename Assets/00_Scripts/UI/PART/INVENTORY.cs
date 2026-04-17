using System.Collections.Generic;
using UnityEngine;

public class INVENTORY : UIPART
{
    public Item_Panel Item_Panel;
    public Transform Content;

    List<Item_Panel> items = new List<Item_Panel>();
    int ItemMaximumValue = 50;

    private void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        SetInventory();
    }

    public void Init()
    {
        if(ItemFlowController.Item_Pairs.Count >= ItemMaximumValue)
        {
            ItemMaximumValue = ItemFlowController.Item_Pairs.Count;
        }

        for(int i = 0; i < ItemMaximumValue; i++)
        {
            var go = Instantiate(Item_Panel, Content);
            go.gameObject.SetActive(true);
            items.Add(go);
        }

        int value = 0;
        foreach (var item in ItemFlowController.Item_Pairs)
        {
            items[value].Init(item.Value);
            value++;
        }
        SetInventory();
    }

    public void SetInventory()
    {
        for(int i = 0; i < items.Count; i++)
        {
            items[i].SetItem();
        }
    }
}
