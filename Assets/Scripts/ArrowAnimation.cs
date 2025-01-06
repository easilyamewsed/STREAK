using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAnimation : MonoBehaviour
{
    public float arrowHeight = 0.5f;
    public float arrowSpeed = 2f;

    private Vector3 startLocalPosition;
    private float time;

    void Start()
    {

        startLocalPosition = transform.localPosition;
    }

    void Update()
    {
        time += Time.deltaTime * arrowSpeed;

        float newY = Mathf.Sin(time) * arrowHeight;

        transform.localPosition = new Vector3(startLocalPosition.x, startLocalPosition.y + newY, startLocalPosition.z);
    }
}
