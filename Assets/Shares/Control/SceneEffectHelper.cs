using UnityEngine;
using System.Collections;

public class SceneEffectHelper : MonoBehaviour {
	public string EffectName;

	void Awake()
	{
		if (EffectName == string.Empty) {
			Transform tran = this.transform.parent;
			if (tran) {
				this.transform.position = Vector3.zero;
				EffectName = this.transform.parent.name + "_effect";
				this.gameObject.name = this.transform.parent.name + "_effect";
			}
		}
	}
}
