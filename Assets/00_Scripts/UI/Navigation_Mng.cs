using System.Collections.Generic;
using UnityEngine;

public class Navigation_Mng : MonoBehaviour
{
    public static Navigation_Mng instance = null;

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    [SerializeField] private Transform Content;
    [SerializeField] private int Maximum;
    private Nav_Item[] P_Item;

    private void Start()
    {
        P_Item = GetComponentsInChildren<Nav_Item>(true);
    }
    
    public void PanelGet_Item(Item_Scriptable data, int count)
    {
        MakeItem(0).Init(data, count);
    }

    public void PanelGet_Toast(Scriptable_Base data, string key)
    {
        MakeItem(1).Init_Building(data, key);
    }

    private Nav_Item MakeItem(int value)
    {
        var go = Instantiate(P_Item[value], Content);
        go.transform.SetAsFirstSibling();
        go.gameObject.SetActive(true);
        
        if(Content.childCount > Maximum)
        {
            DestroyImmediate(Content.GetChild(Content.childCount - 1).gameObject); // Destory는 다음 프레임에 오브젝트를 파괴하는 함수
        }

        return go;
    }
}
