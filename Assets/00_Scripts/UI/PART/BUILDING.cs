using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BUILDING : UIPART
{
    public Building_Panel BuildingPanel;
    public Transform Content;

    private List<Building_Panel> building_list =  new List<Building_Panel>();
    private List<GameObject> Garvage = new List<GameObject>();

    public GameObject ItemClickTap;

    private Animator animator;
    public bool GetClick = false;

    [SerializeField] private GameObject Building_Item;
    [SerializeField] private Transform Item_Content;
    [SerializeField] private TextMeshProUGUI TimerText;
    
    private Building_Scriptable BuildingObj;

    private void Awake()
    {
        Init();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        GetClick = false;
        SetBuilding();
    }

    public void SetBuildObject()
    {
        bool CanBuild = true;
        for(int i = 0; i <BuildingObj.m_Items.Count; i++)
        {
            ITEM item = BuildingObj.m_Items[i];
            string itemKey = item.Data.Key;
            int inventoryItemCount = ItemFlowController.ItemCount(itemKey);
            if (inventoryItemCount < item.Count) // 인벤토리 아이템 수량 < 필요 아이템 수량
            {
                CanBuild = false;
                break;
            }
        }

        if(CanBuild == false) return;

        for (int i = 0; i <BuildingObj.m_Items.Count; i++)
        {
            ITEM item = BuildingObj.m_Items[i];
            ItemFlowController.REMOVEITEM(item.Data.Key, item.Count);
        }

        Close();

        Base_Mng.Build.SetBuild(BuildingObj);

    }

    // GetComponentInChildren 자식 오브젝트의 특정 컴포넌트를 추적
    public void GetItemsData(Building_Scriptable data)
    {
        BuildingObj = data;

        if(Garvage.Count > 0)
        {
            for(int i = 0; i < Garvage.Count; i++)
            {
                Destroy(Garvage[i]);
            }
            Garvage.Clear();
        }

        for (int i = 0; i < data.m_Items.Count; i++)
        {
            Item_Scriptable itemData = data.m_Items[i].Data;
            var go = Instantiate(Building_Item, Item_Content);
            go.transform.GetComponentInChildren<Image>().sprite = Asset_Mng.Get_Atlas(itemData.Key);
            
            var goText = go.transform.GetComponentInChildren<TextMeshProUGUI>();
            
            goText.text = 
                string.Format("({0}/{1})", 
                data.m_Items[i].Count,
                ItemFlowController.ItemCount(itemData.Key));

            bool MoreItem = ItemFlowController.ItemCount(itemData.Key) >= data.m_Items[i].Count;

            goText.color = MoreItem ? Color.green : Color.red;

            go.gameObject.SetActive(true);

            Garvage.Add(go);
        }

        TimerText.text = Utils.Timer(data.timer);
    }

    public void AnimationChange(string temp)
    {
        animator.SetTrigger(temp);
    }

    private void Init()
    {
        var buildings = Asset_Mng.buildings;

        for(int i = 0; i < buildings.Length; i++)
        {
            var go = Instantiate(BuildingPanel, Content);
            go.Init(buildings[i], this);
            building_list.Add(go);
        }
    }

    public void SetItemClickAnimation(Building_Panel panel)
    {
        ItemClickTap.gameObject.SetActive(true);
        ItemClickTap.transform.SetParent(panel.transform);
        ItemClickTap.transform.localPosition = Vector2.zero;
    }

    private void SetBuilding()
    {
        StartCoroutine(GetOpenCoroutine());
    }


    IEnumerator GetOpenCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < building_list.Count; i++)
        {
            building_list[i].SetData();
            yield return new WaitForSeconds(0.02f);
        }
    }

    private void OnDisable()
    {
        for(int i = 0; i < building_list.Count; i++) building_list[i].gameObject.SetActive(false);
    }
}
