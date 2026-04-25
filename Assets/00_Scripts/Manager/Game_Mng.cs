using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class Game_Mng : MonoBehaviour
{
    public int Stamina, MaxStamina;

    private void Start()
    {
        Stamina = MaxStamina;
        StartCoroutine(DelayStaina());
    }

    IEnumerator DelayStaina()
    {
        yield return new WaitForSeconds(0.02f);
        SetStamina(0, false);
    }

    public void SetStamina(int value, bool GetText = true)
    {
        Stamina += value;
        if (GetText)
        {
            Color color = value > 0 ? Color.green : Color.red;
            Canvas_Holder.instance.GetText(value.ToString(), color);
        }
        Delegate_Holder.OnStaminaChange(value);
    }
}
