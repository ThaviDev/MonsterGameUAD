Shader "Hidden/Threshold"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Threshold ("Brightness Threshold", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline"}
        LOD 100
        ZTest Always
        ZWrite Off
        Cull Off
        Pass
        {
            Name "ThresholdPass"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 positionCS : SV_POSITION;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float _Threshold;

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = input.uv;
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                half3 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv).rgb;
                // Convert to luminance (perceptual brightness)
                half luminance = dot(color, half3(0.2126, 0.7152, 0.0722));
                half3 result = luminance > _Threshold ? half3(1,1,1) : half3(0,0,0);
                return half4(result, 1);
            }
            ENDHLSL
        }
    }
}