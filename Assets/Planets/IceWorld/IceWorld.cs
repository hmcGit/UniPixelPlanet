using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IceWorld : MonoBehaviour, IPlanet {
    [SerializeField] private GameObject PlanetUnder;
    [SerializeField] private GameObject Lakes;
    [SerializeField] private GameObject Clouds;
    private Material m_PlanetUnder;
    private Material m_Lakes;
    private Material m_Clouds;
    private string[] color_vars1 = new string[]{"_Color1", "_Color2", "_Color3"};
    private string[] init_colors1 = new string[] {"#faffff", "#c7d4e1", "#928fb8"};
    
    private string[] color_vars2 = new string[]{"_Color1", "_Color2", "_Color3"};
    private string[] init_colors2 = new string[] {"#4fa4b8", "#4c6885", "#3a3f5e"};
    
    private string[] color_vars3 = new string[]{"_Base_color", "_Outline_color", "_Shadow_Base_color","_Shadow_Outline_color"};
    private string[] init_colors3 = new string[] {"#e1f2ff", "#c0e3ff", "#5e70a5","#404973"};
    private void Awake()
    {
        m_PlanetUnder = PlanetUnder.GetComponent<Image>().material;
        m_Lakes = Lakes.GetComponent<Image>().material;
        m_Clouds = Clouds.GetComponent<Image>().material;
        SetInitialColors();
    }
    public void SetPixel(float amount)
    {
        m_PlanetUnder.SetFloat(ShaderProperties.Key_Pixels, amount);
        m_Lakes.SetFloat(ShaderProperties.Key_Pixels, amount);
        m_Clouds.SetFloat(ShaderProperties.Key_Pixels, amount);
    }

    public void SetLight(Vector2 pos)
    {
        m_PlanetUnder.SetVector(ShaderProperties.Key_Light_origin, pos);
        m_Lakes.SetVector(ShaderProperties.Key_Light_origin, pos);
        m_Clouds.SetVector(ShaderProperties.Key_Light_origin, pos);
    }

    public void SetSeed(float seed)
    {
        var converted_seed = seed % 1000f / 100f;
        m_PlanetUnder.SetFloat(ShaderProperties.Key_Seed, converted_seed);
        m_Lakes.SetFloat(ShaderProperties.Key_Seed, converted_seed);
        m_Clouds.SetFloat(ShaderProperties.Key_Seed, converted_seed);
    }

    public void SetRotate(float r)
    {
        m_PlanetUnder.SetFloat(ShaderProperties.Key_Rotation, r);
        m_Lakes.SetFloat(ShaderProperties.Key_Rotation, r);
        m_Clouds.SetFloat(ShaderProperties.Key_Rotation, r);
    }

    public void UpdateTime(float time)
    {
        
        m_Clouds.SetFloat(ShaderProperties.Key_time, time * 0.5f);
        m_PlanetUnder.SetFloat(ShaderProperties.Key_time, time );
        m_Lakes.SetFloat(ShaderProperties.Key_time, time);
    }

    public void SetCustomTime(float time)
    {
        
        var dt = 10f + time * 60f;
        m_Clouds.SetFloat(ShaderProperties.Key_time, dt * 0.5f);
        m_PlanetUnder.SetFloat(ShaderProperties.Key_time, dt);
        m_Lakes.SetFloat(ShaderProperties.Key_time, dt);
    }
    public void SetInitialColors()
    {
        for (int i = 0; i < color_vars1.Length; i++)
        {
            m_PlanetUnder.SetColor(color_vars1[i], ColorUtil.FromRGB(init_colors1[i]));
        }
        for (int i = 0; i < color_vars2.Length; i++)
        {
            m_Lakes.SetColor(color_vars2[i], ColorUtil.FromRGB(init_colors2[i]));
        }
        for (int i = 0; i < color_vars3.Length; i++)
        {
            m_Clouds.SetColor(color_vars3[i], ColorUtil.FromRGB(init_colors3[i]));
        }
    }
    public Color[] GetColors()
    {
        var colors = new Color[10];
        int pos = 0;
        for (int i = 0; i < color_vars1.Length; i++)
        {
            colors[i] = m_PlanetUnder.GetColor(color_vars1[i]);
        }
        pos = color_vars1.Length;
        for (int i = 0; i < color_vars2.Length; i++)
        {
            colors[i + pos] = m_Lakes.GetColor(color_vars2[i]);
        }
        pos = color_vars1.Length + color_vars2.Length;
        for (int i = 0; i < color_vars3.Length; i++)
        {
            colors[i + pos] = m_Clouds.GetColor(color_vars3[i]);
        }
        return colors;
    }

    public void SetColors(Color[] _colors)
    {
        for (int i = 0; i < color_vars1.Length; i++)
        {
            m_PlanetUnder.SetColor(color_vars1[i], _colors[i]);
        }
        for (int i = 0; i < color_vars2.Length; i++)
        {
            m_Lakes.SetColor(color_vars2[i], _colors[i + color_vars1.Length]);
        }
        for (int i = 0; i < color_vars3.Length; i++)
        {
            m_Clouds.SetColor(color_vars3[i], _colors[i + color_vars1.Length + color_vars2.Length]);
        }
    }
}
