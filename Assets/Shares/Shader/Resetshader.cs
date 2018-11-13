using UnityEngine;
using System.Collections;

public class Resetshader : MonoBehaviour {
	public string resetshadername="Terrain/4blendforT";
	// Use this for initialization
	void Start () {
		Material sm = gameObject.GetComponent<Renderer>().material;
		if(sm==null)
		{
		Debug.LogWarning("no mat");
			return;
		}
		string shadername=sm.shader.name;
		if(shadername==null||shadername.Trim().Length==0)
		{
		shadername=resetshadername;			
		}
		Shader newshader=Shader.Find(shadername);
		if(newshader!=null)
		{
		sm.shader=newshader;	
		}
		else
		{
		Debug.LogWarning("can't reset shader");	
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
