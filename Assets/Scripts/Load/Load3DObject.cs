//======================================
//作者:吴江
//日期:2016/3/22
//用途:NGUI挂载3D模型所用
//======================================

using UnityEngine;
using System.Collections;


public enum NGUI3DType
{
    Unkown,
    Player,
    NPC,
    Monster,
    Mount,
    Entourage,
	Equipment,
}


public class Load3DObject : MonoBehaviour {

    public int configID = -1;
    public NGUI3DType type = NGUI3DType.Unkown;
    public float scale = 100f;
    public float lookRotation = 180f;
    public int[] equipList = new int[5];

    void Start()
    {

    }

	public void StartLoad(int _configID,NGUI3DType _type,int[] _equipList)
	{
		equipList = _equipList;
		StartLoad(_configID,_type);
	}

	public void StartLoad(int _configID,NGUI3DType _type)
	{
		configID = _configID;
		type = _type;
		StartLoad();
	}

	public void StartLoad()
	{
		if (configID <= 0)
		{
			Debug.LogError("发现一个无效NGUI 3D加载组件!" + this.gameObject.name);
			return;
		}
		ClearModel();
		switch (type)
		{
		case NGUI3DType.Player:
			LoadPlayer();
			break;
		case NGUI3DType.Monster:
			LoadMob();
			break;
		case NGUI3DType.NPC:
			LoadNPC();
			break;
		case NGUI3DType.Mount:
			LoadMount();
			break;
		case NGUI3DType.Entourage:
			LoadEntourage();
			break;
		case NGUI3DType.Equipment:
			LoadEquipment();
			break;
		default:
			Debug.LogError("发现一个无效NGUI 3D加载组件!" + this.gameObject.name);
			break;
		}
	}
	public void ClearModel()
	{
		for (int i = 0; i < this.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(this.transform.GetChild(i).gameObject);
		}
	}


    protected void LoadPlayer()
    {
        PlayerBaseData data = new PlayerBaseData();
        data.prof = configID;
        for (int i = 0; i < equipList.Length; i++)
        {
            if (equipList[i] > 0)
            {
                data.equipTypeList.Add(equipList[i]);
            }
        }
        GameCenter.previewManager.TryPreviewSinglePlayer(new PlayerBaseInfo(data), (x) =>
        {
            x.transform.parent = this.transform;
            x.transform.localPosition = Vector3.zero;
            x.transform.localEulerAngles = Vector3.zero;
            x.transform.localScale = Vector3.one * scale;
            x.FaceToNoLerp(lookRotation);
			x.gameObject.SetMaskLayer(LayerMask.NameToLayer("NGUI"));
        });
    }

    protected void LoadNPC()
    {
        if (configID <= 0) return;
        GameCenter.previewManager.TryPreviewSingelNPC(configID, (x) =>
        {
            x.transform.parent = this.transform;
            x.transform.localPosition = Vector3.zero;
            x.transform.localEulerAngles = Vector3.zero;
            x.transform.localScale = Vector3.one * scale;
            x.FaceToNoLerp(lookRotation);
            x.rendererCtrl.SetLayer(LayerMask.NameToLayer("NGUI"));
        });
    }

    protected void LoadMob()
    {
        if (configID <= 0) return;
        MonsterData data = new MonsterData();
        data.prof = configID;
        MonsterInfo info = new MonsterInfo(data);
        GameCenter.previewManager.TryPreviewSingelMonster(info, (x) =>
        {
            x.transform.parent = this.transform;
            x.transform.localPosition = Vector3.zero;
            x.transform.localEulerAngles = Vector3.zero;
            x.transform.localScale = Vector3.one * scale;
            x.FaceToNoLerp(lookRotation);
            x.rendererCtrl.SetLayer(LayerMask.NameToLayer("NGUI"));
        });
    }

    protected void LoadMount()
    {
        if (configID <= 0) return;
        MountRef refData = ConfigMng.Instance.GetMountRef(configID);
        MountInfo info = new MountInfo(refData);
        GameCenter.previewManager.TryPreviewSingelMount(info, (x) =>
        {
            x.transform.parent = this.transform;
            x.transform.localPosition = Vector3.zero;
            x.transform.localEulerAngles = Vector3.zero;
            x.transform.localScale = Vector3.one * scale;
            x.FaceToNoLerp(lookRotation);
            x.rendererCtrl.SetLayer(LayerMask.NameToLayer("NGUI"));
			x.gameObject.SetMaskLayer(LayerMask.NameToLayer("NGUI"));
        });
    }

    protected void LoadEntourage()
    {
        if (configID <= 0) return;
		NewPetRef refData = ConfigMng.Instance.GetNewPetRef(configID);
		MercenaryInfo info = new MercenaryInfo(refData);
		GameCenter.previewManager.TryPreviewSingelEntourage(info, (x) =>
        {
            x.transform.parent = this.transform;
            x.transform.localPosition = Vector3.zero;
            x.transform.localEulerAngles = Vector3.zero;
            x.transform.localScale = Vector3.one * scale;
            x.FaceToNoLerp(lookRotation);
            x.gameObject.SetMaskLayer(LayerMask.NameToLayer("NGUI"));
        });
    }

    protected void LoadEquipment()
    {
        if (configID <= 0) return;
        EquipmentInfo data = new EquipmentInfo(configID, EquipmentBelongTo.PREVIEW);
        GameCenter.previewManager.TryPreviewSingleEquipment(data, (x) =>
        {
            x.transform.parent = this.transform;
            x.transform.localPosition = Vector3.zero;
            x.transform.localEulerAngles = Vector3.zero;
            x.transform.localScale = Vector3.one * scale;
            x.FaceToNoLerp(lookRotation);
            x.gameObject.SetMaskLayer(LayerMask.NameToLayer("NGUI"));
            if (x.rendererCtrl != null)
                x.rendererCtrl.SetLayer(LayerMask.NameToLayer("NGUI"));
        });
    }
}
