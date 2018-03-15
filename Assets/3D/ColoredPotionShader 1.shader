// Upgrade NOTE: upgraded instancing buffer 'MyProperties' to new syntax.

// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ColoredPotionShader" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (0.5, 0, 0, 1) // (R, G, B, A)
	}
	SubShader {
		Tags {
			"Queue" = "Geometry"
			"RenderType" = "Opaque"
		}

		CGPROGRAM
		// Uses the Labertian lighting model
		#pragma surface surf Standard
		#pragma multi_compile_instancing

		sampler2D _MainTex; // The input texture
		half4 _Color; // The input texture
		
		struct Input {
			float2 uv_MainTex;
		};
		
		void surf (Input IN, inout SurfaceOutputStandard o) {

			if (IN.uv_MainTex.y < 0.35 && IN.uv_MainTex.x < 0.3) {
				fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color; //***
				o.Albedo = c.rgb;
				o.Metallic = 0;
				o.Smoothness = 0;
				// o.Emission = tex2D (_MainTex, IN.uv_MainTex) * _Color * 2;
			} else {
				o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
				o.Metallic = 0.2;
				o.Smoothness = 0.8;
			}
			
		}
		ENDCG
	}
}