using System;
using UnityEngine;

public class Wheather_Mng : MonoBehaviour
{
    public Light directionalLight;
    public Vector3 sunRotationOffset;
    public Gradient sunColorGradient;

    [Range(0, 24)] public float currentTime = 12.0f; // 0~24시간
    public float m_TimeSpeed = 60.0f; // 하루를 1분으로 표기

    private void Update()
    {
        UpdateTime();
        RotateSun();
        UpdateSunColor();
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
