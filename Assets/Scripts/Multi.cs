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
            renderers[i].material.SetColor("Color", multiColor);
            renderers[i].material.color = renderers[i].material.GetColor("Color");

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
