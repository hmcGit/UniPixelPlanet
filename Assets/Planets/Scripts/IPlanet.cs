using UnityEngine;

public interface IPlanet {
    void SetPixel(float amount);
    void SetLight(Vector2 pos);
    void SetSeed(float seed);
    void SetRotate(float r);
    void UpdateTime(float time);
    void SetCustomTime(float time);
    
}
