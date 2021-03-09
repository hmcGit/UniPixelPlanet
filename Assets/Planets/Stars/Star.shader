Shader "Unlit/Star"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    	
	    _Pixels("Pixels", range(10,100)) = 0.0
	    _Rotation("Rotation",range(0.0, 6.28)) = 0.0
	    _Time_speed("Time Speed",range(-1.0, 1.0)) = 0.2
	        	
	    _GradientTex("Texture", 2D) = "white" {}
    	_TILES("TILES", range(0,20)) = 1
    	
	    _Size("Size",float) = 50.0
	    _OCTAVES("OCTAVES", range(0,20)) = 0
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
			float _Time_speed;
            float _Size;
            int _OCTAVES;
            int _Seed;
			float time;
            sampler2D _GradientTex;
            float _TILES;            
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
				return frac(sin(dot(coord.xy ,float2(12.9898,78.233))) * 43758.5453 * _Seed);
			}

			float noise(float2 coord){
				float2 i = floor(coord);
				float2 f = frac(coord);
				
				float a = rand(i);
				float b = rand(i + float2(1.0, 0.0));
				float c = rand(i + float2(0.0, 1.0));
				float d = rand(i + float2(1.0, 1.0));

				float2 cubic = f * f * (3.0 - 2.0 * f);

				return lerp(a, b, cubic.x) + (c - a) * cubic.y * (1.0 - cubic.x) + (d - b) * cubic.x * cubic.y;
			}

			float fbm(float2 coord){
				float value = 0.0;
				float scale = 0.5;

				for(int i = 0; i < _OCTAVES ; i++){
					value += noise(coord) * scale;
					coord *= 2.0;
					scale *= 0.5;
				}
				return value;
			}

			bool dither(float2 uv1, float2 uv2) {
				return mod(uv1.x+uv2.y,2.0/_Pixels) <= 1.0 / _Pixels;
			}

			float2 rotate(float2 coord, float angle){
				coord -= 0.5;
				//coord *= mat2(float2(cos(angle),-sin(angle)),float2(sin(angle),cos(angle)));            	
            	coord = mul(coord,float2x2(float2(cos(angle),-sin(angle)),float2(sin(angle),cos(angle))));
				return coord + 0.5;
			}
			
			float2 spherify(float2 uv) {
				float2 centered= uv *2.0-1.0;
				float z = sqrt(1.0 - dot(centered.xy, centered.xy));
				float2 sphere = centered/(z + 1.0);
				return sphere * 0.5+0.5;
			}
			float2 Hash2(float2 p) {
				float t = (time+10.0)*.3;
				//p = mod(p, vec2(1.0,1.0)*round(_Size));
				return float2(noise(p), noise(p*float2(.3135+sin(t), .5813-cos(t))));
			}

			// Tileable cell noise by Dave_Hoskins from shadertoy: https://www.shadertoy.com/view/4djGRh
			float Cells(in float2 p, in float numCells) {
				p *= numCells;
				float d = 1.0e10;
				for (int xo = -1; xo <= 1; xo++)
				{
					for (int yo = -1; yo <= 1; yo++)
					{
						float2 tp = floor(p) + float2(float(xo), float(yo));
						tp = p - tp - Hash2(mod(tp, numCells / _TILES));
						d = min(d, dot(tp, tp));
					}
				}
				return sqrt(d);
			}


			fixed4 frag(v2f i) : COLOR {
				// pixelize uv
            	
				float2 pixelized = floor(i.uv*_Pixels)/_Pixels;				
				//uv.y = 1 - uv.y;
				// use dither val later to mix between colors
				bool dith = dither(i.uv, pixelized);
				
				pixelized = rotate(pixelized, _Rotation);
				
				// spherify has to go after dither
				pixelized = spherify(pixelized);
				
				// use two different _Sized cells for some variation
				float n = Cells(pixelized - float2(time * _Time_speed * 2.0, 0), 10);
				n *= Cells(pixelized - float2(time * _Time_speed * 2.0, 0), 20);
				//n *= Cells(pixelized - vec2(time * _Time_speed * 2.0, 0), 30);
				
				// adjust cell value to get better looking stuff
				n*= 2.;
				n = clamp(n, 0.0, 1.0);
				if (dith) { // here we dither
					n *= 1.3;
				}
				
				// constrain values 4 possibilities and then choose color based on those
				float interpolate = floor(n * 3.0) / 3.0;
				float3 c = tex2D(_GradientTex, float2(interpolate, 0.0)).rgb;
				
				// cut out a circle
				float a = step(distance(pixelized, float2(0.5,0.5)), .5);
				
				return fixed4(c, a);
				}
            
            ENDCG
        }
    }
}
