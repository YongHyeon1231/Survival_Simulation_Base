using UnityEngine;

public class Cam_Movement : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private float PosX = 0.0f;
    [SerializeField] private float PosY = 10.0f;
    [SerializeField] private float PosZ = -10.0f;

    [SerializeField] private float m_Speed = 2.0f;

    private void LateUpdate()
    {
        Move();
    }

    private void Move()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(
            player.position.x + PosX,
            player.position.y + PosY,
            player.position.z + PosZ
            ), m_Speed * Time.deltaTime);

            transform.LookAt(player);
    }
}
