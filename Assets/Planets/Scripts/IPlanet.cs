using System.Collections.Generic;
using UnityEngine;

public interface IPlanet {
    void SetPixel(float amount);
    void SetLight(Vector2 pos);
    void SetSeed(float seed);
    void SetRotate(float r);
    void UpdateTime(float time);
    void SetCustomTime(float time);
    Color[] GetColors();
    void SetColors(Color[] _colors);
    void SetInitialColors();

}
