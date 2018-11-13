//==========================
//作者:吴江
//日期：2015/11/13
//用途：组队系统的单个队员的界面
//
//
//修改：贺丰
//日期：2016/1/12
//==============================



using UnityEngine;
using System.Collections;

public class TeamMemberSingle : MonoBehaviour
{

    #region 控件引用
    public UILabel levelLabel;
    public UILabel nameLabel;
    public GameObject leaderTag;
    public UISprite profImage;

    protected TeamMenberInfo info = null;
    #endregion


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
        levelLabel.text = info.baseInfo.lev.ToString();
        nameLabel.text = info.baseInfo.name.ToString();
        PlayerConfig refData = ConfigMng.Instance.GetPlayerConfig((int)info.baseInfo.prof);
        if (refData != null)
        {
            profImage.spriteName = refData.res_head_Icon;
        }
        leaderTag.SetActive((int)info.baseInfo.uid == GameCenter.teamMng.LeaderId);
    }

    public TeamMenberInfo GetInfo()
    {
        return info;
    }



    public TeamMemberSingle CreateNew(Transform _parent, int _index)
    {
        GameObject obj = Instantiate(this.gameObject) as GameObject;
        obj.transform.parent = _parent;
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = new Vector3(127 + _index * 293, -188, 0);
        return obj.GetComponent<TeamMemberSingle>();
    }


}
