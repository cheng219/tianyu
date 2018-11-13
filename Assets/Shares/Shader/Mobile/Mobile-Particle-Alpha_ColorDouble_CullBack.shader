// Simplified Alpha Blended Particle shader. Differences from regular Alpha Blended Particle one:
// - no Tint color
// - no Smooth particle support
// - no AlphaTest
// - no ColorMask

Shader "Mobile/Particles/Alpha Blended_ColorDouble_CullBack" {
Properties {
	_TintColorB ("_TintColorB", Color) = (0.5,0.5,0.5,0.5)
	_MainTex ("_MainTex", 2D) = "white" {}
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha OneMinusSrcAlpha
	Cull Back Lighting Off ZWrite Off Fog {Mode Off  Color (0,0,0,0) }
	
	BindChannels {
		Bind "Color", color
		Bind "Vertex", vertex
		Bind "TexCoord", texcoord
	}
	
	SubShader {
		Pass {
			SetTexture [_MainTex] {
				combine texture * primary Double
			}
			SetTexture [_MainTex] {
				constantColor [_TintColorB]
				combine previous * constant//* primary
			}
		}
	}
}
}
