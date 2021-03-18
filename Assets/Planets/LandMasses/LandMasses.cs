using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class LandMasses : MonoBehaviour, IPlanet {
    [SerializeField] private GameObject Water;
    [SerializeField] private GameObject Land;
    [SerializeField] private GameObject Cloud;
    private Material m_Water;
    private Material m_Land;
    private Material m_Clouds;
    private string[] color_vars1 = new string[]{"_Color1", "_Color2", "_Color3"};
    private string[] init_colors1 = new string[] {"#92E8C0", "#4FA4B8", "#2C354D"};
    private string[] color_vars2 = new string[]{"_Color1", "_Color2", "_Color3","_Color4"};
    private string[] init_colors2 = new string[] {"#C8D45D", "#63AB3F", "#2F5753", "#283540"};
    private string[] color_vars3 = new string[]{"_Base_color", "_Outline_color", "_Shadow_Base_color","_Shadow_Outline_color"};
    private string[] init_colors3 = new string[] {"#DFE0E8", "#A3A7C2", "#686F99","#404973"};
    private void Awake()
    {
        m_Water = Water.GetComponent<Image>().material;
        m_Land = Land.GetComponent<Image>().material;
        m_Clouds = Cloud.GetComponent<Image>().material;
        SetInitialColors();
    }
    public void SetPixel(float amount)
    {
        m_Water.SetFloat(ShaderProperties.Key_Pixels, amount);
        m_Land.SetFloat(ShaderProperties.Key_Pixels, amount);
        m_Clouds.SetFloat(ShaderProperties.Key_Pixels, amount);
    }

    public void SetLight(Vector2 pos)
    {
        m_Water.SetVector(ShaderProperties.Key_Light_origin, pos);
        m_Land.SetVector(ShaderProperties.Key_Light_origin, pos);
        m_Clouds.SetVector(ShaderProperties.Key_Light_origin, pos);
    }

    public void SetSeed(float seed)
    {
        var converted_seed = seed % 1000f / 100f;
        m_Water.SetFloat(ShaderProperties.Key_Seed, converted_seed);
        m_Land.SetFloat(ShaderProperties.Key_Seed, converted_seed);
        m_Clouds.SetFloat(ShaderProperties.Key_Seed, converted_seed);
        m_Clouds.SetFloat(ShaderProperties.Key_Cloud_cover, Random.Range(0.35f, 0.6f));
    }

    public void SetRotate(float r)
    {
        m_Water.SetFloat(ShaderProperties.Key_Rotation, r);
        m_Land.SetFloat(ShaderProperties.Key_Rotation, r);
        m_Clouds.SetFloat(ShaderProperties.Key_Rotation, r);
    }

    public void UpdateTime(float time)
    {
        m_Clouds.SetFloat(ShaderProperties.Key_time, time * 0.5f);
        m_Water.SetFloat(ShaderProperties.Key_time, time );
        m_Land.SetFloat(ShaderProperties.Key_time, time);
    }

    public void SetCustomTime(float time)
    {
        
        var dt = 10f + time * 60f;
        m_Clouds.SetFloat(ShaderProperties.Key_time, dt * 0.5f);
        m_Water.SetFloat(ShaderProperties.Key_time, dt);
        m_Land.SetFloat(ShaderProperties.Key_time, dt);
    }
    public void SetInitialColors()
    {
        for (int i = 0; i < color_vars1.Length; i++)
        {
            m_Water.SetColor(color_vars1[i], ColorUtil.FromRGB(init_colors1[i]));
        }
        for (int i = 0; i < color_vars2.Length; i++)
        {
            m_Land.SetColor(color_vars2[i], ColorUtil.FromRGB(init_colors2[i]));
        }
        for (int i = 0; i < color_vars3.Length; i++)
        {
            m_Clouds.SetColor(color_vars3[i], ColorUtil.FromRGB(init_colors3[i]));
        }
    }
    public Color[] GetColors()
    {
        var colors = new Color[11];
        int pos = 0;
        for (int i = 0; i < color_vars1.Length; i++)
        {
            colors[i] = m_Water.GetColor(color_vars1[i]);
        }
        pos = color_vars1.Length;
        for (int i = 0; i < color_vars2.Length; i++)
        {
            colors[i + pos] = m_Land.GetColor(color_vars2[i]);
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
        var colorArray = _colors.ToList();

        for (int i = 0; i < color_vars1.Length; i++)
        {
            var key = color_vars1[i];
            var val = colorArray.GetRange(i, 1).FirstOrDefault();
            m_Water.SetColor(key,val );
        }
        for (int i = 0; i < color_vars2.Length; i++)
        {
            var key = color_vars2[i];
            var val = colorArray.GetRange(i + color_vars1.Length, 1).FirstOrDefault();
            m_Land.SetColor(key,val );
        }
        for (int i = 0; i < color_vars3.Length; i++)
        {
            var key = color_vars3[i];
            var val = colorArray.GetRange(i + color_vars1.Length + color_vars2.Length, 1).FirstOrDefault();
            m_Clouds.SetColor(key,val );
        }

    }
}
