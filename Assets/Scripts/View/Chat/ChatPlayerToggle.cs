//======================================================
//作者:黄洪兴
//日期:2016/7/21
//用途:私聊组件
//======================================================


using UnityEngine;
using System.Collections;

/// <summary>
/// 聊天玩家Toggle
/// </summary>
public class ChatPlayerToggle : MonoBehaviour {


    public GameObject redObj;
    public UILabel redNum;
    public GameObject privateBtn;
    public UILabel playerName;
    public GameObject checkMark;

    protected ChatInfo curChatInfo = null;

    #region unity3D

    void Awake()
    {
      //  EventDelegate.Set(toggle.onChange, SelectToggle);
    }

    protected void SelectToggle(GameObject _obj)
    {

        if(playerName!=null)
            GameCenter.chatMng.PrivateChatTo(playerName.text);
    }

    void OnEnable()
    {
        if (privateBtn!=null) UIEventListener.Get(privateBtn).onClick += SelectToggle;
    }

    void OnDisable()
    {
        if (privateBtn!=null) UIEventListener.Get(privateBtn).onClick -= SelectToggle;
    }

     public void FillInfo(string _name) 
    {
        bool b= false;
        if (redNum != null)
        {
            redNum.gameObject.SetActive(GameCenter.chatMng.CurTargetName == _name);
            if (GameCenter.chatMng.PrivateChatNewNum.ContainsKey(_name))
            {
                redNum.text = GameCenter.chatMng.PrivateChatNewNum[_name].ToString();
                if (GameCenter.chatMng.PrivateChatNewNum[_name]>0)
                {
                    b = true;
                redNum.gameObject.SetActive(true);
                }
            }
            else
            {
                redNum.gameObject.SetActive(false);
            }
        }
        if (redObj != null)
        {
            redObj.gameObject.SetActive(b);
        }
        if (this != null) this.gameObject.SetActive(true);
        if (playerName!=null) playerName.text = _name;
        if (checkMark != null)
            checkMark.SetActive(_name == GameCenter.chatMng.CurTargetName);
    }

    #endregion

}
