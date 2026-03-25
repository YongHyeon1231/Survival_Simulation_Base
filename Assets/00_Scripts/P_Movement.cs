using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class P_Movement : MonoBehaviour
{
    [Header("#Movement Settings")]
    public float moveSpeed = 5.0f;

    [Space(20f)]

    [Header("#Mouse Rotation")]
    public LayerMask groundLayer;
    public float rotationSpeed = 10.0f;

    private CharacterController controller;
    private Animator animator;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        RotateTowardsMouse();
    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal"); //A/D or Left/Right
        float vertical = Input.GetAxis("Vertical"); //W/S or Up/Down

        Vector3 cameraForward = Camera.main.transform.forward; //forward는 Inspector에 따로 있는 값이 아니라, Rotation으로부터 계산되는 방향 벡터다
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0.0f;
        cameraRight.y = 0.0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = cameraForward * vertical + cameraRight * horizontal;

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        float currentSpeed = moveDirection.magnitude * moveSpeed;
        animator.SetFloat("a_Speed", currentSpeed);
    }

    void RotateTowardsMouse() // 마우스 위치로 캐릭터를 회전시키는 함수
    {
        //카메라로부터 마우스 위치로 광선을 쏜다. 여기서 방향처리는 해줌
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //광선이 땅에 닿았는지, 닿았다면 hit에 정보가 담긴다. Mathf.Infinity는 광선 거리 groundLayer는 레이어 마스크로, 광선이 충돌할 레이어를 지정한다.
        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer)) 
        {
            Vector3 targetPosition = hit.point; //hit.point는 광선이 충돌한 지점의 좌표를 반환한다. 이 좌표를 타겟 위치로 사용한다.

            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0.0f; //회전은 수평면에서만 이루어지도록 y축 방향을 제거한다.

            if(direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction); //LookRotation은 주어진 방향을 바라보는 회전을 생성한다.
                //Slerp는 두 회전 사이를 부드럽게 보간한다. 현재 회전에서 타겟 회전으로 rotationSpeed에 따라 보간한다.
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); 
            }
        }
    }
}
