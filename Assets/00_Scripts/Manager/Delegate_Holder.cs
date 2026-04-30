using UnityEngine;

public delegate void Interaction();
public delegate void Stamina(int value);
public delegate void HP(int hp);
public delegate void OnRainIntensityChanged(float Intensity);
public delegate void OnWindStrengthChanged(float Strength);


public class Delegate_Holder : MonoBehaviour
{
    public static event Interaction OnInteraction;
    public static event Interaction OnInteractionOut;
    public static event Stamina OnStamina;
    public static event HP OnHP;
    public static event OnRainIntensityChanged OnRainIntensityChanged;
    public static event OnWindStrengthChanged OnWindStrengthChanged;


    public static void OnStartInteraction() => OnInteraction?.Invoke();
    public static void OnOutInteraction() => OnInteractionOut?.Invoke();
    public static void OnStaminaChange(int value) => OnStamina?.Invoke(value);
    public static void OnHPChange(int value) => OnHP?.Invoke(value);
    public static void OnRainIntensityChange(float Intensity) => OnRainIntensityChanged?.Invoke(Intensity);
    public static void OnWindStrengthChange(float Strength) => OnWindStrengthChanged?.Invoke(Strength);
}
