///////////////////////////////////////////////////////////////////////////////
//作者：吴江
//日期:2015/5/28
//用途：对象池
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;




/// <summary>
/// 我们主要使用这个 by 吴江
/// </summary>
/// <typeparam name="T"></typeparam>
[System.Serializable]
public class exComponentPool<T> where T : PoolObject
{
    /// <summary>
    /// 池挂载物体
    /// </summary>
    public Transform poolTrasfrom;
    /// <summary>
    /// 对象预制
    /// </summary>
    [System.NonSerialized]
    public GameObject prefab;
    /// <summary>
    /// 最低数量
    /// </summary>
    [System.NonSerialized]
    public int size = 0;
    /// <summary>
    /// 池内对象列表
    /// </summary>
    public List<T> insideData = new List<T>();
    /// <summary>
    /// 池外对象列表
    /// </summary>
    public List<T> outsideData = new List<T>();

    /// <summary>
    /// 初始化一定数量的实例,但这个size并不是绝对的，而只是最低额。 by 吴江
    /// </summary>
    /// <param name="_prefab"></param>
    /// <param name="_size"></param>
    public void Init(GameObject _prefab, int _size, Transform _parent)
    {
        poolTrasfrom = _parent;
        if (prefab != null)
        {
            GameObject.DestroyImmediate(prefab);
        }
        if (insideData.Count > 0)
        {
            foreach (var item in insideData)
            {
                GameObject.DestroyImmediate(item);
            }
            insideData.Clear();
        }
        if (outsideData.Count > 0)
        {
            foreach (var item in outsideData)
            {
                GameObject.DestroyImmediate(item);
            }
            outsideData.Clear();
        }
        prefab = _prefab;
        prefab.transform.parent = _parent;
        size = _size;
        if (prefab != null)
        {
            for (int i = 0; i < size; ++i)
            {
                Add();
            }
        }
    }

    /// <summary>
    /// 新建一个对象，放入池内 by吴江
    /// </summary>
    protected bool Add()
    {
        if (prefab == null)
        {
            return false;
        }
        GameObject obj = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
        obj.name = prefab.name;
        GameObject.DontDestroyOnLoad(obj);
        T t = obj.GetComponent<T>();
        if (t == null) t = obj.AddComponent<T>();
        obj.transform.parent = poolTrasfrom.transform;
        insideData.Add(t);
        return true;
    }


