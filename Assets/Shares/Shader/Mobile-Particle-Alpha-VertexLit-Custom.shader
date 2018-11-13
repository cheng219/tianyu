// Simplified VertexLit Blended Particle shader. Differences from regular VertexLit Blended Particle one:
// - no AlphaTest
// - no ColorMask

Shader "Mobile/Particles/VertexLit Blended Custom" {
Properties {
	_EmisColor ("Emissive Color", Color) = (.2,.2,.2,0)
	_MainTex ("Particle Texture", 2D) = "white" {}
	_ColliderColor ("Collider Color", Color) = (0.01,0.01,0.9,0.8)
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha OneMinusSrcAlpha
	Cull Off ZWrite Off Fog { Color (0,0,0,0) }
	
	Lighting On

	SubShader {
		Pass {
			Ztest Greater

			Material { Emission [_ColliderColor] }
			ColorMaterial AmbientAndDiffuse

			SetTexture [_MainTex] {
				combine texture * primary
			}
		}
		Pass {
			Material { Emission [_EmisColor] }
			ColorMaterial AmbientAndDiffuse

			SetTexture [_MainTex] {
				combine texture * primary
			}
		}
	}
}
}