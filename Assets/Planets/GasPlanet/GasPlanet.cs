using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GasPlanet : MonoBehaviour, IPlanet {
    [SerializeField] private GameObject Cloud1;
    [SerializeField] private GameObject Cloud2;
    private Material m_cloud1;
    private Material m_cloud2;
    private string[] color_vars1 = new string[]{"_Base_color", "_Outline_color", "_Shadow_base_color","_Shadow_outline_color"};
    private string[] init_colors1 = new string[] {"#3b2027", "#3b2027", "#21181b","#21181b"};
    private string[] color_vars2 = new string[]{"_Base_color", "_Outline_color", "_Shadow_base_color","_Shadow_outline_color"};
    private string[] init_colors2 = new string[] {"#f0b541", "#cf752b","#ab5130","#7d3833"};
    private void Awake()
    {
        m_cloud1 = Cloud1.GetComponent<Image>().material;
        m_cloud2 = Cloud2.GetComponent<Image>().material;
        SetInitialColors();
    }
    public void SetPixel(float amount)
    {
        m_cloud1.SetFloat(ShaderProperties.Key_Pixels, amount);
        m_cloud2.SetFloat(ShaderProperties.Key_Pixels, amount);
    }

    public void SetLight(Vector2 pos)
    {
        m_cloud1.SetVector(ShaderProperties.Key_Light_origin, pos);
        m_cloud2.SetVector(ShaderProperties.Key_Light_origin, pos);
    }

    public void SetSeed(float seed)
    {
        var converted_seed = seed % 1000f / 100f;
        m_cloud1.SetFloat(ShaderProperties.Key_Seed, converted_seed);
        m_cloud2.SetFloat(ShaderProperties.Key_Seed, converted_seed);
        m_cloud2.SetFloat(ShaderProperties.Key_Cloud_cover, Random.Range(0.28f, 0.5f));
    }

    public void SetRotate(float r)
    {
        m_cloud1.SetFloat(ShaderProperties.Key_Rotation, r);
        m_cloud2.SetFloat(ShaderProperties.Key_Rotation, r);
    }

    public void UpdateTime(float time)
    {
        m_cloud1.SetFloat(ShaderProperties.Key_time, time * 0.5f);
        m_cloud2.SetFloat(ShaderProperties.Key_time, time  * 0.5f);
    }

    public void SetCustomTime(float time)
    {
        var dt = 10f + time * 60f;
        m_cloud1.SetFloat(ShaderProperties.Key_time, dt * 0.5f);
        m_cloud2.SetFloat(ShaderProperties.Key_time, dt * 0.5f);
    }
    public void SetInitialColors()
    {
        for (int i = 0; i < color_vars1.Length; i++)
        {
            m_cloud1.SetColor(color_vars1[i], ColorUtil.FromRGB(init_colors1[i]));
        }
        for (int i = 0; i < color_vars2.Length; i++)
        {
            m_cloud2.SetColor(color_vars2[i], ColorUtil.FromRGB(init_colors2[i]));
        }
    }
    public Color[] GetColors()
    {
        var colors = new Color[10];
        int pos = 0;
        for (int i = 0; i < color_vars1.Length; i++)
        {
            colors[i] = m_cloud1.GetColor(color_vars1[i]);
        }
        pos = color_vars1.Length;
        for (int i = 0; i < color_vars2.Length; i++)
        {
            colors[i + pos] = m_cloud2.GetColor(color_vars2[i]);
        }
        return colors;
    }

    public void SetColors(Color[] _colors)
    {
        for (int i = 0; i < color_vars1.Length; i++)
        {
            m_cloud1.SetColor(color_vars1[i], _colors[i]);
        }
        for (int i = 0; i < color_vars2.Length; i++)
        {
            m_cloud2.SetColor(color_vars2[i], _colors[i + color_vars1.Length]);
        }
    }
}