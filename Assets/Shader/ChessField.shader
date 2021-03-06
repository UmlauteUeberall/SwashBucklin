﻿Shader "Custom/ChessField" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_LineSize ("Line Size", Range (0,1)) = 0.1
		_GridSize ("Grid Size", Range(1,100)) = 20
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 4.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _LineSize;
		float _GridSize;

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			float2 wp = IN.worldPos.xz + float2(1000,1000);
			wp += float2(0.5f, 0.5f) * _GridSize;
			wp = float2(fmod(wp.x, _GridSize), fmod(wp.y, _GridSize));
			//wp = float2(0.5f, 0.5f) - wp;
			wp = float2(abs(wp.x), abs(wp.y));
			if (wp.x < _LineSize || wp.y < _LineSize)
			{
				o.Albedo = half3(0, 0, 0);
			}
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
