///////////////////////////////////////////////////////////////
//作者：吴江
//日期：2015/5/5
//用途：游戏中的3D对象基类
//////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;



/// <summary>
/// 游戏中的3D对象基类 by吴江
/// </summary>
public class InteractiveObject : FSMBase
{
    protected bool isHide = false;
    /// <summary>
    /// 是否隐藏挂起 
    /// </summary>
    public bool IsHide
    {
        get
        {
            return isHide;
        }
        set
        {
            if (isHide != value)
            {
                isHide = value;
                OnUpdateHide(isHide);
            }
        }
    }
    /// <summary>
    /// 是否已经初始化完毕 
    /// </summary>
    public bool inited { get { return inited_; } }
    /// <summary>
    /// 是否虚拟体状态
    /// </summary>
    public bool isDummy { get { return isDummy_; } }
    /// <summary>
    /// 是否被暂存（以后优化的时候这个字段有用） by吴江
    /// </summary>
    public bool isCache { set { isCache_ = value; } }
    /// <summary>
    /// 是否正在加载实体
    /// </summary>
    public bool isDownloading { set { isDownloading_ = value; } get { return isDownloading_; } }
    /// <summary>
    /// 服务端唯一ID
    /// </summary>
    public virtual int id { get; set; }
    /// <summary>
    /// 所处坐标改变的事件
    /// </summary>
    public System.Action<GameStage.Sector, GameStage.Sector> onSectorChanged = null;

    /// <summary>
    /// 类型
    /// </summary>
    [System.NonSerialized]
    public ObjectType typeID = ObjectType.Unknown;
    /// <summary>
    /// 所属的控制台
    /// </summary>
    [System.NonSerialized]
    public GameStage gameStage = null;
    /// <summary>
    /// 当前所在的坐标
    /// </summary>
    [System.NonSerialized]
    public GameStage.Sector curSector = null;
    /// <summary>
    /// 渲染控制器
    /// </summary>
    [System.NonSerialized]
    public RendererCtrl rendererCtrl = null;
    /// <summary>
    /// 特效控制器
    /// </summary>
    [System.NonSerialized]
    public FXCtrl fxCtrl = null;
    /// <summary>
    /// 碰撞器
    /// </summary>
    [System.NonSerialized]
    public Collider mouseCollider = null;
    /// <summary>
    /// 文字控制器
    /// </summary>
    [System.NonSerialized]
    public HeadTextCtrl headTextCtrl = null;

    /// <summary>
    /// 是否已经初始化完毕
    /// </summary>
    protected bool inited_ = false;
    /// <summary>
    /// 是否虚拟体状态
    /// </summary>
    protected bool isDummy_ = false;
    /// <summary>
    /// 是否被暂存
    /// </summary>
    protected bool isCache_ = false;
    /// <summary>
    /// 是否正在加载实体中
    /// </summary>
    protected bool isDownloading_ = false;
    protected bool isShowing = false;
    public bool IsShowing
    {
        get
        {
            return isShowing;
        }
    }
    /// <summary>
    /// 实体加载信息
    /// </summary>
    protected AssetMng.DownloadID pendingDownload = null; 


    /// <summary>
    /// 可视距离（坐标格为单位）
    /// </summary>
    public int cullDistance = 15;

    /// <summary>
    /// 高度
    /// </summary>
    protected float height = 5.0f;
    // <summary>
    /// 高度
    /// </summary>
    public float Height
    {
        get { return height; }
    }

    protected float beAttackRadius = 0.0f;
    /// <summary>
    /// 可被攻击半径
    /// </summary>
    public float BeAttackRadius
    {
        get
        {
            return beAttackRadius;
        }
    }
    /// <summary>
    /// 名字高度
    /// </summary>
    protected float nameHeight = 3.0f;
    /// <summary>
    /// 名字高度
    /// </summary>
    public float NameHeight
    {
        get
        {
            return nameHeight;
        }
    }

	protected float scaleBuffValue = 1.0f;
	public float ScaleBuffValue
	{
		get
		{
			return scaleBuffValue;
		}
	}


    /// <summary>
    /// 对象身边的随机点，用来给跟随的智能NPC或者宠物
    /// </summary>
    private List<PostionSlot> positionSlots_ = new List<PostionSlot>();
    public List<PostionSlot> positionSlots
    {
        get { return positionSlots_; }
    }

    public int AddPositionSlot()
    {
        positionSlots_.Add(new PostionSlot(this));
        return positionSlots_.Count - 1;
    }

