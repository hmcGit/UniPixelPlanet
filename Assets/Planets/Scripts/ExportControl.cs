using System;
using UnityEngine;
using UnityEngine.UI;

public class ExportControl : MonoBehaviour {
    [SerializeField] private Button btnUpWidth;
    [SerializeField] private Button btnDownWidth;
    [SerializeField] private Button btnUpHeight;
    [SerializeField] private Button btnDownHeight;

    [SerializeField] private InputField inputWidth;
    [SerializeField] private InputField inputHeight;
    [SerializeField] private Text textFrame;
    [SerializeField] private Text textResolution;

    private int width = 5;
    private int height = 2;
    private int frames = 0;
    private const int max_frame = 100;
    private void Start()
    {
        updateFrame();
        
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }
    public void OnUpdateFrames()
    {
        var w = 0;
        var h = 0;
        int.TryParse(inputWidth.text, out w);
        int.TryParse(inputHeight.text, out h);
        if (w < 1 || w > max_frame) {
            inputWidth.text = width.ToString();
            return;
        }
        
        if (h < 1 || h > max_frame) {
            inputHeight.text = height.ToString();
            return;
        }

        width = w;
        height = h;
        updateFrame();
    }

    private void updateFrame()
    {
        frames = width * height;
        
        inputWidth.text = width.ToString();
        textFrame.text = frames.ToString();
        textResolution.text = (width * 100).ToString() + "x" + (height * 100).ToString();
    }
    public void OnWidthUp()
    {
        if (width < max_frame)
        {
            width++;
        }
        updateFrame();

    }
    public void OnWidthDown()
    {
        if (width > 1)
        {
            width--;
        }
        updateFrame();
    }
    public void OnHeightUp()
    {
        if (height < max_frame)
        {
            height++;
        }
        updateFrame();

    }
    public void OnHeightDown()
    {
        if (height > 1)
        {
            height--;
        }
        updateFrame();
    }
}
