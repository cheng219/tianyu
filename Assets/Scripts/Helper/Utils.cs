//=================================================
//作者：吴江
//日期：2015/5/23
//用途：工具类
//=================================================


using System;
using UnityEngine;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
class Utils
{
    public const float dungeonTopOffset = 100;
    public const float dungeonBottomOffset = 3;



    public static short X(int p) { return (short)(p >> 16); }
    public static short Y(int p) { return (short)(p); }

    public static Vector3 Int2ToDir(int _dir)
    {
        return new Vector3((float)X(_dir) / 100.0f, 0.0f, (float)Y(_dir) / 100.0f);
    }

    public static Vector3 Int2ToVector3(List<int> _position, float _precision = 10.0f)
    {
        return new Vector3(_position[0] / _precision + 0.000001f, 0.0f, _position[1] / _precision + 0.000001f);
    }

    public static Vector3 IntToVector3(List<int> _position, float _precision = 10.0f)
    {
        return new Vector3(_position[0] / _precision + 0.000001f, _position[1] / _precision + 0.000001f, _position[2] / _precision + 0.000001f);
    }

    public static Vector3 IntToVector3(int _x, int _y, int _z, float _precision = 10.0f)
    {
        return new Vector3(_x / _precision + 0.000001f, _y / _precision + 0.000001f, _z / _precision + 0.000001f);
    }


    public static Vector3 LineCast(Vector3 _pos, float _startHeight, float _endHeight)
    {
        Vector3 start = new Vector3(_pos.x, _startHeight, _pos.z);
        Vector3 end = new Vector3(_pos.x, _endHeight, _pos.z);

        int layerMask = LayerMask.NameToLayer("Terrain");
        RaycastHit hitInfo;
        if (Physics.Linecast(start, end, out hitInfo, layerMask))
        {
            return hitInfo.point;
        }
        if (_pos.y < 0) { return _pos; }
        return _pos;
    }


    public static Vector3 LineCastInScene(Vector3 _pos)
    {
        return LineCast(_pos, _pos.y + 3.0f, _pos.y - 3.0f);
    }

    

#region 创建闭包，用于循环体内的显式绑定循环参数，如果参数个数不够可以再加

    public static System.Action Functor<TBindArg1> (TBindArg1 bindArg1, System.Action<TBindArg1> callback) {
        return (() => { callback(bindArg1); });
    }
    public static System.Action<TArg1> Functor<TArg1, TBindArg1> (TBindArg1 bindArg1, System.Action<TArg1, TBindArg1> callback) {
        return ((arg1) => { callback(arg1, bindArg1); });
    }
    public static System.Action<TArg1, TArg2> Functor<TArg1, TArg2, TBindArg1> (TBindArg1 bindArg1, System.Action<TArg1, TArg2, TBindArg1> callback) {
        return ((arg1, arg2) => { callback(arg1, arg2, bindArg1); });
    }
    public static System.Action Functor<TBindArg1, TBindArg2> (TBindArg1 bindArg1, TBindArg2 bindArg2, System.Action<TBindArg1, TBindArg2> callback) {
        return (() => { callback(bindArg1, bindArg2); });
    }
    public static System.Action<TArg1> Functor<TArg1, TBindArg1, TBindArg2> (TBindArg1 bindArg1, TBindArg2 bindArg2, System.Action<TArg1, TBindArg1, TBindArg2> callback) {
        return ((arg1) => { callback(arg1, bindArg1, bindArg2); });
    }
    public static System.Action<TArg1, TArg2> Functor<TArg1, TArg2, TBindArg1, TBindArg2> (TBindArg1 bindArg1, TBindArg2 bindArg2, System.Action<TArg1, TArg2, TBindArg1, TBindArg2> callback) {
        return ((arg1, arg2) => { callback(arg1, arg2, bindArg1, bindArg2); });
    }

#endregion


