//==========================
//作者:吴江
//日期：2015/11/13
//用途：组队系统的单个队员的界面
//==============================



using UnityEngine;
using System.Collections;

public class TeamMemberListSingle : MonoBehaviour
{

    #region 控件引用
    public UILabel nameLabel;
	public UILabel levelLabel;
    public GameObject leaderTag;
    public UISlider hpSlider;
	public GameObject isRobot;//是否是化身
	public UISprite mateIcon;
    #endregion

    protected TeamMenberInfo info = null;

    public void SetInfo(TeamMenberInfo _info)
    {
        if (info != null)
        {
            info.OnBaseInfoUpdate -= Refresh;
        }
        info = _info;
        info.OnBaseInfoUpdate += Refresh;
        Refresh();
    }


    private void Refresh()
    {
		if(nameLabel != null)nameLabel.text = info.baseInfo.name.ToString();
		if(levelLabel != null)levelLabel.text = ConfigMng.Instance.GetLevelDes((int)info.baseInfo.lev);
		if(leaderTag != null)leaderTag.SetActive((int)info.baseInfo.uid == GameCenter.teamMng.LeaderId);
		if(hpSlider != null)hpSlider.value = info.baseInfo.hp / (float)info.baseInfo.limit_hp;
		if(isRobot != null)isRobot.SetActive(info.baseInfo.robot_state == 1);
		PlayerConfig playerConfig = ConfigMng.Instance.GetPlayerConfig((int)info.baseInfo.prof);
		if(mateIcon != null && playerConfig != null)mateIcon.spriteName = playerConfig.res_head_Icon;
    }

    public TeamMemberListSingle CreateNew(Transform _parent, int _index)
    {
        GameObject obj = Instantiate(this.gameObject) as GameObject;
        obj.transform.parent = _parent;
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
		TeamMemberListSingle team = obj.GetComponent<TeamMemberListSingle>();
		return team;
    }


}
