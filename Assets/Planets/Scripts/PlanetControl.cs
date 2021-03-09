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
    private float time = 0f;
    private float pixels = 100;
    private int seed = 0;
    private bool override_time = false;
    private void Start()
    {
        OnChangeSeedRandom();
    }

    private int selected_planet = 0;

    public void OnSelectPlanet()
    {
        selected_planet = dd_planets.value;
        
        for (int i = 0; i < planets.Length; i++)
        {
            if (i == selected_planet) {
                planets[i] .SetActive(true);
            } else {
                planets[i].SetActive(false);
            }
        }
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
