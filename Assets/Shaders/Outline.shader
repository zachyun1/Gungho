﻿Shader "Outline" {
	Properties{
		[Toggle] _ToggleOutlinePass("OutlinePass", Int) = 0
		//[Toggle] _BasePass("BasePass", Int) = 0
		_Color("Main Color", Color) = (0,0,0,1)
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_Outline("Outline width", Range(0.0, 0.03)) = 0.03
		_MainTex("Base (RGB)", 2D) = "white" { }
	}

	CGINCLUDE
	#include "UnityCG.cginc"

	struct appdata {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};

	struct v2f {
		float4 pos : POSITION;
		float4 color : COLOR;
	};

	uniform float _Outline;
	uniform float4 _OutlineColor;

	v2f vert(appdata v) {
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);

		float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
		float2 offset = TransformViewToProjection(norm.xy);

		o.pos.xy += offset * o.pos.z * _Outline;
		o.color = _OutlineColor;
		return o;
	}

	ENDCG

	SubShader
	{
		Tags { "Queue" = "Transparent" }

		Pass
		{
			Name "OUTLINE"
			Tags { "LightMode" = "Always" }
			Cull Off
			ZWrite Off
			ColorMask RGB 
			Blend SrcAlpha OneMinusSrcAlpha 

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			half4 frag(v2f i) : COLOR
			{
				return i.color;
			}
			ENDCG
		}

		Pass
		{
			Name "BASE"
			//Tags{ "LightMode" = "Always" }
			ZWrite On
			ZTest LEqual
			Blend SrcAlpha OneMinusSrcAlpha
			Material
			{
				Diffuse[_Color]
				Ambient[_Color]
			}
			Lighting On
			SetTexture[_MainTex]
			{
				ConstantColor[_Color]
				Combine texture * constant
			}
			SetTexture[_MainTex]
			{
				Combine previous * primary DOUBLE
			}
		}
	}
	CustomEditor "CustomShaderGUI"
	Fallback "Diffuse"
}