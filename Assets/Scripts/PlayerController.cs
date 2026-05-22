using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Color charColor;
    public Renderer[] charRenderer;
    public bool isPlay;
    public float forwardSpeed;
    public float horizontalSpeed;
    Rigidbody charRb;
    public Transform parentCollect;
    public Transform stackPosition;
    public bool end;
    public float forwardForce, forceInc, forceReduce;
    public static Action<float> kick;
    public static PlayerController instance;
    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Awake()
    {
        charRb = GetComponent<Rigidbody>();
        SetColor(charColor);
    }
    void Update()
    {
        Move();
    }
    void SetColor(Color c)
    {
        charColor = c;
        for (int i = 0; i < charRenderer.Length; i++)
        {
            // FIX #1: "_Color" is the correct shader property name
            charRenderer[i].material.SetColor("_Color", charColor);
        }
    }
    void MoveForward()
    {
        charRb.linearVelocity = Vector3.forward * forwardSpeed;
    }
    void MoveHorizontal()
    {
        RaycastHit raycast;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out raycast, 100))
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(raycast.point.x, transform.position.y, transform.position.z), Time.deltaTime * horizontalSpeed);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ColorWall"))
        {
            SetColor(other.GetComponent<ColorWall>().GetColor());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinishLineStart"))
        {
            end = true;
        }
        if (other.CompareTag("FinishLineEnd"))
        {
            charRb.linearVelocity = Vector3.zero;
            isPlay = false;
            LaunchStack();
            return; // FIX #4: early return so we don't process Collect after finish
        }

        // FIX #4: guard moved after FinishLine checks so finish line always triggers
        if (end)
        {
            return;
        }

        if (other.CompareTag("Collect"))
        {
            Transform otherTransform = other.transform;

            if (charColor == otherTransform.GetComponent<Collect>().GetColor())
            {
                GameController.instance.UpdateScore(otherTransform.GetComponent<Collect>().value);
            }
            else
            {
                GameController.instance.UpdateScore(otherTransform.GetComponent<Collect>().value * -1);
                Destroy(other.gameObject);
                if (parentCollect != null)
                {
                    if (parentCollect.childCount > 0)
                    {
                        // use localScale.y / 2 + 0.5 extra height offset
                        parentCollect.position -= Vector3.up * (parentCollect.GetChild(parentCollect.childCount - 1).localScale.y / 2f + 0.5f);
                        Destroy(parentCollect.GetChild(parentCollect.childCount - 1).gameObject);
                    }
                    else
                    {
                        Destroy(parentCollect.gameObject);
                        parentCollect = null;
                    }
                }
                return;
            }

            Rigidbody otherRb = otherTransform.GetComponent<Rigidbody>();
            otherRb.isKinematic = true;
            other.enabled = false;
            if (parentCollect == null)
            {
                parentCollect = otherTransform;
                parentCollect.position = stackPosition.position;
                parentCollect.parent = stackPosition;
            }
            else
            {
                // use localScale.y / 2 + 0.5 extra height offset
                parentCollect.position += Vector3.up * (otherTransform.localScale.y / 2f + 0.5f);
                otherTransform.position = stackPosition.position;
                otherTransform.parent = parentCollect;
            }
        }
    }
    void LaunchStack()
    {
        // FIX #2: only invoke kick if there are subscribers (stack is not empty)
        if (kick != null)
        {
            kick(forwardForce);
        }
    }
    void Move()
    {
        if (isPlay == true)
        {
            MoveForward();
        }
        if (end)
        {
            forwardForce -= forceReduce * Time.deltaTime;
            if (forwardForce < 0)
            {
                forwardForce = 0;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (end)
            {
                forwardForce += forceInc;
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (end)
            {
                return;
            }
            if (isPlay == false)
            {
                isPlay = true;
            }
            MoveHorizontal();
        }
    }
}
