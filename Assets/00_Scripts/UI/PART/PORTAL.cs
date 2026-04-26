using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PORTAL : UIPART
{
     public Unit_Panel[] panels;
    private Portal m_Portal;
    [SerializeField] private Image MainIcon;
    [SerializeField] private TextMeshProUGUI MainSpeech;
    [SerializeField] private TextMeshProUGUI MainName;

    [SerializeField] private GameObject Panel;
    [SerializeField] private Transform Content;

    List<GameObject> Garvage = new List<GameObject>();
    Unit_Scriptable Data;

     private void Start()
    {
        for(int i = 0; i < panels.Length; i++)
        {
            panels[i].Init(this);
        }
    }

    public void Init(Portal portal)
    {
        m_Portal = portal;
    }

    public void SetBuildObject()
    {
        bool CanBuild = true;
        for(int i = 0; i <Data.itemList.Count; i++)
        {
            ITEM item = Data.itemList[i];
            string itemKey = item.Data.Key;
            int inventoryItemCount = ItemFlowController.ItemCount(itemKey);
            if (inventoryItemCount < item.Count) // 인벤토리 아이템 수량 < 필요 아이템 수량
            {
                CanBuild = false;
                break;
            }
        }
        
        Portal portal = m_Portal;

        if(CanBuild == false) return;

        for (int i = 0; i < Data.itemList.Count; i++)
            ItemFlowController.REMOVEITEM(Data.itemList[i].Data.Key, Data.itemList[i].Count);

        Close();
        portal.GetComponent<Building_OBJ>().SetMakeData(Data.Key, Data.timer, () => portal.GetWorker());
        // 생성하기
    }

    private void MainSetActive(bool isActive)
    {
        MainIcon.gameObject.SetActive(isActive);
        MainName.gameObject.SetActive(isActive);
        MainSpeech.gameObject.SetActive(isActive);
    }

    public override void Close()
    {
        Delegate_Holder.OnOutInteraction();
        base.Close();
    }

    public void SetData(Unit_Scriptable m_Data, Unit_Panel panel)
    {
        Data = m_Data;

        for(int i = 0; i <panels.Length; i++) panels[i].transform.GetChild(0).gameObject.SetActive(false);

        panel.transform.GetChild(0).gameObject.SetActive(true);

        if(Garvage.Count > 0)
        {
            for(int i = 0; i < Garvage.Count; i++)
            {
                Destroy(Garvage[i]);
            }
            Garvage.Clear();
        }

        MainSetActive(true);

        MainIcon.sprite = Asset_Mng.Get_Atlas(m_Data.Key);
        MainName.text = Utils.Localization_text(String_Table.Unit, m_Data.Key);
        MainSpeech.text = Utils.Localization_text(String_Table.Unit, m_Data.Key + "_Speech_Value");

        for(int i = 0; i < m_Data.itemList.Count; i++)
        {
            Item_Scriptable itemData = m_Data.itemList[i].Data;
            var go = Instantiate(Panel, Content);
            go.SetActive(true);

            Utils.FindBase<Image>(go.transform, "Icon").sprite = Asset_Mng.Get_Atlas(itemData.Key);
            Utils.FindBase<TextMeshProUGUI>(go.transform, "Title").text = Utils.Localization_text(String_Table.Item, itemData.Key);

            var goText = Utils.FindBase<TextMeshProUGUI>(go.transform, "Count");
            goText.text = string.Format("({0}/{1})", m_Data.itemList[i].Count, ItemFlowController.ItemCount(itemData.Key));
            goText.color = ItemFlowController.ItemCount(itemData.Key) >= m_Data.itemList[i].Count ? Color.green : Color.red;

            Garvage.Add(go);
        }
    }
}
