Shader "CustomRenderTexture/VignetteShader"
{
	Properties
	{
		_RTexIn("Camera Texture", 2D) = "Noise" {}
		_Color("Color", Color) = (1,1,1,1)
		_CenterX("Center x", Float) = 0.5
		_CenterY("Center y", Float) = 0.5
		_Radius("Radius", Float) = 0.3
		_Softness("Softness", Float) = 0.2
	}

    SubShader
    {
        Blend One Zero

        Pass
        {
            Name "VignetteShader"

            CGPROGRAM
            #include "UnityCustomRenderTexture.cginc"
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
            #pragma target 3.0

            float4      _Color;
            float		_CenterX;
            float		_CenterY;
            float2		_Center = float2(_CenterX, _CenterY);
            sampler2D	_RTexIn;

            float4 frag(v2f_customrendertexture IN) : SV_Target
            {
                float2 uv = IN.localTexcoord.xy;
            	float4 col = tex2D(_RTexIn, uv);
            	float dist = length(uv - center);
            	float radius
            	col += _Color * abs(sqrt(pow((float2(0.5,0.5) - uv).x, 2) + pow((float2(0.5,0.5) - uv).y, 2)));
                
				return col;
            }
            ENDCG
        }
    }
}
