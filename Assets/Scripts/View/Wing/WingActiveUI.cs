//===============================
//日期：2016/3/24
//作者：鲁家旗
//用途描述:翅膀激活界面
//===============================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WingActiveUI : MonoBehaviour
{
    #region 控件数据
    /// <summary>
    /// 名字
    /// </summary>
    public UILabel nameLabel;
    /// <summary>
    /// 等级
    /// </summary>
    public UILabel levLabel;
    /// <summary>
    /// 属性名
    /// </summary>
    public List<UILabel> attributeName = new List<UILabel>();
    /// <summary>
    /// 属性值
    /// </summary>
    public List<UILabel> attributeNum = new List<UILabel>();
    /// <summary>
    /// 下级属性值
    /// </summary>
    public List<UILabel> nextAttributeNum = new List<UILabel>();
    /// <summary>
    /// 强化按钮
    /// </summary>
    public UIButton promoteBtn;

    /// <summary>
    /// 技能图片
    /// </summary>
    public UISpriteEx skillIcon;
    /// <summary>
    /// 技能强化的下级描述
    /// </summary>
    public UILabel skillUIDes;
    /// <summary>
    /// 技能描述
    /// </summary>
    public UILabel skillDes;
    /// <summary>
    /// 经验条
    /// </summary>
    public UISlider expSlider;
    /// <summary>
    /// 经验条上的百分比
    /// </summary>
    public UILabel expLabel;

    /// <summary>
    /// 消耗材料名
    /// </summary>
    public UILabel consumeName;
    /// <summary>
    /// 消耗材料
    /// </summary>
    public UILabel needConsume;

    /// <summary>
    /// 金币图片
    /// </summary>
    public UISprite consumeSp;
    /// <summary>
    /// 金币
    /// </summary>
    public UILabel needConsumGold;

    /// <summary>
    /// 元宝图片
    /// </summary>
    public UISprite acerSp;
    /// <summary>
    /// 元宝数量
    /// </summary>
    public UILabel consumeAcer;
    /// <summary>
    /// 是否快捷购买
    /// </summary>
    public UIToggle isQuickBuy;
    /// <summary>
    /// 选中技能图片 √
    /// </summary>
    public UISprite selectSkill;
    /// <summary>
    /// 装备按钮
    /// </summary>
    public UIButton equBtn;
    /// <summary>
    /// 隐藏按钮
    /// </summary>
    public UIButton unlodBtn;
    /// <summary>
    /// 进度条
    /// </summary>
    public UISlider progressBar;
    /// <summary>
    /// 翅膀图片
    /// </summary>
    public List<UISpriteEx> wingIcon = new List<UISpriteEx>();
    public ItemUI[] wingItem;
    /// <summary>
    /// 已有材料数
    /// </summary>
    protected int num = 0;
    /// <summary>
    /// 已有金币数
    /// </summary>
	protected ulong goldNum = 0;
    /// <summary>
    /// 点击材料按钮
    /// </summary>
    public UIButton consumBtn;
    /// <summary>
    /// 翅膀的最高等级
    /// </summary>
    protected int maxWingLev = 0;
    /// <summary>
    /// 翅膀开始拥有技能的等级
    /// </summary>
    protected int wingHaveSkillLev = 0;
    protected WingInfo data = null;
    protected float wingStageOne = 0;
    protected float wingStageTwo = 0;
    protected float wingStageThree = 0;
    public UIFxAutoActive effect;//进度特效
    public UIFxAutoActive addLevEffect;//升级特效
    #endregion

    void OnEnable()
    {
        if (promoteBtn != null) EventDelegate.Add(promoteBtn.onClick, OnClickPromoteBtn);
        if (equBtn != null) EventDelegate.Add(equBtn.onClick, OnClickEquBtn);
        if (unlodBtn != null) EventDelegate.Add(unlodBtn.onClick, OnClickUnlodBtn);
        //点击消耗材料，弹出信息框
        if (consumBtn != null) UIEventListener.Get(consumBtn.gameObject).onClick = delegate
         {
             ToolTipMng.ShowEquipmentTooltip(new EquipmentInfo(data.WingPromoteList[0].eid, EquipmentBelongTo.PREVIEW), ItemActionType.None, ItemActionType.None, ItemActionType.None, ItemActionType.None);
         };
        GameCenter.wingMng.OnGetWingChange -= showEffect;
        GameCenter.wingMng.OnGetWingChange += showEffect;
        GameCenter.wingMng.OnAddLev -= ShowAddLevEffect;
        GameCenter.wingMng.OnAddLev += ShowAddLevEffect;
    }
    void OnDisable()
    {
        if (promoteBtn != null) EventDelegate.Remove(promoteBtn.onClick, OnClickPromoteBtn);
        if (equBtn != null) EventDelegate.Remove(equBtn.onClick, OnClickEquBtn);
        if (unlodBtn != null) EventDelegate.Remove(unlodBtn.onClick, OnClickUnlodBtn);
        GameCenter.wingMng.OnGetWingChange -= showEffect;
        GameCenter.wingMng.OnAddLev -= ShowAddLevEffect;
    }
    /// <summary>
    /// 淬炼
    /// </summary>
    void OnClickPromoteBtn()
    {
        num = GameCenter.inventoryMng.GetNumberByType(data.WingPromoteList[0].eid);
        goldNum = GameCenter.mainPlayerMng.MainPlayerInfo.TotalCoinCount;
         
         // 淬炼提升判断条件
        if (data.WingLev == maxWingLev)
        {
            //("提示翅膀已到达满级!!!");
            GameCenter.messageMng.AddClientMsg(280);
        }
        else if (isQuickBuy.value == false && data.WingPromoteNum[0] > num)//没有勾选上直接用元宝购买的按钮，且材料不足
        {
            MessageST mst = new MessageST();
            mst.messID = 217;
            string str2 = string.Empty;
            if (num == 0)
                 str2 = data.WingPromoteNum[0].ToString();
            else
                str2 = (data.WingPromoteNum[0] - num).ToString();
            mst.words = new string[3] { data.ConsumeYb.ToString(), str2, ConfigMng.Instance.GetEquipmentRef(data.WingPromoteList[0].eid).name };
            mst.delYes = UseYB;
            GameCenter.messageMng.AddClientMsg(mst);
        }
        else if (isQuickBuy.value == true && data.WingPromoteNum[0] > num)//勾选上直接用元宝购买的按钮，且材料不足
        {
            UseYB(null);
        }
        //铜钱不足提示
        else if (goldNum < (ulong)data.WingPromoteNum[1])
        {
            GoldNotEnough();
        }
        else
        {
            GameCenter.wingMng.C2S_RequestUpLev(WingState.UPWINGLEV, data.WingId, false);
        }
    }

    void UseYB(object[] pars)
    {
        if ((ulong)data.ConsumeYb > GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount)// (元宝)
        {
            MessageST mst = new MessageST();
            mst.messID = 137;
            mst.delYes = delegate
            {
                //充值界面
                GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
            };
            GameCenter.messageMng.AddClientMsg(mst);
        }
        //铜钱不足提示
        else if (goldNum < (ulong)data.WingPromoteNum[1])
        {
            GoldNotEnough();
        }
        else
        {
            GameCenter.wingMng.C2S_RequestUpLev(WingState.UPWINGLEV, data.WingId, true);
        }
    }
    /// <summary>
    /// 铜钱不足提示
    /// </summary>
    void GoldNotEnough()
    { 
        MessageST mst = new MessageST();
        mst.messID = 12;
        EquipmentRef equipRef = ConfigMng.Instance.GetEquipmentRef(data.WingPromoteList[1].eid);
        mst.words = new string[1] {equipRef == null ? string.Empty : equipRef.name };
        GameCenter.messageMng.AddClientMsg(mst);
    }
    /// <summary>
    /// 穿戴
    /// </summary>
    void OnClickEquBtn()
    {
        GameCenter.wingMng.C2S_RequestChangeWing(data.WingId , true);
    }
    /// <summary>
    /// 卸下
    /// </summary>
    void OnClickUnlodBtn()
    {
        GameCenter.wingMng.C2S_RequestChangeWing(data.WingId , false);
    }

    #region 刷新翅膀激活窗口
    void ShowAddLevEffect()
    {
        if (addLevEffect != null)
        {
            addLevEffect.ShowFx();
        }
    }
    void showEffect()
    {
        if (effect != null)
        {
            effect.ReShowFx();
        } 
    }
    public void SetWingActiveUI(WingInfo _data)
    {
        data = _data;
        wingHaveSkillLev = ConfigMng.Instance.GetWingHaveSkillLev(data.WingId);
        maxWingLev = ConfigMng.Instance.GetWingMaxLev(data.WingId);
        if(nameLabel != null)nameLabel.text = data.WingName;
        if(levLabel != null)levLabel.text = data.WingLev.ToString();
        for (int i = 0; i < attributeName.Count; i++)
        {
            if (attributeName[i] != null) attributeName[i].text = data.WingAttributeName[i] + ":";
            if (attributeNum[i] != null) attributeNum[i].text = data.WingAttributeNum[i];
            if (nextAttributeNum[i] != null) nextAttributeNum[i].text = data.WingNextAttributeNum[i];
        }
        
        //技能图片
        if(skillIcon != null) skillIcon.spriteName = data.WingSkillIcon;
        if (skillUIDes != null) skillUIDes.text = data.WingUIDes;
        if (data.WingLev < wingHaveSkillLev)
        {
            if(skillDes != null)skillDes.text = data.WingSkillNotActiveDes;
            if(skillIcon != null)skillIcon.IsGray = UISpriteEx.ColorGray.Gray;
        }
        else
        {
            //根据技能ID去显示技能描述
            SkillLvDataRef skill = ConfigMng.Instance.GetSkillLvDataRef(data.skillId, data.skillLev);
            //技能描述值(所有翅膀第一个值都读skillLarge这个字段；第一个翅膀第二个值读powerOne，第二个翅膀第二个值powerOne/100,第三个翅膀第二个值powerOne/100，第四个翅膀第二个值powerTwo，第五个翅膀第二个值powerOne/100，第六个翅膀写死的)
            if (skill != null && skillDes != null)
                skillDes.text = UIUtil.Str2Str(data.WingSkillDes, new string[2] { "[00ff00]" + data.skillLarge / 100 + "[-]", data.WingId != 4 ? "[00ff00]" + (data.WingId != 1 ? skill.powerOne/100 : skill.powerOne) + "[-]" : "[00ff00]" + skill.powerTwo + "[-]" });
            if (skillIcon != null) skillIcon.IsGray = UISpriteEx.ColorGray.normal;
        }


        if (data.WingLev == maxWingLev)
        {
            if (expLabel != null) expLabel.text = data.WingNeedExp + "/" + data.WingNeedExp;
            if (expSlider != null) expSlider.value = 1;
        }
        else
        {
            if (expLabel != null) expLabel.text = data.WingExp + "/" + data.WingNeedExp;
            if (expSlider != null) expSlider.value = (float)data.WingExp / data.WingNeedExp;
        }
        //消耗材料
        if (consumeName != null) consumeName.text = "[u][b]" + data.WingPromoteName[0];
        num = GameCenter.inventoryMng.GetNumberByType(data.WingPromoteList[0].eid);
        if (data.WingPromoteNum.Count != 0 && data.WingPromoteNum[0] > num)
        {
            if(needConsume != null) needConsume.text = "[b]" + data.WingPromoteNum[0] + "/[ff0000]" + num;
        }
        else
        {
            if (needConsume != null) needConsume.text = data.WingPromoteNum[0] + "/[ffffff]" + num;
        }

        //绑铜
        goldNum = GameCenter.mainPlayerMng.MainPlayerInfo.TotalCoinCount;
        if (data.WingPromoteNum.Count != 0 && (ulong)data.WingPromoteNum[1] > goldNum)
        {
            if (needConsumGold != null) needConsumGold.text = "[b]" + data.WingPromoteNum[1] + "/[ff0000]" + goldNum;
        }
        else
        {
            if (needConsumGold != null) needConsumGold.text = data.WingPromoteNum[1] + "/[ffffff]" + goldNum;
        }
        //消耗元宝
        if (consumeAcer != null)
        {
            if ((ulong)data.ConsumeYb > GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount)
            {
                consumeAcer.text = data.ConsumeYb + "/[ff0000]" + GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount.ToString();
            }
            else
            {
                consumeAcer.text = data.ConsumeYb + "/[ffffff]" + GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount.ToString();
            }
        }

        //穿戴脱下按钮
        if (!data.WingState)
        {
            if(selectSkill != null) selectSkill.gameObject.SetActive(false);
            if(equBtn != null) equBtn.gameObject.SetActive(true);
            if(unlodBtn != null) unlodBtn.gameObject.SetActive(false);
        }
        else if (data.WingState)
        {
            if (selectSkill != null) selectSkill.gameObject.SetActive(false);
            if (equBtn != null) equBtn.gameObject.SetActive(false);
            if (unlodBtn != null) unlodBtn.gameObject.SetActive(true);
            if (data.WingLev >= wingHaveSkillLev)
                selectSkill.gameObject.SetActive(true);
        }
        //进度条
        if (progressBar != null)
        {
            progressBar.value = data.CurTotalExp;
        }
        //翅膀等阶item
        for (int i = 0; i < wingItem.Length; i++)
        {
            wingItem[i].FillInfo(new EquipmentInfo(data.WingItemList[i], EquipmentBelongTo.PREVIEW));
            if (data.WingLev < data.AllWingItemLev[1])
            {
                if (wingIcon[0] != null) wingIcon[0].IsGray = UISpriteEx.ColorGray.normal;
                if (wingIcon[1] != null) wingIcon[1].IsGray = UISpriteEx.ColorGray.Gray;
                if (wingIcon[2] != null) wingIcon[2].IsGray = UISpriteEx.ColorGray.Gray;
            }
            else if (data.WingLev >= data.AllWingItemLev[1] && data.WingLev < data.AllWingItemLev[2])
            {
                if (wingIcon[1] != null) wingIcon[1].IsGray = UISpriteEx.ColorGray.normal;
                if (wingIcon[2] != null) wingIcon[2].IsGray = UISpriteEx.ColorGray.Gray;
            }
            else if (data.WingLev >= data.AllWingItemLev[2])
            {
                if (wingIcon[1] != null) wingIcon[1].IsGray = UISpriteEx.ColorGray.normal;
                if (wingIcon[2] != null) wingIcon[2].IsGray = UISpriteEx.ColorGray.normal;
            }
        }
    }
    #endregion
}
