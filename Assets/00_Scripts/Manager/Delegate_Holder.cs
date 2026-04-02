using UnityEngine;

public delegate void Interaction();

public class Delegate_Holder : MonoBehaviour
{
    public static event Interaction OnInteraction;
    public static event Interaction OnInteractionOut;

    public static void OnStartInteraction() => OnInteraction?.Invoke();
    public static void OnOutInteraction() => OnInteractionOut?.Invoke();
}
