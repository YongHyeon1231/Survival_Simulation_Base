using Unity.VisualScripting;
using UnityEngine;

public class Portal : M_Object
{
    UIPART part = null;
    public override void Interaction(Character character)
    {
        base.Interaction(character);
        part = Canvas_Holder.instance.GetUIPART("PORTAL");
        part.Open();
        part.GetComponent<PORTAL>().Init(this);
    }
}
