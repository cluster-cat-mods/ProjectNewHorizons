Shader "Unlit/StandardBillboard"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _mainTex("Texture", 2D) = "white" {}
		_ambCol("Ambient Color", Color) = (1,1,1,1)
		_ambInt("Ambient Intensity", Float) = 0.1
		_specInt("Specular Intensity", Float) = 0.1
		_smoothness("Smoothness", Float) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
			#include "UnityLightingCommon.cginc"
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            	float4 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            	float4 normal : NORMAL;
            };
            
            float4      _Color;
            sampler2D	_mainTex;
			float4		_ambCol;
			float		_ambInt;
			float		_specInt;
			float		_smoothness;

            v2f vert (appdata v)
            {
                v2f o;
				o.uv = v.uv;
                
                float3 centerWS = mul(UNITY_MATRIX_M, float4(0,0,0,1)).xyz;
                float3 up = float3(0,1,0);
                float3 forward = _WorldSpaceCameraPos - centerWS;
                forward.y = 0;
                forward = normalize(forward);
                float3 right = normalize(cross(up, forward));
                up = cross(forward, right);
                float3 scale;
                scale.x = length(UNITY_MATRIX_M._m00_m10_m20);
                scale.y = length(UNITY_MATRIX_M._m01_m11_m21);
                scale.z = length(UNITY_MATRIX_M._m02_m12_m22);
                float3 localOffset = v.vertex * scale;
                float3 worldPos = centerWS + localOffset.x * right + localOffset.y * up + localOffset.z * forward;
                float4 clipPos = mul(UNITY_MATRIX_VP, float4(worldPos, 1));
            	
            	float3 normalWS = right * v.normal.x + up * v.normal.y + forward * v.normal.z;
                
            	o.normal = float4(normalize(normalWS), 1);
                o.vertex = clipPos;
            	
                return o;	
            }

            
            fixed4 frag (v2f i) : SV_Target
            {
                //fixed4 diffCol = tex2D(_mainTex, i.uv);
            	fixed4 diffCol = tex2D(_mainTex, i.uv) * _Color;
				_ambCol *= diffCol;
				float diffInt = max(dot(i.normal, _WorldSpaceLightPos0), 0);
				float4 reflection = reflect(_WorldSpaceLightPos0, i.normal);
				float specForm = pow(max(dot(normalize(_WorldSpaceCameraPos - i.vertex), reflection), 0), _smoothness);
				float4 col = _ambInt * _ambCol + (diffInt + _specInt * specForm) * _LightColor0 * diffCol;
				return col;
            }
            ENDCG
        }

		// cast shadows:
		Pass
		{
			Tags{ "LightMode" = "ShadowCaster" }
			CGPROGRAM
			#pragma vertex VSMain
			#pragma fragment PSMain

			float4 VSMain(float4 vertex:POSITION) : SV_POSITION
			{
				return UnityObjectToClipPos(vertex);
			}

			float4 PSMain(float4 vertex:SV_POSITION) : SV_TARGET
			{
				return 0;
			}

			ENDCG
		}
    }
}