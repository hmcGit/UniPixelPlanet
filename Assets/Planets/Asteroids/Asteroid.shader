Shader "Unlit/Asteroid"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    	
	    _Pixels("Pixels", range(10,100)) = 100.0
	    _Rotation("Rotation",range(0.0, 6.28)) = 0.0
    	_Light_origin("Light origin", Vector) = (0.39,0.39,0.39,0.39)
	    _Time_speed("Time Speed",range(-1.0, 1.0)) = 0.2
    	    	
	    _Color1("Color1", Color) = (1,1,1,1)
    	_Color2("Color2", Color) = (1,1,1,1)
    	_Color3("Color3", Color) = (1,1,1,1)
    	
	    _Size("size",float) = 5.294
	    _OCTAVES("OCTAVES", range(0,20)) = 2
	    _Seed("seed",range(1, 10)) = 1.567
    	
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
            float _Light_border_1;
			float _Light_border_2;
            float _Size;
            int _OCTAVES;
            int _Seed;
    		fixed4 _Color1;
            fixed4 _Color2;
            fixed4 _Color3;
            
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
					c *= circleNoise((uv * _Size) + (float(i+1)+10.));
				}
				return 1.0 - c;
			}

			fixed4 frag(v2f i) : COLOR {
				// pixelize uv
            	
				float2 uv = floor(i.uv*_Pixels)/_Pixels;				
				//uv.y = 1 - uv.y;
		
				// we use this val later to interpolate between shades
				bool dith = dither(uv, i.uv);
				
				// distance from center
				float d = distance(uv, float2(0.5,0.5));
				
				// optional rotation, do this after the dither or the dither will look very messed up
				uv = rotate(uv, _Rotation);
				
				// two noise values with one slightly offset according to light source, to create shadows later
				float n = fbm(uv * _Size);
				float n2 = fbm(uv * _Size + (rotate(_Light_origin, _Rotation)-0.5) * 0.5);
				
				// step noise values to determine where the edge of the asteroid is
				// step cutoff value depends on distance from center
				float n_step = step(0.2, n - d);
				float n2_step = step(0.2, n2 - d);
				
				// with this val we can determine where the shadows should be
				float noise_rel = (n2_step + n2) - (n_step + n);
				
				// two crater values, again one extra for the shadows
				float c1 = crater(uv );
				float c2 = crater(uv + (_Light_origin-0.5)*0.03);

				// now we just assign colors depending on noise values and crater values
				// base
				float3 col = _Color2.rgb;
				
				// noise
				if (noise_rel < -0.06 || (noise_rel < -0.04 && dith)) {
					col = _Color1.rgb;
				}
				if (noise_rel > 0.05 || (noise_rel > 0.03 && dith)) {
					col = _Color3.rgb;
				}
				
				// crater
				if (c1 > 0.4)  {
					col = _Color2.rgb;
				}
				if (c2<c1) {
					col = _Color3.rgb;
				}
				
				return fixed4(col, n_step);
				}
            
            ENDCG
        }
    }
}