    /// <summary>
    /// 从对象池中获取一个对象 by 吴江
    /// </summary>
    /// <returns></returns>
    public T Request()
    {
        //先确保池内有对象 by吴江 
        if (insideData.Count <= 0)
        {
            Add();
        }
        T result = null;
        if (insideData.Count > 0)
        {
            result = insideData[0];
        }
        while (result == null)
        {
			if(insideData.Count > 0)insideData.RemoveAt(0);
            if (insideData.Count <= 0)
            {
                if (!Add())
                {
                    break;
                }
            }
            if (insideData.Count > 0)
            {
                result = insideData[0];
            }
        }
        //然后将池内合适的对象取出至池外 by吴江 
        if (result != null)
        {
            insideData.RemoveAt(0);
            outsideData.Add(result);
            return result;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 从对象池中获取一个对象 by 吴江
    /// </summary>
    /// <returns></returns>
    public T Request(Vector2 _pos)
    {
        T result = Request();
        result.transform.position = new Vector3(_pos.x, _pos.y, result.transform.position.z);
        return result;
    }


    /// <summary>
    /// 从对象池中获取一个对象 by 吴江
    /// </summary>
    /// <returns></returns>
    public T Request(Vector3 _pos)
    {
        T result = Request();
        result.transform.parent = null;
        result.transform.position = _pos;
        return result;
    }

    /// <summary>
    /// 从对象池中获取一个对象 by 吴江
    /// </summary>
    /// <returns></returns>
    public T Request(Vector3 _pos, Quaternion _rot, Transform _parent = null)
    {
        T result = Request();
		if(result != null)
		{
			result.transform.parent = _parent;
			result.transform.localPosition = _pos;
			result.transform.localRotation = _rot;
		}
        return result;
    }

    /// <summary>
    /// 归还一个对象到对象池中 by 吴江
    /// </summary>
    /// <param name="_item"></param>
    public void Return(T _item)
    {
		if(poolTrasfrom != null)_item.transform.parent = poolTrasfrom.transform;
		else Debug.Log("poolTrasfrom is null");
        _item.transform.position = Vector3.zero;
        _item.transform.rotation = Quaternion.identity;
        _item.lastBeUsedTime = Time.time;
        outsideData.Remove(_item);
        insideData.Add(_item);
    }



    /// <summary>
    /// 将对象池外的对象全部无条件收回对象池 by 吴江
    /// </summary>
    public void CollectAllBack()
    {
        //为避免数组越界，必须建立一个新的临时列表，用于控制 by 吴江
        List<T> tempList = new List<T>();
        foreach (var item in outsideData)
        {
            tempList.Add(item);
        }
        foreach (var item in tempList)
        {
            item.OnDespawned();
            Return(item);
        }
    }


    /// <summary>
    /// 清理对象池中的冗余对象  by 吴江
    /// </summary>
    public void Tick()
    {
        if (size <= 0) return;
        if (insideData.Count <= size) return;
        lock (insideData)
        {
            List<T> needDelete = new List<T>();
            for (int i = size; i < insideData.Count; i++)
            {
                if (insideData[i].NeedDelete())
                {
                    needDelete.Add(insideData[i]);
                }
            }
            foreach (var item in needDelete)
            {
                insideData.Remove(item);
                if (item != null)
                {
                    if (item.gameObject != null)
                    {
                        GameObject.Destroy(item.gameObject);
                    }
                    GameObject.Destroy(item);
                }
            }
        }
    }
}


/// <summary>
/// 固定数量的简单对象池，暂时不用 by 吴江
/// </summary>
/// <typeparam name="T"></typeparam>
[System.Serializable]
public class exPool<T> where T : class, new()
{

    [System.NonSerialized]
    public int size = 0;
    [System.NonSerialized]
    public int idx = 0;
    [System.NonSerialized]
    public T[] data;
    [System.NonSerialized]
    public T[] initData;


    public void Init(int _size)
    {
        size = _size;
        initData = new T[size];
        data = new T[size];
        for (int i = 0; i < size; ++i)
        {
            T obj = new T();
            initData[i] = obj;
            data[i] = initData[i];
        }
        idx = size - 1;
    }


    public void Reset()
    {
        for (int i = 0; i < size; ++i)
        {
            data[i] = initData[i];
        }
        idx = size - 1;
    }


    public T Request()
    {
        if (idx < 0)
        {
            Debug.LogError("Error: the pool do not have enough free item.");
            return null;
        }

        T result = data[idx];
        --idx;
        return result;
    }


    public void Return(T _item)
    {
        ++idx;
        data[idx] = _item;
    }
}

/// <summary>
/// 固定数量的简单对象池，暂时不用 by 吴江
/// </summary>
/// <typeparam name="T"></typeparam>
[System.Serializable]
public class exGameObjectPool
{

    public GameObject prefab;
    public int size = 0;
    [System.NonSerialized]
    public int idx = 0;
    [System.NonSerialized]
    public GameObject[] data;
    [System.NonSerialized]
    public GameObject[] initData;


    public void Init()
    {
        Init(prefab, size);
    }


    public void Init(GameObject _prefab, int _size)
    {
        prefab = _prefab;
        size = _size;
        initData = new GameObject[size];
        data = new GameObject[size];
        if (prefab != null)
        {
            for (int i = 0; i < size; ++i)
            {
                GameObject obj = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
                initData[i] = obj;
                data[i] = initData[i];
            }
        }
        idx = size - 1;
    }


    public void Clear()
    {
        for (int i = 0; i < size; ++i)
        {
            if (initData[i])
            {
                Game.DestroyObject(initData[i]);
            }
        }
        idx = 0;
        initData = null;
        data = null;
    }



    public void Reset()
    {
        for (int i = 0; i < size; ++i)
        {
            data[i] = initData[i];
        }
        idx = size - 1;
    }


    public GameObject Request()
    {
        if (idx < 0)
        {
            Debug.LogError("Error: the pool do not have enough free item.");
            return null;
        }

        GameObject result = data[idx];
        --idx;
        return result;
    }


    public GameObject Request(Vector2 _pos)
    {
        GameObject result = Request();
        result.transform.position = new Vector3(_pos.x, _pos.y, result.transform.position.z);
        return result;
    }


    public GameObject Request(Vector3 _pos, Quaternion _rot)
    {
        GameObject result = Request();
        result.transform.position = _pos;
        result.transform.rotation = _rot;
        return result;
    }


    public T Request<T>() where T : MonoBehaviour
    {
        GameObject go = Request();
        if (go)
            return go.GetComponent<T>();
        return null;
    }


    public T Request<T>(Vector2 _pos) where T : MonoBehaviour
    {
        GameObject go = Request(_pos);
        if (go)
            return go.GetComponent<T>();
        return null;
    }


    public T Request<T>(Vector3 _pos, Quaternion _rot) where T : MonoBehaviour
    {
        GameObject go = Request(_pos, _rot);
        if (go)
            return go.GetComponent<T>();
        return null;
    }


    public void Return(GameObject _item)
    {
        ++idx;
        // _item.gameObject.SetActive(false);
        data[idx] = _item;
    }
}
