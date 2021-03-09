using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LandMasses : MonoBehaviour, IPlanet {
    [SerializeField] private GameObject Water;
    [SerializeField] private GameObject Land;
    [SerializeField] private GameObject Cloud;
    private Material m_Water;
    private Material m_Land;
    private Material m_Clouds;
    private void Awake()
    {
        m_Water = Water.GetComponent<Image>().material;
        m_Land = Land.GetComponent<Image>().material;
        m_Clouds = Cloud.GetComponent<Image>().material;
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
}
