Shader "Custom/Blob"
{
    Properties
    {
        _BaseColor ("BaseColor", Color) = (1,1,1,1)
        _NoiseColor ("NoiseColor", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BumpMap ("NormalMap", 2D) = "bump" {}
        _NoiseTex ("NoiseTex", 2D) = "white" {}
        _Smoothness("Smoothness", Range(0,1)) = 0.819
        _Amplitude("Amplitude", Range(0.5,1.5)) = 1
        _Frequency("Frequency", Range(0.1,1)) = 0.35
        _Radius("Radius", Range(0.1,2)) = 0.814
        _RimPower("RimPower", Range(0.1, 10)) = 1.2
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" } // 알파 블렌딩 셰이더 적용

        CGPROGRAM
        #pragma surface surf Standard addshadow vertex:vert // addshadow 옵션은 현재 렌더링 결과에 Blob 의 그림자 추가 -> Blob 그림자가 더 자연스럽게 렌더링되도록 함.

        // 변수 선언
        fixed4 _BaseColor;
        float4 _NoiseColor;
        sampler2D _MainTex;
        sampler2D _BumpMap;
        sampler2D _NoiseTex;
        float _Smoothness;
        float _Amplitude; // Blob 주기
        float _Frequency; // Blob 규모
        float _Radius; // 로컬 원점으로부터 각 버텍스까지의 최대 반경
        float _RimPower; // rim 값을 exponential 하게 계산 -> rim 영역 두께 조정 

        // Classic Perlin Noise 코드 추가
        float3 mod289(float3 x)
        {
            return x - floor(x * (1.0 / 289.0)) * 289.0;
        }
        float4 mod289(float4 x)
        {
            return x - floor(x * (1.0 / 289.0)) * 289.0;
        }
        float4 permute(float4 x)
        {
            return mod289(((x * 34.0) + 1.0) * x);
        }
        float4 taylorInvSqrt(float4 r)
        {
            return 1.79284291400159 - 0.85373472095314 * r;
        }
        float3 fade(float3 t) {
            return t * t * t * (t * (t * 6.0 - 15.0) + 10.0);
        }
        float cnoise(float3 P)
        {
            float3 Pi0 = floor(P);
            float3 Pi1 = Pi0 + float3(1.0, 1.0, 1.0);
            Pi0 = mod289(Pi0);
            Pi1 = mod289(Pi1);
            float3 Pf0 = frac(P);
            float3 Pf1 = Pf0 - float3(1.0, 1.0, 1.0);
            float4 ix = float4(Pi0.x, Pi1.x, Pi0.x, Pi1.x);
            float4 iy = float4(Pi0.yy, Pi1.yy);
            float4 iz0 = Pi0.zzzz;
            float4 iz1 = Pi1.zzzz;
            float4 ixy = permute(permute(ix) + iy);
            float4 ixy0 = permute(ixy + iz0);
            float4 ixy1 = permute(ixy + iz1);
            float4 gx0 = ixy0 * (1.0 / 7.0);
            float4 gy0 = frac(floor(gx0) * (1.0 / 7.0)) - 0.5;
            gx0 = frac(gx0);
            float4 gz0 = float4(0.5, 0.5, 0.5, 0.5) - abs(gx0) - abs(gy0);
            float4 sz0 = step(gz0, float4(0.0, 0.0, 0.0, 0.0));
            gx0 -= sz0 * (step(0.0, gx0) - 0.5);
            gy0 -= sz0 * (step(0.0, gy0) - 0.5);
            float4 gx1 = ixy1 * (1.0 / 7.0);
            float4 gy1 = frac(floor(gx1) * (1.0 / 7.0)) - 0.5;
            gx1 = frac(gx1);
            float4 gz1 = float4(0.5, 0.5, 0.5, 0.5) - abs(gx1) - abs(gy1);
            float4 sz1 = step(gz1, float4(0.0, 0.0, 0.0, 0.0));
            gx1 -= sz1 * (step(0.0, gx1) - 0.5);
            gy1 -= sz1 * (step(0.0, gy1) - 0.5);
            float3 g000 = float3(gx0.x, gy0.x, gz0.x);
            float3 g100 = float3(gx0.y, gy0.y, gz0.y);
            float3 g010 = float3(gx0.z, gy0.z, gz0.z);
            float3 g110 = float3(gx0.w, gy0.w, gz0.w);
            float3 g001 = float3(gx1.x, gy1.x, gz1.x);
            float3 g101 = float3(gx1.y, gy1.y, gz1.y);
            float3 g011 = float3(gx1.z, gy1.z, gz1.z);
            float3 g111 = float3(gx1.w, gy1.w, gz1.w);
            float4 norm0 = taylorInvSqrt(float4(dot(g000, g000), dot(g010, g010), dot(g100, g100), dot(g110, g110)));
            g000 *= norm0.x;
            g010 *= norm0.y;
            g100 *= norm0.z;
            g110 *= norm0.w;
            float4 norm1 = taylorInvSqrt(float4(dot(g001, g001), dot(g011, g011), dot(g101, g101), dot(g111, g111)));
            g001 *= norm1.x;
            g011 *= norm1.y;
            g101 *= norm1.z;
            g111 *= norm1.w;
            float n000 = dot(g000, Pf0);
            float n100 = dot(g100, float3(Pf1.x, Pf0.yz));
            float n010 = dot(g010, float3(Pf0.x, Pf1.y, Pf0.z));
            float n110 = dot(g110, float3(Pf1.xy, Pf0.z));
            float n001 = dot(g001, float3(Pf0.xy, Pf1.z));
            float n101 = dot(g101, float3(Pf1.x, Pf0.y, Pf1.z));
            float n011 = dot(g011, float3(Pf0.x, Pf1.yz));
            float n111 = dot(g111, Pf1);
            float3 fade_xyz = fade(Pf0);
            float4 n_z = lerp(float4(n000, n100, n010, n110), float4(n001, n101, n011, n111), fade_xyz.z);
            float2 n_yz = lerp(n_z.xy, n_z.zw, fade_xyz.y);
            float n_xyz = lerp(n_yz.x, n_yz.y, fade_xyz.x);
            return 2.2 * n_xyz;
        }

        // 버텍스 쉐이더에서 노이즈값에 따른 각 정점의 Blob 움직임 계산
        void vert(inout appdata_full v) {
            // 만약 position 벡터의 길이를 놓고 본다면, 로컬 원점으로부터 각 버텍스까지의 반경과도 같음. 
            // 따라서, 최대 반경인 _Radius 만큼을 나눠주면 길이가 1인 vec3 가 되겠군. 
            // -> 여기에 시간변수 _Time.y 을 더해서 매 시간마다, 버텍스마다 변화하는 랜덤한 noise 값을 리턴받음.
            float noise = cnoise(v.vertex.xyz / _Radius + _Time.y);

            // 랜덤한 noise 값에 blob 변화주기의 제곱을 곱해주고, amplitude 를 더해줌으로써 blob의 주기와 규모를 계산함.
            v.vertex.xyz *= noise * pow(_Frequency, 2.0) + _Amplitude;

            // 참고로, 유니티 버텍스 쉐이더는 버텍스 오브젝트공간 좌표계에서 클립좌표까지 자동으로 변환하므로, 별도의 행렬변환 처리를 안해줘도 됨.
            // glsl 보다 코드 작성이 훨씬 간편함. 버텍스 오브젝트공간 좌표만 계산해주면 되니까!
        }

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float2 uv_NoiseTex;
            float3 viewDir; // 카메라 뷰 벡터 포함 -> rim 라이트 계산
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _BaseColor;
            float4 noise = tex2D(_NoiseTex, IN.uv_NoiseTex + _Time.x); // noise 텍스쳐 샘플링 시, uv좌표에 시간값을 더해 텍스쳐 애니메이션 구현

            // 노말맵 샘플링 및 노말값 적용 -> 노멀벡터 변경
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap)); 

            // rim 값 계산
            float rim = saturate(dot(o.Normal, IN.viewDir)); // 프래그먼트의 노멀벡터와 카메라 뷰 벡터 내적의 음수값 제거 > 조명값 계산이 부정확해지는 문제 방지
            rim = pow(1 - rim, _RimPower) * 1.25; // rim 값을 거꾸로 뒤집고, _RimPower 만큼 거듭제곱해서 rim 영역 두께 조절 + rim 값 범위를 0 ~ 1.25 까지 확장

            o.Albedo = c.rgb + noise * _NoiseColor.rgb * rim; // 뒤집어진 rim 영역 (가장자리 부분) 위주로 animated noise 가 적용되도록 함
            o.Smoothness = _Smoothness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}

/*
  noise 의 frequency(빈도, 주기)와 amplitude(규모, 진폭)

  1. frequency 는 blob 의 빈도, 주기, 자글자글한 정도를 결정해 줌. 
  기본적으로 noise 는 -1 ~ 1 사이의 랜덤값인데, 
  여기에 0에 가까운 frequency 값을 곱할수록 노이즈값 범위의 차이가 좁혀질 것이고, 
  큰 frequency 값을 곱할수록 노이즈값 범위가 확대되겠지? 
  ex> -1 ~ 1 * 10 => -10 ~ 10 이 되는 것처럼!

  2. amplitude 값을 더해주면 blob 의 기본 규모, 사이즈를 결정해 줌.
  ex> -1 ~ 1 + 10 => 9 ~ 11 이 되서
  노이즈값의 기본 규모가 9 ~ 11 사이의 값으로 확대되는 것임. 
  9보다 작은 랜덤 노이즈값은 나오지 못할테니 기본 규모가 굉장히 커지는 것임.
*/