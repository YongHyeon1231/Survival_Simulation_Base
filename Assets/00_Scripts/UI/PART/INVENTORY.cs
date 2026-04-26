using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class INVENTORY : UIPART
{
    public Item_Panel Item_Panel;
    public Transform Content;

    public Image WeightFill;
    public TextMeshProUGUI weightText;

    List<Item_Panel> items = new List<Item_Panel>();
    Dictionary<string, ITEM> Inventory_Items = new Dictionary<string, ITEM>();
    int ItemMaximumValue = 50;

    public GameObject ItemClickTap;

    private void Start()
    {
        Init();
        ItemFlowController.OnItemGet += SetItemList;
        ItemFlowController.OnItemGet += SetInventory;
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
        SetItemList();
        SetInventory();
    }

    // public void SetItemList()
    // {
    //     int value = 0;
    //     foreach (var item in ItemFlowController.Item_Pairs)
    //     {
    //         if(Inventory_Items.ContainsKey(item.Value.Data.Key) == false && items[value].parentPanel == null)
    //         {
    //             items[value].Init(item.Value, this);
    //             Inventory_Items.Add(item.Value.Data.Key, item.Value);
    //         }
    //         value++;
    //     }
    // }

    public void SetItemList()
    {
        foreach (var panel in items) panel.Clear();
        Inventory_Items.Clear();

        int index = 0;
        foreach(var pair in ItemFlowController.Item_Pairs)
        {
            items[index].Init(pair.Value, this);
            Inventory_Items.Add(pair.Key, pair.Value);
            index++;
        }
    }

    public void SetInventory()
    {
        for(int i = 0; i < items.Count; i++)
        {
            items[i].SetItem();
        }

        WeightFill.fillAmount = ItemFlowController.Weight() / ItemFlowController.Player_Weight;
        weightText.text = string.Format("({0:0.0}/{1:0.0})", ItemFlowController.Weight(), ItemFlowController.Player_Weight);
    }

    public void SetItemClickAnimation(Item_Panel panel)
    {
        ItemClickTap.gameObject.SetActive(true);
        ItemClickTap.transform.SetParent(panel.transform);
        ItemClickTap.transform.localPosition = Vector2.zero;
    }
}
