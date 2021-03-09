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

    private void Awake()
    {
        m_PlanetUnder = PlanetUnder.GetComponent<Image>().material;
        m_Lakes = Lakes.GetComponent<Image>().material;
        m_Clouds = Clouds.GetComponent<Image>().material;
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
}
