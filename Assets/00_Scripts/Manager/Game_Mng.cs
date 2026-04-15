using UnityEngine;

public class Game_Mng : MonoBehaviour
{
    public static Game_Mng instance = null;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    
}
