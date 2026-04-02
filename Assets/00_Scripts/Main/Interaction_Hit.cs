using UnityEngine;

public class Interaction_Hit :  M_Object
{
    public override void Interaction()
    {
        P_Movement.instance.AnimationChange(m_Data.m_Type.ToString());
        base.Interaction();
    }
}
