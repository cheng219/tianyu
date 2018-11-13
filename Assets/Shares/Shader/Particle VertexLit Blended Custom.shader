Shader "Particles/VertexLit Blended Custom" {
Properties {
	_EmisColor ("Emissive Color", Color) = (.2,.2,.2,0)
	_MainTex ("Particle Texture", 2D) = "white" {}
	_ColliderColor ("Collider Color", Color) = (0.01,0.01,0.9,0.8)
}

SubShader {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Tags { "LightMode" = "Vertex" }
	Cull Off
	Lighting On
	ColorMaterial AmbientAndDiffuse
	ZWrite Off
	ColorMask RGB
	Blend SrcAlpha OneMinusSrcAlpha
	AlphaTest Greater .001
	Pass { 
		Ztest Greater

		Material { Emission [_ColliderColor] }
		 SetTexture [_MainTex] {
				constantColor [_ColliderColor]
				combine constant * primary
			}

			SetTexture [_MainTex] {
				combine texture * previous DOUBLE
			}
	}

	Pass { 
		Material { Emission [_EmisColor] }
		SetTexture [_MainTex] { combine primary * texture }
	}
}
}