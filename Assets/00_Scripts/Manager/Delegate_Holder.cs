using UnityEngine;

public delegate void Interaction();
public delegate void Stamina(int value);

public class Delegate_Holder : MonoBehaviour
{
    public static event Interaction OnInteraction;
    public static event Interaction OnInteractionOut;
    public static event Stamina OnStamina;

    public static void OnStartInteraction() => OnInteraction?.Invoke();
    public static void OnOutInteraction() => OnInteractionOut?.Invoke();
    public static void OnStaminaChange(int value) => OnStamina?.Invoke(value);
}
