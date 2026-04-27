using System.Collections;
using UnityEngine;

public class Interaction_Hit :  M_Object
{
    float shakeAmount = 5.0f;
    float shakeDuration = 0.5f;

    private Quaternion originalRotation;

    private void Start()
    {
        originalRotation = transform.rotation;
        HP = m_Data.HP;
    }

    public override void Interaction(Character character)
    {
        base.Interaction(character);
        character.AnimationChange(m_Data.m_Type.ToString());
        character.EquipmentChange(m_Data.m_Type, true);
    }

    public override void OnHit(Character character)
    {
        base.OnHit(character);

        if (gameObject.activeInHierarchy) // 활성화 상태일 때만 흔들기
            ShakeTree(transform.position - P_Movement.instance.transform.position);

        if(HP <= 0 )
        {
            var items = ItemFlowController.DROPITEMLIST(m_Data.Drop_Items);
            for(int i = 0; i < items.Count; i++)
            {
                var go = Instantiate(item_Prefab, transform.position, Quaternion.identity);
                go.Init(items[i]);
            }
        }
    }

    private void ShakeTree(Vector3 attackDirection)
    {
        Vector3 oppositeDirection = -attackDirection.normalized;

        // 물체 회전에 관한 것은 transform.rotation 과 transform.eulerAngles(오일러 값)를 사용하여 계산할 수 있습니다.
        // 둘의 차이는 transform.rotation은 Quaternion 형태로 회전을 나타내고, transform.eulerAngles는 Vector3 형태로 회전을 나타냅니다.
        // 특정 오브젝트의 인스펙터창의 Rotation값을 글자 그대로 가져오기 위해서는 transform.eulerAngles를 사용해야 합니다. transform.rotation은 Quaternion 형태로 회전을 나타내므로, 인스펙터창의 Rotation값과는 다르게 표현됩니다.

        // Quaternion targetRotation = Quaternion.Euler(
        //     originalRotation.eulerAngles.x + originalRotation.eulerAngles.y * shakeAmount, 
        //     originalRotation.eulerAngles.y,
        //     originalRotation.eulerAngles.z + oppositeDirection.x * shakeAmount);

        // Quaternion targetRotation = Quaternion.Euler(
        //     -oppositeDirection.z * shakeAmount,
        //     0,
        //     oppositeDirection.x * shakeAmount
        //     );

        Quaternion targetRotation = Quaternion.Euler(
            originalRotation.eulerAngles.x + shakeAmount, 
            originalRotation.eulerAngles.y,
            originalRotation.eulerAngles.z + shakeAmount);

        StopAllCoroutines();
        StartCoroutine(ShakeAnimation(targetRotation));
    }

    private IEnumerator ShakeAnimation(Quaternion targetRotation)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < shakeDuration / 2)
        {
            transform.rotation = Quaternion.Slerp(
                originalRotation, 
                targetRotation, 
                elapsedTime / (shakeDuration / 2)
                );
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0.0f;
        while(elapsedTime < shakeDuration / 2)
        {
            transform.rotation = Quaternion.Slerp(
                targetRotation, 
                originalRotation, 
                elapsedTime / (shakeDuration / 2)
                );
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = originalRotation;
    }
}
