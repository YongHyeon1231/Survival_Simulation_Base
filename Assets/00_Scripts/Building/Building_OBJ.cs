using System.ComponentModel;
using UnityEngine;

public enum Material_Type
{
    Opaque,
    Transparent,
}

public class Building_OBJ : MonoBehaviour
{
    Renderer renderer;
    Collider collider;
    public Material Opaque_M, Transparent_M;

    public Color[] colors;
    public bool CanBuild = false;

    private void Awake()
    {
        renderer = GetComponentInChildren<Renderer>();
        collider = GetComponentInChildren<Collider>();
    }

    public void SetTrigger(bool active)
    {
        collider.isTrigger = active;
    }

    public void SetMaterial(Material_Type type)
    {
        switch (type)
        {
            case Material_Type.Opaque:
                renderer.material = Opaque_M;
                break;
            case Material_Type.Transparent:
                renderer.material = Transparent_M;
                break;
        }
    }

    int overlapCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name != "Terrain")
        {
            overlapCount++;
            SetMaterial_Color(1);
            CanBuild = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        overlapCount--;
        if(overlapCount <= 0)
        {
            if(other.gameObject.name != "Terrain")
            {
                overlapCount = 0;
                SetMaterial_Color(0);
                CanBuild = true;
            }
        }
    }

    public void SetMaterial_Color(int value)
    {
        renderer.material.SetColor("_EmissionColor", colors[value]);
    }
}
