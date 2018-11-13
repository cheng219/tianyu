//==================================
//作者：朱素云
//日期：2016/4/11
//用途：飞升界面
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoaringWnd : SubWnd
{ 
    public SoaringAttibuteUi curAttributeUi;
    public SoaringAttibuteUi nextAttributeUi; 
    public UISprite[] starts; 
    public UIButton gotoPractice;  
    public SoaringPromoteUi promote;//提升
    public SoaringPromoteUi breakOut;//境界提升 
    private PracticeEffect practiceEffect;
    public Transform manji;
    void Awake()
    { 
        practiceEffect = this.GetComponent<PracticeEffect>();
        if (manji != null) manji.gameObject.SetActive(false);
    }
    void HideAllStar()
    {
        for (int i = 0; i < starts.Length; i++)
        {
            starts[i].gameObject.SetActive(false);
        }
    }
    protected override void OnOpen()
    { 
        base.OnOpen();
        if (gotoPractice != null) UIEventListener.Get(gotoPractice.gameObject).onClick = delegate { GameCenter.uIMng.SwitchToUI(GUIType.PRACTICE); }; 
        GameCenter.practiceMng.OnPracticeUpdata += Refresh;
        Refresh(); 
    }

    protected override void OnClose()
    {
        base.OnClose();
        GameCenter.practiceMng.OnPracticeUpdata -= Refresh;
        CancelInvoke("HideAllStar");
    }

    void Refresh()
    {
        PracticeData data = GameCenter.practiceMng.data;   
		if(data == null){return ;}
        if (data.stratNum >= ConfigMng.Instance.GetStyliteRefTable().Count - 1)//满级
        {
            int id = ConfigMng.Instance.GetStyliteRefTable().Count - 1;
            if (curAttributeUi != null) curAttributeUi.SetFullLev(id);
            if (nextAttributeUi != null) nextAttributeUi.SetFullLev(id);
            for (int i = 0; i < starts.Length; i++)
            {
                starts[i].gameObject.SetActive(true);
            }
            if (breakOut != null) breakOut.gameObject.SetActive(false);
            if (promote != null) promote.gameObject.SetActive(false);
            if (manji != null) manji.gameObject.SetActive(true);
            return;
        }
        if (manji != null && manji.gameObject.activeSelf) manji.gameObject.SetActive(false);
		
		
        if ((70 * (data.stratNum / 70) + (data.stratNum / 70 - 1)) == data.stratNum && data.stratNum > 0)//突破
        {
            FlyUpRef flyUpRef = ConfigMng.Instance.GetFlyUpRef((data.stratNum) / 70 + 1);
            if (flyUpRef != null)
            {
                if (breakOut != null)
                { 
                    if(curAttributeUi != null)curAttributeUi.SetLeft(data.stratNum, true);
                    if(nextAttributeUi != null)nextAttributeUi.SetLeft(data.stratNum, true);
                    breakOut.gameObject.SetActive(true);
                    breakOut.Show(flyUpRef.xianQi, true, flyUpRef.needLev);
                }
                if (promote != null) promote.gameObject.SetActive(false);
            }
        }
        else//提升
        {
            StyliteRef nextStyliteRef = ConfigMng.Instance.GetStyliteRefByStart(data.stratNum + 1);
            if (nextStyliteRef != null)
            {
                if (promote != null)
                { 
                    if (curAttributeUi != null) curAttributeUi.SetLeft(data.stratNum, false);
                    if (nextAttributeUi != null) nextAttributeUi.SetLeft(data.stratNum, false);
                    promote.gameObject.SetActive(true);
                    promote.Show(nextStyliteRef.lingqi, false);
                }
                if (breakOut != null) breakOut.gameObject.SetActive(false);
            }
        }
        if (practiceEffect != null && GameCenter.practiceMng.bodyEffectNum > 0)
        { 
            practiceEffect.ShowBodyEffects(GameCenter.practiceMng.bodyEffectNum / 71 + 1);
        }


        for (int i = 0; i < starts.Length; i++)
        {
            if (starts[i] != null)
            {
                if (data.stratNum < 70)
                {
                    if ((data.stratNum) % 7 > i && (70 * (data.stratNum / 70) + (data.stratNum / 70 - 1)) != data.stratNum)
                        starts[i].gameObject.SetActive(true);
                    else
                        starts[i].gameObject.SetActive(false);
                    if ((data.stratNum) % 7 == 0 && data.stratNum > 0 && (70 * (data.stratNum / 70) + (data.stratNum / 70 - 1)) != data.stratNum)
                    {
                        starts[i].gameObject.SetActive(true);
                        CancelInvoke("HideAllStar");
                        Invoke("HideAllStar", 0.3f);
                    }
                }
                else
                {
                    if ((data.stratNum - (data.stratNum / 71)) % 7 > i && (70 * (data.stratNum / 70) + (data.stratNum / 70 - 1)) != data.stratNum)
                        starts[i].gameObject.SetActive(true);
                    else
                        starts[i].gameObject.SetActive(false);
                    if ((data.stratNum - (data.stratNum / 71)) % 7 == 0 && (70 * (data.stratNum / 70) + (data.stratNum / 70 - 1)) != data.stratNum)
                    {
                        starts[i].gameObject.SetActive(true);
                        CancelInvoke("HideAllStar");
                        Invoke("HideAllStar", 0.3f);
                    }
                }
            }
        }
        if (((70 * (data.stratNum / 70) + (data.stratNum / 70 - 1)) == data.stratNum) && data.stratNum > 0)
        {
            for (int i = 0; i < starts.Length; i++)
            {
                if (starts[i] != null) starts[i].gameObject.SetActive(true);
            }
        }
    }
}
