using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LandRivers : MonoBehaviour, IPlanet {
    [SerializeField] private GameObject Land;
    [SerializeField] private GameObject Cloud;

    private Material m_Land;
    private Material m_Cloud;
    private void Awake()
    {
        m_Land = Land.GetComponent<Image>().material;
        m_Cloud = Cloud.GetComponent<Image>().material;
    }
    public void SetPixel(float amount)
    {
        m_Land.SetFloat(ShaderProperties.Key_Pixels, amount);
        m_Cloud.SetFloat(ShaderProperties.Key_Pixels, amount);
    }

    public void SetLight(Vector2 pos)
    {
        m_Land.SetVector(ShaderProperties.Key_Light_origin, pos);
        m_Cloud.SetVector(ShaderProperties.Key_Light_origin, pos);
    }

    public void SetSeed(float seed)
    {
        var converted_seed = seed % 1000f / 100f;
        m_Land.SetFloat(ShaderProperties.Key_Seed, converted_seed);
        m_Cloud.SetFloat(ShaderProperties.Key_Seed, converted_seed);
        m_Cloud.SetFloat(ShaderProperties.Key_Cloud_cover, Random.Range(0.35f, 0.6f));
    }

    public void SetRotate(float r)
    {
        m_Land.SetFloat(ShaderProperties.Key_Rotation, r);
        m_Cloud.SetFloat(ShaderProperties.Key_Rotation, r);
    }

    public void UpdateTime(float time)
    {
        m_Cloud.SetFloat(ShaderProperties.Key_time, time * 0.25f);
        m_Land.SetFloat(ShaderProperties.Key_time, time * 0.5f);
    }

    public void SetCustomTime(float time)
    {
        var dt = 10f + time * 60f;
        m_Cloud.SetFloat(ShaderProperties.Key_time, dt * 0.25f );
        m_Land.SetFloat(ShaderProperties.Key_time, dt * 0.5f);
    }
}
