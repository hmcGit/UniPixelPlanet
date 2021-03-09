using UnityEngine;

[ExecuteInEditMode]
public class GradientTextureMultiGenerate : MonoBehaviour {

    [SerializeField] private Gradient gradient1;
    [SerializeField] private Gradient gradient2;
    [SerializeField] private Material mat1;
    [SerializeField] private Material mat2;

    private void Awake()
    {
        //Material material = new Material(Shader.Find("Unlit/DryTerran"));
        mat1.SetTexture(ShaderProperties.Key_TextureKeyword1, CreateTexture1());
        mat1.SetTexture(ShaderProperties.Key_TextureKeyword2, CreateTexture2());
        mat2.SetTexture(ShaderProperties.Key_TextureKeyword1, CreateTexture1());
        mat2.SetTexture(ShaderProperties.Key_TextureKeyword2, CreateTexture2());
        //GetComponent<Renderer>().material = material;
    }

    private Texture2D CreateTexture1()
    {
        Texture2D texture = new Texture2D(128, 1);
        for (int h = 0; h < texture.height; h++)
        {
            for (int w = 0; w < texture.width; w++)
            {
                texture.SetPixel(w, h, gradient1.Evaluate((float)w / texture.width));
            }
        }

        texture.Apply();
        texture.wrapMode = TextureWrapMode.Clamp;
        return texture;
    }
    private Texture2D CreateTexture2()
    {
        Texture2D texture = new Texture2D(128, 1);
        for (int h = 0; h < texture.height; h++)
        {
            for (int w = 0; w < texture.width; w++)
            {
                texture.SetPixel(w, h, gradient2.Evaluate((float)w / texture.width));
            }
        }

        texture.Apply();
        texture.wrapMode = TextureWrapMode.Clamp;
        return texture;
    }
}