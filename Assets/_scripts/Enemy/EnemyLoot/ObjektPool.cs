using System.Collections.Generic;
using UnityEngine;

public class ObjektPool<T> where T : MonoBehaviour
{
    private T prefab;
    private Rigidbody _rigidbodyPrefab;
    
    private List<T> pooledObjects = new List<T>();

    public ObjektPool(T prefab, Rigidbody rigidbodyPrefab)
    {
        if (prefab == null)
            throw new System.ArgumentNullException(nameof(prefab), "ObjektPool prefab is null. Assign the prefab (EssencePrefab) in the EnemyHealth inspector.");
        this.prefab = prefab;
        _rigidbodyPrefab = rigidbodyPrefab;
    }

    public T GetPoolObjekt()
    {
        foreach(T poolObject in pooledObjects)
        {
            if(!poolObject.gameObject.activeInHierarchy)
            {
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
        _rigidbodyPrefab.useGravity = true;
        /*_rigidbodyPrefab.linearVelocity = Vector3.zero;
        instance.transform.position = new Vector3(instance.transform.position.x, instance.transform.position.y, instance.transform.position.z);
        instance.transform.rotation = Quaternion.identity;*/
        pooledObjects.Add(instance);
        return instance;
    }
}