// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "beekeeper/honey"
 {
     Properties
     {
         _HiveTexture("Hive", 2D)            = "" {}
         _HoneyTexture("Honey", 2D)          = "" {}
         _AlphaTexture("Alpha", 2D)          = "" {}
         _FillRange("Fill", Range(0.0, 1.0)) = 0.5
         _UnscaledTime("Time", Float)        = 1.0
     }

     SubShader
     {
         Tags
         {
             "Queue" = "Transparent"
             "IgnoreProjector" = "True"
             "RenderType" = "Transparent"
             "PreviewType" = "Plane"
             "CanUseSpriteAtlas" = "True"
         }

         Cull Off
         Lighting Off
         ZWrite Off
         /* Blend One OneMinusSrcAlpha    //Premultiplied transparency */
         Blend SrcAlpha OneMinusSrcAlpha // Traditional transparency

         Pass
         {
             CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag

             struct VertexInput
             {
                 float4 vertex:        POSITION;
                 float4 color:         COLOR;
                 float2 texcoordHive:  TEXCOORD0;
                 float2 texcoordHoney: TEXCOORD1;
                 float2 texcoordAlpha: TEXCOORD2;
             };

             struct VertexOutput
             {
                 float4 vertex:        SV_POSITION;
                 fixed4 color:         COLOR;
                 float2 texcoordHive:  TEXCOORD0;
                 float2 texcoordHoney: TEXCOORD1;
                 float2 texcoordAlpha: TEXCOORD2;
             };

             VertexOutput vert(VertexInput input)
             {
                 VertexOutput output;
                 output.vertex = UnityObjectToClipPos(input.vertex);
                 output.color = input.color;
                 output.texcoordHive = input.texcoordHive;
                 output.texcoordHoney = input.texcoordHoney;
                 output.texcoordAlpha = input.texcoordAlpha;
                 return output;
             }

             sampler2D _HiveTexture;
             sampler2D _HoneyTexture;
             sampler2D _AlphaTexture;
             float _FillRange;
             float _UnscaledTime;

             fixed4 frag(VertexOutput input) : SV_Target
             {
                _UnscaledTime *= 0.8;
                 float2 texcoord = input.texcoordHive;
                 fixed4 hive = tex2D(_HiveTexture, texcoord) * input.color;
                 if (hive.a < 0.01) {
                    float alpha = _FillRange
                                + 0.01 * sin(2 * 3.14 * 2 * _UnscaledTime - 10 * texcoord.x)
                                + 0.005 * sin(2 * 3.14 * 1 * _UnscaledTime - 20 * texcoord.x)
                                - 0.02 * sin(2 * 3.14 * 0.598345 * _UnscaledTime - 6 * texcoord.x + 0.01);
                     if (texcoord.y < alpha) {
                         fixed4 silhouette = tex2D(_AlphaTexture, texcoord);

                         texcoord.y += 0.1 * sin(0.1 * _UnscaledTime - 1 * texcoord.x + 0);
                         texcoord.y -= 0.05 * _UnscaledTime;
                         texcoord.x += 0.02 * sin(2 * 3.14 * 0.2 * _UnscaledTime - 20 * texcoord.y);
                         texcoord.x -= 0.03 * sin(2 * 3.14 * 0.3 * _UnscaledTime - 5 * texcoord.y);

                         fixed4 honey = tex2D(_HoneyTexture, texcoord) * input.color;
                         return (honey + input.color * (0.3*pow(input.texcoordHive.y / _FillRange,10)
                         + 0.4*pow(input.texcoordHive.y/_FillRange, 2))) * silhouette.r * 0.8;
                     }
                     return 0;
                 }
                 return hive;
                 /* return honey.r * silhouette.r * (1 - hive.r) + hive.r; */
             }
             ENDCG
         }
     }
 }
