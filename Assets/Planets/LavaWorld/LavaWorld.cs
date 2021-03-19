using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LavaWorld : MonoBehaviour, IPlanet {
    [SerializeField] private GameObject PlanetUnder;
    [SerializeField] private GameObject Craters;
    [SerializeField] private GameObject LavaRivers;
    private Material m_Planet;
    private Material m_Craters;
    private Material m_Rivers;
    private string[] color_vars1 = new string[]{"_Color1", "_Color2", "_Color3"};
    private string[] init_colors1 = new string[] {"#8f4d57", "#52333f", "#3d2936"};
    
    private string[] color_vars2 = new string[]{"_Color1", "_Color2"};
    private string[] init_colors2 = new string[] {"#52333f", "#3d2936"};
    
    private string[] color_vars3 = new string[]{"_Color1", "_Color2", "_Color3"};
    private string[] init_colors3 = new string[] {"#ff8933", "#e64539", "#ad2f45"};
    private void Awake()
    {
        m_Planet = PlanetUnder.GetComponent<Image>().material;
        m_Craters = Craters.GetComponent<Image>().material;
        m_Rivers = LavaRivers.GetComponent<Image>().material;
        SetInitialColors();
    }
    public void SetPixel(float amount)
    {
        m_Planet.SetFloat(ShaderProperties.Key_Pixels, amount);
        m_Craters.SetFloat(ShaderProperties.Key_Pixels, amount);
        m_Rivers.SetFloat(ShaderProperties.Key_Pixels, amount);
    }

    public void SetLight(Vector2 pos)
    {
        m_Planet.SetVector(ShaderProperties.Key_Light_origin, pos);
        m_Craters.SetVector(ShaderProperties.Key_Light_origin, pos);
        m_Rivers.SetVector(ShaderProperties.Key_Light_origin, pos);
    }

    public void SetSeed(float seed)
    {
        var converted_seed = seed % 1000f / 100f;
        m_Planet.SetFloat(ShaderProperties.Key_Seed, converted_seed);
        m_Craters.SetFloat(ShaderProperties.Key_Seed, converted_seed);
        m_Rivers.SetFloat(ShaderProperties.Key_Seed, converted_seed);
    }

    public void SetRotate(float r)
    {
        m_Planet.SetFloat(ShaderProperties.Key_Rotation, r);
        m_Craters.SetFloat(ShaderProperties.Key_Rotation, r);
        m_Rivers.SetFloat(ShaderProperties.Key_Rotation, r);
    }

    public void UpdateTime(float time)
    {
        m_Planet.SetFloat(ShaderProperties.Key_time, time * 0.5f);
        m_Craters.SetFloat(ShaderProperties.Key_time, time  * 0.5f);
        m_Rivers.SetFloat(ShaderProperties.Key_time, time  * 0.5f);
    }

    public void SetCustomTime(float time)
    {
        var dt = 10f + time * 60f;
        m_Planet.SetFloat(ShaderProperties.Key_time, dt * 0.5f);
        m_Craters.SetFloat(ShaderProperties.Key_time, dt * 0.5f);
        m_Rivers.SetFloat(ShaderProperties.Key_time, dt * 0.5f);
    }
    public void SetInitialColors()
    {
        for (int i = 0; i < color_vars1.Length; i++)
        {
            m_Planet.SetColor(color_vars1[i], ColorUtil.FromRGB(init_colors1[i]));
        }
        for (int i = 0; i < color_vars2.Length; i++)
        {
            m_Craters.SetColor(color_vars2[i], ColorUtil.FromRGB(init_colors2[i]));
        }
        for (int i = 0; i < color_vars3.Length; i++)
        {
            m_Rivers.SetColor(color_vars3[i], ColorUtil.FromRGB(init_colors3[i]));
        }
    }
    public Color[] GetColors()
    {
        var colors = new Color[8];
        int pos = 0;
        for (int i = 0; i < color_vars1.Length; i++)
        {
            colors[i] = m_Planet.GetColor(color_vars1[i]);
        }
        pos = color_vars1.Length;
        for (int i = 0; i < color_vars2.Length; i++)
        {
            colors[i + pos] = m_Craters.GetColor(color_vars2[i]);
        }
        pos = color_vars1.Length + color_vars2.Length;
        for (int i = 0; i < color_vars3.Length; i++)
        {
            colors[i + pos] = m_Rivers.GetColor(color_vars3[i]);
        }
        return colors;
    }

    public void SetColors(Color[] _colors)
    {
        for (int i = 0; i < color_vars1.Length; i++)
        {
            m_Planet.SetColor(color_vars1[i], _colors[i]);
        }
        for (int i = 0; i < color_vars2.Length; i++)
        {
            m_Craters.SetColor(color_vars2[i], _colors[i + color_vars1.Length]);
        }
        for (int i = 0; i < color_vars3.Length; i++)
        {
            m_Rivers.SetColor(color_vars3[i], _colors[i + color_vars1.Length + color_vars2.Length]);
        }
    }
}
