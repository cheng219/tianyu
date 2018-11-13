//===============================
//日期：2016/3/24
//作者：鲁家旗
//用途描述:翅膀未激活界面
//===============================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WingNotActiveUI : MonoBehaviour {

    #region 控件数据
    /// <summary>
    /// 底下描述信息
    /// </summary>
    public UILabel desNameLabel;
    /// <summary>
    /// 属性描述
    /// </summary>
    public UILabel attribuDes;
    /// <summary>
    /// 技能提示Label
    /// </summary>
    public UILabel skillLabel;
    /// <summary>
    /// 技能图片 
    /// </summary>
    public UISpriteEx skillIcon;
    /// <summary>
    /// 技能描述
    /// </summary>
    public UILabel skillDes;
    /// <summary>
    /// 消耗描述 
    /// </summary>
    public List<UILabel> conditionLabel = new List<UILabel>();
    /// <summary>
    /// 激活按钮
    /// </summary>
    public UIButton activeBtn;
    /// <summary>
    /// 开启条件的消耗品价格
    /// </summary>
    protected float consumePrice = 0;

    protected WingRef data = null;
    #endregion

    void OnEnable()
    {
        if (activeBtn != null) EventDelegate.Add(activeBtn.onClick, OnClickActiveBtn);
    }
    void OnDisable()
    {
        if (activeBtn != null) EventDelegate.Remove(activeBtn.onClick, OnClickActiveBtn);
    }
    /// <summary>
    /// 激活按钮
    /// </summary>
    void OnClickActiveBtn()
    {
        EquipmentRef equipRef = ConfigMng.Instance.GetEquipmentRef(data.condition_2[0]);
        if(equipRef != null)
            consumePrice = (int)equipRef.diamonPrice * data.condition_2[1];
        //前置翅膀没有激活
        if (!GameCenter.wingMng.WingDic.ContainsKey(data.id - 1) && data.id - 1 >= 1)
        {
            // 提示前置翅膀等级不足
            MessageST mst = new MessageST();
            mst.messID = 218;
            WingRef lastWingRef = ConfigMng.Instance.GetWingRef(data.id - 1, 1);
            mst.words = new string[1] { lastWingRef == null ? string.Empty : lastWingRef.name };
            GameCenter.messageMng.AddClientMsg(mst);
        }
        //前置翅膀激活，没有达到15级
        else if (GameCenter.wingMng.WingDic.ContainsKey(data.id - 1) && (GameCenter.wingMng.WingDic[data.id - 1] as WingInfo).WingLev < data.condition_1[1])
        {
            // 提示前置翅膀等级不足
            MessageST mst = new MessageST();
            mst.messID = 218;
            WingRef lastWingRef = ConfigMng.Instance.GetWingRef(data.id - 1, 1);
            mst.words = new string[1] { lastWingRef == null ? string.Empty : lastWingRef.name };
            GameCenter.messageMng.AddClientMsg(mst);
        }
        else if (data.condition_2.Count != 0 && GameCenter.inventoryMng.GetNumberByType(data.condition_2[0]) < data.condition_2[1])
        {
            //提示材料不足！！！
            MessageST mst = new MessageST();
            mst.messID = 217;
            //是否花费多少元宝？购买多少个？什么？
            mst.words = new string[3] { (ConfigMng.Instance.GetEquipmentRef(data.condition_2[0]).diamonPrice * data.condition_2[1]).ToString(), data.condition_2[1].ToString(), ConfigMng.Instance.GetEquipmentRef(data.condition_2[0]).name };
            mst.delYes = delegate
            {
                if (GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount < consumePrice)
                {
                    MessageST message = new MessageST();
                    message.messID = 210;
                    message.delYes = delegate
                    {
                        // 充值界面
                        GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
                    };
                    GameCenter.messageMng.AddClientMsg(message);
                }
                else
                {
                    GameCenter.wingMng.C2S_RequestUpLev(WingState.ACTIVEWING, data.id, true);
                }
            };
            GameCenter.messageMng.AddClientMsg(mst);
        }
        else
        {
            GameCenter.wingMng.C2S_RequestUpLev(WingState.ACTIVEWING, data.id, false);
        }
    }

    #region 刷新
    public void SetWingNotActiveUI(WingRef _info)
    {
        data = _info;
        if(desNameLabel != null) desNameLabel.text = _info.name;
        if(attribuDes != null) attribuDes.text = _info.des;
        if(skillLabel != null) skillLabel.text = _info.skill_des;
        if(skillDes != null) skillDes.text = _info.not_active_skill;
        //技能图片
        WingRef wingRef = ConfigMng.Instance.GetWingRef(_info.id, ConfigMng.Instance.GetWingMaxLev(_info.id));
        int skillId = 0;
        if(wingRef != null)
             skillId = wingRef.passivity_skill.skillid;
        SkillMainConfigRef skillRef = ConfigMng.Instance.GetSkillMainConfigRef(skillId);
        if (skillIcon != null)
        {
            skillIcon.spriteName = skillRef == null ? string.Empty : skillRef.skillIcon;
            skillIcon.IsGray = UISpriteEx.ColorGray.Gray;
        }
        //消耗 第一个翅膀的开启条件不同只需消耗材料
        if (_info.id == 1)
        {
			if(ConfigMng.Instance.GetEquipmentRef(_info.condition_2[0]) != null){
                string[] name = new string[2] { GameHelper.GetStringWithBagNumber(_info.condition_2[0], (ulong)_info.condition_2[1]), string.Empty };
                if (conditionLabel[0] != null) conditionLabel[0].text = ConfigMng.Instance.GetUItext(6, name);
                if (conditionLabel[1] != null) conditionLabel[1].gameObject.SetActive(false);
			}
        }
        else
        {
            if (conditionLabel[1] != null) conditionLabel[1].gameObject.SetActive(true);
            if (ConfigMng.Instance.GetWingRef(_info.condition_1[0], 1) != null)
            {
                string[] name = new string[2] { ConfigMng.Instance.GetWingRef(_info.condition_1[0], 1).name, _info.condition_1[1].ToString() };
                if (conditionLabel[0] != null) conditionLabel[0].text = ConfigMng.Instance.GetUItext(7, name);
            }
            if (ConfigMng.Instance.GetEquipmentRef(_info.condition_2[0]) != null)
            {
                string[] conname = new string[2] { GameHelper.GetStringWithBagNumber(_info.condition_2[0], (ulong)_info.condition_2[1]), string.Empty };
                if (conditionLabel[1] != null) conditionLabel[1].text = ConfigMng.Instance.GetUItext(8, conname);
            }
        }
    }
    #endregion
}
