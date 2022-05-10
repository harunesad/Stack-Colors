using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorWall : MonoBehaviour
{
    public Color newColor;
    void Awake()
    {
        Color color = newColor;
        color.a = 0.5f;
        Renderer renderer = gameObject.GetComponentInChildren<Renderer>();
        renderer.material.SetColor("Color", color);
        renderer.material.color = renderer.material.GetColor("Color");
    }
    public Color GetColor()
    {
        return newColor;
    }
}
