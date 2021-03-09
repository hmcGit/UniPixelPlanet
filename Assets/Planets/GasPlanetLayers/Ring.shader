Shader "Unlit/Ring"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
	    _Pixels("Pixels", range(10,300)) = 100.0
	    _Rotation("Rotation",range(0.0, 6.28)) = 0.0    		    
    	_Light_origin("Light origin", Vector) = (0.39,0.39,0.39,0.39)
	    _Time_speed("Time Speed",range(-1.0, 1.0)) = 0.2
	    
    	_Light_border_1("Light border1",range(0.0, 1.0)) = 0.52
	    _Light_border_2("Light border2",range(0.0, 1.0)) = 0.62
    	
    	_Ring_width("Ring Width",range(0.0, 0.15)) = 0.1
	    _Ring_perspective("Ring Perspective",float) = 4.0
    	_Scale_rel_to_planet("Scale rel to Planet",float) = 6.0
	    
        _ColorScheme("ColorScheme", 2D) = "white" {}
    	_Dark_ColorScheme("Dark ColorScheme", 2D) = "black" {}

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
			float2 _Light_origin;    	
			float _Time_speed;
            float _Light_border_1;
			float _Light_border_2;

            float _Ring_width;
            float _Ring_perspective;
            float _Scale_rel_to_planet;
            
			sampler2D _ColorScheme;
            sampler2D _Dark_ColorScheme;
            float _Size;
            int _OCTAVES;
            int _Seed;
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
			float rand(float2 coord) {
				coord = mod(coord, float2(2.0,1.0)*round(_Size));
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


			// by Leukbaars from https://www.shadertoy.com/view/4tK3zR
			float circleNoise(float2 uv) {
			    float uv_y = floor(uv.y);
			    uv.x += uv_y*.31;
			    float2 f = frac(uv);
				float h = rand(float2(floor(uv.x),floor(uv_y)));
			    float m = (length(f-0.25-(h*0.5)));
			    float r = h*0.25;
			    return smoothstep(0.0, r, m*0.75);
			}
			bool dither(float2 uv_pixel, float2 uv_real) {
				return mod(uv_pixel.x+uv_real.y,2.0/_Pixels) <= 1.0 / _Pixels;
			}

			float2 spherify(float2 uv) {
				float2 centered= uv *2.0-1.0;
				float z = sqrt(1.0 - dot(centered.xy, centered.xy));
				float2 sphere = centered/(z + 1.0);
				return sphere * 0.5+0.5;
			}

			float2 rotate(float2 coord, float angle){
				coord -= 0.5;
				//coord *= float2x2(float2(cos(angle),-sin(angle)),float2(sin(angle),cos(angle)));
            	coord = mul(coord,float2x2(float2(cos(angle),-sin(angle)),float2(sin(angle),cos(angle))));
				return coord + 0.5;
			}

			fixed4 frag(v2f i) : COLOR {
				// pixelize uv
            	
				float2 uv = floor(i.uv*_Pixels)/_Pixels;				
				//uv.y = 1 - uv.y;

				// we use this value later to dither between colors
				bool dith = dither(uv, uv);
				
				float light_d = distance(uv, _Light_origin);
				uv = rotate(uv, _Rotation);
				
				// center is used to determine ring position
				float2 uv_center = uv - float2(0.0, 0.5);
				
				// tilt ring
				uv_center *= float2(1.0, _Ring_perspective);
				float center_d = distance(uv_center,float2(0.5, 0.0));
				
				
				// cut out 2 circles of different _Sizes and only intersection of the 2.
				float ring = smoothstep(0.5-_Ring_width*2.0, 0.5-_Ring_width, center_d);
				ring *= smoothstep(center_d-_Ring_width, center_d, 0.4);
				
				// pretend like the ring goes behind the planet by removing it if it's in the upper half.
				if (uv.y < 0.5) {
					ring *= step(1.0/_Scale_rel_to_planet, distance(uv,float2(0.5,0.5)));
				}
				
				// rotate material in the ring
				uv_center = rotate(uv_center+float2(0, 0.5), time*_Time_speed);
				// some noise
				ring *= fbm(uv_center*_Size);
				
				// apply some colors based on final value
				float posterized = floor((ring+pow(light_d, 2.0)*2.0)*4.0)/4.0;
				float3 col;
				if (posterized <= 1.0) {
					col = tex2D(_ColorScheme, float2(posterized, uv.y)).rgb;
				} else {
					col = tex2D(_Dark_ColorScheme, float2(posterized-1.0, uv.y)).rgb;
				}
				float ring_a = step(0.28, ring);				
				return fixed4(col, ring_a);
			}
            
            ENDCG
        }
    }
}
