//======================================================
//作者:朱素云
//日期:2017/1/12
//用途:火焰山战场阵营图分仙界和妖界
//======================================================
using UnityEngine;
using System.Collections;

public class BattleFieldCamp : MonoBehaviour 
{
    /// <summary>
    /// 积分滑条
    /// </summary>
    public UISlider jifenSli;
    /// <summary>
    /// 积分
    /// </summary>
    public UILabel scoreLab;
    /// <summary>
    /// 帅的状态是否交战
    /// </summary>
    public GameObject general;
    /// <summary>
    /// 上将的复活时间 
    /// </summary>
    public UITimer upSoldiersRecover;
    /// <summary>
    /// 上将交战中
    /// </summary>
    public GameObject upSoldiersFight;
    /// <summary>
    /// 下将的复活时间
    /// </summary>
    public UITimer downSoldiersRecover;
    /// <summary>
    /// 下将交战中
    /// </summary>
    public GameObject downSoldiersFight;

    public UISpriteEx upSoldiersEx;
    public UISpriteEx downSoldiersEx;
    public TweenWidth generalTween;
    public TweenWidth upTween;
    public TweenWidth downTween;

    void OnEnable()
    { 
    
    }

    void OnDisable()
    { 
    
    }

    /// <summary>
    /// 设置阵营积分
    /// </summary>
    public void SetScore(st.net.NetBase.mountain_amount_score _score)
    {
        if (jifenSli != null) jifenSli.value = (float)_score.amount_score / 100000;
        if (scoreLab != null) scoreLab.text = _score.amount_score.ToString();
    }

    /// <summary>
    /// 设置阵营战斗状态
    /// </summary> 
    public void SetCampData(BattleCampInfo _info) 
    {
        //Debug.Log("generalState :   " + _info.generalState + "      upGeneralStateCD : " + _info.upGeneralStateCD + "      downGeneralStateCD : " + _info.downGeneralStateCD);
        SetGeneralState(_info.generalState < 0);
        SetUpSoldiersState(_info.upGeneralStateCD);
        SetDownSoldiersState(_info.downGeneralStateCD); 
    }


    /// <summary>
    /// 设置帅的状态
    /// </summary> 
    void SetGeneralState(bool _b)
    {
        if (general != null) general.SetActive(_b);
        if (generalTween != null)
        {
            if (_b) generalTween.enabled = true;
            else
                generalTween.enabled = false;
        }
    }

    /// <summary>
    /// 设置上将的状态
    /// </summary>
    /// <param name="_b"></param>
    void SetUpSoldiersState(int _time)
    {
        if (_time > 0)//上将在复活
        {
            if (upSoldiersFight != null) upSoldiersFight.SetActive(false);
            if (upSoldiersEx != null) upSoldiersEx.IsGray = UISpriteEx.ColorGray.Gray;
            if (upTween != null) upTween.enabled = false;
            if (upSoldiersRecover != null)
            {
                upSoldiersRecover.transform.parent.gameObject.SetActive(true);
                upSoldiersRecover.StartIntervalTimer(_time);
                upSoldiersRecover.onTimeOut = (x) =>
                {
                    if (upSoldiersFight != null) upSoldiersFight.SetActive(true); 
                    upSoldiersRecover.transform.parent.gameObject.SetActive(false);
                    if (upSoldiersEx != null) upSoldiersEx.IsGray = UISpriteEx.ColorGray.normal;
                };
            }
        }
        else if (_time < 0)//上将在交战
        {
            if (upSoldiersFight != null) upSoldiersFight.SetActive(true);
            if (upSoldiersEx != null) upSoldiersEx.IsGray = UISpriteEx.ColorGray.normal;
            if (upTween != null) upTween.enabled = true;
            if (upSoldiersRecover != null)
            {
                upSoldiersRecover.transform.parent.gameObject.SetActive(false); 
            }
        }
        else//无状态
        {
            if (upSoldiersFight != null) upSoldiersFight.SetActive(false);
            if (upSoldiersEx != null) upSoldiersEx.IsGray = UISpriteEx.ColorGray.normal;
            if (upTween != null) upTween.enabled = false;
            if (upSoldiersRecover != null)
            {
                upSoldiersRecover.transform.parent.gameObject.SetActive(false); 
            }
        }
    }

    /// <summary>
    /// 设置下将的状态
    /// </summary>
    /// <param name="_b"></param>
    void SetDownSoldiersState(int _time)
    {
        if (_time > 0)//下将在复活
        {
            if (downSoldiersFight != null) downSoldiersFight.SetActive(false);
            if (downSoldiersEx != null) downSoldiersEx.IsGray = UISpriteEx.ColorGray.Gray;
            if (downTween != null) downTween.enabled = false;
            if (downSoldiersRecover != null)
            {
                downSoldiersRecover.transform.parent.gameObject.SetActive(true);
                downSoldiersRecover.StartIntervalTimer(_time);
                downSoldiersRecover.onTimeOut = (x) =>
                {
                    if (downSoldiersFight != null) downSoldiersFight.SetActive(true);
                    downSoldiersRecover.transform.parent.gameObject.SetActive(false);
                    if (downSoldiersEx != null) downSoldiersEx.IsGray = UISpriteEx.ColorGray.normal;
                };
            }
        }
        else if (_time < 0)//下将在交战
        {
            if (downSoldiersFight != null) downSoldiersFight.SetActive(true);
            if (downSoldiersEx != null) downSoldiersEx.IsGray = UISpriteEx.ColorGray.normal;
            if (downTween != null) downTween.enabled = true;
            if (downSoldiersRecover != null)
            {
                downSoldiersRecover.transform.parent.gameObject.SetActive(false); 
            }
        }
        else
        {
            if (downSoldiersFight != null) downSoldiersFight.SetActive(false);
            if (downSoldiersEx != null) downSoldiersEx.IsGray = UISpriteEx.ColorGray.normal;
            if (downTween != null) downTween.enabled = false;
            if (downSoldiersRecover != null)
            {
                downSoldiersRecover.transform.parent.gameObject.SetActive(false); 
            }
        }
    }
}
