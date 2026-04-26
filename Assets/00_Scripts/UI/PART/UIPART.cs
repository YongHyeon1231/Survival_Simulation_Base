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
        Canvas_Holder.Uis.Enqueue(this);
    }

    public virtual void Close()
    {
        if (IsActive == false)
        {
            Debug.LogWarning($"{gameObject.name} is Not Active.");
            return;
        }
        if (Canvas_Holder.Uis.Count > 0) Canvas_Holder.Uis.Dequeue();
        if (GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().SetTrigger("Out");
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
