using System.Collections.Generic;
using UnityEngine;

public class P_Finder : MonoBehaviour
{
    [SerializeField] private float checkRadius = 5.0f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] Canvas uiCanvas;
    [SerializeField] private GameObject IconPrefab;

    [SerializeField] private float activationDistance = 3.0f;

    private Dictionary<Transform, GameObject> activeIcons = new Dictionary<Transform, GameObject>();

    private void Update()
    {
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, checkRadius, interactableLayer);

        HashSet<Transform> currentObjects = new HashSet<Transform>(); // 중복되지 않은 변수

        foreach(Collider obj in nearbyObjects)
        {
            Transform targetTransform = obj.transform;

            float distance = Vector3.Distance(transform.position, targetTransform.position);

            if(distance <= activationDistance)
            {
                ShowIcon(targetTransform);
                currentObjects.Add(targetTransform);
            }
        }

        List<Transform> toRemove = new List<Transform>();
        foreach(var iconEntry in activeIcons)
        {
            if (!currentObjects.Contains(iconEntry.Key))
            {
                iconEntry.Value.GetComponent<UI_Animation_Handler>().AnimationChange("Out");
                toRemove.Add(iconEntry.Key);
            }
        }

        foreach(var transformToRemove in toRemove) activeIcons.Remove(transformToRemove);
    }

    private void ShowIcon(Transform targetTransform) 
    {
        if(activeIcons.ContainsKey(targetTransform))
        {
            UpdateIconPosition(targetTransform, activeIcons[targetTransform]);
            return;
        }

        GameObject iconInstance = Instantiate(IconPrefab, uiCanvas.transform);
        activeIcons[targetTransform] = iconInstance;

        UpdateIconPosition(targetTransform, iconInstance);
    }

    private void UpdateIconPosition(Transform targetTransform, GameObject Icon)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(
            new Vector3(targetTransform.position.x,
            targetTransform.position.y + 1.5f,
            targetTransform.position.z)
            ); // 웓드 좌표 -> 스크린좌표

        Icon.GetComponent<RectTransform>().position = screenPosition;
    }
}
