using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Asset_Mng 
{
    public static SpriteAtlas atlas = Resources.Load<SpriteAtlas>("Atlas");
    public static Building_Scriptable[] buildings = Resources.LoadAll<Building_Scriptable>("Building");

    public static Sprite Get_Atlas(string temp)
    {
        if(atlas == null)
        {
            Debug.LogError("Atlas 에셋을 찾을 수 없습니다. Resources/Atlas 경로를 확인하세요.");
            return null;
        }
        return atlas.GetSprite(temp);
    }
}
