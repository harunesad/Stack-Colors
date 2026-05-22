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
        // FIX #1: "_Color" is the correct shader property name
        renderer.material.SetColor("_Color", color);
    }
    public Color GetColor()
    {
        return newColor;
    }
}
