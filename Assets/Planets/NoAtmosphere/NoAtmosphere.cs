using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoAtmosphere : MonoBehaviour, IPlanet {
    [SerializeField] private GameObject Land;
    [SerializeField] private GameObject Craters;
    private Material m_Land;
    private Material m_Craters;
    private void Awake()
    {
        m_Land = Land.GetComponent<Image>().material;
        m_Craters = Craters.GetComponent<Image>().material;
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
}