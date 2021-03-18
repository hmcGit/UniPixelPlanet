using UnityEngine;
using UnityEngine.UI;
[ExecuteInEditMode]
public class GradientTextureGenerate : MonoBehaviour {

    [SerializeField] private Gradient gradient;
    [SerializeField] private Material targetMaterial;

    private GradientColorKey[] _colorKeys;
    private void Awake()
    {
        targetMaterial = GetComponent<Image>().material;    
        //Material material = new Material(Shader.Find("Unlit/DryTerran"));
        targetMaterial.SetTexture(ShaderProperties.Key_GradientTex, CreateTexture());
        //GetComponent<Renderer>().material = material;
    }

    public void SetColors(GradientColorKey[] colorkey, GradientAlphaKey[] alphakey)
    {
        gradient = new Gradient();
        gradient.SetKeys(colorkey,alphakey);
        targetMaterial.SetTexture(ShaderProperties.Key_GradientTex, CreateTexture());
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