    public PostionSlot GetIdleSlot()
    {
        foreach (PostionSlot slot in positionSlots_)
        {
            if (null == slot.occupyObj)
            {
                return slot;
            }
        }
        int index = AddPositionSlot();
        return positionSlots_[index];
    }


    protected Transform receivePoint = null;

    public virtual Transform GetReceivePoint()
    {
        if (receivePoint == null)
        {
            receivePoint = new GameObject("receivePoint").transform;
            receivePoint.parent = this.transform;
            receivePoint.localEulerAngles = Vector3.zero;
            receivePoint.localPosition = Vector3.zero;
            receivePoint.localScale = Vector3.one;
        }
        return receivePoint;
    }

    /// <summary>
    /// 尽量避免使用Awake等unity控制流程的接口来初始化，而改用自己调用的接口 by吴江
    /// </summary>
    protected virtual void Init()
    {
        rendererCtrl = this.gameObject.GetComponentInChildrenFast<RendererCtrl>(true);
        if (rendererCtrl != null)
        {
            rendererCtrl.Init();
        }
        fxCtrl = this.gameObject.GetComponentInChildrenFast<FXCtrl>(true);
        headTextCtrl = GetComponent<HeadTextCtrl>();
    }


    /// <summary>
    /// 注册事件
    /// </summary>
    protected virtual void Regist()
    {

    }
    /// <summary>
    /// 注销事件
    /// </summary>
    public virtual void UnRegist()
    {

    }

    public void DelayToDestroy(float time)
    {
        Invoke("DestroySelf", time);
    }

    public virtual void DestroySelf()
    {
        Destroy(this);
    }

    protected void OnDestroy()
    {
        if (!isCache_ && gameStage != null)
        {
            gameStage.RemoveObject(this);
            gameStage = null;
        }
        Destroy(gameObject);
    }

    protected void Sleep()
    {
        if (gameStage != null)
        {
            gameStage.RemoveObject(this);
            gameStage = null;
        }
    }

    public virtual void DoRingEffect(Color _color, bool _active,float _radius)
    {
        if (fxCtrl == null) return;
        fxCtrl.DoRingEffect(_color, _active, _radius);
    }



    protected virtual void OnUpdateHide(bool _hide)
    {
        gameObject.SetActive(!_hide);
    }

    /// <summary>
    /// 刷新着色器
    /// </summary>
    /// <param name="newGO"></param>
    public static void RefreshShader(GameObject newGO)
    {
        Renderer[] ren = newGO.GetComponentsInChildren<Renderer>(true);
        foreach (var a in ren)
        {
            foreach (var b in a.sharedMaterials)
            {
                b.shader = Shader.Find(b.shader.name);
            }
        }
    }
    /// <summary>
    /// 添加阻挡碰撞器
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    protected void AddBlockCollider(float x = 1f, float y = 2f, float z = 1f)
    {
        //GameObject blockCollider = new GameObject("Block Collider", typeof(BoxCollider));
        //blockCollider.transform.parent = transform;
        //blockCollider.transform.localPosition = Vector3.zero;
        //blockCollider.transform.localRotation = Quaternion.identity;
        //((BoxCollider)blockCollider.collider).size = new Vector3(x, y, z);
        //((BoxCollider)blockCollider.collider).center = new Vector3(0, y / 2f, 0);
        //blockCollider.layer = LayerMng.transparentWall;
    }




    public virtual void CopyFromDummy(InteractiveObject _object)
    {
        transform.parent = _object.transform.parent;
        transform.localPosition = _object.transform.localPosition;
        transform.localRotation = _object.transform.localRotation;
    }


    /// <summary>
    /// 获取鼠标悬停图标（手游用不到） by吴江
    /// </summary>
    /// <returns></returns>
    public virtual Texture2D[] GetCursor() { return null; }


    /// <summary>
    /// 鼠标进入的事件（手游用不到）  by吴江
    /// </summary>
    public virtual void OnHoverIn() { }

    /// <summary>
    /// 鼠标离开的事件（手游用不到）  by吴江
    /// </summary>
    public virtual void OnHoverOut() { }

    /// <summary>
    /// 鼠标悬停的事件（手游用不到）  by吴江
    /// </summary>
    public virtual void OnHoverStay() { }

    /// <summary>
    /// 发出互动声音 by吴江 
    /// </summary>
    public virtual void InteractionSound()
    {
    }
    /// <summary>
    /// 发出消亡声音 by吴江 
    /// </summary>
    public virtual void DeadSound()
    {
    }



