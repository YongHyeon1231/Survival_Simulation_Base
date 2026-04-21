using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Asset_Mng 
{
    public static SpriteAtlas atlas = Resources.Load<SpriteAtlas>("Atlas");
    public static Building_Scriptable[] buildings = Resources.LoadAll<Building_Scriptable>("Building");

    public static Sprite Get_Atlas(string temp)
    {
        return atlas.GetSprite(temp);
    }
}
