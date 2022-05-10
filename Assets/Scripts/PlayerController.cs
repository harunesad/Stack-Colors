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
            charRenderer[i].material.SetColor("Color", charColor);
            charRenderer[i].material.color = charRenderer[i].material.GetColor("Color"); 
        }
    }
    void MoveForward()
    {
        charRb.velocity = Vector3.forward * forwardSpeed;
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
            charRb.velocity = Vector3.zero;
            isPlay = false;
            LaunchStack();
        }
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
                        parentCollect.position -= Vector3.up * parentCollect.GetChild(parentCollect.childCount - 1).localScale.y;
                        Destroy(parentCollect.GetChild(parentCollect.childCount - 1).gameObject);
                    }
                    else
                    {
                        Destroy(parentCollect.gameObject);
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
                parentCollect.position += Vector3.up * (otherTransform.localScale.y);
                otherTransform.position = stackPosition.position;
                otherTransform.parent = parentCollect;
            }
        }
    }
    void LaunchStack()
    {
        kick(forwardForce);
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
