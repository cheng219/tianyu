//======================================================
//作者:黄洪兴
//日期:2016/07/29
//用途:地图玩家头像
//======================================================
using UnityEngine;
using System.Collections;

public class MapPlayerHead : MonoBehaviour {


    public UISprite headSprite;


    public void Refresh()
    {
        if (headSprite != null)
        {
                headSprite.spriteName = GameCenter.mainPlayerMng.MainPlayerInfo.HeadIconName;
                //headSprite.MakePixelPerfect();
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
