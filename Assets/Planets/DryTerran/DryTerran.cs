using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DryTerran : MonoBehaviour, IPlanet {
    [SerializeField] private GameObject Land;
    private Material m_Land;
    private void Awake()
    {
        m_Land = Land.GetComponent<Image>().material;
    }
    public void SetPixel(float amount)
    {
        m_Land.SetFloat(ShaderProperties.Key_Pixels, amount);
    }

    public void SetLight(Vector2 pos)
    {
        m_Land.SetVector(ShaderProperties.Key_Light_origin, pos);
    }

    public void SetSeed(float seed)
    {
        var converted_seed = seed % 1000f / 100f;
        m_Land.SetFloat(ShaderProperties.Key_Seed, converted_seed);
    }

    public void SetRotate(float r)
    {
        m_Land.SetFloat(ShaderProperties.Key_Rotation, r);
    }

    public void UpdateTime(float time)
    {
        m_Land.SetFloat(ShaderProperties.Key_time, time  );
    }

    public void SetCustomTime(float time)
    {
        var dt = 10f + time * 60f;
        m_Land.SetFloat(ShaderProperties.Key_time, dt);
    }
    public Color[] GetColors()
    {
        var colors = new Color[1];
        return colors;
    }

    public void SetColors(Color[] _colors)
    {
        for (int i = 0; i < _colors.Length; i++)
        {
        }
    }
}
