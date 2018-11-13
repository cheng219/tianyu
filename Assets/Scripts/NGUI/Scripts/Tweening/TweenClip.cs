using UnityEngine;
using System.Collections;

// 控制panel的裁剪区域，目前只实现改变X轴 -- by ms 
public class TweenClip : MonoBehaviour {
	
	public float time;
	public enum style {Once = 0,Loop = 1};
	public style _mystyle = style.Once;
	UIPanel _mypanel;
	float delta;
	Vector4 vec4;

	// Use this for initialization
	void Start () {
		_mypanel = gameObject.GetComponent<UIPanel>();
		if (!_mypanel)
		{
#if UNITY_EDITOR
		 Debug.LogError("No UIpanel attach,you idiot!");
#endif
		 return;
		}
		delta = 0.0f;
		vec4 = _mypanel.finalClipRegion;
	}
	
	// Update is called once per frame
	void Update () {
		if (_mystyle == style.Once)
		  if(delta >= vec4.z) return;
			else {
				  delta += time;
		          _mypanel.SetRect(vec4.x,vec4.y,delta,vec4.w);
			}
		else if(_mystyle == style.Loop) 		
			{
			  delta = delta < vec4.z ? delta + time:0.0f;
			  _mypanel.SetRect(vec4.x,vec4.y,delta,vec4.w);
			}
    }
}
