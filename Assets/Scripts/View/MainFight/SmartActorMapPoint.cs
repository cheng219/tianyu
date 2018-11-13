//======================================
//作者:吴江
//日期:2016//1/18
//用途:活动对象在小地图上的表现对象
//======================================

using UnityEngine;
using System.Collections;

public class SmartActorMapPoint : MonoBehaviour {

    protected SmartActor target;
    protected UISprite us = null;

    public void SetTarget(SmartActor _sm)
    {
        target = _sm;
        target.onSectorChanged += OnPositionChange;
        us = this.gameObject.GetComponent<UISprite>();
        if (us != null && target.typeID == ObjectType.MOB)
        {
            us.color = ConfigMng.Instance.GetRelationColor(GameCenter.curMainPlayer.Camp, _sm.Camp, GameCenter.curGameStage.SceneType);
        }
    }

    void OnDestroy()
    {
        if (target != null)
        {
            target.onSectorChanged -= OnPositionChange;
            Destroy(this.gameObject);
        }
    }


    protected void OnPositionChange(GameStage.Sector _old, GameStage.Sector _new)
    {
        if (this == null || target == null || target.isDummy || transform == null) return;
        transform.localPosition = new Vector3(_new.c * Mathf.PI, _new.r * Mathf.PI, 0);
    }

}
