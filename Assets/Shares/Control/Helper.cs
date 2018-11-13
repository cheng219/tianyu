///////////////////////////////////////////////////////////////////////////////
//作者：吴江
//日期：2015/5/13
//用途：一些扩展接口
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public static class Helper {

    static void ProcessChild<T> ( Transform _trans, ref List<T> _list ) where T : Component
    {
        T c = _trans.GetComponent<T>();
        if (c != null) _list.Add(c);
        foreach( Transform child in _trans )
            ProcessChild<T>( child, ref _list );
    }


    /// <summary>
    /// 预制无法通过GetComponentInChildren去获取components，所以用这个接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_trans"></param>
    /// <returns></returns>
    public static T[] GetAllComponents<T> ( Transform _trans ) where T : Component
    {
        List<T> result = new List<T>();
        ProcessChild<T>(_trans, ref result);
        return result.ToArray();
    }
}

internal static partial class UnityEngineExtends
{


    /// <summary>
    /// 设置整体layer by吴江
    /// </summary>
    /// <param name="_obj"></param>
    /// <param name="_layer"></param>
    public static void SetMaskLayer(this GameObject _obj, int _layer)
    {
        if (_obj == null) return;
        _obj.layer = _layer;
        int count = _obj.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            SetMaskLayer(_obj.transform.GetChild(i).gameObject, _layer);
        }
    }


    /// <summary>
    /// 设置整体颜色 by吴江
    /// </summary>
    /// <param name="_obj"></param>
    /// <param name="_layer"></param>
    public static void SetColor(this GameObject _obj, Color _color)
    {
        if (_obj == null) return;
        int count = _obj.transform.childCount;
        Renderer rd = _obj.GetComponent<Renderer>();
        if (rd == null) rd = _obj.GetComponent<SkinnedMeshRenderer>();
        if (rd == null) rd = _obj.GetComponent<MeshRenderer>();
        if (rd == null) rd = _obj.GetComponent<ParticleRenderer>();
        if (rd != null && rd.material != null)
        {
            Material[] mts = rd.materials;
            for (int i = 0; i < mts.Length; i++)
            {
                mts[i].color = _color;
                try
                {
                    Color tempColor = mts[i].GetColor("_TintColor");
                    mts[i].SetColor("_TintColor", _color.SetAlpha(tempColor.a));
                }
                catch
                {
                }
            }
            rd.materials = mts;
        }
        for (int i = 0; i < count; i++)
        {
            SetColor(_obj.transform.GetChild(i).gameObject, _color);
        }
    }

    /// <summary> Use this method instead of GetComponentInChildren that may alloc lots of GC </summary>
    public static List<T> GetComponentsInChildrenFast<T>(this GameObject go, bool includeInactive = false) where T : Component
    {
        List<T> list = new List<T>();
        if (includeInactive || go.activeInHierarchy)
        {
            Component comp = go.GetComponent(typeof(T));
            if (comp != null)
            {
                list.Add(comp as T);
            }
        }
        Transform transform = go.transform;
        if (transform != null)
        {
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; ++i)
            {
                Transform child = transform.GetChild(i);
                List<T> componentsInChildren = child.gameObject.GetComponentsInChildrenFast<T>(includeInactive);
                foreach (var item in componentsInChildren)
                {
                    list.Add(item);
                }
            }
        }
        return list;
    }

    /// <summary> Use this method instead of GetComponentInChildren that may alloc lots of GC </summary>
    public static T GetComponentInChildrenFast<T>(this GameObject go, bool includeInactive = false) where T : Component
    {
        if (includeInactive || go.activeInHierarchy) {
            Component comp = go.GetComponent(typeof(T));
            if (comp != null) {
                return comp as T;
            }
        }
        Transform transform = go.transform;
        if (transform != null)
        {
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; ++i) {
                Transform child = transform.GetChild(i);
                T componentInChildren = child.gameObject.GetComponentInChildrenFast<T>(includeInactive);
                if (componentInChildren != null) {
                    return componentInChildren;
                }
            }
        }
        return null;
    }
    public static T GetComponentInChildrenFast<T> (this Component component, bool includeInactive = false) where T : Component 
    {
        return component.gameObject.GetComponentInChildrenFast<T>(includeInactive);
    }

    public static AnimationState GetPlayingAnimation(this Animation ani)
    {
        if (ani.isPlaying) {
            foreach (AnimationState state in ani) {
                if (ani.IsPlaying(state.name)) {
                    return state;
                }
            }
        }
        return null;
    }

    public static AnimationState GetState(this Animation ani, int index)
    {
        int i = 0;
        foreach (AnimationState state in ani) {
            if (i == index) {
                return state;
            }
            ++i;
        }
        return null;
    }

    public static T GetComponent<T>(this GameObject go, bool create) where T : Component
    {
        T comp = go.GetComponent(typeof(T)) as T;
        if (create && comp == null) {
            comp = go.AddComponent(typeof(T)) as T;
        }
        return comp;
    }
    public static T GetComponent<T> (this Component component, bool create) where T : Component 
    {
        T comp = component.GetComponent(typeof(T)) as T;
        if (create && comp == null) {
            comp = component.gameObject.AddComponent(typeof(T)) as T;
        }
        return comp;
    }

    public static Vector3 SetX (this Vector3 vector, float x) {
        vector.x = x;
        return vector;
    }
    /// <summary>
    /// 简单的辅助方法，返回一个新的Vector，无副作用，用来减少代码行数
    /// </summary>
    /// <example>
    /// 例如
    /// Vector3 vec = transform.localPosition;
    /// vec.y = height;
    /// transform.localPosition = vec;
    /// 可以写成
    /// transform.localPosition = transform.localPosition.SetY(height);
    /// 
    /// 例如
    /// var dis = mob.transform.position - pos;
    /// dis.y = 0;
    /// float sqrDis = dis.sqrMagnitude;
    /// 可以写成
    /// float sqrDis = (mob.transform.position - pos).SetY(0).sqrMagnitude;
    /// </example>
    public static Vector3 SetY (this Vector3 vector, float y) {
        vector.y = y;
        return vector;
    }
    public static Vector3 SetZ (this Vector3 vector, float z) {
        vector.z = z;
        return vector;
    }
    public static Vector2 SetX (this Vector2 vector, float x) {
        vector.x = x;
        return vector;
    }
    public static Color SetAlpha (this Color color, float alpha) {
        color.a = alpha;
        return color;
    }
}

