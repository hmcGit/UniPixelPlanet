using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stars : MonoBehaviour, IPlanet {
    [SerializeField] private GameObject StarBackground;
    [SerializeField] private GameObject Star;
    [SerializeField] private GameObject StarFlares;
    private Material m_Starbackground;
    private Material m_Star;
    private Material m_StarFlares;
    [SerializeField] private GradientTextureGenerate _gradientStar;
    [SerializeField] private GradientTextureGenerate _gradientStarFlare;
    
    private string gradient_vars = "_GradientTex";
    

    private GradientColorKey[] colorKey1 = new GradientColorKey[4];
    private GradientColorKey[] colorKey2 = new GradientColorKey[2];
    private GradientAlphaKey[] alphaKey1 = new GradientAlphaKey[4];
    private GradientAlphaKey[] alphaKey2 = new GradientAlphaKey[2];
    
    private string[] color_vars1 = new string[]{"_Color1"};
    private string[] init_colors1 = new string[] {"#ffffe4"};
    
    private string[] _colors1 = new[] {"#f5ffe8", "#77d6c1", "#1c92a7", "#033e5e"};
    private string[] _colors2 = new[] {"#77d6c1", "#ffffe4"};
    private float[] _color_times1 = new float[4] { 0f, 0.33f, 0.66f, 1.0f };
    private float[] _color_times2 = new float[2] { 0f, 1.0f };
    private void Awake()
    {
        m_Starbackground = StarBackground.GetComponent<Image>().material;
        m_Star = Star.GetComponent<Image>().material;
        m_StarFlares = StarFlares.GetComponent<Image>().material;
        SetInitialColors();
    }
    public void SetPixel(float amount)
    {
        m_Starbackground.SetFloat(ShaderProperties.Key_Pixels, amount * 2);
        m_Star.SetFloat(ShaderProperties.Key_Pixels, amount);
        m_StarFlares.SetFloat(ShaderProperties.Key_Pixels, amount * 2);
    }

    public void SetLight(Vector2 pos)
    {
        return;
    }

    public void SetSeed(float seed)
    {
        var converted_seed = seed % 1000f / 100f;
        m_Starbackground.SetFloat(ShaderProperties.Key_Seed, converted_seed);
        m_Star.SetFloat(ShaderProperties.Key_Seed, converted_seed);
        m_StarFlares.SetFloat(ShaderProperties.Key_Seed, converted_seed);
       // setGragientColor(seed);
    }

    public void SetRotate(float r)
    {
        m_Starbackground.SetFloat(ShaderProperties.Key_Rotation, r);
        m_Star.SetFloat(ShaderProperties.Key_Rotation, r);
        m_StarFlares.SetFloat(ShaderProperties.Key_Rotation, r);
    }

    public void UpdateTime(float time)
    {
        m_Starbackground.SetFloat(ShaderProperties.Key_time, time );
        m_Star.SetFloat(ShaderProperties.Key_time, time  * 0.1f);
        m_StarFlares.SetFloat(ShaderProperties.Key_time, time  );
    }

    public void SetCustomTime(float time)
    {
        m_Starbackground.SetFloat(ShaderProperties.Key_time, time);
        m_Star.SetFloat(ShaderProperties.Key_time, time);
        m_StarFlares.SetFloat(ShaderProperties.Key_time, time);
    }
    public void SetInitialColors()
    {
        for (int i = 0; i < color_vars1.Length; i++)
        {
            m_Starbackground.SetColor(color_vars1[i], ColorUtil.FromRGB(init_colors1[i]));
        }
        setGragientColor();
    }

    private void setGragientColor()
    {
        for (int i = 0; i < colorKey1.Length; i++)
        {
            colorKey1[i].color = default(Color);
            ColorUtility.TryParseHtmlString(_colors1[i], out colorKey1[i].color);

            colorKey1[i].time = _color_times1[i];
            alphaKey1[i].alpha = 1.0f;
            alphaKey1[i].time = _color_times1[i];
        }
        
        
        for (int i = 0; i < colorKey2.Length; i++)
        {
            colorKey2[i].color = default(Color);
            ColorUtility.TryParseHtmlString(_colors2[i], out colorKey2[i].color);

            colorKey2[i].time = _color_times2[i];
            alphaKey2[i].alpha = 1.0f;
            colorKey2[i].time = _color_times2[i];
        }
        _gradientStar.SetColors(colorKey1,alphaKey1,gradient_vars);
        _gradientStarFlare.SetColors(colorKey2,alphaKey2,gradient_vars);
    }
    public Color[] GetColors()
    {
        var colors = new Color[7];
        for (int i = 0; i < color_vars1.Length; i++)
        {
            colors[i] = m_Starbackground.GetColor(color_vars1[i]);
        }
        var size = color_vars1.Length;
        
        var gradColors = _gradientStar.GetColorKeys();
        for (int i = 0; i < gradColors.Length; i++)
        {
            colors[i + size] = gradColors[i].color;
        }
        size += gradColors.Length;
        
        var gradColors2 = _gradientStarFlare.GetColorKeys();
        for (int i = 0; i < gradColors2.Length; i++)
        {
            colors[i + size] = gradColors2[i].color;
        }

        return colors;
    }

    public void SetColors(Color[] _colors)
    {
        for (int i = 0; i < color_vars1.Length; i++)
        {
            m_Starbackground.SetColor(color_vars1[i], _colors[i]);
        }
        var size = color_vars1.Length;
        
        for (int i = 0; i < colorKey1.Length; i++)
        {
            colorKey1[i].color = _colors[i + size];
            colorKey1[i].time = _color_times1[i];
            alphaKey1[i].alpha = 1.0f;
            alphaKey1[i].time = _color_times1[i];
        }
        _gradientStar.SetColors(colorKey1,alphaKey1,gradient_vars);
        size += colorKey1.Length;
        
        for (int i = 0; i < colorKey2.Length; i++)
        {
            colorKey2[i].color = _colors[ i + size ];
            colorKey2[i].time = _color_times2[i];
            alphaKey2[i].alpha = 1.0f;
            alphaKey2[i].time = _color_times2[i];
        }
        _gradientStarFlare.SetColors(colorKey2,alphaKey2,gradient_vars);

    }
}
