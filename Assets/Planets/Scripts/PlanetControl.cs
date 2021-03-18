using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = System.Random;


public class PlanetControl : MonoBehaviour {
    
    [SerializeField] private Slider sliderPixel;
    [SerializeField] private Text textPixel;
    [SerializeField] private Slider sliderRotation;
    [SerializeField] private InputField inputSeed;

    [SerializeField] private GameObject[] planets;
    [SerializeField] private Dropdown dd_planets;

    [SerializeField] private ExportControl _exportControl;
    [SerializeField] private GameObject exportPanel;
    [SerializeField] private MaterialSave _materialSave;
    [SerializeField] private GameObject pref_colorButton;
    [SerializeField] private RectTransform colorButtonHolder;
    
    private float time = 0f;
    private float pixels = 100;
    private int seed = 0;
    private bool override_time = false;
    private List<Color> colors = new List<Color>();
    private List<GameObject> colorBtns = new List<GameObject>();
    private int selectedColorButtonID = 0;
    private GameObject selectedColorButton;
    private void Start()
    {
        OnChangeSeedRandom();
        GetColors();
        MakeColorButtons();
    }

    private int selected_planet = 0;

    public void OnClickChooseColor()
    {
        selectedColorButton = EventSystem.current.currentSelectedGameObject;
        selectedColorButtonID = EventSystem.current.currentSelectedGameObject.GetComponent<ColorChooserButton>().ButtonID;
        ColorPicker.Create(colors[selectedColorButtonID], "Choose color", onColorChanged, onColorSelected, false);
    }

    private void onColorChanged(Color currentColor)
    {
        colors[selectedColorButtonID] = currentColor;
        SetColor();
    }

    private void onColorSelected(Color finishedColor)
    {
        colors[selectedColorButtonID] = finishedColor;
        SetColor();
    }
    private void MakeColorButtons()
    {
        for (int i = 0; i < colors.Count; i++)
        {
            var btn = GameObject.Instantiate(pref_colorButton, colorButtonHolder);
            colorBtns.Add(btn);
            btn.GetComponent<Image>().color = colors[i];
            btn.GetComponent<ColorChooserButton>().ButtonID = i;
            btn.GetComponent<Button>().onClick.AddListener(() => OnClickChooseColor());
        }
    }
    private void GetColors()
    {
        foreach (var btn in colorBtns)
        {
            DestroyImmediate(btn);
        }
        
        colors.Clear();
        colorBtns.Clear();
        colors = planets[selected_planet].GetComponent<IPlanet>().GetColors().ToList();
    }

    private void SetColor()
    {
        //Debug.Log(selected_planet + ":"+planets[selected_planet]);
        selectedColorButton.GetComponent<Image>().color = colors[selectedColorButtonID];
        planets[selected_planet].GetComponent<IPlanet>().SetColors(colors.ToArray());
    }
    public void OnSelectPlanet()
    {
        selected_planet = dd_planets.value;
        IPlanet _planet = null;
        for (int i = 0; i < planets.Length; i++)
        {
            if (i == selected_planet) {
                planets[i] .SetActive(true);
            } else {
                planets[i].SetActive(false);
            }
        }
        
        GetColors();
        MakeColorButtons();
    }
    public void OnSliderPixelChanged()
    {
        pixels = sliderPixel.value;
        planets[selected_planet].GetComponent<IPlanet>().SetPixel(sliderPixel.value);
        textPixel.text = pixels.ToString("F0") + "x" + pixels.ToString("F0");

    }
    public void OnSliderRotationChanged()
    {
        planets[selected_planet].GetComponent<IPlanet>().SetRotate(sliderRotation.value);

    }
    public void OnLightPositionChanged(Vector2 pos)
    {
        planets[selected_planet].GetComponent<IPlanet>().SetLight(pos);
    }

    private void UpdateTime(float time)
    {
        planets[selected_planet].GetComponent<IPlanet>().UpdateTime(time);
        
    }

    public void OnChangeSeedInput()
    {
        if (int.TryParse(inputSeed.text, out seed))
        {
            planets[selected_planet].GetComponent<IPlanet>().SetSeed(seed);
        }
    }
    public void OnChangeSeedRandom()
    {
        seedRandom();
        planets[selected_planet].GetComponent<IPlanet>().SetSeed(seed);
    }
    private void seedRandom()
    {
        UnityEngine.Random.InitState( System.DateTime.Now.Millisecond );
        seed = UnityEngine.Random.Range(0, int.MaxValue);
        inputSeed.text = seed.ToString();

    }
    private void Update()
    {
        if (isOnGui()) return;
        
        if (Input.GetMouseButton(0))
        {
            var pos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            OnLightPositionChanged(pos);
        }

        time += Time.deltaTime;
        if (!override_time) {
            UpdateTime(time);
        }

    }

    private bool isOnGui()
    {
        var eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        var result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, result);

        if (result.Count(x => x.gameObject.GetComponent<Selectable>()) > 0) {
            return true;
        }

        return false;
    }

    public void OnExportPng()
    {
        var mats = new List<Material>();
        var images = planets[selected_planet].GetComponentsInChildren<Image>();
        foreach (var img in images)
        {
            mats.Add(img.material);
        }
        _materialSave.SaveImage(mats, seed.ToString());
        mats.Clear();
    }

    public void OnExportSheets()
    {
        override_time = true;
        var customeSize = 100;
        if (dd_planets.value == 9)
        {
            customeSize = 200;
        }
        var mats = new List<Material>();
        var images = planets[selected_planet].GetComponentsInChildren<Image>();
        foreach (var img in images)
        {
            mats.Add(img.material);
            Debug.Log(img.gameObject.name);
        }

        IPlanet iplanet = planets[selected_planet].GetComponent<IPlanet>();
        _materialSave.SaveSheets(mats, seed.ToString(), _exportControl.GetWidth(), _exportControl.GetHeight(),iplanet, customeSize);
        mats.Clear();
        override_time = false;
    }
    
    public void ShowExport()
    {
        exportPanel.SetActive(true);
    }

    public void HideExport()
    {
        exportPanel.SetActive(false);
    }
    
}
