//==========================================
//作者:吴江
//日期:2015/5/28
//用途：名字




using UnityEngine;
using System.Collections;

public class SelectPlayerStruct : MonoBehaviour {

    /// <summary>
    /// 头像
    /// </summary>
    public UISprite icon;

    protected PlayerBaseInfo playerBaseInfo = null;

    public PlayerBaseInfo Info
    {
        get
        {
            return playerBaseInfo;
        }
    }

    public int LastLoginTime
    {
        get 
        {
            return playerBaseInfo.GetLastLoginTime;
        }
    }

    public int ServerInstanceID
    {
        get
        {
            return playerBaseInfo == null ? -1 : playerBaseInfo.ServerInstanceID;
        }
    }

    public void Refresh(PlayerBaseInfo _info)
    {
        playerBaseInfo = _info;

        if (playerBaseInfo != null)
        {
            
            if (icon != null)
            {
				icon.gameObject.SetActive(true);
                icon.spriteName = playerBaseInfo.IconName;
            }
        }
        else
        {
			if(icon != null)
            	icon.gameObject.SetActive(false);
        }
    }
}
