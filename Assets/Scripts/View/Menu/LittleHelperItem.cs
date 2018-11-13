//=====================================
//作者:黄洪兴
//日期:2016/6/1
//用途:小助手UI
//========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
/// 小助手UIUI组件
/// </summary>
public class LittleHelperItem : MonoBehaviour
{
    #region 控件数据
    public UILabel itemName;
    public GameObject[] stars = new GameObject[5];
    public GameObject GoBtn;


    #endregion
    #region 数据

    /// <summary>
    /// 当前填充的数据
    /// </summary>
    LittleHelperRef littleHelperRef;
    //	protected SkillInfo oldSkillinfo;  //for upgrade effect -by ms
    #endregion
    // Use this for initialization
    void Start()
    {
        if (GoBtn != null)
        {
            UIEventListener.Get(GoBtn).onClick -= SetBtn;
            UIEventListener.Get(GoBtn).onClick += SetBtn;
        }
        
    }

    void SetBtn(GameObject go)
    {
        if (littleHelperRef == null)
            return;
        if (littleHelperRef.buttontype == 1)
        {

            Debug.Log("场景ID" + ConfigMng.Instance.GetNPCAIRefByType(littleHelperRef.npcId).scene + "坐标" + ConfigMng.Instance.GetNPCAIRefByType(littleHelperRef.npcId).sceneX + ":" + ConfigMng.Instance.GetNPCAIRefByType(littleHelperRef.npcId).sceneY);
           // GameCenter.curMainPlayer.CancelCommands();
            GameCenter.curMainPlayer.GoTraceTarget(ConfigMng.Instance.GetNPCAIRefByType(littleHelperRef.npcId).scene, ConfigMng.Instance.GetNPCAIRefByType(littleHelperRef.npcId).sceneX, ConfigMng.Instance.GetNPCAIRefByType(littleHelperRef.npcId).sceneY);
            //GameCenter.taskMng.TraceToScene(ConfigMng.Instance.GetNPCAIRefByType(littleHelperRef.npcId).scene, new Vector3(ConfigMng.Instance.GetNPCAIRefByType(littleHelperRef.npcId).sceneX, 0, ConfigMng.Instance.GetNPCAIRefByType(littleHelperRef.npcId).sceneY));
            //Command_MoveTo moveto = new Command_MoveTo();
            //moveto.destPos = ActorMoveFSM.LineCast(new Vector3(ConfigMng.Instance.GetNPCAIRefByType(littleHelperRef.npcId).sceneX, 0, ConfigMng.Instance.GetNPCAIRefByType(littleHelperRef.npcId).sceneY), true);
            //moveto.maxDistance = 0f;
            //GameCenter.curMainPlayer.commandMng.PushCommand(moveto);
            GameCenter.uIMng.SwitchToUI(GUIType.NONE);

        }
        else if (littleHelperRef.buttontype == 2)
        {
            GameCenter.uIMng.SwitchToUI((GUIType)Enum.Parse(typeof(GUIType), littleHelperRef.uiType, true));
            
        }
        else if (littleHelperRef.buttontype == 3)
        {

            GameCenter.uIMng.SwitchToSubUI((SubGUIType)Enum.Parse(typeof(SubGUIType), littleHelperRef.uiType, true));
        }

    }







    /// <summary>
    /// 填充数据
    /// </summary>
    /// <param name="_info"></param>
    public void FillInfo(LittleHelperRef _info)
    {
        if (_info == null)
        {
            littleHelperRef = null;
            return;
        }
        else
        {
            littleHelperRef = _info;
        }
        Refresh();
    }


    void ShowStar(int _num)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            if (i >= _num)
            {
                stars[i].SetActive(false);
            }
            else
            {
                stars[i].SetActive(true);
            }
        }
    }
    /// <summary>
    /// 刷新表现
    /// </summary>
    public void Refresh()
    {
        if (itemName != null)
            itemName.text = littleHelperRef.name;
        ShowStar(littleHelperRef.star);
        
    }





}
