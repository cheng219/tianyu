//============================
//作者：何明军
//日期：2016/3/23
//用途：副本组队系统数据
//============================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 单人，多人副本组数据
/// </summary>
public class CopyInItemDataInfo{
	/// <summary>
	/// 队员ID
	/// </summary>
	public int id;
	/// <summary>
	/// 剩余次数
	/// </summary>
	public int num;
	/// <summary>
	/// 购买次数
	/// </summary>
	public int buyNum;
	/// <summary>
	/// 具体副本数据
	/// </summary>
	public Dictionary<int,CopyInItemDataInfo.CopySceneData> copyScenes = new Dictionary<int,CopySceneData>();
	/// <summary>
	/// 选中副本
	/// </summary>
	public int curCopyScene;
	/// <summary>
	/// 具体副本数据结构
	/// </summary>
	public class CopySceneData{
		/// <summary>
		/// 副本id
		/// </summary>
		public int scene;
		/// <summary>
		/// 副本星级
		/// </summary>
		public int star;
		/// <summary>
		/// 构造
		/// </summary>
		public CopySceneData(int _type,int _star){
			scene = _type;
			star = _star;
		}
		/// <summary>
		/// 构造
		/// </summary>
		public CopySceneData(){}
		public CopySceneData(st.net.NetBase.single_many_star info){
			UpdateData(info);
		}
		/// <summary>
		/// 更新
		/// </summary>
		public void UpdateData(st.net.NetBase.single_many_star info){
			scene = (int)info.copy_type;
			star = (int)info.star_num;
		}
		/// <summary>
		/// 更新
		/// </summary>
		public void UpdateData(int _star){
			star = _star;
		}
	}
	/// <summary>
	/// 构造
	/// </summary>
	public CopyInItemDataInfo(){}
	/// <summary>
	/// 构造
	/// </summary>
	public CopyInItemDataInfo(int _id){
		id = _id;
	}
	/// <summary>
	/// 构造
	/// </summary>
	public CopyInItemDataInfo(st.net.NetBase.single_many_list info){
		UpdateData(info);
	}
	/// <summary>
	/// 更新
	/// </summary>
	public void UpdateData(st.net.NetBase.single_many_list info){
		id = (int)info.copy_id;
		num = (int)info.challenge_num;
		buyNum = (int)info.buy_num;
		for (int i = 0; i < info.single_many_star.Count; i++)
		{
			st.net.NetBase.single_many_star data = info.single_many_star[i];
			if (copyScenes.ContainsKey((int)data.copy_type))
			{
				copyScenes[(int)data.copy_type].UpdateData(data);
			}
			else
			{
				copyScenes[(int)data.copy_type] = new CopySceneData(data);
			}
		}
	}
	/// <summary>
	/// 更新
	/// </summary>
	public void UpdateData(int copy_id,int challenge_num,int buy_num){
		id = copy_id;
		num = challenge_num;
		buyNum = buy_num;
	}
	
	CopyGroupRef refData;
	
	CopyGroupRef RefData{
		get{
			if(refData == null)
            {
				refData = ConfigMng.Instance.GetCopyGroupRef(id);
			}
			return refData;
		}
	}
	/// <summary>
	/// 是否是镇魔塔
	/// </summary>
	public bool IsMagic{
		get{
            return RefData != null ? RefData.sort == 3 : false;
		}
	}
    /// <summary>
    /// 副本名字
    /// </summary>
    public string CopyName
    {
        get
        {
            return RefData == null ? string.Empty : RefData.name;
        }
    }
}
