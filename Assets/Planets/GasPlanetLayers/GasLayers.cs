using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GasLayers : MonoBehaviour, IPlanet {
    [SerializeField] private GameObject _GasLayers;
    [SerializeField] private GameObject _Ring;

    private Material m_GasLayers;
    private Material m_Ring;

    private void Awake()
    {
        m_GasLayers = _GasLayers.GetComponent<Image>().material;
        m_Ring = _Ring.GetComponent<Image>().material;
    }
    public void SetPixel(float amount)
    {
        m_GasLayers.SetFloat(ShaderProperties.Key_Pixels, amount);
        m_Ring.SetFloat(ShaderProperties.Key_Pixels, amount * 3f);
    }

    public void SetLight(Vector2 pos)
    {
        m_GasLayers.SetVector(ShaderProperties.Key_Light_origin, pos * 1.3f  );
        m_Ring.SetVector(ShaderProperties.Key_Light_origin, pos * 1.3f );
    }

    public void SetSeed(float seed)
    {
        var converted_seed = seed % 1000f / 100f;
        m_GasLayers.SetFloat(ShaderProperties.Key_Seed, converted_seed);
        m_Ring.SetFloat(ShaderProperties.Key_Seed, converted_seed);
      //  _Ring.SetFloat("cloud_cover", Random.Range(0.28f, 0.5f));
    }

    public void SetRotate(float r)
    {
        m_GasLayers.SetFloat(ShaderProperties.Key_Rotation, r);
        m_Ring.SetFloat(ShaderProperties.Key_Rotation, r + 0.7f);
    }

    public void UpdateTime(float time)
    {
        m_GasLayers.SetFloat(ShaderProperties.Key_time, time * 0.5f);
        m_Ring.SetFloat(ShaderProperties.Key_time, time  * 0.5f * -3f);
    }

    public void SetCustomTime(float time)
    {
        var dt = 10f + time * 60f;
        m_GasLayers.SetFloat(ShaderProperties.Key_time, dt * 0.5f);
        m_Ring.SetFloat(ShaderProperties.Key_time, dt * 0.5f * -3f);
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