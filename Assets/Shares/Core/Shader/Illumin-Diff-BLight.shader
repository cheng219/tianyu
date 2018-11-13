Shader "DSelf-Illumin/Illumin-Diff-BLight" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
		_Illum ("Illumin (A)", 2D) = "white" {}
		_EmissionLM ("Emission (Lightmapper)", Float) = 0
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
		
		_BackLightColor ("Back Light Color", Color) = (0.1,0.2,1,1)
		_BackLightModul("Modulus", Range (0.1, 5.0)) = 2.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert
		
		sampler2D _MainTex;
		sampler2D _Illum;
		fixed4 _Color;
		
		float4 _BackLightColor;
		float _BackLightModul;
		
		struct Input {
			float2 uv_MainTex;
			float2 uv_Illum;
			
			float4 pos;
			float3 normalPos;
		};
		
		void vert (inout appdata_full v, out Input o) {
          UNITY_INITIALIZE_OUTPUT(Input,o);
      	  
          o.pos = v.vertex;
          o.normalPos = v.normal;
          
		  
      	}
		
		void surf (Input IN, inout SurfaceOutput o) {
			float4 backLightColor;
			//rim(back) light
			float3 viewDir = normalize(ObjSpaceViewDir (IN.pos ) );
			float parm = dot(IN.normalPos, viewDir);
			
			parm = 1-parm;
			parm = pow(parm,_BackLightModul);
			parm = max(parm,0.0);
			//o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
			backLightColor = _BackLightColor*parm;
			//o.alpha = parm;
			////////////////////////////////////////////////////
			
			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 c = tex * _Color;
			o.Albedo = c.rgb;
			o.Emission = (c.rgb * tex2D(_Illum, IN.uv_Illum).a) + backLightColor.xyz;
			
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Self-Illumin/VertexLit"
}
