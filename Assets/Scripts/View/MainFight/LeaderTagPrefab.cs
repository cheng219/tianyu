//======================================
//作者:吴江
//日期:2016/4/29
//用途:头顶阵营领袖标记
//======================================

using UnityEngine;
using System.Collections;

public class LeaderTagPrefab : MonoBehaviour {

    public GameObject horde;
    public GameObject hordeSecond;
    public GameObject alliance;
    public GameObject allianceSecond;



    public void Active(HeadTextCtrl.LeaderTagType _type)
    {
        switch (_type)
        {
            case HeadTextCtrl.LeaderTagType.Horde:
                if (horde != null) horde.SetActive(true);
                if (alliance != null) alliance.SetActive(false);
                if (hordeSecond != null) hordeSecond.SetActive(false);
                if (allianceSecond != null) allianceSecond.SetActive(false);
                break;
            case HeadTextCtrl.LeaderTagType.HordeSecond:
                if (horde != null) horde.SetActive(false);
                if (alliance != null) alliance.SetActive(false);
                if (hordeSecond != null) hordeSecond.SetActive(true);
                if (allianceSecond != null) allianceSecond.SetActive(false);
                break;
            case HeadTextCtrl.LeaderTagType.Alliance:
                if (horde != null) horde.SetActive(false);
                if (alliance != null) alliance.SetActive(true);
                if (hordeSecond != null) hordeSecond.SetActive(false);
                if (allianceSecond != null) allianceSecond.SetActive(false);
                break;
            case HeadTextCtrl.LeaderTagType.AllianceSecond:
                if (horde != null) horde.SetActive(false);
                if (alliance != null) alliance.SetActive(false);
                if (hordeSecond != null) hordeSecond.SetActive(false);
                if (allianceSecond != null) allianceSecond.SetActive(true);
                break;
            default:
                if (horde != null) horde.SetActive(false);
                if (alliance != null) alliance.SetActive(false);
                if (hordeSecond != null) hordeSecond.SetActive(false);
                if (allianceSecond != null) allianceSecond.SetActive(false);
                break;
        }
    }
}
