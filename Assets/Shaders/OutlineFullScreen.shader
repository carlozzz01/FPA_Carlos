Shader "Custom/OutlineFullScreen"
{
    HLSLINCLUDE

    #pragma vertex Vert
    #pragma fragment Frag

    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/RenderPass/CustomPass/CustomPassCommon.hlsl"

    TEXTURE2D_X(_SilhouetteTexture);
    float4 _OutlineColor;
    float  _OutlineWidth;

    float4 Frag(Varyings varyings) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(varyings);

        float2 uv = varyings.positionCS.xy / _ScreenParams.xy;
        float2 texelSize = 1.0 / _ScreenParams.xy;

        float sample0 = SAMPLE_TEXTURE2D_X(_SilhouetteTexture, s_linear_clamp_sampler, uv + float2( _OutlineWidth,  0) * texelSize).r;
        float sample1 = SAMPLE_TEXTURE2D_X(_SilhouetteTexture, s_linear_clamp_sampler, uv + float2(-_OutlineWidth,  0) * texelSize).r;
        float sample2 = SAMPLE_TEXTURE2D_X(_SilhouetteTexture, s_linear_clamp_sampler, uv + float2( 0,  _OutlineWidth) * texelSize).r;
        float sample3 = SAMPLE_TEXTURE2D_X(_SilhouetteTexture, s_linear_clamp_sampler, uv + float2( 0, -_OutlineWidth) * texelSize).r;
        float center  = SAMPLE_TEXTURE2D_X(_SilhouetteTexture, s_linear_clamp_sampler, uv).r;

        float maxSample = max(max(sample0, sample1), max(sample2, sample3));
        float outline = maxSample * (1.0 - center);

        if (outline <= 0.01) discard;

        return _OutlineColor;
    }

    ENDHLSL

    SubShader
    {
        Tags { "RenderPipeline" = "HDRenderPipeline" }

        Pass
        {
            Name "OutlineFullScreen"
            ZWrite Off
            ZTest Always
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            HLSLPROGRAM
            ENDHLSL
        }
    }
}