    #region 时间显示
    /// <summary>
    /// 传入总秒数,返回00:00:00格式的时间字符串
    /// </summary>
    /// <param name="_seconds"></param>
    /// <returns></returns>
    public static string FormatTimeShort(float _seconds)
    {
        return FormatTimeShort((int)_seconds);
    }
    /// <summary>
    /// 传入总秒数,返回00:00:00格式的时间字符串
    /// </summary>
    /// <param name="_seconds"></param>
    /// <returns></returns>
    public static string FormatTimeShort(int _seconds)
    {
        int totalS = Mathf.Max(0, _seconds);
        int s = totalS % 60;
        int totalM = totalS / 60;
        int m = totalM % 60;
        int h = totalM / 60;
        if (h > 0)
        {
            return string.Format("{0:D2}:{1:D2}:{2:D2}", h, m, s);
        }
        else
        {
            return string.Format("{0:D2}:{1:D2}", m, s);
        }
    }
    #endregion



    /// <summary>
    /// 获取主角身边的随机一个可站立点
    /// </summary>
    public static Vector3 GetRandomPos(Transform _tar)
    {
        if (_tar == null) return Vector3.zero;
        List<Vector3> posList = new List<Vector3>();
        Vector3 pos = _tar.position;
        posList.Add(new Vector3(pos.x + 1,0, pos.z));
		posList.Add(new Vector3(pos.x + 1,0, pos.z + 1));
		posList.Add(new Vector3(pos.x + 1,0, pos.z - 1));
		posList.Add(new Vector3(pos.x - 1,0, pos.z));
		posList.Add(new Vector3(pos.x - 1,0, pos.z + 1));
		posList.Add(new Vector3(pos.x - 1,0, pos.z - 1));
		posList.Add(new Vector3(pos.x,0, pos.z));
		posList.Add(new Vector3(pos.x,0, pos.z + 1));
		posList.Add(new Vector3(pos.x,0, pos.z - 1));
        Vector3 initPos = Vector3.zero;
		System.Random random = new System.Random();
		while(posList.Count > 0)//用随机,否则都是一个点
		{
			int count = posList.Count;
			int index = random.Next(0,count-1);
			if (GameStageUtility.CheckPosition(pos, posList[index]))
			{
				initPos = posList[index];
				break;
			}else
			{
				posList.RemoveAt(index);
			}
		}
        if (initPos == Vector3.zero)
        {
            initPos = pos;
        }
		return ActorMoveFSM.LineCast(initPos,true);
    }
	/// <summary>
	/// 获取目标点(NPC)周围的随机一个可站立点
	/// </summary>
	public static Vector3 GetRandomPos(Vector3 _tarVector3)
	{
		if (_tarVector3.Equals(Vector3.zero)) return Vector3.zero;
		List<Vector3> posList = new List<Vector3>();
		Vector3 pos = _tarVector3;
		posList.Add(new Vector3(pos.x + 1,0,pos.z));
		posList.Add(new Vector3(pos.x + 1,0, pos.z + 1));
		posList.Add(new Vector3(pos.x + 1,0, pos.z - 1));
		posList.Add(new Vector3(pos.x - 1,0, pos.z));
		posList.Add(new Vector3(pos.x - 1,0, pos.z + 1));
		posList.Add(new Vector3(pos.x - 1, 0,pos.z - 1));
		posList.Add(new Vector3(pos.x, 0,pos.z));
		posList.Add(new Vector3(pos.x,0, pos.z + 1));
		posList.Add(new Vector3(pos.x,0, pos.z - 1));
		Vector3 initPos = Vector3.zero;
		for (int i = 0; i < posList.Count; i++)
		{
			if (GameStageUtility.CheckPosition(pos, posList[i]))
			{
				initPos = posList[i];
				break;
			}
		}
		if (initPos == Vector3.zero)
		{
			initPos = pos;
		}
		return initPos;
	}
}