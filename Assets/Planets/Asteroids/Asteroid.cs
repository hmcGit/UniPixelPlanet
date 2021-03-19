using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Asteroid : MonoBehaviour, IPlanet {
    [SerializeField] private GameObject g_Asteroid;
    private Material m_Asteroid;
    private string[] color_vars = new string[]{"_Color1", "_Color2", "_Color3"};
    
    private string[] init_colors = new string[] {"#a3a7c2", "#4c6885", "#3a3f5e"};

private void Awake()
    {
        m_Asteroid = g_Asteroid.GetComponent<Image>().material;
        SetInitialColors();
    }
    public void SetPixel(float amount)
    {
        m_Asteroid.SetFloat(ShaderProperties.Key_Pixels, amount);
    }

    public void SetLight(Vector2 pos)
    {
        m_Asteroid.SetVector(ShaderProperties.Key_Light_origin, pos);
    }

    public void SetSeed(float seed)
    {
        var converted_seed = seed % 1000f / 100f;
        m_Asteroid.SetFloat(ShaderProperties.Key_Seed, converted_seed);
    }

    public void SetRotate(float r)
    {
        m_Asteroid.SetFloat(ShaderProperties.Key_Rotation, r);
    }

    public void UpdateTime(float time)
    {
        return;
    }

    public void SetCustomTime(float time)
    {
        var dt = time * 6.28f;
        time = Mathf.Clamp(dt,0.1f, 6.28f);
        m_Asteroid.SetFloat(ShaderProperties.Key_Rotation, time);
    }
    public void SetInitialColors()
    {
        for (int i = 0; i < color_vars.Length; i++)
        {
            m_Asteroid.SetColor(color_vars[i], ColorUtil.FromRGB(init_colors[i]));
        }
    }
    public Color[] GetColors()
    {
        var colors = new Color[3];
        for (int i = 0; i < color_vars.Length; i++)
        {
            colors[i] = m_Asteroid.GetColor(color_vars[i]);
        }
        return colors;
    }

    public void SetColors(Color[] _colors)
    {
        for (int i = 0; i < _colors.Length; i++)
        {
            m_Asteroid.SetColor(color_vars[i], _colors[i]);
        }
    }
}
