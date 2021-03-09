Shader "Unlit/LavaCraters"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    	
	    _Pixels("Pixels", range(10,100)) = 0.0
	    _Rotation("Rotation",range(0.0, 6.28)) = 0.0
    	_Light_origin("Light origin", Vector) = (0.39,0.39,0.39,0.39)
	    _Time_speed("Time Speed",range(-1.0, 1.0)) = 0.2
	    
	    _Light_border("Light border",range(0.0, 1.0)) = 0.52
	        	
	    _Color1("Color1", Color) = (1,1,1,1)
    	_Color2("Color2", Color) = (1,1,1,1)
    	
	    _Size("Size",float) = 50.0
	    _Seed("Seed",range(1, 10)) = 1
	    time("time",float) = 0.0
    	
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
            #include "../cginc/hlmod.cginc"
            
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
            float _Pixels;
            float _Rotation;
			float2 _Light_origin;    	
			float _Time_speed;
            float _Light_border;
            float _Size;
            int _Seed;
			float time;
    		fixed4 _Color1;
            fixed4 _Color2;
                        
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
			
			float rand(float2 coord) {
				coord = mod(coord, float2(1.0,1.0)*round(_Size));
				return frac(sin(dot(coord.xy ,float2(12.9898,78.233))) * 15.5453 * _Seed);
			}
			// by Leukbaars from https://www.shadertoy.com/view/4tK3zR
			float circleNoise(float2 uv) {
			    float uv_y = floor(uv.y);
			    uv.x += uv_y*.31;
			    float2 f = frac(uv);
				float h = rand(float2(floor(uv.x),floor(uv_y)));
			    float m = (length(f-0.25-(h*0.5)));
			    float r = h*0.25;
			    return m = smoothstep(r-.10*r,r,m);
			}

			float crater(float2 uv) {
				float c = 1.0;
				for (int i = 0; i < 2; i++) {
					c *= circleNoise((uv * _Size) + (float(i+1)+10.) + float2(time*_Time_speed,0.0));
				}
				return 1.0 - c;
			}

			float2 spherify(float2 uv) {
				float2 centered= uv *2.0-1.0;
				float z = sqrt(1.0 - dot(centered.xy, centered.xy));
				float2 sphere = centered/(z + 1.0);
				return sphere * 0.5+0.5;
			}

			float2 rotate(float2 coord, float angle){
				coord -= 0.5;
				//coord *= mat2(float2(cos(angle),-sin(angle)),float2(sin(angle),cos(angle)));            	          	
            	coord = mul(coord,float2x2(float2(cos(angle),-sin(angle)),float2(sin(angle),cos(angle))));
				return coord + 0.5;
			}
			fixed4 frag(v2f i) : COLOR {
				// pixelize uv
            	
				float2 uv = floor(i.uv*_Pixels)/_Pixels;				
				//uv.y = 1 - uv.y;
	
				// check distance from center & distance to light
				float d_circle = distance(uv, float2(0.5,0.5));
				float d_light = distance(uv , float2(_Light_origin));
				// cut out a circle
				float a = step(d_circle, 0.5);
				
				uv = rotate(uv, _Rotation);
				uv = spherify(uv);
					
				float c1 = crater(uv );
				float c2 = crater(uv +(_Light_origin-0.5)*0.03);
				float3 col = _Color1.rgb;
				
				a *= step(0.5, c1);
				if (c2<c1-(0.5-d_light)*2.0) {
					col = _Color2.rgb;
				}
				if (d_light > _Light_border) {
					col = _Color2.rgb;
				} 

				// cut out a circle
				a*= step(d_circle, 0.5);
				return fixed4(col, a);
				}
            
            ENDCG
        }
    }
}
