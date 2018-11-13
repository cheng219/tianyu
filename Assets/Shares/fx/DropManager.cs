///////////////////////////////////////////////////////////////////////////////////////////
//作者：沙新佳 
//最后修改时间：#Date#
//脚本描述： 这只是一个下冰效果的管理器原型
///////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DropManager : MonoBehaviour {

	public float areaRadius = 5f;
	public float birthHeight = 5f;
	public int maxCountSize =5;
	public int minCountSize =4;

	public float maxDelayTime = 0.8f;
	public float minDelayTime = 0f;

	public GameObject[] preSources;
	public GameObject bottomObj;
	public List<GameObject> templist = new List<GameObject>();

	
	public int CountSize
	{
		get
		{
			return Random.Range(minCountSize,maxCountSize);
		}
	}
	void Awake()
	{
		int count = preSources.Length;
		for (int i = 0; i < count; i++) {
			preSources[i].SetActive(false);
		}
	}


	void Start()
	{
		 //  Random.insideUnitCircle
	}

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		Play();
	}
	/// <summary>
	/// Raises the disble event.
	/// </summary>
	void OnDisable()
	{
		int count = templist.Count;
		for (int i = 0; i < count; i++) {
			Destroy(templist[i]);
		}
		templist.Clear();
	}


	void Play()
	{
		//Vector2 originValue =  Random.insideUnitCircle;
		//Vector3 birthPosition = new Vector3(originValue.x,birthHeight,originValue.y);
		//获得一个随机源
		GameObject originObj = GetRandomSource();
		int count = CountSize;
		for (int i = 0; i < count; i++) {
			GameObject obj = Instantiate(originObj) as GameObject;
			Transform tran = obj.transform;
			tran.parent = this.gameObject.transform;
			Vector3 birthPosition;
			Vector2 originValue =  Random.insideUnitCircle;;
			if(i==0)
			{
				birthPosition = new Vector3(0f,0f,0f);
				tran.localPosition =  birthPosition;
			}
			else
			{
				int mark =  i%4;
				switch (mark) {
				case 0:
					do
					{
						originValue =  Random.insideUnitCircle;
					}
					while(!(originValue.x>0 && originValue.y>0));
					break;
				case 1:
					do
					{
						originValue =  Random.insideUnitCircle;
					}
					while(!(originValue.x<0 && originValue.y>0));
					break;
				case 2:
					do
					{
						originValue =  Random.insideUnitCircle;
					}
					while(!(originValue.x<0 && originValue.y<0));
					break;
				case 3:
					do
					{
						originValue =  Random.insideUnitCircle;
					}
					while(!(originValue.x>0 && originValue.y<0));
					break;
				}
				birthPosition = new Vector3( originValue.x*areaRadius,0f, originValue.y*areaRadius);
				tran.localPosition = birthPosition;
			}
			//Transform bin = tran.FindChild("bing");

			//起始面
			Vector3 startPosition = new Vector3(tran.position.x,this.transform.position.y+birthHeight,tran.position.z);
			//目标面
			Vector3 targetPosition = new Vector3(tran.position.x,this.transform.position.y,tran.position.z);
			//对象显示
			//bin.position = startPosition;
			Delay mydelay =  obj.GetComponent<Delay>();
			if(mydelay==null)mydelay = obj.AddComponent<Delay>();
			mydelay.delayTime = Random.Range(minDelayTime,maxDelayTime);

			obj.SetActive(true);
			//动画移动
			//iTween.MoveTo(bin.gameObject,targetPosition,1f);
			//tran.position+Vector3.down*(birthHeight-0.5f)

			templist.Add(obj);
		}
	}



	GameObject GetRandomSource()
	{
		int count = preSources.Length;
		if(count == 0)
		{
			Debug.LogError("preSources is null");
			return new GameObject();
		}
		else if(count ==1)
		{
			if(preSources[0]==null)
			{
				Debug.LogError("preSources is null");
				return new GameObject();
			}
			else
				return preSources[0];
		}
		else
		{
			int randomValue = Random.Range(0,count);
			return preSources[randomValue];
		}
	}
	/// <summary>
	/// 预览位置框
	/// </summary>
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.white;
		//Gizmos.DrawSphere (transform.position, 1);
		Gizmos.DrawWireCube(transform.position+Vector3.up*birthHeight,new Vector3(areaRadius*2,0.2f,areaRadius*2));
		Gizmos.DrawWireSphere(transform.position,areaRadius);//+Vector3.up*birthHeight
	}
}
