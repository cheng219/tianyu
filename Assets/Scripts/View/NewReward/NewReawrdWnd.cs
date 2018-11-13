//===============================
//日期：2017/2/8
//作者：唐源
//用途描述:有新奖励窗口界面
//===============================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewReawrdWnd : GUIBase {
    #region 数据
    //private Dictionary<int, GameObject> itemDic = new Dictionary<int, GameObject>();
    #endregion 
    #region UI控件
    public UIButton closeBtn;
    public UIButton getRewardBtn;
    public UILabel newFunctionHintsText;
    public UILabel title;
    public GameObject oldItem;
    public ItemUIContainer uiContainer;
    public UITweener tween_1;
    public UITweener tween_2;
    public GameObject effect;
    private bool canGetReward;
    public UIExGrid exGrid;

    public UITexture modelTexture;
    #endregion
    void Awake()
    {
        mutualExclusion = true;
        AddUIEvent();
    }
    void AddUIEvent()
    {
        if (closeBtn != null)
            UIEventListener.Get(closeBtn.gameObject).onClick = Close;
        if (getRewardBtn != null)
            UIEventListener.Get(getRewardBtn.gameObject).onClick = OnClickGetReward;
    }
    #region 事件句柄
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            RefreshNewFunctionHints(TaskType.Main);
            GameCenter.taskMng.OnTaskGroupUpdate += RefreshNewFunctionHints;
            GameCenter.mainPlayerMng.UpdateFunctionReward += RefreshNewFunctionHints;
        }
        else
        {
            GameCenter.taskMng.OnTaskGroupUpdate -= RefreshNewFunctionHints;
            GameCenter.mainPlayerMng.UpdateFunctionReward -= RefreshNewFunctionHints;
            //itemDic.Clear();
        }
    }
    #endregion
    #region 控件事件
    /// <summary>
    /// 关闭窗口
    /// </summary>
    void Close(GameObject _obj)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
    }
    /// <summary>
    /// 领奖
    /// </summary>
    void OnClickGetReward(GameObject _go)
    {
        UISpriteEx spEx = getRewardBtn.GetComponentInChildren<UISpriteEx>();
        if (spEx!=null&&spEx.IsGray == UISpriteEx.ColorGray.normal)
        {
            EffectAnimation();
        }
    }
    #endregion
    #region 界面刷新
    void RefreshNewFunctionHints(TaskType _type)
    {
        if (GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef == null)
            return;
        if (_type == TaskType.Main && GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef.sort == SceneType.SCUFFLEFIELD)
        {
            NewFunctionHintsRef curFunctionRef = ConfigMng.Instance.GetNewFunctionHintsById(GameCenter.mainPlayerMng.curGetRewardStep);
            if (curFunctionRef != null)
            {
                if (GameCenter.uIMng.CurOpenType != GUIType.NPCDIALOGUE && GameCenter.uIMng.CurOpenType != GUIType.NEWFUNCTIONTIPUI)
                {
                    bool canGetReward = GameCenter.taskMng.CurTaskCanGetReward(1, curFunctionRef.Step);
                    //itemDic.Clear();
                    ShowNewFunctionHints(curFunctionRef, canGetReward);
                    return;
                }
            }
        }
    }
    void ShowNewFunctionHints(NewFunctionHintsRef _ref, bool _canGetReward)
    {
        canGetReward = _canGetReward;
        if (newFunctionHintsText != null)
            newFunctionHintsText.text = _ref.text;
        if (title != null) title.text = _ref.title;
        if (modelTexture != null)
        {
            switch (_ref.modelType)
            {
                case ObjectType.MOB:
                    GameCenter.previewManager.TryPreviewSingelMonster(_ref.modelId, modelTexture, PreviewConfigType.Dialog,_ref.animName);
                    break;
                case ObjectType.NPC:
                    GameCenter.previewManager.TryPreviewSingelNPC(_ref.modelId, modelTexture, PreviewConfigType.Dialog, _ref.animName);
                    break;
                case ObjectType.Entourage:
                    GameCenter.previewManager.TryPreviewSingelEntourage(_ref.modelId, modelTexture, _ref.animName);
                    break;
                case ObjectType.Mount:
                    GameCenter.previewManager.TryPreviewSingelMount(_ref.modelId, modelTexture, _ref.animName);
                    break;
                case ObjectType.Player:
                    GameCenter.previewManager.TryPreviewSinglePlayer(GameCenter.mainPlayerMng.MainPlayerInfo, modelTexture, true, _ref.animName);
                    break;
                case ObjectType.DropItem:
                    EquipmentInfo info = new EquipmentInfo(_ref.modelId,EquipmentBelongTo.PREVIEW);
                    GameCenter.previewManager.TryPreviewSingleEquipment(info, modelTexture);
                    break;
            }
        }
        if (uiContainer != null)
        {
            exGrid.repositionNow = true;
            uiContainer.RefreshItems(_ref.reward, _ref.reward.Count, _ref.reward.Count);
            exGrid.Reposition();   
        }
        if (getRewardBtn != null)
        {
            UISpriteEx spEx = getRewardBtn.GetComponentInChildren<UISpriteEx>();
            if (spEx != null)
            {
                if (_canGetReward)
                {
                    spEx.IsGray = UISpriteEx.ColorGray.normal;
                }
                else
                {
                    spEx.IsGray = UISpriteEx.ColorGray.Gray;
                }
            }
        }
    }
    //Tween 动画特效
    void EffectAnimation()
    {
        Effect();
        GameCenter.mainPlayerMng.C2S_ReqGetFuncReward(GameCenter.mainPlayerMng.curGetRewardStep);
        if (GameCenter.mainPlayerMng.curGetRewardStep == 8)
        {
            OnClose();
        }
        //EventDelegate.Add(tween_1.onFinished, Effect);
        //Effect();
    }
    public void Effect()
    {
       if (effect!=null)
        effect.SetActive(true);
        //tween_2.gameObject.SetActive(true);
        //tween_2.ResetToBeginning();
        //tween_2.PlayForward();
        //GameCenter.uIMng.ReleaseGUI(GUIType.NEWREWARDPREVIEW);
    }
    public void PlayerAnimation()
    {
        //tween_1.gameObject.SetActive(true);
        //tween_1.ResetToBeginning();
        //tween_1.PlayForward();
        //Debug.Log("执行完特效之后");
        if (!canGetReward)
             Invoke("OnClose", 1.0f);
    }
    public void OnClose()
    {
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
    }
    #endregion
}
