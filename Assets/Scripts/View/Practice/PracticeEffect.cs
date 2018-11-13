//======================================================
//作者:朱素云
//日期:2016/7/22
//用途:修行特效
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PracticeEffect : MonoBehaviour
{
    /// <summary>
    /// 修行仙气数或灵气数
    /// </summary>
    public UILabel practiceVal;
    /// <summary>
    /// 灵气特效
    /// </summary>
    public GameObject effectDust;
    /// <summary>
    /// 灵气爆炸特效
    /// </summary>
    public UIFxAutoActive lingqiBreak;
    /// <summary>
    /// 仙气特效
    /// </summary>
    public GameObject effectReike;
    /// <summary>
    /// 仙气爆炸特效
    /// </summary>
    public UIFxAutoActive xianqiBreak;
    /// <summary>
    /// 人体脉络图特效
    /// </summary>
    public List<GameObject> bodyEffectList = new List<GameObject>();
    /// <summary>
    /// 显示人体脉络图特效
    /// </summary>
    /// <param name="_num">亮几个部位当前所处境界id</param>
    public void ShowBodyEffects(int _num)
    {
        for (int i = 0,max = bodyEffectList.Count; i < max; i++)
        {
            if (i < _num)
            { 
                bodyEffectList[i].SetActive(true);
            }
            else
            { 
                bodyEffectList[i].SetActive(false);
            }
        }
    }

    #region 灵修特效
    /// <summary>
    /// 爆炸特效
    /// </summary>
    public UIFxAutoActive effectBoo;
    /// <summary>
    /// 移动特效
    /// </summary>
    public Transform moveStar;
    /// <summary>
    /// 移动到星星后爆炸的特效
    /// </summary>
    public UIFxAutoActive starBoo;
    private LingXiuType type = LingXiuType.NONE;
    /// <summary>
    /// 播放灵修经验升级特效
    /// </summary>
    public void ShowLXEffect()
    {
        if (effectBoo != null)
        {
            effectBoo.ReShowFx();
        }
    }

    /// <summary>
    /// 播放等级升级特效
    /// </summary>
    /// <param name="_position"></param>
    public void ShowLXEffect(Vector3 _position, LingXiuType _type)
    { 
        type = _type;
        starBoo.transform.localPosition = _position; 
        if (effectBoo != null)
        { 
            effectBoo.ShowFx();
            //effectBoo.ShowFx(()=>{
                if(moveStar != null)
                {
                    moveStar.gameObject.SetActive(true);
                    moveStar.localPosition = Vector3.zero;//回位 
                    TweenPosition tween = TweenPosition.Begin(moveStar.gameObject, 0.9f, _position); 
                    EventDelegate.Remove(tween.onFinished, ShowStarBoo);
                    EventDelegate.Add(tween.onFinished, ShowStarBoo); 
                }
           // }); 
        }
    }

    void ShowStarBoo()
    {
        if (starBoo != null)
        { 
            starBoo.ShowFx(() => {
                if (moveStar != null) moveStar.gameObject.SetActive(false);
                moveStar.localPosition = Vector3.zero;//回位
                if (GameCenter.mercenaryMng.OnLingXiuStarUpdate != null && type != LingXiuType.NONE)
                    GameCenter.mercenaryMng.OnLingXiuStarUpdate(type);
            });
        } 
    }
    #endregion
}
