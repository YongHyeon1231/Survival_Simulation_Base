using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private float spreadRadius = 10.0f; // 퍼지는 반경
    [SerializeField] private float arcHeight = 5.0f; // 포물선 높이
    [SerializeField] private float moveSpeed = 8.0f; // 아이템 이동 속도
    [SerializeField] private GameObject GetParticle; // 아이템 획득 시 파티클 효과

    Transform player;

    private void Start()
    {
        player = P_Movement.instance.transform;
        StartCoroutine(SpreadAndMoveToPlayer());
    }

    IEnumerator SpreadAndMoveToPlayer() // 오브젝트 주변으로 퍼지는 코루틴
    {
        // insideUnitCircle은 2D 평면에서 반지름이 1인 원 안의 랜덤한 점을 반환
        // insideUnitSphere는 3D 공간에서 반지름이 1인 구 안의 랜덤한 점을 반환
        Vector3 spreadDirection = Random.insideUnitSphere * spreadRadius; 
        Vector3 spreadPosition = transform.position + spreadDirection;

        spreadPosition.y = Mathf.Max(spreadPosition.y, arcHeight); // 아이템이 땅에 묻히지 않도록 y값을 최소 5.0f로 설정

        float spreadTime = 0.3f;
        float elapsedTime = 0.0f;

        Vector3 startPosition = transform.position;

        while(elapsedTime < spreadTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / spreadTime;
            transform.position = Vector3.Lerp(startPosition, spreadPosition, t);
            yield return null; // 1프레임정도 대기
        }

        StartCoroutine(MoveToPlayer(spreadPosition)); // 퍼진 위치에서 플레이어로 이동하는 코루틴 시작
    }

    IEnumerator MoveToPlayer(Vector3 startPosition) // 플레이어에게 이동하는 코루틴
    {
        float journeyTime;
        float elapsedTime;
        Vector3 endPosition;

        while(true) 
        {
            endPosition = player.position + new Vector3(0.0f, 1.0f, 0.0f);
            journeyTime = Vector3.Distance(startPosition, endPosition) / moveSpeed; // 시간 = 거리 / 속도
            elapsedTime = 0.0f;

            while(elapsedTime < journeyTime)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / journeyTime;
                Vector3 currentPos = Vector3.Lerp(startPosition, endPosition, t);
                transform.position = currentPos;

                endPosition = player.position + new Vector3(0.0f, 1.0f, 0.0f); // 플레이어 위치 업데이트

                yield return null;
            }

            if(Vector3.Distance(transform.position, player.position + new Vector3(0.0f, 1.0f, 0.0f)) < 0.5f) break;

            startPosition = transform.position; // 시작 위치를 갱신하여 부드럽게 이동
        }

        Instantiate(GetParticle, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
