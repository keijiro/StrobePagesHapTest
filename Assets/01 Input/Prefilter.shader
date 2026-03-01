Shader "Karbon/Prefilter"
{
HLSLINCLUDE

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
#include "Packages/jp.keijiro.bodypix/Shaders/Common.hlsl"
#include "Assets/00 Common/Shaders/CustomRenderTexture.hlsl"

TEXTURE2D(_MainTex);
float4 _MainTex_TexelSize;

TEXTURE2D(_BodyPixTex);
float4 _BodyPixTex_TexelSize;

half4 FragUpdate(CustomRenderTextureVaryings i) : SV_Target
{
    float2 uv = i.globalTexcoord.xy;

    // Color
    float3 color = SAMPLE_TEXTURE2D(_MainTex, sampler_LinearClamp, uv).rgb;

    // Human stencil
    BodyPix_Mask mask = BodyPix_SampleMask(uv, _BodyPixTex, _BodyPixTex_TexelSize.zw);
    float alpha = smoothstep(0.4, 0.6, BodyPix_EvalSegmentation(mask));

    return float4(color, alpha);
}

ENDHLSL

    SubShader
    {
        ZTest Always ZWrite Off Cull Off
        Pass
        {
            Name "Update"
            HLSLPROGRAM
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment FragUpdate
            ENDHLSL
        }
    }
}
