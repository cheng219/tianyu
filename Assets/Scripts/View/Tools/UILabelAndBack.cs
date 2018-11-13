//==============================================
//作者：邓成
//日期：2016/3/8
//用途：UILabel和UISprite相互适应的工具界面
//=================================================

using UnityEngine;
using System.Collections;
using System.Text;
using System;

public class UILabelAndBack : MonoBehaviour {
	public UILabel labText;
	public UISprite background;
	/// <summary>
	/// 背景高度-文字高度
	/// </summary>
	public int backgroundAddHight = 15;

	void Start () {
        UIEventListener.Get(gameObject).onClick = OnClickLabelBack;
	}
	/// <summary>
	/// 设置文字类容,并返回文字高度
	/// </summary>
	public int SetTextData(string _text)
	{
		if(labText != null && !string.IsNullOrEmpty(_text))
		{
			labText.transform.parent.gameObject.SetActive(true);
			labText.text = _text.Replace("\\n","\n");
            BoxCollider box = labText.GetComponent<BoxCollider>();
            if (box != null)
            {
                box.size = box.size.SetY(labText.height);
                box.center = box.center.SetY(-1*labText.height/2f);
            }
			return labText.height+backgroundAddHight;
		}
		labText.transform.parent.gameObject.SetActive(false);
		return 0;
	}
	/// <summary>
	/// 设置UI跳转的详细信息
	/// </summary>
	public int SetUIAccess(string uiDes,string uiType)
	{
		StringBuilder builder = new StringBuilder();
        builder.Append(ConfigMng.Instance.GetUItext(349)).Append("\n");
		builder.Append(GameHelper.GetUrlString(uiDes,uiType));
		labText.text = builder.ToString();
		return labText.height;
	}
	/// <summary>
	/// 设置UI跳转的详细信息
	/// </summary>
	public int SetUIAccess(string EquipAddress)
	{
		string[] address = EquipAddress.Split(',');
		StringBuilder builder = new StringBuilder();
		builder.Append(ConfigMng.Instance.GetUItext(349)).Append("\n");
		for (int i = 0,max=address.Length; i < max; i++) {
			builder.Append("[00ff11]").Append(i+1).Append("  ").Append(GameHelper.GetUrlString(address[i])).Append("[-]");
			if(i<max-1)builder.Append("\n");
		}
		labText.transform.parent.gameObject.SetActive(true);
		labText.text = builder.ToString();
		return labText.height;
	}

    protected EquipmentInfo curEquipmentInfo = null;
    public void SetEquipmentInfo(EquipmentInfo _info)
    {
        curEquipmentInfo = _info;
    }

	void ClickLabelBack(GameObject go)
	{
		if(GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneType != SceneType.SCUFFLEFIELD)
		{
			GameCenter.messageMng.AddClientMsg(405);
			return;
		}
		string url = labText.GetUrlAtPosition(UICamera.lastWorldPosition);
		if(!string.IsNullOrEmpty(url))
		{ 
			string[] urlStr = url.Split('|');
			if(urlStr.Length == 3)
			{
				switch(urlStr[0])
				{
				case "0":
					break;
				case "1"://跳到一级界面
					GUIType uiType = (GUIType)Enum.Parse(Type.GetType("GUIType"),urlStr[2]);
                    if (uiType == GUIType.NEWMALL && curEquipmentInfo != null)
                    {
                        GameCenter.buyMng.OpenBuyWnd(curEquipmentInfo);
                    }
                    else
                    {
                        GameCenter.uIMng.SwitchToUI(uiType);
                    }
					ToolTipMng.CloseAllTooltip(); 
					break;
				case "2"://跳到二级界面
					SubGUIType subUiType = (SubGUIType)Enum.Parse(Type.GetType("SubGUIType"),urlStr[2]);
					GameCenter.uIMng.SwitchToSubUI(subUiType);
					ToolTipMng.CloseAllTooltip(); 
					break;
				case "3":
					int npcID = 0;
					if(int.TryParse(urlStr[2],out npcID))
					{
						ToolTipMng.CloseAllTooltip(); 
						GameCenter.uIMng.SwitchToUI(GUIType.NONE);
						GameCenter.taskMng.PublicTraceToNpc(npcID);
					}
					break;
				}
			}else
			{
				Debug.Log("数据错误!");
			}
		}
		Debug.Log("url:"+url);
	}



    /// <summary>
    /// 根据界面id设置一个UILabel链接至UI界面
    /// </summary>
    void OnClickLabelBack(GameObject go)
    {
        if (GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneType != SceneType.SCUFFLEFIELD)
        {
            GameCenter.messageMng.AddClientMsg(405);
            return;
        } 

        string url = labText.GetUrlAtPosition(UICamera.lastWorldPosition);
        if (!string.IsNullOrEmpty(url))
        {
            string[] urlStr = url.Split('|');
            switch (urlStr[0])
            {
                case "0":
                case "1":
                case "2":
                    {
                        if (urlStr.Length == 3)
                        {
                            int uiType = int.Parse(urlStr[2]);
                            if (GameCenter.inventoryMng != null)
                            {
                                GameCenter.inventoryMng.SkipWndById(uiType);
                            }
                            else
                            {
                                Debug.Log(" inventoryMng 为null, 不能使用跳转界面功能");
                            }
                            ToolTipMng.CloseAllTooltip();
                        }
                    } break;
                case "3":
                    int npcID = 0;
                    if (int.TryParse(urlStr[2], out npcID))
                    {
                        ToolTipMng.CloseAllTooltip();
                        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
                        GameCenter.taskMng.PublicTraceToNpc(npcID);
                    }
                    break;
            }
        }
    }  
}
