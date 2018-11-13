//======================================================
//作者:朱素云
//日期:2017/3/30
//用途:在线奖励ui
//======================================================
using UnityEngine;
using System.Collections;

public class OnlineRewardUi : MonoBehaviour {

    public GameObject chooseBack;
    public TweenScale areadyTake;
    public UISpriteEx iconEx;
    public ItemUI item;
    public UILabel rewardRasiol;//奖池元宝百分比
    public GameObject diamondSprite;
    public float rasiol;//奖励
    public Transform parent;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CreatItem()
    {
        if (parent != null)
            item = ItemUI.CreatNew(parent, Vector3.zero, Vector3.one); 
    }
}
