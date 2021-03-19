using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoAtmosphere : MonoBehaviour, IPlanet {
    [SerializeField] private GameObject Land;
    [SerializeField] private GameObject Craters;
    private Material m_Land;
    private Material m_Craters;
    
    private string[] color_vars1 = new string[]{"_Color1", "_Color2", "_Color3"};
    private string[] init_colors1 = new string[] {"#A3A7C2", "#4C6885", "#3A3F5E"};
    private string[] color_vars2 = new string[] {"_Color1", "_Color2"};
    private string[] init_colors2 = new string[] {"#4C6885", "#3A3F5E"};
    private void Awake()
    {
        m_Land = Land.GetComponent<Image>().material;
        m_Craters = Craters.GetComponent<Image>().material;
        SetInitialColors();
    }
    public void SetPixel(float amount)
    {
        m_Land.SetFloat(ShaderProperties.Key_Pixels, amount);
        m_Craters.SetFloat(ShaderProperties.Key_Pixels, amount);
    }

    public void SetLight(Vector2 pos)
    {
        m_Land.SetVector(ShaderProperties.Key_Light_origin, pos);
        m_Craters.SetVector(ShaderProperties.Key_Light_origin, pos);
    }

    public void SetSeed(float seed)
    {
        var converted_seed = seed % 1000f / 100f;
        m_Land.SetFloat(ShaderProperties.Key_Seed, converted_seed);
        m_Craters.SetFloat(ShaderProperties.Key_Seed, converted_seed);
    }

    public void SetRotate(float r)
    {
        m_Land.SetFloat(ShaderProperties.Key_Rotation, r);
        m_Craters.SetFloat(ShaderProperties.Key_Rotation, r);
    }

    public void UpdateTime(float time)
    {
        m_Land.SetFloat(ShaderProperties.Key_time, time  * 0.5f);
        m_Craters.SetFloat(ShaderProperties.Key_time, time  * 0.5f);
    }

    public void SetCustomTime(float time)
    {
        var dt = 10f + time * 60f;
        m_Land.SetFloat(ShaderProperties.Key_time, dt * 0.5f);
        m_Craters.SetFloat(ShaderProperties.Key_time, dt * 0.5f);
    }
    public void SetInitialColors()
    {
        for (int i = 0; i < color_vars1.Length; i++)
        {
            m_Land.SetColor(color_vars1[i], ColorUtil.FromRGB(init_colors1[i]));
        }
        for (int i = 0; i < color_vars2.Length; i++)
        {
            m_Craters.SetColor(color_vars2[i], ColorUtil.FromRGB(init_colors2[i]));
        }
    }
    public Color[] GetColors()
    {
        var colors = new Color[10];
        int pos = 0;
        for (int i = 0; i < color_vars1.Length; i++)
        {
            colors[i] = m_Land.GetColor(color_vars1[i]);
        }
        pos = color_vars1.Length;
        for (int i = 0; i < color_vars2.Length; i++)
        {
            colors[i + pos] = m_Craters.GetColor(color_vars2[i]);
        }
        return colors;
    }

    public void SetColors(Color[] _colors)
    {
        for (int i = 0; i < color_vars1.Length; i++)
        {
            m_Land.SetColor(color_vars1[i], _colors[i]);
        }
        for (int i = 0; i < color_vars2.Length; i++)
        {
            m_Craters.SetColor(color_vars2[i], _colors[i + color_vars1.Length]);
        }
    }
}