using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class P_Movement : MonoBehaviour
{
    [Header("#Movement Settings")]
    public float moveSpeed = 5.0f;

    private CharacterController controller;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
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
    }
}
