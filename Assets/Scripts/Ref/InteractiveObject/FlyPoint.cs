//=============================================
//作者：吴江
//日期:2015/5/15
//用途：传送点对象
//=============================================



using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FlyPoint : InteractiveObject
{
    public GameObject Effect = null;
    protected FXRoot fxRoot = null;

    /// <summary>
    /// 客户端静态配置数据（禁止修改）
    /// </summary>
    protected FlyPointRef refData = null;
    /// <summary>
    /// 客户端静态配置数据（禁止修改）
    /// </summary>
    public FlyPointRef RefData
    {
        get { return refData; }
    }


    public static FlyPoint CreateDummy(FlyPointRef _info)
    {
        GameObject newGO = new GameObject("Dummy FlyPoint[" + _info.id + "]");
        newGO.tag = "FlyPoint";

        FlyPoint flyPoint = newGO.AddComponent<FlyPoint>();
        newGO.SetMaskLayer(LayerMask.NameToLayer("Static"));
        flyPoint.id = _info.id;
        flyPoint.refData = _info;
        flyPoint.isDummy_ = true;
        GameCenter.curGameStage.PlaceGameObjectFromStaticRef(flyPoint, (int)_info.sceneVector.x, (int)_info.sceneVector.z, _info.direction, (int)_info.sceneVector.y);
        GameCenter.curGameStage.AddObject(flyPoint);
        return flyPoint;
    }


    protected new void Awake()
    {
        typeID = ObjectType.FlyPoint;
        base.Awake();
    }



    public virtual void StartAsyncCreate(System.Action<FlyPoint> _callback)
    {
        string pointName = string.Empty;

        this.gameObject.SetMaskLayer(LayerMask.NameToLayer("Static"));
        this.gameObject.name = "FlyPoint[" + refData.id + "]";
        isDummy_ = false;
        Effect = null;
        typeID = ObjectType.FlyPoint;
        id = refData.id;



        AssetMng.GetEffectInstance(refData.resourceModel, (x) =>
                {
                    if (this == null)
                    {
                        Destroy(x);
                        return;
                    }
                    x.SetMaskLayer(LayerMask.NameToLayer("Static"));
                    Effect = x;

                    if (Effect == null)
                        return;
                    Effect.transform.parent = gameObject.transform;
                    Effect.transform.localPosition = new Vector3(0, 0, 0);
                    Effect.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                    Effect.name = "effect";
                });


        if (headTextCtrl == null) headTextCtrl = this.gameObject.AddComponent<HeadTextCtrl>();
        if (refData.name.Contains("\\n"))
        {
            pointName = refData.name.Replace("\\n", "\r\n");
        }
        headTextCtrl.SetName(pointName == string.Empty ? refData.name : pointName);
        if (RefData.needLv > GameCenter.mainPlayerMng.MainPlayerInfo.Level)
        {
            headTextCtrl.SetTitle("Lv" + RefData.needLv);
        }
        height = 2.0f;
        ActiveBoxCollider(true,3.0f);
        if (_callback != null)
        {
            _callback(this);
        }

    }

    public static void OnMainPlayerTriggerFlyPoint(MainPlayer _mainPlayer, FlyPoint _point)
    {
        if (_point == null || _mainPlayer == null || !_mainPlayer.hasCtrlAwake) return;

        if (_mainPlayer.Level < (ulong)_point.RefData.needLv)
        {
            GameCenter.messageMng.AddClientMsg(161);
            return;
        }

        _mainPlayer.curTrigerFlyPoint = _point;
        if (_point.RefData != null)
        {
            switch (_point.RefData.sort)
            {
                case FlyPointSort.openUI:
                    break;
                case FlyPointSort.targetScene:
                    GameCenter.mainPlayerMng.C2S_Fly(_point.RefData.id);
                    break;
                case FlyPointSort.recall:
                    GameCenter.mainPlayerMng.C2S_Fly(_point.RefData.id);
                    break;
                default:
                    break;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameCenter.curMainPlayer.gameObject)
        {
            OnMainPlayerTriggerFlyPoint(GameCenter.curMainPlayer, this);
            if (GameCenter.curMainPlayer != null && GameCenter.curMainPlayer.CurClickFlyPoint == this)
            {
                GameCenter.curMainPlayer.CurClickFlyPoint = null;
                GameCenter.curMainPlayer.commandMng.CancelCommands();
                //这里直接取消命令,会导致任务寻路的时候,进入传送门就会取一个寻路指令(然后删除)。。结果任务寻路最后停在传送门处  by邓成
                //不取消,又会导致点击传送门,进入传送门之后还会前往那个点击的位置  于是加了个判断
            }
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameCenter.curMainPlayer.gameObject)
        {
            if (GameCenter.curMainPlayer.curTrigerFlyPoint = this)
            {
                GameCenter.curMainPlayer.curTrigerFlyPoint = null;
            }
        }
    }


    void OnEnable()
    {
        if (Effect == null) return;
        if (fxRoot == null)
        {
            fxRoot = Effect.GetComponent<FXRoot>();
        }
        if (fxRoot != null)
        {
            fxRoot.Play(0.1f,1.0f);
        }
    //    GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += OnMainPlayerUpdateLevel;
    }

    void OnDisable()
    {
    //    GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= OnMainPlayerUpdateLevel;
    }

	protected void OnMainPlayerUpdateLevel(ActorBaseTag _tag, ulong _value,bool _fromAbility)
    {
        if (_tag == ActorBaseTag.Level)
        {
            if ((ulong)RefData.needLv < _value)
            {
                headTextCtrl.SetTitle(string.Empty);
            }
        }
    }
}
