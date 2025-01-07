using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleGrab : MonoBehaviour
{
    public ProgressBar progressBar;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            progressBar.CollectibleGrabbed();
            Destroy(gameObject);
        }
    }
}