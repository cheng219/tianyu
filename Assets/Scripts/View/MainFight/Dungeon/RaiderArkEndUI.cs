//======================================================
//作者:鲁家旗
//日期:2016/11/18
//用途:夺宝奇兵结算界面
//======================================================
using UnityEngine;
using System.Collections;
using st.net.NetBase;
using System.Collections.Generic;

public class RaiderArkEndUI : MonoBehaviour {
    public UILabel whiteOrb;
    public UILabel greenOrb;
    public UILabel blueOrb;
    public UILabel violetOrb;
    public UIButton exitBtn;
    public UITimer timer;
    void Awake()
    {
        if (exitBtn != null) UIEventListener.Get(exitBtn.gameObject).onClick = CloseWnd;
    }
    void CloseWnd(GameObject _go)
    {
        GameCenter.duplicateMng.C2S_OutCopy();
    }
    public void Refresh(List<reward_list> _list)
    {
        //倒计时结束跳出副本
        if (timer != null) timer.StartIntervalTimer(30);
        timer.onTimeOut = delegate { GameCenter.duplicateMng.C2S_OutCopy(); };
        for (int i = 0, max = _list.Count; i < max; i++)
        {
            switch (_list[i].type)
            {
                case 1:
                    if (whiteOrb != null) whiteOrb.text = _list[i].num + "/40";
                    break;
                case 2:
                    if (greenOrb != null) greenOrb.text = _list[i].num.ToString();
                    break;
                case 3:
                    if (blueOrb != null) blueOrb.text = _list[i].num.ToString();
                    break;
                case 4:
                    if (violetOrb != null) violetOrb.text = _list[i].num.ToString();
                    break;
            }
        }
    }
}
