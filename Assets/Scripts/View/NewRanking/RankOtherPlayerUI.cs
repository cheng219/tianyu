//======================================================
//作者:鲁家旗
//日期:2016.8.18
//用途:排行榜查看其他玩家信息
//======================================================
using UnityEngine;
using System.Collections;

public class RankOtherPlayerUI : MonoBehaviour {

    #region 外部控件
    //public UIToggle playTog;
    public UILabel vipLev;
    public UILabel Lev;
    public UILabel Name;
    /// <summary>
    /// 帮会
    /// </summary>
    public UILabel guild;
    /// <summary>
    /// 称号
    /// </summary>
    public UILabel title;
    /// <summary>
    /// 3D展示
    /// </summary>
    public UITexture dressingRoomTexture;
    //public Load3DObject dressingRoomTexture;

    public UILabel labProf;
    public UILabel killingValue;
    public UILabel fightVal;
    public UILabel labVip;
    public UILabel[] labAttrs;

    public ItemUI[] equipItems;
    public SkillUI[] skillItems;
    #endregion
    /// <summary>
    /// 当前人物数据
    /// </summary>
    private PlayerBaseInfo curTargetInfo;

    void OnEnable()
    {
        GameCenter.previewManager.OnGotRankOtherInfo -= RefreshAll;
        GameCenter.previewManager.OnGotRankOtherInfo += RefreshAll;
        RefreshAll();
    }
    void OnDisable()
    {
        GameCenter.previewManager.OnGotRankOtherInfo -= RefreshAll;
        if (dressingRoomTexture != null) dressingRoomTexture.mainTexture = null;
    }
    void RefreshAll()
    {
        curTargetInfo = GameCenter.previewManager.CurAskPlayerInfo;
        if (curTargetInfo == null) return;
        if (GameCenter.newRankingMng.CurOtherId == 0 || GameCenter.newRankingMng.CurOtherId != curTargetInfo.ServerInstanceID) return;
        curTargetInfo.UpdateStarEffect();
        RefreshEquipment();
        RefreshPreview();
    }
    /// <summary>
    /// 刷新身上装备
    /// </summary>
    void RefreshEquipment()
    {
        if (curTargetInfo == null) return;
        /////填充空白
        for (int i = 0; i < equipItems.Length; i++)
        {
            if (equipItems[i] != null)
            {
                equipItems[i].FillInfo(null);
            }
        }
        /////填充已经装备好的数据
        for (int i = 0, max = curTargetInfo.RealEquipmentInfoList.Count; i < max; i++)
        {
            EquipmentInfo item = curTargetInfo.RealEquipmentInfoList[i];
            if (equipItems.Length > (int)item.Slot - 1 && equipItems[(int)item.Slot - 1] != null)
            {
                equipItems[(int)item.Slot - 1].FillInfo(item);
            }
        }
    }
    /// <summary>
    /// 刷新人物预览
    /// </summary>
    void RefreshPreview()
    {
        if (vipLev != null)
            vipLev.text = curTargetInfo.VipLev.ToString();
        if (Lev != null)
            Lev.text = curTargetInfo.LevelDes;
        if (Name != null)
            Name.text = curTargetInfo.Name;
        if (guild != null)
            guild.text = curTargetInfo.GuildName;
        if (title != null)
            title.text = curTargetInfo.TitleName;
        if (curTargetInfo != null && dressingRoomTexture != null)
        {
            GameCenter.previewManager.TryPreviewSinglePlayer(curTargetInfo, dressingRoomTexture);
        }
        if (labProf != null) labProf.text = curTargetInfo.ProfName;
        if (killingValue != null) killingValue.text = curTargetInfo.KillingValue.ToString();
        if (fightVal != null) fightVal.text = curTargetInfo.FightValue.ToString();
        if (labAttrs != null)
        {
            for (int i = 0, max = labAttrs.Length; i < max; i++)
            {
                if (labAttrs[i] == null) continue;
                switch (i)
                {
                    case 0:
                        labAttrs[i].text = curTargetInfo.AttackStr;
                        break;
                    case 1:
                        labAttrs[i].text = curTargetInfo.DefStr;
                        break;
                    case 2:
                        labAttrs[i].text = curTargetInfo.MaxHPText;
                        break;
                    case 3:
                        labAttrs[i].text = curTargetInfo.Hit.ToString();
                        break;
                    case 4:
                        labAttrs[i].text = curTargetInfo.Tough.ToString();
                        break;
                    case 5:
                        labAttrs[i].text = curTargetInfo.Crit.ToString();
                        break;
                    case 6:
                        labAttrs[i].text = curTargetInfo.Dodge.ToString();
                        break;
                    case 7:
                        labAttrs[i].text = curTargetInfo.LuckyValue.ToString();
                        break;
                }
            }
        }
        if (skillItems != null)
        {
            for (int i = 0, max = skillItems.Length; i < max; i++)
            {
                if (skillItems[i] != null)
                {
                    skillItems[i].isEnable = true;
                    if (i >= curTargetInfo.CurSkillList.Count)
                    { 
                        skillItems[i].FillInfo(null);
                        skillItems[i].itemName.gameObject.SetActive(false);
                        continue;
                    }
                    if (curTargetInfo.CurSkillList[i].CurSkillMode == SkillMode.CLIENTSKILL) //by黄洪兴 只有客户端技能需要展示
                    {
                        skillItems[i].FillInfo((curTargetInfo.CurSkillList.Count > i) ? curTargetInfo.CurSkillList[i] : null); 
                    } 
                }
            }
        }
    }

}
