using UnityEngine;

public class Portal : M_Object
{
    UIPART part = null;
    public override void Interaction()
    {
        base.Interaction();
        part = Canvas_Holder.instance.GetUIPART("PORTAL");
    }
}
