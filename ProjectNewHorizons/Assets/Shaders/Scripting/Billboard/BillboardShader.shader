Shader "Unlit/StandardBillboard_img"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Scale("Image Scale", Float) = 1

        
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
            float4 _MainTex_ST;
            float4 _Color;
            float _Scale;

            v2f vert (appdata v)
            {
                v2f o;
                
                float3 center_world_space = mul(UNITY_MATRIX_M, float4(0,0,0,1)).xyz;
                float3 cam_world_space = _WorldSpaceCameraPos;
                float3 forward = cam_world_space - center_world_space;
                forward.y = 0;
                forward = normalize(forward);
                
                float3 up = float3(0,1,0);
                float3 right = normalize(cross(forward, up));
                
                float3 local_offset = v.vertex.xyz;
                float3 world_pos = center_world_space + right * local_offset.x * _Scale + up * local_offset.y * _Scale;
                
                o.vertex = mul(UNITY_MATRIX_VP, float4(world_pos, 1));

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;

            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                fixed4 texCol = tex2D(_MainTex, uv);
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return texCol;
            }
            ENDCG
        }
    }
}