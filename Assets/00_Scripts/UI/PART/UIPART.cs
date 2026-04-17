using UnityEngine;

public class UIPART : MonoBehaviour
{
    public bool IsActive => gameObject.activeSelf;
    public virtual void Open()
    {
        if (IsActive == true)
        {
            Debug.LogWarning($"{gameObject.name} is Already Active.");
            return;
        }
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        if (IsActive == false)
        {
            Debug.LogWarning($"{gameObject.name} is Not Active.");
            return;
        }
        gameObject.SetActive(false);
    }

    public virtual void Toggle()
    {
        if (IsActive) 
        {
            Close();
        }
        else
        {
            Open();
        }
    }
}
