
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUP_Description : MonoBehaviour
{
    RectTransform rect;
    [SerializeField] private Image IconImage;
    [SerializeField] private TextMeshProUGUI TitleText;
    [SerializeField] private TextMeshProUGUI ExplaneText;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    // private void Update()
    // {
    //     if(Input.GetMouseButtonDown(0))
    //     {
    //         Canvas_Holder.instance.DestroyPopup();
    //     }
    // }

    public void Set_PopUp(String_Table type, string key, Vector2 pos)
    {
        rect.pivot = PivotPoint(pos);

        rect.anchoredPosition = pos;

        IconImage.sprite = Asset_Mng.Get_Atlas(key);
        TitleText.text = Utils.Localization_text(type, key);
        ExplaneText.text= Utils.Localization_text(type, key + ".Value");
    }

    private Vector2 PivotPoint(Vector2 pos)
    {
        float xPos = pos.x > Screen.width / 2 ? 1.0f : 0.0f;
        float yPos = pos.y > Screen.height / 2 ? 1.0f : 0.0f;

        return new Vector2(xPos, yPos);
    }
}
