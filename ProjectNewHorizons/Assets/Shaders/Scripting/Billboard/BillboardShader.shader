Shader "Unlit/StandardBillboard"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _HoloCol ("Hologram Color", Color) = (1,1,1,1)
        _NoiseScale ("Noise Scale", Float) = 1
        
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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float4 _MainTex_ST;
            float4 _HoloCol;
            float _NoiseScale;

            v2f vert (appdata v)
            {
                v2f o;

                float4 origin = float4(0,0,0,1);
                float4 world_origin = mul(UNITY_MATRIX_M, origin);
                float4 view_origin = mul(UNITY_MATRIX_V, world_origin);
                float4 world_to_view_translation = view_origin - world_origin;

                float4 world_pos = mul(UNITY_MATRIX_M, v.vertex);
                float4 view_pos = world_pos + world_to_view_translation;
                float4 clip_pos = mul(UNITY_MATRIX_P, view_pos);

                o.vertex = clip_pos;

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;

            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                fixed4 texCol = tex2D(_MainTex, uv);
                fixed4 noiseCol = tex2D(_NoiseTex, uv * _NoiseScale - 0.2*fmod(_Time.y, 2) * fmod(_Time.y, 1));
                noiseCol.w = 1;
                float gray = dot(texCol.xyz, float3(0.299, 0.587, 0.114));
                float4 col = float4(gray, gray, gray, texCol.w) * _HoloCol;
                
                if (col.w > 0.2)
                {
                    col.w -= pow(0.125 + 0.125* sin((2 * UNITY_PI) / 1 * (uv.y + 0.5 * _Time.y)), 0.5);
                }
                else
                {
                    col.w = 0;
                }

                
                
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}