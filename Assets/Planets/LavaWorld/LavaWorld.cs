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
    private void Awake()
    {
        m_Planet = PlanetUnder.GetComponent<Image>().material;
        m_Craters = Craters.GetComponent<Image>().material;
        m_Rivers = LavaRivers.GetComponent<Image>().material;
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
}
