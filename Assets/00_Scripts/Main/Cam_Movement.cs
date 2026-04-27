using System.Collections;
using UnityEngine;

public class Cam_Movement : MonoBehaviour
{
    public static Cam_Movement instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private Transform player;

    [SerializeField] private float PosX = 0.0f;
    [SerializeField] private float PosY = 15.0f;
    [SerializeField] private float PosZ = -10.0f;

    [SerializeField] private float m_Speed = 2.0f;

    [Header("## Camera Shake")]
    [SerializeField] private float Duration;
    [SerializeField] private float Power;
    Vector3 OriginalPos;
    bool isCameraShake = false;

    private Vector3 shakeOffset;


    private void Start()
    {
        player = P_Movement.instance.transform;
    }

    private void LateUpdate()
    {
        // if(isCameraShake) return; //카메라가 흔들리는 동안에는 카메라가 따라가지 않음.

        Move();
        transform.position += shakeOffset;
    }

    private void Move()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(
            player.position.x + PosX,
            player.position.y + PosY,
            player.position.z + PosZ
            ), m_Speed * Time.deltaTime);

            // transform.LookAt(player);
    }

    public void CameraShake()
    {
        if(isCameraShake) return;

        isCameraShake = true;
        StartCoroutine(CameraShake_Coroutine());
    }

    // IEnumerator CameraShake_Coroutine() 
    // {
    //     OriginalPos = transform.localPosition;
    //     float timer = 0.0f;
    //     while (timer <= Duration)
    //     {
    //         transform.localPosition = Random.insideUnitSphere * Power + OriginalPos;

    //         timer += Time.deltaTime;
    //         yield return null;
    //     }

    //     transform.localPosition = OriginalPos;
    //     isCameraShake = false;
    // }

    IEnumerator CameraShake_Coroutine() 
    {
        float timer = 0.0f;
        while (timer <= Duration)
        {
            shakeOffset = Random.insideUnitCircle * Power;

            timer += Time.deltaTime;
            yield return null;
        }

        shakeOffset = Vector3.zero;
        isCameraShake = false;
    }
}
