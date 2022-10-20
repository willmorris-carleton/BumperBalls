Shader "Custom/GraphPlaneShader"
{
    Properties
    {
        _LineColor ("Line Color", Color) = (1,1,1,1)
        _BackgroundColor ("Background Color", Color) = (0,0,0,1)
        _Size ("Graph Scale", Range(1.0,5.0)) = 1.0
        _LineSize ("Line Width", Range(0.01,0.5)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        struct Input
        {
            float3 worldPos;
        };

        float4 _LineColor;
        float4 _BackgroundColor;
        float _Size;
        float _LineSize;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float4 col = _BackgroundColor;
            
            if (fmod(abs(IN.worldPos.x), _Size) < _LineSize || fmod(abs(IN.worldPos.z), _Size) < _LineSize) {
                col = _LineColor;
            }
            
            o.Albedo = col;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
