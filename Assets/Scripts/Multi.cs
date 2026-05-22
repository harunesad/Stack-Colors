using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi : MonoBehaviour
{
    public float multiValue;
    public Color multiColor;
    public Renderer[] renderers;
    void Awake()
    {
        SetColor();
    }
    void SetColor()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            // FIX #1: "_Color" is the correct shader property name
            renderers[i].material.SetColor("_Color", multiColor);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Collect"))
        {
            GameController.instance.UpdateMulti(multiValue);
        }
    }
}
