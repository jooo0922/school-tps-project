Shader "Custom/Blob"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" } // 불투명 셰이더 적용

        CGPROGRAM
        #pragma surface surf Standard vertex:vert

        sampler2D _MainTex;

        // 버텍스 쉐이더에서 노이즈값에 따른 각 정점의 Blob 움직임 계산
        void vert(inout appdata_full v) {
            
        }

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
