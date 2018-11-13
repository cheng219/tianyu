using UnityEngine;
using System.Collections;

public class AfsShockWave : MonoBehaviour {
	public float ShockPower = 8f;
	public float ShockRadius = 3f;
	public float ShockSpeed = 5f;
	public Collider PushCollider;
	public float ShockCount = 100f;
	public float ShockDelay = 5f;

	void Awake()
	{
		if(PushCollider==null)
			PushCollider = this.gameObject.AddComponent<SphereCollider>();
	}

	// Use this for initialization
	void Start () {
//		iTween.Init(gameObject);
//		iTween.tweens.a
//		iTween.Defaults.loopType= iTween.LoopType.loop;

		//iTween.RotateBy(gameObject, iTween.Hash("x", .25, "easeType", "easeInOutBack", "loopType", "pingPong", "delay", .4));
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.Space))
		{
			iTween.PunchScale(gameObject,new Vector3(5f,5f,5f),1.5f);
			//iTween.RotateBy(gameObject, iTween.Hash("x", .25, "easeType", "easeInOutBack", "loopType", "pingPong", "delay", .4));
		}

	}
}
