using UnityEngine;

public class M_Object : MonoBehaviour
{
    public Object_Scriptable m_Data;
    public bool GetInteraction = false;

    public int HP;
    public virtual void Interaction()
    {
        P_Handler.m_Object = this;
        GetInteraction = true;
        HP_Init();
    }

    public virtual void OnHit()
    {
        HP_Init();
    }

    public virtual void HP_Init()
    {
        if(HP <= 0)
        {
            HP = 0;
            Particle_Handler.instance.OnParticle(transform.GetChild(0).GetComponent<MeshRenderer>());
            Destroy(this.gameObject);
            Delegate_Holder.OnOutInteraction();
            return;
        }
        Canvas_Holder.instance.BoardFill(HP, m_Data.HP);
    }
}
