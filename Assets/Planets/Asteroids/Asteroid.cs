using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Asteroid : MonoBehaviour, IPlanet {
    [SerializeField] private GameObject g_Asteroid;
    private Material m_Asteroid;
    private void Awake()
    {
        m_Asteroid = g_Asteroid.GetComponent<Image>().material;
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
}
