/// <summary>
/// 竞技场入口界面
/// 何明军
/// 2016/4/19
/// </summary>
using UnityEngine;
using System.Collections;

public class ArenaInPlayerUI : MonoBehaviour {

	public UISprite icon;
	public UILabel fight;
	public UILabel name;
	public UILabel rank;
	
	public GameObject killBtn;
	public GameObject checkBtn;
	public GameObject firendBtn;
	public GameObject emailBtn;
	public GameObject chatBtn;
	public GameObject btnParent;

    void GoToRecharge()
    {
        MessageST goldMst = new MessageST();
        goldMst.messID = 210;
        goldMst.delYes = delegate
        {
            GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
        };
        GameCenter.messageMng.AddClientMsg(goldMst);
    }
	public void SetArenaPlayer(OtherPlayerInfo other){
        if (icon != null)
        {
            icon.spriteName = other.IconHalf;
            icon.MakePixelPerfect();
        }
        if (fight != null) fight.text = other.FightValue.ToString();
		if(name != null)name.text = other.Name;
		if(rank != null)rank.text = ConfigMng.Instance.GetUItext(20,new string[1]{GameCenter.arenaMng.ArenaServerDataInfo.GetPlayerRank(other.ServerInstanceID)});
        if (btnParent != null) btnParent.SetActive(false);
		
		UIEventListener.Get(killBtn).onClick = delegate {
            if (GameCenter.arenaMng.ArenaServerDataInfo.surplus_time > 0)
            {
                MessageST mst = new MessageST();
                mst.messID = 236;
                GameCenter.messageMng.AddClientMsg(mst);
            }
            else if (GameCenter.arenaMng.ArenaServerDataInfo.challenge_num <= 0)
            {
                //MessageST mst = new MessageST();
                //mst.messID = 168;
                //GameCenter.messageMng.AddClientMsg(mst);

                StepConsumptionRef stepData = ConfigMng.Instance.GetStepConsumptionRef(GameCenter.arenaMng.ArenaServerDataInfo.buyChallengeTimes + 1);
                int goldNum = 0;
                if (stepData.areaTime.Count > 0)
                {
                    goldNum = stepData.areaTime[0].count;
                }
                else
                {
                    Debug.Log("StepConsumptionRef配置出错");
                    return;   
                }
                if (!GameCenter.arenaMng.ShowAreaTimeTip)
                {
                    if (GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount < (ulong)goldNum)
                    {
                        GoToRecharge();
                    }
                    else
                    {
                        GameCenter.arenaMng.C2S_ReqBuyChallengeTimes();
                        GameCenter.arenaMng.C2S_RepArenaKill(other.ServerInstanceID);
                    }
                }
                else
                {
                    MessageST mst = new MessageST();
                    object[] pa = { 1 };
                    mst.pars = pa;
                    mst.delPars = delegate(object[] ob)
                    {
                        if (ob.Length > 0)
                        {
                            bool b = (bool)ob[0];
                            if (b)
                                GameCenter.arenaMng.ShowAreaTimeTip = false;
                        }
                    };
                    mst.messID = 488;
                    mst.words = new string[1] { goldNum.ToString() };
                    mst.delYes = delegate
                    {
                        if (GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount < (ulong)goldNum)
                        {
                            GoToRecharge();
                        }
                        else
                        {
                            GameCenter.arenaMng.C2S_ReqBuyChallengeTimes();
                            GameCenter.arenaMng.C2S_RepArenaKill(other.ServerInstanceID);
                        }
                    };
                    GameCenter.messageMng.AddClientMsg(mst);
                }
            }
            else
                GameCenter.arenaMng.C2S_RepArenaKill(other.ServerInstanceID);
		};
		UIEventListener.Get(checkBtn).onClick = delegate {
			GameCenter.previewManager.C2S_AskOPCPreview(other.ServerInstanceID);
		};
		UIEventListener.Get(firendBtn).onClick = delegate {
            GameCenter.friendsMng.C2S_AddFriend(other.ServerInstanceID);
		};
		UIEventListener.Get(emailBtn).onClick = delegate {
			GameCenter.mailBoxMng.mailWriteData = new MailWriteData(other.Name);
			GameCenter.uIMng.SwitchToSubUI(SubGUIType.BMail);
		};
		UIEventListener.Get(chatBtn).onClick = delegate {
			GameCenter.chatMng.OpenPrivateWnd(other.Name);
		};
		UIEventListener.Get(btnParent).onClick = delegate {
			btnParent.SetActive(false);
		};
	}
	
}
