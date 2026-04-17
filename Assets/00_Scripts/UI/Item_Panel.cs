using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item_Panel : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    public ITEM m_Item;
    public GameObject m_ITEMPANEL;
    public Image Rarity;
    public Image Item_Icon;
    public TextMeshProUGUI ItemCountText;
    public TextMeshProUGUI ItemWeightText;
    public INVENTORY parentPanel;

    public void Init(ITEM item, INVENTORY inventory)
    {
        this.m_Item = item;
        this.parentPanel = inventory;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(parentPanel == null) return;
        parentPanel.SetItemClickAnimation(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(parentPanel == null) return;
        if(parentPanel.ItemClickTap.activeSelf == true)
        {
            parentPanel.ItemClickTap.gameObject.SetActive(false);
        }
    }

    public void SetItem()
    {
        m_ITEMPANEL.SetActive(m_Item.Data == null ? false : true);
        if(m_Item.Data != null)
        {
            Rarity.sprite = Asset_Mng.Get_Atlas(m_Item.Data.rarity.ToString());
            Item_Icon.sprite = Asset_Mng.Get_Atlas(m_Item.Data.ItemID.ToString());
            ItemCountText.text = m_Item.Count.ToString();
            ItemWeightText.text = string.Format("{0:0.0}", ItemFlowController.WeightItem(m_Item.Data.ItemID));
        }
        else
        {
            Rarity.sprite = Asset_Mng.Get_Atlas("DefaultSquare");
        }
    }
}
