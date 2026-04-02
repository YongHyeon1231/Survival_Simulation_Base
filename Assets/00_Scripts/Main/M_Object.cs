using UnityEngine;

public class M_Object : MonoBehaviour
{
    public Object_Scriptable m_Data;
    public bool GetInteraction = false;
    public virtual void Interaction()
    {
        GetInteraction = true;
    }
}
