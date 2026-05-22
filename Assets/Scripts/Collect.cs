using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    public int value;
    public Color collectColor;
    Renderer render;
    public Rigidbody collectRb;
    public Collider collectCol;
    private void OnEnable()
    {
        PlayerController.kick += Kick;
    }
    private void OnDisable()
    {
        PlayerController.kick -= Kick;
    }
    void Kick(float force)
    {
        transform.parent = null;
        collectCol.enabled = true;
        collectRb.isKinematic = false;
        collectRb.AddForce(new Vector3(0, force, force));
    }
    private void Awake()
    {
        render = gameObject.GetComponent<Renderer>();
        // FIX #1: "_Color" is the correct shader property name
        render.material.SetColor("_Color", collectColor);
    }
    public Color GetColor()
    {
        return collectColor;
    }
}
