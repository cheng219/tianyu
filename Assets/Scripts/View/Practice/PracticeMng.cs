//==================================
//作者：朱素云
//日期：2016/4/10
//用途：修行飞升管理类
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class PracticeMng
{
    #region 数据
    public bool isTenTimesYG = false;
    /// <summary>
    /// 当前修行数据
    /// </summary>
    public PracticeData data = null;
    /// <summary>
    /// 人体脉络特效点
    /// </summary>
    public int bodyEffectNum = 0;
    /// <summary>
    /// 指点类型
    /// </summary>
    public int practiceType = 0;
    /// <summary>
    /// 运功增加的仙气
    /// </summary>
    public int addDust = 0;
    /// <summary>
    /// 运功增加的灵气
    /// </summary>
    public int addReiki = 0;
    /// <summary>
    /// 运功时的仙气列表
    /// </summary>
    public List<int> dustList = new List<int>();
    /// <summary>
    /// 运功时的灵气列表
    /// </summary>
    public List<int> reikiList = new List<int>();   

    #endregion

    #region 事件
    /// <summary>
    /// 修行数据变化事件
    /// </summary>
    public System.Action OnPracticeUpdata;
    /// <summary>
    /// 运功事件变化
    /// </summary>
    public System.Action OnExerciseUpdata;
    #endregion

    #region 构造
    public static PracticeMng CreateNew()
    {
        if (GameCenter.practiceMng == null)
        {
            PracticeMng practiceMng = new PracticeMng();
            practiceMng.Init();
            return practiceMng;
        }
        else
        {
            GameCenter.practiceMng.UnRegist();
            GameCenter.practiceMng.Init();
            return GameCenter.practiceMng;
        }
    } 
    protected void Init()
    { 
        MsgHander.Regist(0xD477, S2C_GetPromoteList);
        MsgHander.Regist(0xD475, S2C_GetUiInfo);
        MsgHander.Regist(0xD473, S2C_GetXiuXing);
    } 
    protected void UnRegist()
    {
        MsgHander.UnRegist(0xD477, S2C_GetPromoteList);
        MsgHander.UnRegist(0xD475, S2C_GetUiInfo);
        MsgHander.UnRegist(0xD473, S2C_GetXiuXing);
        practiceType = 0;
        addDust = 0;
        addReiki = 0;
        bodyEffectNum = 0;
        dustList.Clear();
        reikiList.Clear();
        isTenTimesYG = false;
    } 
    /// <summary>
    /// 添加提示框
    /// </summary>
    /// <param name="_id">提示框id</param>
    /// <param name="_str">字符串1</param>
    /// <param name="_str1">字符串2</param>
    public void ReminderWnd(int _id, string _str, string _str1 = null)
    {
        MessageST mst = new MessageST();
        mst.messID = _id;
        if (_str1 != null)
            mst.words = new string[2] { _str , _str1};
        else
            mst.words = new string[1] { _str };
        GameCenter.messageMng.AddClientMsg(mst); 
    }
    /// <summary>
    /// 飞升红点
    /// </summary>
    public void SetSoarRedMind(ActorBaseTag tag, ulong value, bool _fromAbility)
    {
        if (tag == ActorBaseTag.HighFlyUpRes || tag == ActorBaseTag.LowFlyUpRes)
        {
            if (data == null || data.stratNum >= ConfigMng.Instance.GetStyliteRefTable().Count - 1)
            {
                GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.SOARING, false);
                return;
            }
            bool isRed = false;
            if ((70 * (data.stratNum / 70) + (data.stratNum / 70 - 1)) == data.stratNum && data.stratNum > 0)
            {
                FlyUpRef flyUpRef = ConfigMng.Instance.GetFlyUpRef((data.stratNum) / 70 + 1);
                if (flyUpRef != null)
                {
                    if (GameCenter.mainPlayerMng.MainPlayerInfo.HighFlyUpRes >= flyUpRef.xianQi && GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel >= flyUpRef.needLev)
                    {
                        isRed = true;
                    }
                }
            }
            else
            {
                StyliteRef nextStyliteRef = ConfigMng.Instance.GetStyliteRefByStart(data.stratNum + 1);
                if (nextStyliteRef != null)
                {
                    if (GameCenter.mainPlayerMng.MainPlayerInfo.LowFlyUpRes >= nextStyliteRef.lingqi)
                    {
                        isRed = true;
                    }
                }
            }
            GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.SOARING, isRed);
        }
    }
    /// <summary>
    /// 修行红点
    /// </summary>
    public void SetPracticeRedMind(ActorBaseTag tag, ulong value, bool _fromAbility)
    {
        if (tag == ActorBaseTag.BindCoin || tag == ActorBaseTag.UnBindCoin || tag == ActorBaseTag.CoinLimit)
        {
            if (data == null || data.coinTime <= 0)
            {
                GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.PRACTICE, false);
                return;
            }
            bool isRed = false;
            if (GameCenter.mainPlayerMng.MainPlayerInfo.TotalCoinCount >= data.CoinYG)
            {
                isRed = true;
            }
            GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.PRACTICE, isRed);
        }
    }
    #endregion

    #region S2C
    /// <summary>
    /// 获取界面信息 飞升(点数等级)
    /// </summary> 
    void S2C_GetUiInfo(Pt pt)
    {
        pt_update_fly_up_lev_d475 msg = pt as pt_update_fly_up_lev_d475;
        if (msg != null)
        {
            bodyEffectNum = (int)msg.lev; 
            if (this.data == null)
            {
                this.data = new PracticeData((int)msg.lev);
            }
            else
            {
                this.data.Updata((int)msg.lev); 
            }
            SetSoarRedMind(ActorBaseTag.HighFlyUpRes , 1 ,true);
        }
        if (OnPracticeUpdata != null) OnPracticeUpdata();
    }
    /// <summary>
    /// 修行(铜钱运功次数 ，元宝运功次数)
    /// </summary> 
    void S2C_GetXiuXing(Pt pt)
    {
        pt_fly_up_xiuxing_info_d473 msg = pt as pt_fly_up_xiuxing_info_d473;
        if (msg != null)
        {
            //Debug.Log(" 铜钱运功次数 ：  " + msg.num1 + "   , 元宝运功次数 ： " + msg.num2);
            if (data != null) this.data.UpdatePracticeData(msg.num1, msg.num2);
            else data = new PracticeData(msg.num1, msg.num2);
            SetPracticeRedMind(ActorBaseTag.BindCoin , 1 , true);
        }
    }
    /// <summary>
    /// 获取灵气列表和仙气列表
    /// </summary> 
    void S2C_GetPromoteList(Pt pt)
    {
        pt_update_yunqi_tuan_list_d477 msg = pt as pt_update_yunqi_tuan_list_d477;
        practiceType = 0;
        addDust = 0; addReiki = 0;
        if (msg != null)
        {
            dustList.Clear();
            reikiList.Clear();
            practiceType = (int)msg.state;
            //Debug.logger.Log("仙气团数：" + msg.xian_qi_list.Count + "灵气团数：" + msg.ling_qi_list.Count);
            for (int i = 0; i < msg.xian_qi_list.Count; i++)
            {
                this.dustList.Add(msg.xian_qi_list[i]);
                addDust += msg.xian_qi_list[i];
                //Debug.logger.Log("仙气   " + msg.xian_qi_list[i]);
            }
            for (int i = 0; i < msg.ling_qi_list.Count; i++)
            {
                this.reikiList.Add(msg.ling_qi_list[i]);
                addReiki += msg.ling_qi_list[i];
                //Debug.logger.Log("灵气   " + msg.ling_qi_list[i]);
            }
        }
        if (OnExerciseUpdata != null) OnExerciseUpdata();
    }  
    #endregion

    #region C2S
    /// <summary>
    /// 请求ui信息 1飞升 2修行
    /// </summary> 
    public void C2S_ReinState(practiceType _type)
    { 
        pt_req_fly_up_xiuxing_d472 msg = new pt_req_fly_up_xiuxing_d472();
        msg.state = (uint)_type;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 修行请求 1铜钱运功 2元宝运功 3高人指点 4仙人指点 5 十倍运功
    /// </summary> 
    public void C2S_ReqExcercise(ExcerciseType stege) 
    {
        if (stege == ExcerciseType.TENTIMESYG) isTenTimesYG = true;
        //Debug.logger.Log("请求1铜钱运功 2元宝运功 3高人指点 4仙人指点 5 十倍运功 :" + (uint)stege);
        pt_req_xiuxing_d476 msg = new pt_req_xiuxing_d476();
        msg.state = (uint)stege;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求收功 空协议
    /// </summary>
    public void C2S_ReqStopExcercise()
    {
        pt_req_shou_gong_d478 msg = new pt_req_shou_gong_d478();
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求提升
    /// </summary>
    public void C2S_ReqUpLev()
    {
        if (data != null)
        {
            if (data.stratNum <= ConfigMng.Instance.GetStyliteRefTable().Count - 1)
            {
                pt_req_fly_up_lev_d474 msg = new pt_req_fly_up_lev_d474();
                NetMsgMng.SendMsg(msg);
            }
        }
    }
    /// <summary>
    /// 请求兑换仙气
    /// </summary>
    public void C2S_ReqConvent(uint _num)
    { 
        pt_req_conversion_xian_qi_d479 msg = new pt_req_conversion_xian_qi_d479();
        msg.num = _num;
        NetMsgMng.SendMsg(msg);
    }
    #endregion

}  
public enum practiceType
{ 
    /// <summary>
    /// 飞升
    /// </summary>
    SOARING = 1,
    /// <summary>
    /// 修行
    /// </summary>
    PRACRICE = 2, 
} 
public enum ExcerciseType
{ 
    /// <summary>
    /// 铜钱运功
    /// </summary>
    COINEXC = 1,
    /// <summary>
    /// 元宝运功
    /// </summary>
    DIAMONDEXC = 2,
    /// <summary>
    /// 高人指点
    /// </summary>
    MASTERADVICE = 3,
    /// <summary>
    /// 仙人指点
    /// </summary>
    FAIRYADVICE = 4,
    /// <summary>
    /// 十倍运功
    /// </summary>
    TENTIMESYG = 5,
    /// <summary>
    /// 十倍高人指点
    /// </summary>
    TENMASTERADVICE = 6,
    /// <summary>
    /// 十倍仙人指点
    /// </summary>
    TENFAIRYADVICE = 7,
}