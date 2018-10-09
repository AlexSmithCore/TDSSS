Shader "Custom/LandscapeShader" {
	Properties {
		_Color ("Color", Color) = (0,0,0,0)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BaseNorm ("Base Normal", 2D) = "bump" {}
		_FirstTex ("First (RGB)", 2D) = "white" {}
		_FirstNorm ("First Normal", 2D) = "bump" {}
		_SecondTex ("Second (RGB)", 2D) = "white" {}
		_SecondNorm ("Second Normal", 2D) = "bump" {}
		_ThirdTex ("Third (RGB)", 2D) = "white" {}
		_ThirdNorm ("Third Normal", 2D) = "bump" {}
		_FourthTex ("Fourth (RGB)", 2D) = "white" {}
		_FourthNorm ("Fourth Normal", 2D) = "bump" {}
		_MixTex ("Mix", 2D) = "white" {}
		//_Glossiness ("Smoothness", Range(0,1)) = 0.5
		//_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert

		// Use shader model 3.0 target, to get nicer looking lighting
		//#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BaseNorm;
		sampler2D _FirstTex;
		sampler2D _FirstNorm;
		sampler2D _SecondTex;
		sampler2D _SecondNorm;
		sampler2D _ThirdTex;
		sampler2D _ThirdNorm;
		sampler2D _FourthTex;
		sampler2D _FourthNorm;
		sampler2D _MixTex;

		struct Input {
			float2 uv_MainTex;
			float2 uv_MixTex;
		};

		//half _Glossiness;
		//half _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 cMain = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed4 cFirst = tex2D (_FirstTex, IN.uv_MainTex);
			fixed4 cSecond = tex2D (_SecondTex, IN.uv_MainTex);
			fixed4 cThird = tex2D (_ThirdTex, IN.uv_MainTex);
			fixed4 cFourth = tex2D (_FourthTex, IN.uv_MainTex);

			fixed4 nMain = tex2D (_BaseNorm, IN.uv_MainTex);
			fixed4 nFirst = tex2D (_FirstNorm, IN.uv_MainTex);
			fixed4 nSecond = tex2D (_SecondNorm, IN.uv_MainTex);
			fixed4 nThird = tex2D (_ThirdNorm, IN.uv_MainTex);
			fixed4 nFourth = tex2D (_FourthNorm, IN.uv_MainTex);

			fixed4 cMix = tex2D (_MixTex, IN.uv_MixTex);

			o.Albedo = (lerp(cMain, cFirst, cMix.r) + lerp(cMain, cSecond, cMix.g) + lerp(cMain, cThird, cMix.b) + lerp(cMain, cFourth, cMix.a));
			o.Normal =  (lerp(nMain, nFirst, cMix.r) + lerp(nMain, nSecond, cMix.g) + lerp(nMain, nThird, cMix.b) + lerp(nMain, nFourth, cMix.a));
			o.Alpha = cMain.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
