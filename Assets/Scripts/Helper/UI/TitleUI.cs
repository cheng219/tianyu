//=====================================
//作者:黄洪兴
//日期:2016/3/22
//用途:称号组件
//========================================


using UnityEngine;
using System.Collections;
/// <summary>
/// 称号UI组件
/// </summary>
public class TitleUI : MonoBehaviour
{
	#region 控件数据
	/// <summary>
	/// 称号名字
	/// </summary>
	public UISprite titleName;
	public GameObject useBtn;
	public GameObject chooseBtn;
	/// <summary>
	/// 被选中的图片
	/// </summary>
	public GameObject chooseObj;
	/// <summary>
	/// 穿戴中的图片
	/// </summary>
	public GameObject useObj;
	/// <summary>
	/// 是否被选中
	/// </summary>
	public bool isChoose
	{
		get { return titleinfo == GameCenter.titleMng.ChooseTitle; }
	}
	/// <summary>
	/// 是否穿戴
	/// </summary>
	public bool isUse
	{
		get { return titleinfo == GameCenter.titleMng.CurUseTitle; }
	}
	#endregion 
	#region 数据
	/// <summary>
	/// 选择事件
	/// </summary>
	public System.Action<TitleUI> OnSelectEvent = null;
	/// <summary>
	/// 当前填充的数据
	/// </summary>
	public TitleInfo titleinfo;
	//	protected SkillInfo oldSkillinfo;  //for upgrade effect -by ms
	#endregion
	// Use this for initialization
	void Start () {
        if (chooseBtn != null) UIEventListener.Get(chooseBtn).onClick -= ChooseTitle;
        if (chooseBtn != null) UIEventListener.Get(chooseBtn).onClick += ChooseTitle;
        if (useBtn != null) UIEventListener.Get(useBtn).onClick -= UseTitle;
        if (useBtn != null) UIEventListener.Get(useBtn).onClick += UseTitle;

	}

	void ChooseTitle(GameObject obj)
	{
		
		GameCenter.titleMng.ChooseTitle = titleinfo;
        if (GameCenter.titleMng.UpdateTitle!=null)
		GameCenter.titleMng.UpdateTitle ();
        if (GameCenter.titleMng.UpDateTargetTitle != null)
            GameCenter.titleMng.UpDateTargetTitle();

	}

	void UseTitle(GameObject obj)
	{
		if (titleinfo.IsOwn) {
			if (GameCenter.titleMng.CurUseTitle == titleinfo) {

				GameCenter.titleMng.C2S_UseTitle (titleinfo.ID, 0);
			} else {
				GameCenter.titleMng.C2S_UseTitle (titleinfo.ID, 1);
			}
		} else {
			GameCenter.messageMng.AddClientMsg (417);
		}
		GameCenter.titleMng.ChooseTitle = titleinfo;
        if (GameCenter.titleMng.UpdateTitle != null)
		GameCenter.titleMng.UpdateTitle ();
        if (GameCenter.titleMng.UpDateTargetTitle != null)
            GameCenter.titleMng.UpDateTargetTitle();

	}


	/// <summary>
	/// 填充数据
	/// </summary>
	/// <param name="_info"></param>
	public void FillInfo(TitleInfo _info)
	{
		if (_info == null) {
			titleinfo = null;
		} 
		else {
			titleinfo = _info;

			//			oldSkillinfo = skillinfo;
		}
		RefreshTitle ();
	}
	/// <summary>
	/// 刷新表现
	/// </summary>
	public void RefreshTitle()
	{
//		Debug.Log ("此时角色穿戴的称号为"+GameCenter.titleMng.CurUseTitle.ID);
		if (titleinfo != null) {
            if (titleName != null)
            {
                titleName.spriteName = titleinfo.IconName;
                titleName.MakePixelPerfect();
                UISpriteEx spritex = titleName as UISpriteEx;
                if (spritex != null)
                {
                    spritex.IsGray = titleinfo.IsOwn ? UISpriteEx.ColorGray.normal : UISpriteEx.ColorGray.Gray;
                }
            }
			if(chooseObj!=null)chooseObj.SetActive (isChoose);
			if(useObj!=null)useObj.SetActive (isUse);

		} else {
			
		}

	}
}
