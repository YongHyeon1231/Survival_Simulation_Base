using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class Item_Panel : MonoBehaviour
{
    public ITEM m_Item;
    public GameObject m_ITEMPANEL;
    public Image Rarity;
    public Image Item_Icon;
    public TextMeshProUGUI ItemCountText;
    public TextMeshProUGUI ItemWeightText;

    public void Init(ITEM item)
    {
        this.m_Item = item;
    }

    public void SetItem()
    {
        m_ITEMPANEL.SetActive(m_Item.Data == null ? false : true);
        if(m_Item.Data != null)
        {
            Rarity.sprite = Asset_Mng.Get_Atlas(m_Item.Data.rarity.ToString());
            Item_Icon.sprite = Asset_Mng.Get_Atlas(m_Item.Data.ItemID.ToString());
            ItemCountText.text = m_Item.Count.ToString();
        }
        else
        {
            Rarity.sprite = Asset_Mng.Get_Atlas("DefaultSquare");
        }
    }
}
