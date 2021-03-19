using System;
using UnityEngine;
using UnityEngine.UI;
[ExecuteInEditMode]
public class GradientTextureGenerate : MonoBehaviour {

    [SerializeField] private Gradient gradient;
    [SerializeField] private Material targetMaterial;

    [SerializeField] private string Shader_key = "";
    private GradientColorKey[] _colorKeys;
    private void Awake()
    {
      //  targetMaterial = GetComponent<Image>().material;    
        //Material material = new Material(Shader.Find("Unlit/DryTerran"));
        targetMaterial.SetTexture(Shader_key, CreateTexture());
        //GetComponent<Renderer>().material = material;
    }

    public void SetColors(GradientColorKey[] colorkey, GradientAlphaKey[] alphakey,string key = "")
    {
        if (string.IsNullOrEmpty(key))
        {
            key = Shader_key;
        }
        gradient = new Gradient();
        gradient.SetKeys(colorkey,alphakey);
        targetMaterial.SetTexture(key, CreateTexture());
        _colorKeys = new GradientColorKey[colorkey.Length];
        _colorKeys = colorkey;

    }

    public GradientColorKey[] GetColorKeys()
    {
        return _colorKeys;
    }
    private Texture2D CreateTexture()
    {
        Texture2D texture = new Texture2D(128, 1);
        for (int h = 0; h < texture.height; h++)
        {
            for (int w = 0; w < texture.width; w++)
            {
                texture.SetPixel(w, h, gradient.Evaluate((float)w / texture.width));
            }
        }

        texture.Apply();
        texture.wrapMode = TextureWrapMode.Clamp;
        return texture;
    }
}