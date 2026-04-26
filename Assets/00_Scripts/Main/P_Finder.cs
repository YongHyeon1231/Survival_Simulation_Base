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
    private Transform closetObject;
    [HideInInspector]public bool OnInteraction = false;

    private void Start()
    {
        Delegate_Holder.OnInteraction += OnInteractionVoid;
        Delegate_Holder.OnInteractionOut += OnInteractionOut;
    }

    private void OnInteractionVoid()
    {
        OnInteraction = true;
        transform.LookAt(closetObject.transform.position);
        closetObject = null;
        IconInit();
    }

    private void OnInteractionOut()
    {
        OnInteraction = false;
        P_Movement.instance.EquipmentAllDeactive();
        foreach (var icon in activeIcons.Values) Destroy(icon);
        activeIcons.Clear();
    }

    private void Update()
    {
        if(OnInteraction) return;

        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, checkRadius, interactableLayer);

        closetObject = null;
        float closetDistance = Mathf.Infinity;

        foreach(Collider obj in nearbyObjects)
        {
            Transform targetTransform = obj.transform;

            float distance = Vector3.Distance(transform.position, targetTransform.position);

            if(distance <= activationDistance && distance < closetDistance)
            {
                closetObject = targetTransform;
                closetDistance = distance;
            }
        }

        if (closetObject != null)
        {
            ShowIcon(closetObject);

            if(Input.GetKeyDown(KeyCode.F))
            {
                M_Object subObject = null;
                if(closetObject.GetComponent<M_Object>() == null)
                {
                    subObject = closetObject.transform.parent.GetComponent<M_Object>();
                }
                else 
                {
                    subObject = closetObject.GetComponent<M_Object>();
                }
                subObject.Interaction(GetComponent<Character>());
                Delegate_Holder.OnStartInteraction();
            }
        }

        IconInit();
    }

    private void IconInit()
    {
        List<Transform> toRemove = new List<Transform>();
        foreach(var iconEntry in activeIcons)
        {
            if(iconEntry.Key != closetObject)
            {
                // iconEntry.Value.GetComponent<UI_Animation_Handler>().AnimationChange("Out");
                // toRemove.Add(iconEntry.Key);
                var handler = iconEntry.Value.GetComponent<UI_Animation_Handler>();
                handler.AnimationChange("Out");
                // Destroy(iconEntry.Value, 1.0f);
                toRemove.Add(iconEntry.Key);
            }
        }
        foreach(var transformToRemove in toRemove)
        {
            activeIcons.Remove(transformToRemove);
        }
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
