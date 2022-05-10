using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    public Transform target;
    float followSpeed = 5;
    private void FixedUpdate()
    {
        SetTarget(target);
    }
    public void SetTarget(Transform targetTransform)
    {
        if (PlayerController.instance.isPlay == true)
        {
            Vector3 posCam = new Vector3(4.27f, 3.65f, -4f);
            transform.position = Vector3.Lerp(transform.position, targetTransform.position + posCam, Time.deltaTime);
        }
        if (PlayerController.instance.end == true && PlayerController.instance.isPlay == false)
        {
            Vector3 posCam = new Vector3(0, 1.65f, -2f);
            transform.position = Vector3.Lerp(transform.position, PlayerController.instance.parentCollect.position + posCam, Time.deltaTime);
        }
    }
}
