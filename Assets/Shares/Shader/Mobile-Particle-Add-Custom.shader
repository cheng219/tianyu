// Simplified Additive Particle shader. Differences from regular Additive Particle one:
// - no Tint color
// - no Smooth particle support
// - no AlphaTest
// - no ColorMask

Shader "Mobile/Particles/AdditiveCustom" {
Properties {
	_MainTex ("Particle Texture", 2D) = "white" {}
	_ColliderColor ("Collider Color", Color) = (0.01,0.01,0.9,0.8)
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha One
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
		}

		Pass {
			SetTexture [_MainTex] {
				combine texture * primary
			}
		}
	}
}
}