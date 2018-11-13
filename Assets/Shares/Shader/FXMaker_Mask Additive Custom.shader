Shader "FXMaker/Mask Additive Custom" {
	Properties {
		_MainTex ("Particle Texture", 2D) = "white" {}
		_Mask ("Mask", 2D) = "white" {}
		_ColliderColor ("Collider Color", Color) = (0.01,0.01,0.9,0.8)
	}

	Category {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha One
// 		AlphaTest Greater .01
// 		ColorMask RGB
		Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
		BindChannels {
 			Bind "Color", color
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
		}
		
		SubShader {
			Pass {
				Ztest Greater

				SetTexture [_MainTex] {
					constantColor [_ColliderColor]
					combine constant * primary
				}

				SetTexture [_MainTex] {
					combine texture * previous DOUBLE
				}

				SetTexture [_Mask] {
				constantColor [_ColliderColor]
				combine texture * previous
				}


			}
			Pass {
 				SetTexture [_Mask] {combine texture * primary}
				SetTexture [_MainTex] {
					combine texture * previous
				}
			}
		}
	}
}
