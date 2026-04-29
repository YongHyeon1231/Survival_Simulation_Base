using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Object_Mng : MonoBehaviour
{
    private CullingGroup cullingGroup;
    private BoundingSphere[] boundingSpheres;
    public GameObject MonsterSpawner;
    private List<GameObject> SetObjects = new List<GameObject>();

    public float cullingGroupRadius = 5.0f;
    public float spawnAngleValue = 65.0f;
    public float CenterLimitValue = 10.0f;
    public int Maximum;
    public Transform ObjectParentTransform;
    private Object_Scriptable[] m_Datas;
    public float checkRadius;

    private void Start()
    {
        m_Datas = Resources.LoadAll<Object_Scriptable>("Object");

        GetSpawnObject();
    }

    private void MakeCulling()
    {
        boundingSpheres = new BoundingSphere[SetObjects.Count];

        cullingGroup = new CullingGroup();
        cullingGroup.targetCamera = Camera.main;
        cullingGroup.SetBoundingSpheres(boundingSpheres);
        cullingGroup.SetBoundingSphereCount(SetObjects.Count);

        for(int i = 0; i < SetObjects.Count; i++)
        {
            boundingSpheres[i] = new BoundingSphere(SetObjects[i].transform.position, cullingGroupRadius);
        }

        cullingGroup.onStateChanged += OnStateChanged;
    }

    public void RemoveObject(GameObject obj)
    {
        int index = SetObjects.IndexOf(obj);

        if(index < 0) return;

        SetObjects.RemoveAt(index);

        List<BoundingSphere> newSpheres = new List<BoundingSphere>(boundingSpheres);
        newSpheres.RemoveAt(index);
        boundingSpheres = newSpheres.ToArray();

        cullingGroup.SetBoundingSpheres(boundingSpheres);
        cullingGroup.SetBoundingSphereCount(boundingSpheres.Length);
    }

    public void OnDestroy()
    {
        if (cullingGroup != null)
        {
            cullingGroup.Dispose();
            cullingGroup = null;
        }
    }

    private void OnStateChanged(CullingGroupEvent evt)
    {
        if(evt.isVisible)
        {
            SetObjects[evt.index].SetActive(true);
        }
        else
        {
            SetObjects[evt.index].SetActive(false);
        }
    }

    public void GetSpawnObject()
    {
        StartCoroutine(CreateObjectStart());
    }

    private IEnumerator CreateObjectStart()
    {
        for(int i = 0; i < Maximum; i++)
        {
            Vector3 pos;
            MakePos(out pos);

            while(Vector3.Distance(pos, Vector3.zero) <= CenterLimitValue || IsPositionOverlapping(pos, checkRadius)) 
            {
                MakePos(out pos);
            }

            var GetObject = m_Datas[Random.Range(0, m_Datas.Length - 1)].obj;

            var go = Instantiate(GetObject, 
                new Vector3(pos.x, GetObject.transform.position.y, pos.z), 
                Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f),
                ObjectParentTransform );

            go.gameObject.SetActive(false);

            SetObjects.Add(go);

            yield return null;
        }

        for (int i = 0; i < 10; i++)
        {
            Vector3 pos;
            MakePos(out pos);

            while(Vector3.Distance(pos, Vector3.zero) <= CenterLimitValue) 
            {
                MakePos(out pos);
            }
            
            var go = Instantiate(MonsterSpawner,
                    new Vector3(pos.x, MonsterSpawner.transform.position.y, pos.z),
                    Quaternion.identity);

            yield return null;
        }

        MakeCulling();
    }

    private bool IsPositionOverlapping(Vector3 position, float chgeckRadius)
    {
        foreach(GameObject obj in SetObjects)
        {
            if(Vector3.Distance(obj.transform.position, position) < checkRadius)
            {
                return true;
            }
        }
        return false;
    }

    private void MakePos(out Vector3 pos)
    {
        pos = Vector3.zero + Random.insideUnitSphere * spawnAngleValue;
        pos.y = 0.0f;
    }

}
