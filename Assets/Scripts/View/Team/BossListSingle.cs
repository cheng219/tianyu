//==========================
//作者:邓成
//日期：2016/6/21
//用途：熔恶之地BOSS列表单个的界面
//==============================

using UnityEngine;
using System.Collections;

public class BossListSingle : MonoBehaviour {

    protected string colorStr = "[ff9933]{0}[-]";
	#region 控件引用
	public UILabel nameLabel;
	public UILabel levelLabel;
	public UILabel labAppear;
    public UILabel labKilled;
	public GameObject timeGo;
	public UITimer timer;
	#endregion

	protected BossChallengeData info = null;

	public void SetInfo(BossChallengeData _info)
	{
		info = _info;
		Refresh();
	}

	private void Refresh()
	{
        bool isAppear = (info.AppearTime <= (int)Time.realtimeSinceStartup);
        if (nameLabel != null) nameLabel.text = isAppear ? info.bossName.ToString() : string.Format(colorStr, info.bossName);
        if (levelLabel != null) levelLabel.text = isAppear ? info.CurBossRef.needLevel.ToString() : string.Format(colorStr, info.CurBossRef.needLevel);
        if (labAppear != null) labAppear.enabled = isAppear;
        if (timeGo != null) timeGo.SetActive(!isAppear);
        if (!isAppear)
		{
			if(timer != null)
			{
				timer.StartIntervalTimer(info.AppearTime - (int)Time.realtimeSinceStartup);
				timer.onTimeOut = (x)=>
				{
					if(timeGo != null)timeGo.SetActive(false);
					if(labAppear != null)labAppear.enabled = true;
                    if (GameCenter.bossChallengeMng != null) GameCenter.bossChallengeMng.BossRelive(info);
				};
			}
		}
	}

    public void SetBossCopyItemInfo(st.net.NetBase.boss_copy_list bossInfo)
    {
        string name = string.Empty;
        string levStr = string.Empty;
        if (bossInfo != null)
        {
            MonsterRef mob = ConfigMng.Instance.GetMonsterRef((int)bossInfo.boss_id);
            if (mob != null) 
            {
                name = mob.name;
                levStr = "Lv."+mob.lv;
            }
        }
        if (nameLabel != null) nameLabel.text = name;
        if (levelLabel != null) levelLabel.text = levStr;
        if (labAppear != null) labAppear.enabled = (bossInfo.boss_kill_state == 2);
        if (labKilled != null) labKilled.enabled = (bossInfo.boss_kill_state == 1);
    }

	public BossListSingle CreateNew(Transform _parent, int _index)
	{
		GameObject obj = Instantiate(this.gameObject) as GameObject;
		obj.transform.parent = _parent;
		obj.transform.localScale = Vector3.one;
		obj.transform.localPosition = Vector3.zero;
		return obj.GetComponent<BossListSingle>();
	}
}
