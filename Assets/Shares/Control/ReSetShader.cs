using UnityEngine;
using System.Collections;

public class ReSetShader : MonoBehaviour {
	private Renderer[] rederers;

	// Use this for initialization
	void Awake () {
		rederers = GetComponentsInChildren<Renderer> ();
	}

	void Start () {
		ResetChildshader ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ResetChildshader()
	{
		//处理 animation
		for (int i = 0; i < rederers.Length; i++)
		{
			if (rederers[i] != null)
			{
				Material[] mats = rederers [i].sharedMaterials;
				for (int j = 0; j < mats.Length; j++) {
					if (mats [j]) {
						mats [j].shader = Shader.Find (mats [j].shader.name);
						Debug.logger.Log(rederers [i].name +" "+mats [j].name + mats [j].shader.name +" Reset" );
					} else {
						Debug.LogWarning(rederers [i].name +" "+j +"号材质槽为空！" );
					}
				}
			}
			else
			{
				Debug.LogWarning("某些特效组件已销毁，这不符合可重用特效");
			}
		}
	}
}
