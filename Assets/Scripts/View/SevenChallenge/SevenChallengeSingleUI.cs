//==============================================
//作者：唐源
//日期：2017/4/16
//用途：七日挑战单个窗口的单个任务UI
//==============================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SevenChallengeSingleUI : MonoBehaviour {
    #region UI控件
    public UIButton btnGo;
    public UILabel title;
    public UILabel content;
    public UILabel num;
    public GameObject finish;
    //public UILabel count;
    #endregion
    #region 实例化
    public static SevenChallengeSingleUI CreateNew(Transform _parent, GameObject _item)
    {
        GameObject go = Instantiate(_item.gameObject);
        if (go != null)
        {
            go.transform.parent = _parent;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            SevenChallengeSingleUI itemUI = go.GetComponent<SevenChallengeSingleUI>();
            return itemUI;
        }
        return null;
    }
    #endregion
    #region 刷新与显示

    //填充静态预制
    public void FillInfo(SevenDaysTaskRef _dataRef)
    {
        //Debug.Log("FillInfo:" + _dataRef.des1);
        if (_dataRef.Action == 1)
        {
            //Debug.Log("_dataRef.Action:"+_dataRef.Action);
            if (btnGo != null)
            {
                UIEventListener.Get(btnGo.gameObject).onClick = delegate
                {
                    if (_dataRef.num == 1)
                    {
                        GUIType type = (GUIType)System.Enum.Parse(typeof(GUIType), _dataRef.ui);
                        //Debug.Log("type:" + type);
                        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
                        //GameCenter.uIMng.ReleaseGUI(GUIType.SEVENCHALLENGE);
                        GameCenter.uIMng.SwitchToUI(type);
                    }
                    else if (_dataRef.num == 2)
                    {
                        SubGUIType type = (SubGUIType)System.Enum.Parse(typeof(SubGUIType), _dataRef.ui);
                        //GameCenter.uIMng.ReleaseGUI(GUIType.SEVENCHALLENGE);
                        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
                        GameCenter.uIMng.SwitchToSubUI(type);
                    }
                };
                btnGo.gameObject.SetActive(true);
                //Invoke("DelayShow",0.75f);
            }
        }
        else
        {
            if (btnGo != null)
                btnGo.gameObject.SetActive(false);
        }
        if (title != null)
        {
            //data.des1;
            title.text = _dataRef.des1;
        }
        if (content != null)
        {
            ////data.des2;
            content.text = _dataRef.des2;
            //Debug.Log("_dataRef.des2:"+ _dataRef.des2);
        }
        if (num != null)
        {
            num.text = 0.ToString() + "/" + _dataRef.task_condition_num.ToString();
            content.text = content.text.Replace("#1", num.text);
        }
        if (finish != null)
        {
            finish.gameObject.SetActive(false);
        }
        updateData(_dataRef);
    }
    void updateData(SevenDaysTaskRef _dataRef)
    {
        st.net.NetBase.single_day_info data = null;
        List<st.net.NetBase.single_day_info> listSingleInfo = GameCenter.sevenChallengeMng.listSingleInfo;
        bool findServerData = false;
        for (int i = 0, length = listSingleInfo.Count; i < length; i++)
        {
            //Debug.Log("listSingleInfo[i].task_id:"+ listSingleInfo[i].task_id+ ", _dataRef.id:" + _dataRef.id);
            if (listSingleInfo[i].task_id == _dataRef.id)
            {
                findServerData = true;
                data = listSingleInfo[i];
                if (content != null)
                {
                    string num = data.task_num.ToString() + "/" + _dataRef.task_condition_num.ToString();
                    //content.text = _dataRef.des2;
                    //num.text = data.task_num.ToString() + "/" + _dataRef.task_condition_num.ToString();
                    content.text = _dataRef.des2.Replace("#1", num);
                }
                //Debug.Log("任务：" + data.task_id + "的状态" + data.finish_state);
                if (data.finish_state == 0)
                {
                    if (finish != null)
                    {
                        finish.gameObject.SetActive(false);
                        //Debug.Log("finish:"+ finish.activeSelf);
                    }
                }
                if (data.finish_state == 1)
                {
                    if (finish != null)
                    {
                        finish.gameObject.SetActive(true);
                        //Debug.Log("finish:" + finish.activeSelf);
                    }
                    if (btnGo != null)
                        btnGo.gameObject.SetActive(false);
                }
            }
        }
        if (findServerData == false) Debug.LogWarning("找不到七日挑战第" + _dataRef.day + "天的后台任务数据：" + _dataRef.des2);
    }
    #endregion
}
