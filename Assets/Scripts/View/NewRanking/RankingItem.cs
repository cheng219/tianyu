//====================
//作者：鲁家旗
//日期：2016/4/19
//用途：排行榜单条数据UI
//====================
using UnityEngine;
using System.Collections;
public class RankingItem : MonoBehaviour
{
    #region 控件数据
    public UILabel rankNumLabel;//排名
    public UILabel nameLabel;//名字
    public UILabel rankDesLabel; //具体描述
    public UIButton lookInfoBtn;//查看按钮
    public UILabel guildLabel;//仙盟特殊一点
    protected GameObject prefab = null;
    public GameObject parent;
    public UITexture modelTex;
    protected GameObject go = null;
    #endregion
    
    public void Refresh(NewRankingInfo _info,int _rankType)
    {
        if(nameLabel != null)nameLabel.text = _info.PlayerName;
        if (_rankType - 1 == 5)
        {
            if (lookInfoBtn != null) lookInfoBtn.gameObject.SetActive(false);
        }
        else if (lookInfoBtn != null)
            lookInfoBtn.gameObject.SetActive(true);
        if (rankDesLabel != null)
        {
            rankDesLabel.gameObject.SetActive(true);
            guildLabel.gameObject.SetActive(false);
            switch (_rankType - 1)
            {
                case 0:
                    rankDesLabel.text = _info.Fighting.ToString();
                    break;
                case 1:
                    rankDesLabel.text = _info.Lev;
                    break;
                case 2:
                    rankDesLabel.text = _info.PetFighting.ToString();
                    break;
                case 3:
                    rankDesLabel.text = _info.MountLev;
                    break;
                case 4:
                    rankDesLabel.text = _info.Endless;
                    break;
                case 5:
                    guildLabel.gameObject.SetActive(true);
                    rankDesLabel.gameObject.SetActive(false);
                    guildLabel.text = _info.GuildLev + "      " +  _info.GuildFighting;;
                    break;
                case 6:
                    rankDesLabel.text = _info.WingName;
                    break;
                case 7:
                    rankDesLabel.text = _info.ToFlowerNum.ToString();
                    break;
                case 8:
                    rankDesLabel.text = _info.FlowerNum.ToString();
                    break;
                case 9:
                    rankDesLabel.text = _info.KillPeople.ToString();
                    break;
                case 10:
                    rankDesLabel.text = _info.KillWickedPreson.ToString();
                    break;
            }
        }
        if (lookInfoBtn != null)
        {
            UIEventListener.Get(lookInfoBtn.gameObject).onClick = delegate
            {
                GameCenter.previewManager.C2S_AskOPCPreview(_info.PlayerId);
                GameCenter.newRankingMng.CurOtherId = _info.PlayerId;
                modelTex.gameObject.SetActive(false);
                if (prefab == null)
                {
                    prefab = exResources.GetResource(ResourceType.GUI, "Ranking/OtherPlayerRanking") as GameObject;
                }
                if (prefab == null)
                {
                    Debug.Log("找不到相关预制！");
                    return;
                }
                if (GameCenter.newRankingMng.otherGo == null)
                {
                    go = Instantiate(prefab) as GameObject;
                    go.transform.parent = parent.transform;
                    go.transform.localPosition = prefab.transform.localPosition;
                    go.transform.localScale = Vector3.one;
                    go.SetActive(true);
                    GameCenter.newRankingMng.otherGo = go;
                }
                else
                {
                    go = GameCenter.newRankingMng.otherGo;
                    go.SetActive(true);
                }
                //GameCenter.previewManager.C2S_AskOpcPetPreview(_info.PlayerId);
                if (go != null)
                    go.GetComponent<RankCheckOpcWnd>().SetOtherInfo((_rankType - 1 == 2 || _rankType - 1 == 3) ?(_rankType - 1) : 1, modelTex);
            };
        }
        //保存当前选中的人物ID
        UIEventListener.Get(this.gameObject).onClick = delegate
        {
            GameCenter.newRankingMng.CurChooseRankPlayerId = _info.PlayerId;
            GameCenter.previewManager.C2S_ReqGetInfo(_info.PlayerId, _rankType == 6 ? 1 : 0);//6是仙盟榜，显示的是盟主信息(服务端加个参数好查找盟主信息)
        };
    }
    void OnDestroy()
    {
        prefab = null;
    }
}
