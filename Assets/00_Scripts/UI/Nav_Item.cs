using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class Nav_Item : MonoBehaviour
{
    [SerializeField] private Image Rarity_Image;
    [SerializeField] private Image Item_Icon_Image;
    [SerializeField] private TextMeshProUGUI Item_Name_Text;

    public void Init(Item_Scriptable m_Data, int count)
    {
        Rarity_Image.sprite = Asset_Mng.Get_Atlas(m_Data.rarity.ToString());
        Item_Icon_Image.sprite = Asset_Mng.Get_Atlas(m_Data.ItemID.ToString());

        Item_Name_Text.text = Utils.Localization_text(String_Table.Item, m_Data.ItemName) + "x" + count.ToString();
    }
}
