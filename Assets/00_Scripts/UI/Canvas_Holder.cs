using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_Holder : MonoBehaviour
{
    public static Canvas_Holder instance = null;

    [SerializeField] private Transform UI_PART_PARENT;
    [SerializeField] private GameObject Board;
    public Image BoardHpFill, BoardHpWhiteFill;
    Coroutine F_Coroutine;

    private Dictionary<string, UIPART> uiParts = new Dictionary<string, UIPART>();

    private void Awake()
    {
        if(instance == null) instance = this;
    }

    private void Start()
    {
        UIPART[] parts = UI_PART_PARENT.GetComponentsInChildren<UIPART>(true); // 비활성화된 자식도 포함하여 모든 UIPART 컴포넌트를 가져옴
        foreach (var part in parts)
        {
            uiParts.Add(part.name, part);
        }

        Delegate_Holder.OnInteraction += GetBoard;
        Delegate_Holder.OnInteractionOut += BoardOut;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            uiParts["INVENTORY"].Toggle();
        }
    }

    public void OpenUI(string uiName)
    {
        if (uiParts.ContainsKey(uiName))
        {
            uiParts[uiName].Open();
        }
        else
        {
            Debug.LogWarning($"UI Part '{uiName}' not found.");
        }
    }

    public void CloseUI(string uiName)
    {
        if(uiParts.ContainsKey(uiName))
        {
            uiParts[uiName].Close();
        }
    }

    public void CloseAllUI()
    {
        foreach(var part in uiParts.Values)
        {
            part.Close();
        }
    }

    public void GetBoard()
    {
        Board.SetActive(true);
        // BoardHpFill.fillAmount = 1.0f;
        // BoardHpWhiteFill.fillAmount = 1.0f;
        // 해당 코드가 있으면 오브젝트에 접근할때 남아있는 체력바가 처음에 최대치로 채워져 있는 것처럼 보임.
    }

    public void BoardOut() => Board.GetComponent<UI_Animation_Handler>().AnimationChange("Out");

    public void AllStopCoroutine() => StopAllCoroutines();

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
