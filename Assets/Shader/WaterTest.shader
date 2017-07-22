// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/WaterTest" 
{
    Properties 
    {
      	_MainTex ("Texture", 2D) = "white" {}
		_SecondWaveTex("Second Wave Tex", 2D) = "white" {}
		_Tint("Color", Color) = (0,0,0,0)
      	_Amount1 ("Extrusion Amount 1", Range(-20,20)) = 0.0
		_Amount2("Extrusion Amount 2", Range(-20,20)) = 0.0
		_Tess("Tesselation", Range(1,32)) = 4
    }

    SubShader 
    {
      	Tags { "RenderType" = "Transparent" }
		Fog { Mode Global }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma glsl
			#pragma vertex vert tessellate:tess
			#pragma fragment frag
			#pragma multi_compile_fog
			//#pragma 
			#pragma target 4.0

			#include "UnityCG.cginc"

			float _Amount1;
			float _Amount2;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _SecondWaveTex;
			float4 _SecondWaveTex_ST;
			float4 _Tint;
			float _Tess;

			struct VertexIn
			{
				float4 vertex : POSITION;
				//float2 uv : TEXCOORD0;
			};


			struct VertexOut
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				UNITY_FOG_COORDS(2)
			};

			float4 tess()
			{
				return _Tess;
			}

			VertexOut vert(VertexIn _in)
			{
				VertexOut o;

				float3 pos = mul(unity_ObjectToWorld, _in.vertex);
				o.vertex = mul(UNITY_MATRIX_MVP, _in.vertex);
				o.uv = pos.xz * _MainTex_ST.xy + _MainTex_ST.zw;
				o.uv2 = pos.xz * _SecondWaveTex_ST.xy + _SecondWaveTex_ST.zw;
				//o.vertex.y -= (_Amount1 * (tex2Dlod(_MainTex, float4(o.uv, 1, 1)) - 0.5)
				//					- _Amount2 * (tex2Dlod(_MainTex, float4(o.uv2, 1, 1))));

				o.vertex.y -= (_Amount1 * (tex2Dlod(_MainTex, float4(o.uv, 1, 1)) * 0.5) 
						+_Amount2 * (tex2Dlod(_SecondWaveTex, float4(o.uv2, 1, 1)) * 0.5));

				UNITY_TRANSFER_FOG(o, o.vertex);

				return o;
			}

			float4 frag(VertexOut _in) : SV_Target
			{
				float4 o = (float4(tex2D(_MainTex, _in.uv).rgb,1) 
					+ float4(tex2D(_SecondWaveTex, _in.uv2).rgb,1)) * _Tint * 0.5;
				
				UNITY_APPLY_FOG(_in.fogCoord, o);
				UNITY_OPAQUE_ALPHA(o.a);


				return o;
			}
			ENDCG
		}
    } 
}