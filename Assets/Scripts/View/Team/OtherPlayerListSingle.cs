//==========================
//作者:邓成
//日期：2016/5/26
//用途：附近的人系统的单个成员的界面
//==============================



using UnityEngine;
using System.Collections;

public class OtherPlayerListSingle : MonoBehaviour
{

	#region 控件引用
	public UILabel nameLabel;
	public UILabel levelLabel;
	#endregion

	protected OtherPlayerInfo info = null;

	public void SetInfo(OtherPlayerInfo _info)
	{
		if (info != null)
		{
			info.OnBaseUpdate -= Refresh;
		}
		info = _info;
		info.OnBaseUpdate += Refresh;
		Refresh();
	}

	protected void Refresh(ActorBaseTag tag,ulong val,bool flag)
	{
		if(tag == ActorBaseTag.Level)
			Refresh();
	}

	private void Refresh()
	{
		if(nameLabel != null)nameLabel.text = info.Name.ToString();
		if(levelLabel != null)levelLabel.text = info.LevelDes;
	}

	public OtherPlayerListSingle CreateNew(Transform _parent, int _index)
	{
		GameObject obj = Instantiate(this.gameObject) as GameObject;
		obj.transform.parent = _parent;
		obj.transform.localScale = Vector3.one;
		obj.transform.localPosition = Vector3.zero;
		return obj.GetComponent<OtherPlayerListSingle>();
	}

	void OnDestroy()
	{
		if(info != null)info.OnBaseUpdate -= Refresh;
	}
}
