using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public GameObject Prefab => prefab;
    [SerializeField] GameObject prefab;
    [SerializeField] int size = 1;
    Transform parent;//指定父对象
    Queue<GameObject> _queue;

    public int Size => size;//初始时尺寸
    public int RuntimeSize => _queue.Count;//运行时尺寸
    
    public void Initialize(Transform parent)
    {
        _queue = new Queue<GameObject>();
        this.parent = parent;
        for (var i = 0; i < size; i++)
        {
            _queue.Enqueue(Copy());
        }
    }

    //
    GameObject Copy()
    {
        var copy = GameObject.Instantiate(prefab,parent);
        copy.SetActive(false);
        return copy;
    }

    GameObject AvailableObject()
    {
        GameObject availableObject;
        if (_queue.Count > 0 && !_queue.Peek().activeSelf)//Peek取出的是队列第一个对象，检测它的activeSelf值，防止返回正在工作的对象
        {
            availableObject = _queue.Dequeue();
        }
        else
        {
            availableObject = Copy();
        }
        //对象出列之后马上入列。
        _queue.Enqueue(availableObject);
        return availableObject;
    }

    //从对象池中获取需要使用的对象，并将其启用SetActive(true)。
    public GameObject PreparedObject()
    {
        GameObject preparedObject = AvailableObject();
        preparedObject.SetActive(true);
        return preparedObject;
    }
    
    public GameObject PreparedObject(Vector3 position)
    {
        GameObject preparedObject = AvailableObject();
        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        return preparedObject;
    }
    
    public GameObject PreparedObject(Vector3 position,Quaternion rotation)
    {
        GameObject preparedObject = AvailableObject();
        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        return preparedObject;
    }
    
    public GameObject PreparedObject(Vector3 position,Quaternion rotation,Vector3 localScale)
    {
        GameObject preparedObject = AvailableObject();
        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        preparedObject.transform.localScale = localScale;
        return preparedObject;
    }
}