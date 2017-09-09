// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/BlinkRangeShaderRevised"
{
	//http://answers.unity3d.com/questions/529814/how-to-have-2-different-objects-at-the-same-place.html
	//Credit here for idea; this should also help when making more advanced Unity shaders later
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_PlayerPosition("Player Position", vector) = (0,0,0,0) //update this in program
		_BlinkRange("Blink Range", float) = 10.0
		_BlinkColor("Blink Range Color", color) = (0,1,1,0.5)
	}
	SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		Pass
		{ 
			Blend SrcAlpha OneMinusSrcAlpha
			LOD 200

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			
			//Input to vertex shader
			struct vertexInput
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};

			//Input to fragment shader
			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float4 position_in_world_space : TEXCOORD0;
				float4 tex : TEXCOORD1;
			};

			uniform sampler2D _MainTex;
			uniform float4 _PlayerPosition;
			uniform float _BlinkRange;
			uniform fixed4 _BlinkColor;
			
			// VERTEX SHADER
			vertexOutput vert(vertexInput input)
			{
				vertexOutput output;
				output.pos = UnityObjectToClipPos(input.vertex);
				output.position_in_world_space = mul(unity_ObjectToWorld, input.vertex);
				output.tex = input.texcoord;
				return output;
			}

			// FRAGMENT SHADER
			float4 frag(vertexOutput input) : COLOR
			{
				// Calculate distance to player position
				float dist = distance(input.position_in_world_space, _PlayerPosition);

				// Return appropriate colour
				if (dist < _BlinkRange) {
					return tex2D(_MainTex, float4(input.tex)); // Visible
				}
				else {
					float4 tex = tex2D(_MainTex, float4(input.tex)); // Outside visible range
					tex.a = 0.1;
					return tex;
				}
			}
			ENDCG
		}
	}
}
