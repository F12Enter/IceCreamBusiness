Shader "Custom/SkyboxBlend"
{
    Properties
    {
        _Skybox1 ("Skybox 1", Cube) = "" {}
        _Skybox2 ("Skybox 2", Cube) = "" {}
        _BlendFactor ("Blend Factor", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float3 texcoord : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            samplerCUBE _Skybox1;
            samplerCUBE _Skybox2;
            float _BlendFactor;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 skybox1Color = texCUBE(_Skybox1, i.texcoord);
                half4 skybox2Color = texCUBE(_Skybox2, i.texcoord);
                return lerp(skybox1Color, skybox2Color, _BlendFactor);
            }
            ENDCG
        }
    }
}
