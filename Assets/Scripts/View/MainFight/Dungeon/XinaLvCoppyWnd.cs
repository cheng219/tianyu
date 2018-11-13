//===============================
//作者：邓成
//日期：2016/4/26
//用途：仙侣副本显示类
//===============================

using UnityEngine;
using System.Collections;

public class XinaLvCoppyWnd : SubWnd {
	public GameObject miyuGo;//蜜语
	public UILabel labDes;
	public UITimer timer;

    public UILabel poem1;
    public UILabel poem2;

	protected override void OnOpen ()
	{
		base.OnOpen (); 
	}
	protected override void OnClose ()
	{
		base.OnClose ();
	}
	protected override void HandEvent (bool _bind)
	{
		base.HandEvent (_bind);
		if(_bind)
		{
			GameCenter.dungeonMng.OnDungeonTimeUpdate += ShowTime;
            GameCenter.dungeonMng.OnCoupleCopyUpdate += ReplaceChar;
            GameCenter.dungeonMng.OnGetCouplePoemId += ShowPoem;
		}else
		{
			GameCenter.dungeonMng.OnDungeonTimeUpdate -= ShowTime;
            GameCenter.dungeonMng.OnCoupleCopyUpdate -= ReplaceChar;
            GameCenter.dungeonMng.OnGetCouplePoemId -= ShowPoem;
		}
	}
	void ShowTime()
	{
		int time = GameCenter.dungeonMng.DungeonTime;
		if(timer != null)timer.StartIntervalTimer(time);
        ShowPoem();
	}
    void ShowPoem()
    { 
        WeddingCoppyRef copyRef = ConfigMng.Instance.GetWeddingCoppyRef(GameCenter.dungeonMng.coupleCopyId);  
        if (copyRef == null) return; 
        poem1.text = copyRef.Des1;
        poem2.text = copyRef.Des2;
    }
    void ReplaceChar()
    {
        string str = GameCenter.dungeonMng.coupleReplaceChar; 
        if (!string.IsNullOrEmpty(str))
        {
            poem1.text = poem1.text.Replace(str, "[ff0000]" + str + "[-]");
            poem2.text = poem2.text.Replace(str, "[ff0000]" + str + "[-]");
        }
    }
}
