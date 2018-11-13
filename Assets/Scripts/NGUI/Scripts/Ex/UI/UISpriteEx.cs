///////////////////////////////////////////////////////////////////////////////////////////
//作者：沙新佳 
//最后修改时间：#Date#
//脚本描述： 
///////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/NGUI Sprite Ex")]
public class UISpriteEx : UISprite
{
	public enum ColorGray
	{
		normal,//标准彩色
		Gray,//灰色
	}
	[HideInInspector][SerializeField]protected Material grayMaterial;
	[HideInInspector][SerializeField] protected ColorGray isGray = ColorGray.Gray;


	/// <summary>
	/// 获取 或者 设置当前灰色替代材质
	/// </summary>
	/// <value>The gray material.</value>
	public Material GrayMaterial {
		get {
			return grayMaterial;
		}
		set {
			grayMaterial = value;
		}
	}

	/// <summary>
	/// 获取 或者 设置 当前UIspriteEx的颜色模式：彩色 或者 灰色
	/// </summary>
	/// <value><c>true</c> if this instance is gray; otherwise, <c>false</c>.</value>
	public virtual ColorGray IsGray
	{
		get
		{
			return isGray;
		}
		set
		{
			if (isGray != value)
			{
				isGray = value;
				MarkAsChanged();
//				Debug.Log(isGray.ToString());
			}
		}
	}

	public override Material material
	{
		get {
			Material mat = base.material;
			if ( mat == null )
			{
				//Debug.LogError("material:null");
				// mat = (mAtlas != null) ? mAtlas.spriteMaterial : null; 
			}
			if(isGray == ColorGray.Gray)
			{
				if (grayMaterial != null)
				{
					if(grayMaterial.shader.name.Contains("Gray"))
					{
						GrayMaterial.SetTexture("_MainTex", mat.GetTexture("_MainTex"));
						//Debug.Log("use Gray mat");
						return GrayMaterial;
					}
					else
					{
						Debug.LogError(grayMaterial.name+"not a Gray Material");
						IsGray = ColorGray.normal;
						return mat;
					}
				}
				else
				{
					Debug.LogError("grayMaterial null");
					IsGray = ColorGray.normal;
					return mat;
				}
			}
			else
			{
				return mat;
			}
			return mat;
		} 
	}

	public override void OnFill (BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{

		base.OnFill(verts,uvs,cols);
		RefreshPanel(this.gameObject);
	}

	#region 刷新Panel模块
	/// <summary>
	/// Gets the most close panel.
	/// </summary>
	/// <returns>The most close panel.</returns>
	/// <param name="rootTrans">Root trans.</param>
	GameObject GetMostClosePanel(Transform rootTrans)
	{
		if (rootTrans.GetComponent<UIPanel>() != null)
		{
			return rootTrans.gameObject;
		}
		else if (rootTrans.parent == null)
		{
			return null;
		}
		else
		{
			return GetMostClosePanel(rootTrans.parent);
		}
	}
	
	GameObject panelObj = null;
	bool selfRefresh = true;

	/// <summary>
	/// Refreshs the panel.
	/// </summary>
	/// <param name="go">Go.</param>
	public void RefreshPanel(GameObject go)
	{
		if (!selfRefresh)
			return;
		
		if (panelObj == null)
		{
			panelObj = GetMostClosePanel(go.transform);
		}
		
		if (panelObj != null)
		{
			panelObj.GetComponent<UIPanel>().enabled = false;
			panelObj.GetComponent<UIPanel>().enabled = true;
			go.SetActive(false);
			go.SetActive(true);
		}
	}

	#endregion
}
