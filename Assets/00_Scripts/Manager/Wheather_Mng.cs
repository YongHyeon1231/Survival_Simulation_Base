using System;
using UnityEngine;

public class Wheather_Mng : MonoBehaviour
{
    [Header("## Sun And Night")]
    public Light directionalLight;
    public Vector3 sunRotationOffset;
    public Gradient sunColorGradient;

    [Range(0, 24)] public float currentTime = 12.0f; // 0~24시간
    public float m_TimeSpeed = 60.0f; // 하루를 1분으로 표기

    [Space(20f)]
    [Header("## Rain")]
    public ParticleSystem rainParticleSystem;
    public float minEmissionRate;
    public float maxEmissionRate;
    private ParticleSystem.EmissionModule emissionModule;

    [Space(20f)]
    [Header("## Wind")]
    public Material windMaterial;
    public float minWindStrength = 0.25f;
    public float maxWindStrength = 1.0f;

    private void Start()
    {
        emissionModule = rainParticleSystem.emission;
        Delegate_Holder.OnRainIntensityChanged += UpdateRainEmission;
        Delegate_Holder.OnWindStrengthChanged += UpdateWindStrength;
    }

    private void Update()
    {
        UpdateTime();
        RotateSun();
        UpdateSunColor();


        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Delegate_Holder.OnRainIntensityChange(0.1f);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            Delegate_Holder.OnRainIntensityChange(0.5f);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            Delegate_Holder.OnRainIntensityChange(1.0f);
        }
        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            Delegate_Holder.OnWindStrengthChange(0.1f);
        }
        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            Delegate_Holder.OnWindStrengthChange(0.5f);
        }
        if(Input.GetKeyDown(KeyCode.Alpha6))
        {
            Delegate_Holder.OnWindStrengthChange(1.0f);
        }
    }
    
    public void UpdateWindStrength(float strength)
    {
        windMaterial.SetFloat("_Wind_Strength", Mathf.Lerp(minWindStrength, maxWindStrength, strength));
    }

    public void UpdateRainEmission(float intensity)
    {
        float emissionRate = Mathf.Lerp(minEmissionRate, maxEmissionRate, intensity);
        emissionModule.rateOverTime = emissionRate;
    }

    private void UpdateTime()
    {
        float timeSpeed = 24.0f / m_TimeSpeed;
        currentTime += Time.deltaTime * timeSpeed;
        if(currentTime >= 24.0f)
        {
            currentTime = 0.0f;
        }
    }

    private void RotateSun()
    {
        float timePercent = currentTime / 24.0f;
        float sunXRotation = Mathf.Lerp(-90.0f, 270.0f, timePercent);
        // float sunYRotation = Mathf.Lerp(-45.0f, 45.0f, Mathf.Sin(timePercent * Mathf.PI));
        float sunYRotation = Mathf.Lerp(-90.0f, 90.0f, timePercent);

        directionalLight.transform.rotation = Quaternion.Euler(
            sunXRotation + sunRotationOffset.x, 
            sunYRotation + sunRotationOffset.y, 
            sunRotationOffset.z);
    }

    private void UpdateSunColor()
    {
        float timePercent = currentTime / 24.0f;
        directionalLight.color = sunColorGradient.Evaluate(timePercent); // Gradient.Evaluate(float t)는 그라디언트에서 t(0~1) 위치의 색상을 반환하는 함수
    }
}
