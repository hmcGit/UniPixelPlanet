using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing.Extension;
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
    
    private GradientColorKey[] colorKey = new GradientColorKey[4];
    private GradientAlphaKey[] alphaKey = new GradientAlphaKey[4];
    private string[] _colors1 = new[] {"#f5ffe8", "#ffd832", "#ff823b", "#7c191a"};
    private string[] _colors2 = new[] {"#f5ffe8", "#77d6c1", "#1c92a7", "#033e5e"};
    private float[] _color_times = new float[4] { 0f, 0.33f, 0.66f, 1.0f };
    private void Awake()
    {
        m_Starbackground = StarBackground.GetComponent<Image>().material;
        m_Star = Star.GetComponent<Image>().material;
        m_StarFlares = StarFlares.GetComponent<Image>().material;
        setGragientColor(1);
    }
    private void setGragientColor(float seed)
    {
        for (int i = 0; i < colorKey.Length; i++)
        {
            colorKey[i].color = default(Color);
            if (seed % 2 == 0) {
                ColorUtility.TryParseHtmlString(_colors1[i], out colorKey[i].color);
            } else {
                ColorUtility.TryParseHtmlString(_colors2[i], out colorKey[i].color);
            }

            colorKey[i].time = _color_times[i];
            alphaKey[i].alpha = 1.0f;
            alphaKey[i].time = _color_times[i];
        }
        _gradientStar.SetColors(colorKey,alphaKey);
        _gradientStarFlare.SetColors(colorKey,alphaKey);

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
        setGragientColor(seed);
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
}
