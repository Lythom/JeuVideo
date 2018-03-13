// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/AnimatedPotionShader" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_AnimationSpeed ("AnimationSpeed", Float) = 1
		_Color ("Color", Color) = (0.5, 0, 0, 1) // (R, G, B, A)
	}
	SubShader {
		Tags {
			"Queue" = "Geometry"
			"RenderType" = "Opaque"
		}

		CGPROGRAM
		// Uses the Labertian lighting model
		#pragma surface surf Lambert
		
		sampler2D _MainTex; // The input texture
		float _AnimationSpeed;
		half4 _Color;
		
		struct Input {
			float2 uv_MainTex;
		};
		
		void surf (Input IN, inout SurfaceOutput o) {
			fixed2 scrolledUV = IN.uv_MainTex; //***
 			fixed yScrollValue = frac(_AnimationSpeed * _Time.y); //***
		   
		   	float remmappedY = (frac(((IN.uv_MainTex.y * IN.uv_MainTex.y * IN.uv_MainTex.y * 8) / 0.25 - 0.0820*4 + yScrollValue)) + 0.0820*4) * (0.25);
			bool insidePotion = IN.uv_MainTex.y < 0.35 && IN.uv_MainTex.x < 0.3;

			float2 uv_pannedTex = float2(
				IN.uv_MainTex.x,
				remmappedY
			);

			if (insidePotion) {
				fixed4 c = tex2D (_MainTex, uv_pannedTex) * _Color; //***
				o.Albedo = c.rgb;
			} else {
				o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
			}
			
		}
		ENDCG
	}
}