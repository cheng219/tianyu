Shader "FXMaker/Mask Alpha Blended Tint Custom" {
	Properties {
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_Mask ("Mask", 2D) = "white" {}
		_ColliderColor ("Collider Color", Color) = (0.01,0.01,0.9,0.8)
	}

	Category {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
// 		AlphaTest Greater .01
// 		ColorMask RGB
		Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
		BindChannels {
			Bind "Color", color
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
		}
		
		// ---- Dual texture cards
		SubShader {
			Pass {
				ZWrite Off
				Ztest Greater

				SetTexture [_MainTex] {
					constantColor [_ColliderColor]
					combine constant * primary
				}
	 			SetTexture [_Mask] {combine texture * previous}
				SetTexture [_MainTex] {
					combine texture * previous DOUBLE
				}
			}

			Pass {
				SetTexture [_MainTex] {
					constantColor [_TintColor]
					combine constant * primary
				}
	 			SetTexture [_Mask] {combine texture * previous}
				SetTexture [_MainTex] {
					combine texture * previous DOUBLE
				}
			}
		}
		
		// ---- Single texture cards (does not do color tint)
		SubShader {
			Pass {
				SetTexture [_Mask] {combine texture * primary}
//				SetTexture [_Mask] {combine texture DOUBLE}
				SetTexture [_MainTex] {
					combine texture * previous
				}
			}
		}
	}
}