/// <summary>
/// 可按距离由小到大排序的比较器
/// </summary>
class DistanceComparer<T> : IComparer<T> where T : Component {
    public Vector3 comparandPos = Vector3.zero;

    static DistanceComparer<T> instance_;
    public static DistanceComparer<T> instance {
        get {
            if (instance_ == null) {
                instance_ = new DistanceComparer<T>();
            }
            return instance_;
        }
    }

    public int Compare (T x, T y) {
        Vector3 xDis = x.transform.position - comparandPos;
        Vector3 yDis = y.transform.position - comparandPos;
        xDis.y = 0;
        yDis.y = 0;
        return xDis.sqrMagnitude.CompareTo(yDis.sqrMagnitude);
    }
}


public class TimeMathf
{
    /// <summary>
    /// 秒数转换成时间字符串，一般用于buff时间显示
    /// </summary>
    /// <param name="_secondsCount"></param>
    /// <returns></returns>
    public static string IntTransToTimeStr(int _secondsCount)
    {
        int min = _secondsCount / 60;
        if (min > 1)
        {
            int hour = min / 60;
            if (hour > 1)
            {
                int day = hour / 24;
                if (day > 1)
                {
                    return day.ToString() + "d";
                }
                return hour.ToString() + "h";
            }
            return min.ToString() + "m";
        }
        return _secondsCount.ToString() + "s";

    }
}