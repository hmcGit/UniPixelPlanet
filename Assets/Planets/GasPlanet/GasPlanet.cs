using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UI;

public class GasPlanet : MonoBehaviour, IPlanet {
    [SerializeField] private GameObject Cloud1;
    [SerializeField] private GameObject Cloud2;
    private Material m_cloud1;
    private Material m_cloud2;
    private void Awake()
    {
        m_cloud1 = Cloud1.GetComponent<Image>().material;
        m_cloud2 = Cloud2.GetComponent<Image>().material;
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
}