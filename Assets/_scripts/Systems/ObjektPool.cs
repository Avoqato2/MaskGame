using System.Collections.Generic;
using UnityEngine;

public class ObjektPool<T> where T : MonoBehaviour
{
    private T prefab;
    private Transform parentContainer;
    
    private List<T> pooledObjects = new List<T>();

    public ObjektPool(T prefab, Transform parentContainer = null)
    {
        if (prefab == null)
            throw new System.ArgumentNullException(nameof(prefab), "ObjektPool prefab is null. Assign the prefab (EssencePrefab) in the EnemyHealth inspector.");
        this.prefab = prefab;
        this.parentContainer = parentContainer;
    }

    public T GetPoolObjekt(Vector3 position, Quaternion rotation)
    {
        foreach(T poolObject in pooledObjects)
        {
            if(!poolObject.gameObject.activeInHierarchy)
            {
                poolObject.transform.position = position;
                poolObject.transform.rotation = rotation;
                poolObject.gameObject.SetActive(true);
                return poolObject;
            }
        }

        T newObject = CreateInstance();
        newObject.transform.position = position;
        newObject.transform.rotation = rotation;
        newObject.gameObject.SetActive(true);
        return newObject;
    }
    
    private T CreateInstance()
    {
        T instance = GameObject.Instantiate(prefab, parentContainer);
        instance.gameObject.SetActive(false);
        pooledObjects.Add(instance);
        return instance;
    }
}