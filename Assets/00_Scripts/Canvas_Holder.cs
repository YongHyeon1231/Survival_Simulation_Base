using System;
using UnityEngine;

public class Canvas_Holder : MonoBehaviour
{
    [SerializeField] private GameObject Board;

    private void Start()
    {
        Delegate_Holder.OnInteraction += GetBoard;
        Delegate_Holder.OnInteractionOut += BoardOut;
    }

    public void GetBoard()
    {
        Board.SetActive(true);
    }

    public void BoardOut() => Board.GetComponent<UI_Animation_Handler>().AnimationChange("Out");
}
