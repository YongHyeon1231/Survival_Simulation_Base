using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_Holder : MonoBehaviour
{
    public static Canvas_Holder instance = null;

    [SerializeField] private Transform UI_PART_PARENT;
    [SerializeField] private GameObject Board;
    public Image BoardHpFill, BoardHpWhiteFill;
    [SerializeField] private TextMeshProUGUI StaminaText;
    [SerializeField] private TextMeshProUGUI HPText;
    [SerializeField] private Image StaminaFill;
    [SerializeField] private Image HPFill;
    Coroutine F_Coroutine;

    private Dictionary<string, UIPART> uiParts = new Dictionary<string, UIPART>();
    private Dictionary<Monster, Directional_Monster_Slider> monsterSliders = new Dictionary<Monster, Directional_Monster_Slider>();
    public static Queue<UIPART> Uis = new Queue<UIPART>();
    PopUP_Description popup;
    public Directional_Monster_Slider monster_Slider;


    private void Awake()
    {
        if(instance == null) instance = this;
    }

    public UIPART GetUIPART(string name)
    {
        if(uiParts.ContainsKey(name))
        {
            return uiParts[name];
        }
        var uiPart = Instantiate(Resources.Load<UIPART>("UI/" + name), UI_PART_PARENT);
        uiParts.Add(name, uiPart);
        uiPart.gameObject.SetActive(false);
        return uiPart;
    }

    public void DestroyPopup()
    {
        if(popup != null) Destroy(popup.gameObject);
    }

    public PopUP_Description GetPopUp()
    {
        DestroyPopup();

        popup = Instantiate(Resources.Load<PopUP_Description>("Prefab/PopUp"), transform);

        return popup;
    }

    private void Start()
    {
        UIPART[] parts = UI_PART_PARENT.GetComponentsInChildren<UIPART>(true); // 비활성화된 자식도 포함하여 모든 UIPART 컴포넌트를 가져옴
        foreach (var part in parts)
        {
            uiParts.Add(part.name, part);
        }

        Delegate_Holder.OnInteractionOut += BoardOut;
        Delegate_Holder.OnStamina += StaminaCheck;
        Delegate_Holder.OnHP += HPCheck;
    }

    private void Update()
    {
        CheckSlider();
        CheckUI(KeyCode.I, "INVENTORY");
        CheckUI(KeyCode.B, "BUILDING");
    }

    public void AddSlider(Monster monster)
    {
        if (monsterSliders.ContainsKey(monster))
        {
            monsterSliders[monster].GetSliderCheck();
        }
        else
        {
            var go = Instantiate(monster_Slider, transform);
            go.monster = monster;
            monsterSliders.Add(monster, go);
            monsterSliders[monster].GetSliderCheck();
        }
    }

    public void RemoveSlider(Monster monster)
    {
        monsterSliders[monster].GetComponent<Animator>().SetTrigger("Out");
        monsterSliders.Remove(monster);
    }

    private void CheckSlider()
    {
        foreach(var slider in monsterSliders)
        {
            Vector3 pos = slider.Key.transform.position;
            pos.y += 2.0f;
            slider.Value.transform.position = Camera.main.WorldToScreenPoint(pos);
            // slider.Value.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(pos);
        }
    }

    public void GetText(string temp, Color color, Vector3 posReal)
    {
        // Vector3 posReal = P_Movement.instance.transform.position;
        posReal.y += 0.5f;
        posReal.x += Random.Range(-0.5f, 0.5f);
        posReal.z += Random.Range(-0.5f, 0.5f);

        var go = Instantiate(Resources.Load<GameObject>("TextObject"), posReal, Quaternion.Euler(55, 0, 0));
        TextMeshPro textObj = go.GetComponent<TextMeshPro>();
        textObj.color = color;
        textObj.text = temp;
    }

    private void HPCheck(int value)
    {
        Character character = P_Movement.instance.GetComponent<Character>();
        HPText.text = character.HP.ToString() + "/" + character.MaxHP.ToString();
        HPFill.fillAmount = (float)character.HP / (float)character.MaxHP;
    }

    private void StaminaCheck(int value)
    {
        StaminaText.text = Base_Mng.Game.Stamina + "/" + Base_Mng.Game.MaxStamina;
        StaminaFill.fillAmount = (float)Base_Mng.Game.Stamina / (float)Base_Mng.Game.MaxStamina;
    }

    private void CheckUI(KeyCode key, string uiName)
    {
        if(Input.GetKeyDown(key))
        {
            P_Movement.instance.ReturnCharacterMove();

            CloseAllUI(uiName);
            DestroyPopup();

            uiParts[uiName].Toggle();
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

    public void CloseAllUI(string name = "")
    {
        foreach(var part in uiParts)
        {
            if(part.Key != name)
            {
                part.Value.Close();
            }
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
