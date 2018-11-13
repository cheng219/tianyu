using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopoutGifts : GUIBase {
	
	public GameObject button;
	public GameObject effect;
	public UIGrid firstGrid;


	public GameObject item;
	protected int howmany;

    protected EquipmentInfo info = null;

    void Awake() 
    {
        
        mutualExclusion = false;
        layer = GUIZLayer.COVER;
    }

    void OnEnable()
    {
        

    }

    protected override void OnOpen()
    {
        base.OnOpen();

        ShowItem(gameObject);
    }

    void ShowItem(GameObject go)
	{
        List<EquipmentInfo> infoList = GameCenter.inventoryMng.randomChestList;

        var list = firstGrid.GetChildList();
		if(list.Count > 0 )
		{
			foreach(var item in list) 
			{
				NGUITools.Destroy(item.gameObject);
			}
		}
		list = null;
        howmany = infoList.Count;
        for (int i = 1; i <= howmany; i++)
        {
            Create(i, infoList[i - 1]);
        }
	}

	void Create(int idx,EquipmentInfo _info)
	{
        if (_info == null) return;
		var go = Instantiate(item);
	//	var moreThan5 = idx > 5 ? 1:0;
	//	idx = idx > 5 ? idx - 5:idx;
		go.transform.parent = item.transform.parent;
		go.transform.localPosition = item.transform.localPosition;
		go.transform.localScale = item.transform.localScale;
        
		go.SetActive(true);
        ItemUI itemTemp = null;
        itemTemp = go.GetComponentInChildren<ItemUI>();
        if (item != null)
        {
            itemTemp.FillInfo(_info);
            itemTemp.ShowName();
        }
        go.GetComponent<TweenPosition>().to = FigureLocalPosition(idx);

        PlayEff(go, _info);
        go.transform.parent = firstGrid.transform;
    
	}

	void PlayEff(GameObject go,EquipmentInfo _info)
	{
        Color col = SetParticalColor(_info);
        if (col != Color.clear)
        {
            var eff = Instantiate(effect);
            ParticleSystem particle =  eff.GetComponentInChildren<ParticleSystem>();
            if (particle != null)
            { }
            eff.transform.parent = go.transform;
            eff.transform.localPosition = effect.transform.localPosition;
            eff.transform.localScale = effect.transform.localScale;
            eff.SetActive(true);
        }
	}

    /// <summary>
    ///计算位置
    /// </summary>
    protected Vector3 FigureLocalPosition(int _curNum) 
    {
        float line = _curNum / (float)firstGrid.maxPerLine;
        int curLine = Mathf.CeilToInt(line);
       // Debug.Log(curLine);
        int col = _curNum % firstGrid.maxPerLine == 0 ? firstGrid.maxPerLine : _curNum % firstGrid.maxPerLine;

        int baseCol = howmany % firstGrid.maxPerLine == 0 ? firstGrid.maxPerLine : howmany % firstGrid.maxPerLine;
        line = howmany / (float)firstGrid.maxPerLine;
        int maxLine = Mathf.CeilToInt(line);

        float positionXBase = 0;
        if (maxLine - curLine > 0)
        {
            positionXBase = -4 * (firstGrid.cellWidth / 2.0f);
        }
        else 
        {
            positionXBase = -firstGrid.cellWidth / 2.0f * (baseCol - 1);
        }

        float poxitionYBase = firstGrid.cellHeight / 2.0f * (maxLine - 1);
        //   Debug.Log(height + ":" + firstGrid.cellHeight);
        float positionX = (col - 1) * firstGrid.cellWidth + positionXBase;
        float positionY = -(curLine - 1) * firstGrid.cellWidth + poxitionYBase;

        return new Vector3(positionX, positionY, 0);

    }

    /// <summary>
    ///根据装备的品质设置粒子颜色
    /// </summary>
    protected Color SetParticalColor(EquipmentInfo _info)
    {
      //  Debug.Log(_info.Quality);
        switch (_info.Quality)
        {
            case 1: 
            case 2: 
            case 3:
                return Color.clear;
            case 4:
                return Color.HSVToRGB(130f,130f,130f,true);
            case 5:
                return Color.yellow;
            default:
                return Color.clear;
        }

    }


    public void CloseWnd() 
    {
    //    GameCenter.uIMng.ReleaseGUI(GUIType.BOXREWARD);
    }


}

