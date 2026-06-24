Shader "Unlit/StandardBillboard"
{
    Properties
    {
     _Color("Color", Color) = (1,1,1,1)   
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            float4 _Color;

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
                float3 worldPos = centerWS + localOffset.x * right + localOffset.y * up + localOffset.z * -forward;
                float4 clipPos = mul(UNITY_MATRIX_VP, float4(worldPos, 1));
                
                o.vertex = clipPos;
            	
                return o;	
            }

            
            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float4 col = _Color;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}