using UnityEngine;

public class Base_Mng : MonoBehaviour
{
    public static Base_Mng instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        Build = GetComponentInChildren<Building_Mng>();
        Game = GetComponentInChildren<Game_Mng>();
    }

    [HideInInspector] public Building_Mng Build;
    [HideInInspector] public Game_Mng Game;
}