    /// <summary>
    /// 激活一个碰撞体 by 吴江
    /// </summary>
    /// <param name="_active"></param>
    public void ActiveBoxCollider(bool _active = true,float radius = 2.5f)
    {
        BoxCollider box = this.gameObject.GetComponentInChildrenFast<BoxCollider>();
        if (box == null) box = this.gameObject.GetComponent<BoxCollider>();
        if (box == null) box = this.gameObject.AddComponent<BoxCollider>();
        box.center = new Vector3(0, this.Height / 2, 0);
        box.size = new Vector3(radius, this.Height, radius);
        box.isTrigger = true;
        box.enabled = _active;
    }

    /// <summary>
    /// 激活一个碰撞体 by 吴江
    /// </summary>
    /// <param name="_active"></param>
    public void ActiveBoxCollider(bool _active, Vector3 _scale)
    {
        BoxCollider box = this.gameObject.GetComponentInChildrenFast<BoxCollider>();
        if (box == null) box = this.gameObject.GetComponent<BoxCollider>();
        if (box == null) box = this.gameObject.AddComponent<BoxCollider>();
        box.center = new Vector3(0, this.Height / 2.0f, 0);
        box.size = new Vector3(_scale.x, _scale.y, _scale.z);
        box.isTrigger = true;
        box.enabled = _active;
    }

    /// <summary>
    /// 激活一个碰撞体 by 吴江
    /// </summary>
    /// <param name="_active"></param>
    public void ActiveBoxCollider(float _scale, bool _active = true)
    {
        BoxCollider box = this.gameObject.GetComponentInChildrenFast<BoxCollider>();
        if (box == null) box = this.gameObject.AddComponent<BoxCollider>();
        box.center = new Vector3(0, this.Height / 2, 0);
        box.size = new Vector3(2.5f * _scale, this.Height, 2.5f * _scale);
        box.isTrigger = true;
        box.enabled = _active;
    }


    /// <summary>
    /// 根据蒙皮模型体积激活一个碰撞体 by 吴江
    /// </summary>
    /// <param name="_active"></param>
    public void ActiveBoxColliderByBound(bool _active = true)
    {
        BoxCollider box = this.gameObject.GetComponentInChildrenFast<BoxCollider>();
        if (box == null) box = this.gameObject.AddComponent<BoxCollider>();
        Renderer rd = this.GetComponentInChildrenFast<SkinnedMeshRenderer>();
        if (rd == null) rd = this.GetComponentInChildrenFast<Renderer>();
        if (rd == null)
        {
            box.center = new Vector3(0, this.Height / 2, 0);
            box.size = new Vector3(2.5f, this.Height, 2.5f);
        }
        else
        {
            Bounds bound = rd.bounds;
            box.center = bound.center;
            box.size = bound.size;
        }
        box.isTrigger = true;
        box.enabled = _active;
    }
}











/// <summary>
/// 对象身边的随机点
/// </summary>
public class PostionSlot
{
    public Vector2 coordinate;
    private int index_ = 0;
    public int index
    {
        get { return index_; }
    }
    public GameObject obj = null;
    public Vector3 position
    {
        get { return obj.transform.position; }
    }
    public GameObject occupyObj = null;
    public InteractiveObject parents = null;

    public PostionSlot(InteractiveObject _parents)
    {
        parents = _parents;
        index_ = _parents.positionSlots.Count;
        if (_parents == null || _parents.gameObject == null)
        {
            obj = null;
            return;
        }
        if (null == obj)
        {
            obj = new GameObject();
        }
        obj.transform.parent = _parents.gameObject.transform;
        if (index_ == 0)
        {
            coordinate = new Vector2(0, 0);
            obj.transform.localPosition = new Vector3(0, 0, 0);
            obj.name = "Player Slot : " + index_.ToString() + "_" + coordinate.x.ToString() + ":" + coordinate.y.ToString();
            return;
        }
        int a = 1;
        while (index_ >= (a - a / 2) * a)
        {
            a += 2;
        }
        int b = a / 2;
        int count = 0;
        for (int i = -b; i < (b + 1); i++) //x
        {
            for (int j = 0; j > -(b + 1); j--)//z
            {
                count++;
                if (count == index_)
                {
                    coordinate = new Vector2(i, j);
                    obj.transform.localPosition = new Vector3(i, 0, j);
                    obj.name = "Player Slot : " + index_.ToString() + "_" + coordinate.x.ToString() + ":" + coordinate.y.ToString();
                }
            }
        }
    }

}
