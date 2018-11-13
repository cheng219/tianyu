//=========================================
//作者：吴江
//日期：2015/11/15
//用途：场景物品互动进度条
//===========================================




using UnityEngine;
using System.Collections;
public class CollectProgressPrefab : MonoBehaviour
{
    public UILabel labDes;
    public UISlider foreward;
    #region 进度条,在enableTime内将foreward的Scale从0变到最大
    float startTime = 0;
    float enableTime = 0;
    bool isEnable = false;

    #endregion
    void Awake()
    {
        foreward.value = 0;
    }
    /// <summary>
    /// 开始
    /// </summary>
    public void SetProgress(float _time, string des)
    {
        labDes.text = des;
        startTime = Time.realtimeSinceStartup;
        enableTime = _time;
        isEnable = true;
    }
    public void EndProgress()
    {
        foreward.value = 0;
        isEnable = false;
    }
    void Update()
    {
        if (isEnable && Time.frameCount % 3 == 0)
        {
            //Vector3 vect = foreward.transform.localScale;
            float tempTime = (Time.realtimeSinceStartup - startTime) / enableTime;
            if (tempTime >= 1)
			{
				isEnable = false;
				gameObject.SetActive(false);//时间到了直接隐藏,防止后台数据延迟导致玩家头上挂着进度条 by邓成
			}
            foreward.value = Mathf.Lerp(0, 1, tempTime);
        }
    }
}