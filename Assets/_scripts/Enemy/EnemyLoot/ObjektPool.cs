using System.Collections.Generic;
using UnityEngine;

public class ObjektPool<T> where T : MonoBehaviour
{
    private T prefab;
    
    private List<T> pooledObjects = new List<T>();

    public ObjektPool(T prefab)
    {
        if (prefab == null)
            throw new System.ArgumentNullException(nameof(prefab), "ObjektPool prefab is null. Assign the prefab (EssencePrefab) in the EnemyHealth inspector.");
        this.prefab = prefab;
    }

    public T GetPoolObjekt()
    {
        foreach(T poolObject in pooledObjects)
        {
            if(!poolObject.gameObject.activeInHierarchy)
            {
                poolObject.transform.position = Vector3.zero;
                poolObject.gameObject.SetActive(true);
                return poolObject;
            }
        }

        T newObject = CreateInstance();
        newObject.gameObject.SetActive(true);
        return newObject;
    }
    
    private T CreateInstance()
    {
        T instance = GameObject.Instantiate(prefab);
        instance.gameObject.SetActive(false);
        pooledObjects.Add(instance);
        return instance;
    }
}