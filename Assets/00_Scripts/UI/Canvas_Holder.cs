using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_Holder : MonoBehaviour
{
    public static Canvas_Holder instance = null;

    [SerializeField] private GameObject Board;
    [SerializeField] private Image BoardHpFill, BoardHpWhiteFill;
    Coroutine F_Coroutine;

    private void Awake()
    {
        if(instance == null) instance = this;
    }

    private void Start()
    {
        Delegate_Holder.OnInteraction += GetBoard;
        Delegate_Holder.OnInteractionOut += BoardOut;
    }

    public void GetBoard()
    {
        Board.SetActive(true);
        // BoardHpFill.fillAmount = 1.0f;
        // BoardHpWhiteFill.fillAmount = 1.0f;
        // 해당 코드가 있으면 오브젝트에 접근할때 남아있는 체력바가 처음에 최대치로 채워져 있는 것처럼 보임.
    }

    public void BoardOut() => Board.GetComponent<UI_Animation_Handler>().AnimationChange("Out");

    public void BoardFill(float hp, float maxHp)
    {
        BoardHpFill.fillAmount = hp / maxHp;
        if (F_Coroutine != null) // 기존 애니메이션 중지 후 새로 시작
        {
            StopCoroutine(F_Coroutine);
        }
        F_Coroutine = StartCoroutine(FillCoroutine());
    }

    IEnumerator FillCoroutine()
    {
        while(BoardHpWhiteFill.fillAmount - BoardHpFill.fillAmount > 0.001f)
        {
            BoardHpWhiteFill.fillAmount = 
            Mathf.Lerp(BoardHpWhiteFill.fillAmount,
                        BoardHpFill.fillAmount, 
                        Time.deltaTime * 2.0f);

            yield return null;
        }

        BoardHpWhiteFill.fillAmount = BoardHpFill.fillAmount; // 최종적으로 정확히 일치하도록 설정
    }
}
