using UnityEngine;

public delegate void Interaction();

public class Delegate_Holder : MonoBehaviour
{
    public static event Interaction OnInteraction;

    public static void OnStartInteraction() => OnInteraction?.Invoke();
}
