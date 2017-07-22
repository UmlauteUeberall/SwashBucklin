Shader "Custom/Ocean" {
	Properties {
		_MainTex("Water Texture", 2D) = "white" {}
		_WaterHM1("Heightmap 1", 2D) = "white" {}
		_WaterHM2("Heightmap 2", 2D) = "white" {}
		_Tint("Color", Color) = (0,0,0,0)
		_Amount1("Extrusion Amount 1", Range(-20,20)) = 0.0
		_Amount2("Extrusion Amount 2", Range(-20,20)) = 0.0
		_Tess("Tesselation", Range(1,32)) = 4


		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_LineSize("Line Size", Range(0,1)) = 0.1
		_GridSize("Grid Size", Range(1,100)) = 20
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert //tessellate:tess

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 4.0

		sampler2D _MainTex;
		sampler2D _WaterHM1;
		float4 _WaterHM1_ST;
		//float4 _MainTex_ST;
		sampler2D _WaterHM2;
		float4 _WaterHM2_ST;
		half _Glossiness;
		half _Metallic;
		float4 _Tint;
		float _Tess;
		float _LineSize;
		float _GridSize;
		float _Amount1;
		float _Amount2;

		struct Input 
		{
			float2 uv_MainTex;
			float3 worldPos;
		};

		float4 tess()
		{
			return _Tess;
		}

		void vert(inout appdata_full v) 
		{
			float3 pos = mul(unity_ObjectToWorld, v.vertex);

			float2 uv1 = pos.xz * _WaterHM1_ST.xy + _WaterHM1_ST.zw;
			float2 uv2 = pos.xz * _WaterHM2_ST.xy + _WaterHM2_ST.zw;;

			v.vertex.y += (_Amount1 + _Amount2) / 4;

			v.vertex.y -= (_Amount1 * (tex2Dlod(_WaterHM1, float4(uv1, 1, 1)) * 0.5)
				+ _Amount2 * (tex2Dlod(_WaterHM2, float4(uv2, 1, 1)) * 0.5));
		}

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Tint;
			o.Albedo = c.rgb;

			float2 wp = IN.worldPos.xz + float2(1000, 1000);
			wp += float2(0.5f, 0.5f) * _GridSize + float2(_LineSize, _LineSize) * 0.5f;
			wp = float2(fmod(wp.x, _GridSize), fmod(wp.y, _GridSize));
			//wp = float2(0.5f, 0.5f) - wp;
			wp = float2(abs(wp.x), abs(wp.y));
			if (wp.x < _LineSize || wp.y < _LineSize)
			{
				o.Albedo = half3(0, 0, 0);
			}

			//o.Albedo = half3(1, 1, 1);


			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
