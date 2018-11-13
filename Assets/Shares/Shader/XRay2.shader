Shader "Custom/XRay_Fixed" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ColliderColor ("_Color", Color) = (0,1,0,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
//		Tags { "RenderType"="Transparent" }
//		Tags { "RenderType"="Overlay" }
		LOD 100
		Pass{
		ZWrite Off
		Ztest Greater
		Blend SrcAlpha OneMinusSrcAlpha
		Tags {"Queue"="Opaque" "LightMode" = "Vertex" }
            ColorMaterial AmbientAndDiffuse
            Lighting On
            SetTexture [_] {
            	constantColor[_ColliderColor]
                combine constant
            } 
		
		}
		
        Pass {
            Tags {"Queue"="Overlay" "LightMode" = "Vertex" }

           // Blend SrcAlpha OneMinusSrcAlpha
            ColorMaterial AmbientAndDiffuse
            Lighting On
            SetTexture [_MainTex] {
                combine texture
            } 
        }
	} 
	FallBack "Diffuse"
}
