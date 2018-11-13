// Simplified Additive Particle shader. Differences from regular Additive Particle one:
// - no Tint color
// - no Smooth particle support
// - no AlphaTest
// - no ColorMask

Shader "Mobile/Particles/Additive_ColorDouble" {
Properties {
	_TintColorA ("_TintColorA", Color) = (0.5,0.5,0.5,0.5)
	_MainTex ("_MainTex", 2D) = "white" {}
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha One
	//Blend SrcColor One
	Cull Off Lighting Off ZWrite Off Fog {Mode Off Color (0,0,0,0) }
	
	BindChannels {
		Bind "Color", color
		Bind "Vertex", vertex
		Bind "TexCoord", texcoord
	}
	
	SubShader {
		Pass {
		ColorMaterial AmbientAndDiffuse
			SetTexture [_MainTex] {
				combine texture * primary Double//* primary
			}
			SetTexture [_MainTex] {
				constantColor [_TintColorA]
				combine previous * constant//* primary
			}
		}
	}
}
}