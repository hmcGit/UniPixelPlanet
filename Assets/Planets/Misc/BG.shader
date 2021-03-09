Shader "Unlit/BG"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
	    _Offset("Offset",Vector) = (1,1,1,1)
    	
    }
    SubShader
    {
        //Tags { "RenderType"="Opaque" }
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True"}
        LOD 100

        Pass
        {
			Tags { "LightMode"="UniversalForward"}

			CULL Off
			ZWrite Off // don't write to depth buffer 
         	Blend SrcAlpha OneMinusSrcAlpha // use alpha blending

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            //#include "../cginc/hlmod.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            vector _Offset;
            float time;            
			struct Input
	        {
	            float2 uv_MainTex;
	        };
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
			float2 rotate(float2 coord, float angle){
				coord -= 0.5;
				//coord *= float2x2(float2(cos(angle),-sin(angle)),float2(sin(angle),cos(angle)));
            	float2 x = float2(cos(angle), -sin(angle));
            	float2 y = float2(sin(angle), cos(angle));
            	coord = mul(coord,float2x2(x,y));
				return coord + 0.5;
			}
			float rand(float2 coord) {
				return frac(sin(dot(coord.xy ,float2(12.9898,78.233))) * 43758.5453);
			}


			fixed4 frag(v2f i) : COLOR {
				_Offset = _Offset * _Time * 0.05;
				float2 uv = rotate(i.uv, _Time * 0.05);
				
				float2 t = _Time * 0.0000001;
				float4 col = float4(1.0,1.0,1.0,1.0) + rand(uv + t ) * 0.03;
				
				float c1 = abs(sin(uv.x * cos(_Offset.x + _Offset.y)+ _Time * 0.105));
				float c2 = abs(sin((cos(uv.x + uv.y)  + cos(_Offset.x + _Offset.y) + _Time * 0.059))) ;
				float c3 = abs(cos(uv.y * sin(_Offset.y) + _Time * 0.0253)) ;

				col = col * fixed4(c1, c2, c3, 1.0);
				return col;
				}
            
            ENDCG
        }
    }
}
