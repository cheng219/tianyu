//============================
//作者：唐源
//日期：2017/2/27
//用途：新手引导管理类
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
using System;

public class NoviceGuideMng
{
    #region 构造
    public static NoviceGuideMng CreateNew()
    {
        if(GameCenter.noviceGuideMng ==null)
        {
            NoviceGuideMng noviceGuideMng = new NoviceGuideMng();
            noviceGuideMng.Init();
            return noviceGuideMng;
        }
        else
        {
            GameCenter.noviceGuideMng.UnRegister();
            GameCenter.noviceGuideMng.Init();
            return GameCenter.noviceGuideMng;
        }
    }
    /// <summary>
    /// 初始化(注册监听)
    /// </summary>
    protected void Init()
    {
        MsgHander.Regist(0xD779, S2C_ServerStartGuide);
        MsgHander.Regist(0xD804, S2C_GuideSeqencingList);
    }
    /// <summary>
    /// 注销
    /// </summary>
    protected void UnRegister()
    {
        MsgHander.UnRegist(0xD779, S2C_ServerStartGuide);
        MsgHander.UnRegist(0xD804, S2C_GuideSeqencingList);
        startGuide = false;
        newFunctionCopyId = 0;
        OnGuideOver.Clear();
    }
    #endregion
    #region 数据
    bool startGuide = false;
    /// <summary>
    /// 是否在进行引导
    /// </summary>
    public bool StartGuide
    {
        get
        {
            return startGuide;
        }
    }
    /// <summary>
    /// 新手引导进入的副本ID
    /// </summary>
    public int newFunctionCopyId = 0;
    /// <summary>
    /// 引导开启的静态配置数据
    /// </summary>
    OpenNewFunctionGuideRef refData;
    public OpenNewFunctionGuideRef RefData
    {
        get
        {
            return refData;
        }
       set
        {
            refData = value;
        }
    }

    #endregion
    #region 事件
    //   /// <summary>
    //   /// 更新新功能事件
    //   /// </summary>
    public Action<FunctionDataInfo> UpdateFunctionData;
    /// <summary>
    /// 开启引导事件
    /// </summary>
    public Action<OpenNewFunctionGuideRef> UpdateGuideData;

    public Action OnOutNewbieCoppyEvent;
    /// <summary>
    /// 引导结束
    /// </summary>
    public List<EventDelegate> OnGuideOver = new List<EventDelegate>();
    /// <summary>
    /// 特殊引导
    /// </summary>
    public System.Action<OpenNewFunctionGuideRef> OnSpecilGuild;
    #endregion

    public void OutNewbieCoppy()
    {
        if (OnOutNewbieCoppyEvent != null)
            OnOutNewbieCoppyEvent();
    }

    #region 协议
    #region C2S协议
    /// <summary>
    /// 指引进度
    /// </summary>
    public void C2S_GuideSeqencing(int seqencing)
    {
        //Debug.Log("发起获取指引进度的请求");
        pt_new_function_open_d803 info = new pt_new_function_open_d803();
        info.id = seqencing;
        NetMsgMng.SendMsg(info);
    }
    #endregion
    #region S2C协议
    void S2C_ServerStartGuide(Pt info)
    {
        pt_update_guidance_d779 msg = info as pt_update_guidance_d779;
        if (msg != null)
        {
            OpenGuide(msg.guidance_id, 1);
            //Debug.Log("获取指引数据");
        }
    }
    protected void S2C_GuideSeqencingList(Pt _info)
    {
        pt_new_function_open_aready_d804 info = _info as pt_new_function_open_aready_d804;
        if (info != null)
        {
            GameCenter.mainPlayerMng.FunctionSequence = info.openlists;
            //Debug.Log("功能开启的进度："+ info.openlists);
        }
        GameCenter.mainPlayerMng.InitFunctionData();
    }
    #endregion
    #endregion
    #region 辅助功能
    /// <summary>
    /// 开启指引
    /// </summary>
    public void OpenGuide(int _type, int _step = 1)
    {
        //Debug.Log("OpenGuide(int _type, int _step = 1)");
        if (_type == 100022 || _type == 100029 || _type == 100030)
        {
            return;//去掉退出副本引导
        }
        OpenNewFunctionGuideRef guideData = ConfigMng.Instance.GetOpenNewFunctionGuideRef(_type, _step);
        if (guideData == null)
        {
            Debug.LogError("没有找到Type = " + _type + ",Step = " + _step + "的指引数据！");
            return;
        }
        OpenGuide(guideData);
    }
    /// <summary>
    /// 开启指引
    /// </summary>
    public void OpenGuide(OpenNewFunctionGuideRef _guideData)
    {
        //Debug.Log("OpenGuide(OpenNewFunctionGuideRef _guideData)");
        GameCenter.uIMng.ReleaseGUI(GUIType.NEWGUID);
        if (_guideData == null) return;
        if (!startGuide)
            startGuide = true;
        if (UpdateGuideData != null)
            UpdateGuideData(_guideData);
    }
    /// <summary>
    /// 指引结束
    /// </summary>
    public void OverGuide()
    {
        if (startGuide) startGuide = false;
    }

    #endregion
}
