using Unity.VisualScripting;
using UnityEngine;

public class Navigation_Mng : MonoBehaviour
{
    public static Navigation_Mng instance = null;

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    [SerializeField] private Transform Content;
    
    private Nav_Item P_Item;

    private void Start()
    {
        P_Item = GetComponentInChildren<Nav_Item>();
        P_Item.gameObject.SetActive(false);
    }
    
    public void PanelGet_Item(Item_Scriptable data)
    {
        var go = Instantiate(P_Item, Content);
        go.gameObject.SetActive(true);
        go.Init(data);
    }
}
